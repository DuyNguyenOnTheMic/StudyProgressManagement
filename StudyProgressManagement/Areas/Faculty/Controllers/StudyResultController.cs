using StudyProgressManagement.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    public class StudyResultController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Faculty/StudyResult
        public ActionResult Index()
        {
            return View();
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

                /*var query_studentcourse_curriculum = db.curricula.Where(s => s.student_course_id == studentCourseId).FirstOrDefault();*/

                /*try
                {*/
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
                    string termId = row["TermID"].ToString();
                    string termName = row["TermName"].ToString();
                    string curriculumId = row["CurriculumID"].ToString();
                    string studyUnitId = row["StudyUnitID"].ToString();
                    string studyUnitAlias = row["StudyUnitAlias"].ToString();
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
                        db.students.Add(new student
                        {
                            // Add student
                            id = studentId,
                            full_name = SetNullOnEmpty(studentName),
                            birth_date = DateTime.Parse(birthDay),
                            birth_place = SetNullOnEmpty(birthPlace),
                            class_student_id = SetNullOnEmpty(classStudentId),
                            student_course_id = studentCourseId
                        });
                        db.SaveChanges();
                    }

                    var query_term = db.terms.Where(t => t.id == termId).FirstOrDefault();
                    if (query_term == null)
                    {
                        db.terms.Add(new term
                        {
                            id = termId,
                            name = termName
                        });
                        db.SaveChanges();
                    }

                    var query_study_unit = db.study_unit.Where(s => s.id == studyUnitId).FirstOrDefault();
                    if (query_study_unit == null)
                    {
                        db.study_unit.Add(new study_unit
                        {
                            id = studyUnitId,
                            alias = studyUnitAlias
                        });
                        db.SaveChanges();
                    }

                    var query_curriculum = db.curricula.Where(c => c.curriculum_id == 
                    curriculumId && c.student_course_id == studentCourseId).FirstOrDefault();

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
                        term_id = termId,
                        curriculum_id = query_curriculum.id,
                        study_unit_id = studyUnitId,
                        student_id = studentId
                    });


                }
                db.SaveChanges();
                /*}
                catch (Exception)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
                }*/
            }
            ViewBag.majors = db.majors.ToList();
            return View();
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