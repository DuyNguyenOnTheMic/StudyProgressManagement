using SignalRProgressBarSimpleExample.Util;
using StudyProgressManagement.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudyProgressManagement.Areas.Faculty.Controllers
{
    public class RegistrationResultController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Faculty/RegistrationResult
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
            var postedTerm = Request.Form["term"];
            int studentCourseId = int.Parse(postedStudentCourse);
            string termId = postedTerm.ToString();


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

                // Generate term name
                string termName = "Học kỳ " + termId[termId.Length - 1];

                var query_term = db.terms.Where(t => t.id == termId).FirstOrDefault();
                if (query_term == null)
                {
                    // Add term
                    db.terms.Add(new term
                    {
                        id = termId,
                        name = termName
                    });
                    db.SaveChanges();
                }

                var query_studentcourse_curriculum = db.curricula.Where(s => s.student_course_id == studentCourseId).FirstOrDefault();
                int itemsCount = dt.Rows.Count;

                // Create a datatable for error curriculums
                DataTable errorCurriculums = new DataTable("Grid");
                errorCurriculums.Columns.AddRange(
                    new DataColumn[3]{
                        new DataColumn("curriculumId"),
                        new DataColumn("curriculumName"),
                        new DataColumn("credits")
                    });


                /*try
                {*/
                //Insert records to database table.
                foreach (DataRow row in dt.Rows)
                {
                    // Declare all columns
                    string studentId = row["Mã SV"].ToString();
                    string studentName = row["Họ tên SV"].ToString();
                    string studentEmail = row["Email SV"].ToString();
                    string studentBirthDate = row["Ngày sinh"].ToString();
                    string studentGender = row["Giới tính"].ToString();
                    string studentClassId = row["Thuộc Lớp"].ToString();
                    string studentFaculty = row["Thuộc Khoa"].ToString();
                    string curriculumId = row["Mã HP"].ToString();
                    string curriculumClassId = row["Mã LHP"].ToString();
                    string curriculumName = row["Tên HP"].ToString();
                    string credits = row["Số TC"].ToString();
                    string registrationType = row["HT Đăng Ký"].ToString();
                    string registrationDate = row["Ngày ĐK"].ToString();
                    string registrationPerson = row["Người ĐK"].ToString();
                    string lecturerId = row["Mã giảng viên"].ToString();
                    string lecturerName = row["Giảng viên"].ToString();
                    string curriculumClassSchedule = row["Thời khóa biểu"].ToString();

                    // Check if student course already has study program
                    /*if (query_studentcourse_curriculum != null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }*/

                    var query_classstudent = db.class_student.Where(s => s.id == studentClassId).FirstOrDefault();
                    if (query_classstudent == null)
                    {
                        // Add class student
                        db.class_student.Add(new class_student
                        {
                            id = studentClassId
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
                            birth_date = DateTime.ParseExact(studentBirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            email = studentEmail,
                            gender = studentGender,
                            faculty = studentFaculty,
                            class_student_id = studentClassId,
                            student_course_id = studentCourseId
                        });
                        db.SaveChanges();
                    }
                    else if (query_student.email == null)
                    {
                        query_student.email = studentEmail;
                        db.Entry(query_student).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }

                    var query_curriculum_class = db.curriculum_class.Where(c => c.id == curriculumClassId).FirstOrDefault();
                    if (query_curriculum_class == null)
                    {
                        // Add curriculum class
                        db.curriculum_class.Add(new curriculum_class
                        {
                            id = curriculumClassId,
                            schedule = SetNullOnEmpty(curriculumClassSchedule)
                        });
                        db.SaveChanges();
                    }


                    var query_curriculum = db.curricula.Where(c => c.curriculum_id ==
                    curriculumId && c.student_course_id == studentCourseId).FirstOrDefault();

                    // Check if lecturer is null
                    if (!string.IsNullOrEmpty(lecturerId))
                    {
                        var query_lecturer = db.lecturers.Where(l => l.id == lecturerId).FirstOrDefault();
                        if (query_lecturer == null)
                        {
                            // Add lecturer
                            db.lecturers.Add(new lecturer
                            {
                                id = lecturerId,
                                name = lecturerName
                            });
                            db.SaveChanges();
                        }
                    }

                    if (query_curriculum != null)
                    {
                        // Add study results
                        db.registration_results.Add(new registration_results
                        {
                            registration_type = registrationType,
                            registration_date = DateTime.ParseExact(registrationDate, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
                            registration_person = SetNullOnEmpty(registrationPerson),
                            term_id = termId,
                            curriculum_id = query_curriculum.id,
                            curriculum_class_id = curriculumClassId,
                            lecturer_id = SetNullOnEmpty(lecturerId),
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
                /* }
                 catch (Exception)
                 {
                     return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
                 }*/
            }
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        public static string SetNullOnEmpty(string value)
        {
            // Check if string is empty
            return value != null && string.IsNullOrEmpty(value.Trim()) ? null : value;
        }

    }
}