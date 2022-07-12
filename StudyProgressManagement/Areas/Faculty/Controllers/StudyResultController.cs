using Newtonsoft.Json;
using SignalRProgressBarSimpleExample.Util;
using StudyProgressManagement.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    [Authorize(Roles = "Faculty")]
    public class StudyResultController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Faculty/StudyResult
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetStudentInfo(string studentId)
        {
            // Get student information
            var query_student = db.students.Where(s => s.id == studentId).FirstOrDefault();
            if (query_student != null)
            {
                var studentInfo = query_student.full_name + " - " + studentId + " - " + query_student.student_course.course + " Ngành " + query_student.student_course.major.name;
                return studentInfo;
            }
            return null;
        }

        [HttpPost]
        public JsonResult GetData(string studentId)
        {
            var query_student = db.students.Where(s => s.id == studentId).FirstOrDefault();
            var query_studyResult = db.study_results;
            // Get study results of student
            if (query_student != null)
            {
                return Json(db.curricula.Where(s => s.student_course_id == query_student.student_course_id).Select(s => new
                {
                    curriculum_id = s.curriculum_id,
                    curriculum_name = s.name,
                    credits = s.credits,
                    theoretical_hours = s.theoretical_hours,
                    practice_hours = s.practice_hours,
                    internship_hours = s.internship_hours,
                    project_hours = s.project_hours,
                    compulsory_or_optional = s.compulsory_or_optional,
                    knowledge_type_group_1 = s.knowledge_type.group_1,
                    knowledge_type_group_2 = s.knowledge_type.group_2,
                    knowledge_type_group_3 = s.knowledge_type.group_3,
                    compulsory_credits = s.knowledge_type.compulsory_credits,
                    optional_credits = s.knowledge_type.optional_credits,
                    mark10 = query_studyResult.Where(d => d.student_id == studentId && d.curriculum_id == s.id)
                    .Select(d => d.mark10).FirstOrDefault().ToString(),
                    mark10_2 = query_studyResult.Where(d => d.student_id == studentId && d.curriculum_id == s.id)
                    .Select(d => d.mark10_2).FirstOrDefault().ToString(),
                    max_mark_10 = query_studyResult.Where(d => d.student_id == studentId && d.curriculum_id == s.id)
                    .Select(d => d.max_mark_10).FirstOrDefault().ToString(),
                    max_mark_letter = query_studyResult.Where(d => d.student_id == studentId && d.curriculum_id == s.id)
                    .Select(d => d.max_mark_letter).FirstOrDefault().ToString(),
                    is_pass = query_studyResult.Where(d => d.student_id == studentId && d.curriculum_id == s.id)
                    .Select(d => d.is_pass).FirstOrDefault().ToString(),

                }).ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json(new { error = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadStudentCourses(string majorId)
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

                // Check if student course has study program
                var query_studentcourse_curriculum = db.curricula.Where(s => s.student_course_id == studentCourseId).FirstOrDefault();
                if (query_studentcourse_curriculum == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict);
                }

                // Validate all columns before delete
                bool isValid = ValidateColumns(dt);
                if (isValid)
                {
                    // Delete all study results
                    Delete(studentCourseId);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
                }

                int itemsCount = dt.Rows.Count;

                // Create a datatable for error curriculums
                DataTable errorCurriculums = new DataTable("Grid");
                errorCurriculums.Columns.AddRange(
                    new DataColumn[3]{
                        new DataColumn("curriculumId"),
                        new DataColumn("curriculumName"),
                        new DataColumn("credits")
                    });

                try
                {
                    //Insert records to database table.
                    foreach (DataRow row in dt.Rows)
                    {
                        // Declare all columns
                        string studentId = row["StudentID"].ToString();
                        string studentName = row["StudentName"].ToString();
                        string birthDay = row["BirthDay"].ToString();
                        string birthPlace = row["BirthPlace"].ToString();
                        string classStudentId = row["ClassStudentID"].ToString();
                        string classStudentName = row["ClassStudentName"].ToString();
                        string yearStudy = row["YearStudy"].ToString();
                        string oldTermId = row["TermID"].ToString();
                        string termName = row["TermName"].ToString();
                        string curriculumId = row["CurriculumID"].ToString();
                        string studyUnitId = row["StudyUnitID"].ToString();
                        string studyUnitAlias = row["StudyUnitAlias"].ToString();
                        string curriculumName = row["CurriculumName"].ToString();
                        string credits = row["Credits"].ToString();
                        string mark10 = row["Mark10"].ToString();
                        string mark10_2 = row["Mark10_2"].ToString();
                        string mark10_3 = row["Mark10_3"].ToString();
                        string mark10_4 = row["Mark10_4"].ToString();
                        string mark10_5 = row["Mark10_5"].ToString();
                        string maxMark10 = row["MaxMark10"].ToString();
                        string maxMark4 = row["maxMark4"].ToString();
                        string maxMarkLetter = row["MaxMarkLetter"].ToString();
                        string isPass = row["IsPass"].ToString();


                        var newTermId = "HK" + studyUnitId.Substring(0, 3);

                        var query_classstudent = db.class_student.Where(s => s.id == classStudentId).FirstOrDefault();
                        if (query_classstudent == null)
                        {
                            // Add class student
                            db.class_student.Add(new class_student
                            {
                                id = classStudentId,
                                name = classStudentName
                            });
                            db.SaveChanges();
                        }

                        var query_student = db.students.Where(s => s.id == studentId).FirstOrDefault();
                        if (query_student == null)
                        {
                            // Add student
                            db.students.Add(new student
                            {
                                id = studentId,
                                full_name = studentName,
                                birth_date = DateTime.ParseExact(birthDay, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                birth_place = birthPlace,
                                class_student_id = classStudentId,
                                student_course_id = studentCourseId
                            });
                            db.SaveChanges();
                        }
                        else if (query_student.birth_place == null)
                        {
                            query_student.birth_place = birthPlace;
                            db.Entry(query_student).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }

                        var query_term = db.terms.Where(t => t.id == newTermId).FirstOrDefault();
                        if (query_term == null)
                        {
                            // Add term
                            db.terms.Add(new term
                            {
                                id = newTermId,
                                name = termName
                            });
                            db.SaveChanges();
                        }

                        var query_study_unit = db.study_unit.Where(s => s.id == studyUnitId).FirstOrDefault();
                        if (query_study_unit == null)
                        {
                            // Add study unit
                            db.study_unit.Add(new study_unit
                            {
                                id = studyUnitId,
                                alias = studyUnitAlias
                            });
                            db.SaveChanges();
                        }

                        var query_curriculum = db.curricula.Where(c => c.curriculum_id ==
                        curriculumId && c.student_course_id == studentCourseId).FirstOrDefault();

                        if (query_curriculum != null)
                        {
                            // Add study results
                            db.study_results.Add(new study_results
                            {
                                mark10 = SetNullOnEmpty(mark10),
                                mark10_2 = SetNullOnEmpty(mark10_2),
                                mark10_3 = SetNullOnEmpty(mark10_3),
                                mark10_4 = SetNullOnEmpty(mark10_4),
                                mark10_5 = SetNullOnEmpty(mark10_5),
                                max_mark_10 = SetNullOnEmpty(maxMark10),
                                max_mark_4 = SetNullOnEmpty(maxMark4),
                                max_mark_letter = SetNullOnEmpty(maxMarkLetter),
                                is_pass = SetNullOnEmpty(isPass),
                                year_study = yearStudy,
                                term_id = newTermId,
                                curriculum_id = query_curriculum.id,
                                study_unit_id = studyUnitId,
                                student_id = studentId,
                                student_course_id = studentCourseId
                            });
                        }
                        else
                        {
                            // Add error curriculums which are not in study program
                            errorCurriculums.Rows.Add(curriculumId, curriculumName, credits);
                        }

                        // Send progress to progress bar
                        Functions.SendProgress("Đang import...", dt.Rows.IndexOf(row), itemsCount);

                    }
                    db.SaveChanges();

                    // Remove duplicate values
                    DataTable distinctTable = errorCurriculums.DefaultView.ToTable( /*distinct*/ true);
                    return DataTableToJson(distinctTable);
                }
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
                }
            }
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        public bool ValidateColumns(DataTable dt)
        {
            // Validate all columns in excel file
            if (ContainColumn("StudentID", dt) && ContainColumn("StudentName", dt) && ContainColumn("BirthDay", dt)
                && ContainColumn("BirthPlace", dt) && ContainColumn("ClassStudentID", dt) && ContainColumn("ClassStudentName", dt)
                && ContainColumn("YearStudy", dt) && ContainColumn("TermID", dt) && ContainColumn("TermName", dt)
                && ContainColumn("CurriculumID", dt) && ContainColumn("StudyUnitID", dt) && ContainColumn("StudyUnitAlias", dt)
                && ContainColumn("CurriculumName", dt) && ContainColumn("Credits", dt) && ContainColumn("Mark10", dt)
                && ContainColumn("Mark10_2", dt) && ContainColumn("Mark10_3", dt) && ContainColumn("Mark10_4", dt)
                && ContainColumn("Mark10_5", dt) && ContainColumn("MaxMark10", dt) && ContainColumn("maxMark4", dt)
                && ContainColumn("MaxMarkLetter", dt) && ContainColumn("IsPass", dt))
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

        public JsonResult DataTableToJson(DataTable table)
        {
            // Convert datatable to Json
            string jsonString = JsonConvert.SerializeObject(table);
            return Json(jsonString, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // Delete all records for re-import
                db.study_results.RemoveRange(db.study_results.Where(c => c.student_course_id == id));
                db.SaveChanges();
            }
            catch (Exception)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public static string SetNullOnEmpty(string value)
        {
            // Check if string is empty
            return value != null && string.IsNullOrEmpty(value.Trim()) ? null : value;
        }
    }
}