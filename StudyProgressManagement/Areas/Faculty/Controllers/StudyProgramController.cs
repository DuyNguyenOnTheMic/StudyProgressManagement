using StudyProgressManagement.Models;
using StudyProgressManagement.Util;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class StudyProgramController : Controller
    {
        readonly SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Faculty/StudyProgram
        public ActionResult Index()
        {
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        [HttpPost]
        public JsonResult GetData(int id)
        {
            // Get curriculum of student courses data from datatabse
            return Json(db.curricula.Where(s => s.student_course_id == id).Select(s => new
            {
                s.curriculum_id,
                s.name,
                s.name_english,
                s.credits,
                s.theoretical_hours,
                s.practice_hours,
                s.internship_hours,
                s.project_hours,
                s.compulsory_or_optional,
                s.prerequisites,
                s.learn_before,
                s.editing_notes,
                knowledge_type_group_1 = s.knowledge_type.group_1,
                knowledge_type_group_2 = s.knowledge_type.group_2,
                knowledge_type_group_3 = s.knowledge_type.group_3,
                s.knowledge_type.compulsory_credits,
                s.knowledge_type.optional_credits

            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadStudentCourses(string majorId)
        {
            // get student courses data from database
            return Json(db.student_course.Where(s => s.major_id == majorId).ToList().OrderBy(x => new string(x.course.Where(char.IsLetter).ToArray())).ThenBy(x =>
            {
                // Natural sorting
                if (int.TryParse(new string(x.course.Where(char.IsDigit).ToArray()), out int number))
                    return number;
                return -1;
            }).Select(s => new
            {
                s.id,
                s.course
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Import()
        {
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Import(HttpPostedFileBase postedFile)
        {
            var postedStudentCourse = Request.Form["student_course"];
            int studentCourseId = int.Parse(postedStudentCourse);


            string filePath = string.Empty;
            if (postedFile != null)
            {
                string path = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                filePath = path + Path.GetFileName(postedFile.FileName);
                string extension = Path.GetExtension(postedFile.FileName);
                postedFile.SaveAs(filePath);

                string conString = ConfigurationManager.ConnectionStrings["ExcelConString"].ConnectionString;

                DataTable dt = new DataTable();
                conString = string.Format(conString, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(conString))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();
                        }
                    }
                }

                // Validate all columns
                bool isValid = ValidateColumns(dt);
                if (!isValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
                }

                // Check if student course already has study program
                var query_studentcourse_curriculum = db.curricula.FirstOrDefault(s => s.student_course_id == studentCourseId);
                if (query_studentcourse_curriculum != null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }


                int itemsCount = dt.Rows.Count;

                try
                {
                    //Insert records to database table.
                    foreach (DataRow row in dt.Rows)
                    {
                        // Declare all columns
                        string knowledgeTypeAlias = row["Mã loại kiến thức"].ToString();
                        string knowledgeTypeName = row["Tên loại kiến thức"].ToString();
                        string compulsoryCredits = row["Số chỉ BB"].ToString();
                        string optionalCredits = row["Số chỉ TC"].ToString();
                        string curriculumId = row["Mã học phần"].ToString();
                        string curriculumName = row["Tên học phần (Tiếng Việt)"].ToString();
                        string curriculumNameEnglish = row["Tên học phần (Tiếng Anh)"].ToString();
                        string credits = row["TC"].ToString();
                        string theoreticalHours = row["LT"].ToString();
                        string practiceHours = row["TH"].ToString();
                        string internshipHours = row["TT"].ToString();
                        string projectHours = row["DA"].ToString();
                        string compulsoryOrOptional = row["Bắt buộc/ Tự chọn"].ToString();
                        string prerequisites = row["Điều kiện tiên quyết"].ToString();
                        string learnBefore = row["Học trước – học sau"].ToString();
                        string editingNotes = row["Ghi chú chỉnh sửa"].ToString();

                        var query_knowledge_type = db.knowledge_type.Where(k => k.knowledge_type_alias ==
                        knowledgeTypeAlias && k.student_course_id == studentCourseId).FirstOrDefault();
                        if (query_knowledge_type == null)
                        {
                            // Add new knowledge type
                            query_knowledge_type = new knowledge_type();
                            if (knowledgeTypeAlias.StartsWith("DC"))
                            {
                                // Add general knowledge type
                                query_knowledge_type.knowledge_type_alias = SetNullOnEmpty(knowledgeTypeAlias);
                                query_knowledge_type.group_1 = "Kiến thức giáo dục đại cương";
                                query_knowledge_type.group_2 = SetNullOnEmpty(knowledgeTypeName);
                                query_knowledge_type.compulsory_credits = ToNullableInt(compulsoryCredits);
                                query_knowledge_type.optional_credits = ToNullableInt(optionalCredits);
                                query_knowledge_type.student_course_id = studentCourseId;
                            }
                            else if (knowledgeTypeAlias.StartsWith("CSN"))
                            {
                                // Add major base knowledge type
                                query_knowledge_type.knowledge_type_alias = SetNullOnEmpty(knowledgeTypeAlias);
                                query_knowledge_type.group_1 = "Kiến thức giáo dục chuyên nghiệp";
                                query_knowledge_type.group_2 = SetNullOnEmpty(knowledgeTypeName);
                                query_knowledge_type.compulsory_credits = ToNullableInt(compulsoryCredits);
                                query_knowledge_type.optional_credits = ToNullableInt(optionalCredits);
                                query_knowledge_type.student_course_id = studentCourseId;
                            }
                            else
                            {
                                // Add major specialized knowledge type
                                query_knowledge_type.knowledge_type_alias = SetNullOnEmpty(knowledgeTypeAlias);
                                query_knowledge_type.group_1 = "Kiến thức giáo dục chuyên nghiệp";
                                query_knowledge_type.group_2 = "Kiến thức chuyên ngành";
                                query_knowledge_type.group_3 = SetNullOnEmpty(knowledgeTypeName);
                                query_knowledge_type.compulsory_credits = ToNullableInt(compulsoryCredits);
                                query_knowledge_type.optional_credits = ToNullableInt(optionalCredits);
                                query_knowledge_type.student_course_id = studentCourseId;
                            }
                            db.knowledge_type.Add(query_knowledge_type);
                            db.SaveChanges();
                        }

                        db.curricula.Add(new curriculum
                        {
                            // Add curriculum data
                            curriculum_id = SetNullOnEmpty(curriculumId),
                            name = SetNullOnEmpty(curriculumName),
                            name_english = SetNullOnEmpty(curriculumNameEnglish),
                            credits = (int)ToNullableInt(credits),
                            theoretical_hours = ToNullableInt(theoreticalHours),
                            practice_hours = ToNullableInt(practiceHours),
                            internship_hours = ToNullableInt(internshipHours),
                            project_hours = ToNullableInt(projectHours),
                            compulsory_or_optional = SetNullOnEmpty(compulsoryOrOptional),
                            prerequisites = SetNullOnEmpty(prerequisites),
                            learn_before = SetNullOnEmpty(learnBefore),
                            editing_notes = SetNullOnEmpty(editingNotes),
                            student_course_id = studentCourseId,
                            knowledge_type_id = query_knowledge_type.id
                        });

                        // Send progress to progress bar
                        Functions.SendProgress("Đang import...", dt.Rows.IndexOf(row), itemsCount);

                    }
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
                }
            }
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // Delete all records for re-import
                db.curricula.RemoveRange(db.curricula.Where(c => c.student_course_id == id));
                db.knowledge_type.RemoveRange(db.knowledge_type.Where(c => c.student_course_id == id));
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteAll(int id)
        {
            try
            {
                // Delete all records and all other study results and registration results
                db.study_results.RemoveRange(db.study_results.Where(c => c.student_course_id == id));
                db.registration_results.RemoveRange(db.registration_results.Where(c => c.student_course_id == id));
                db.curricula.RemoveRange(db.curricula.Where(c => c.student_course_id == id));
                db.knowledge_type.RemoveRange(db.knowledge_type.Where(c => c.student_course_id == id));
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public bool ValidateColumns(DataTable dt)
        {
            // Validate all columns in excel file
            if (ContainColumn("Mã loại kiến thức", dt) && ContainColumn("Tên loại kiến thức", dt)
                && ContainColumn("Số chỉ BB", dt) && ContainColumn("Số chỉ TC", dt) && ContainColumn("Mã học phần", dt)
                && ContainColumn("Tên học phần (Tiếng Việt)", dt) && ContainColumn("Tên học phần (Tiếng Anh)", dt)
                && ContainColumn("TC", dt) && ContainColumn("LT", dt) && ContainColumn("TH", dt) && ContainColumn("TT", dt)
                && ContainColumn("DA", dt) && ContainColumn("Bắt buộc/ Tự chọn", dt) && ContainColumn("Điều kiện tiên quyết", dt)
                && ContainColumn("Học trước – học sau", dt) && ContainColumn("Ghi chú chỉnh sửa", dt))
            {
                return true;
            }
            return false;
        }

        public bool ContainColumn(string columnName, DataTable table)
        {
            // Action to check if datatable contain some columns
            DataColumnCollection columns = table.Columns;
            if (columns.Contains(columnName))
            {
                return true;
            }
            return false;
        }

        public static int? ToNullableInt(string value)
        {
            // Convert string to nullable int
            return value != null && string.IsNullOrEmpty(value.Trim()) ? (int?)null : int.Parse(value);
        }

        public static string SetNullOnEmpty(string value)
        {
            // Check if string is empty
            return value != null && string.IsNullOrEmpty(value.Trim()) ? null : value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}