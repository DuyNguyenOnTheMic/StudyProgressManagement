using StudyProgressManagement.Models;
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
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Faculty/StudyProgram
        public ActionResult Index()
        {
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        public JsonResult GetData()
        {
            // Get curriculum of student courses data from datatabse
            return Json(db.studentcourse_curriculum.Select(s => new
            {
                id = s.curriculum.id,
                name = s.curriculum.name,
                name_english = s.curriculum.name_english,
                credits = s.curriculum.credits,
                theoretical_hours = s.curriculum.theoretical_hours,
                practice_hours = s.curriculum.practice_hours,
                internship_hours = s.curriculum.internship_hours,
                project_hours = s.curriculum.project_hours,
                compulsory_or_optional = s.curriculum.compulsory_or_optional,
                prerequisites = s.curriculum.prerequisites,
                learn_before = s.curriculum.learn_before,
                editing_notes = s.curriculum.editing_notes,
                knowledge_type_name = s.curriculum.knowledge_type.name,
                knowledge_type_group = s.curriculum.knowledge_type.group

            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadStudentCourses(string majorId)
        {
            // get student courses data from database
            return Json(db.student_course.Where(s => s.major_id == majorId).Select(s => new
            {
                id = s.id,
                course = s.course
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

                string conString = string.Empty;
                switch (extension)
                {
                    case ".xls": //Excel 97-03.
                        conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                        break;
                    case ".xlsx": //Excel 07 and above.
                        conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                        break;
                }

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

                var query_studentcourse_curriculum = db.studentcourse_curriculum.Where(s => s.student_course_id == studentCourseId).FirstOrDefault();

                try
                {
                    //Insert records to database table.
                    foreach (DataRow row in dt.Rows)
                    {
                        // Declare all columns
                        string knowledgeTypeId = row["Mã loại kiến thức"].ToString();
                        string knowledgeTypeName = row["Tên loại kiến thức"].ToString();
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

                        // Check if student course already has study program
                        if (query_studentcourse_curriculum != null)
                        {
                            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                        }


                        var query_knowledge_type = db.knowledge_type.Where(k => k.id == knowledgeTypeId).FirstOrDefault();
                        if (query_knowledge_type == null)
                        {
                            // Add knowledge_type
                            if (knowledgeTypeId.StartsWith("DC"))
                            {
                                db.knowledge_type.Add(new knowledge_type
                                {
                                    id = SetNullOnEmpty(knowledgeTypeId),
                                    name = SetNullOnEmpty(knowledgeTypeName),
                                    group = "Kiến thức giáo dục đại cương"
                                });
                            }
                            else
                            {
                                db.knowledge_type.Add(new knowledge_type
                                {
                                    id = SetNullOnEmpty(knowledgeTypeId),
                                    name = SetNullOnEmpty(knowledgeTypeName),
                                    group = "Kiến thức giáo dục chuyên nghiệp"
                                });
                            }
                        }
                        db.SaveChanges();

                        string curriculum = row["Mã học phần"].ToString();
                        var query_curriculum = db.curricula.Where(c => c.id == curriculum).FirstOrDefault();
                        if (query_curriculum == null)
                        {
                            // Add curriculum
                            db.curricula.Add(new curriculum
                            {
                                id = SetNullOnEmpty(curriculumId),
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
                                knowledge_type_id = SetNullOnEmpty(knowledgeTypeId)
                            });
                        }

                        db.studentcourse_curriculum.Add(new studentcourse_curriculum
                        {
                            // Add studentcourse_curriculum
                            student_course_id = studentCourseId,
                            curriculum_id = SetNullOnEmpty(curriculumId)
                        });
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
            // Delete all records for re-import
            db.studentcourse_curriculum.RemoveRange(db.studentcourse_curriculum.Where(c => c.student_course_id == id));
            db.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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
    }
}