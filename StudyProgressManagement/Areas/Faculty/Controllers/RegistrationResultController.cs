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
    public class RegistrationResultController : Controller
    {
        SEP25Team03Entities db = new SEP25Team03Entities();

        // GET: Faculty/RegistrationResult
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            ViewBag.majors = db.majors.ToList();
            return View();
        }

        [HttpPost]
        public JsonResult Search(int studentCourseId, string classStudentId, string studentName)
        {
            // Search student study results
            return Json(db.students.Where(s => s.student_course_id == studentCourseId &&
            s.class_student_id == classStudentId && s.full_name.Contains(studentName)).Select(s => new
            {
                id = s.id,
                full_name = s.full_name
            }).ToList(), JsonRequestBehavior.AllowGet);
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
            if (query_student != null)
            {
                // Get registration results of student
                return Json(db.registration_results.Where(s => s.student_id == studentId).Select(s => new
                {
                    curriculum_id = s.curriculum.curriculum_id,
                    curriculum_name = s.curriculum.name,
                    credits = s.curriculum.credits,
                    registration_type = s.registration_type,
                    registration_date = s.registration_date,
                    curriculum_class_id = s.curriculum_class.id,
                    curriculum_class_schedule = s.curriculum_class.schedule,
                    lecturer_id = s.lecturer.id,
                    lecturer_name = s.lecturer.name,
                    term_id = s.term_id

                }).OrderBy(s => s.term_id).ToList(), JsonRequestBehavior.AllowGet);
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

                // Check if student course has study program
                var query_studentcourse_curriculum = db.curricula.Where(s => s.student_course_id == studentCourseId).FirstOrDefault();
                if (query_studentcourse_curriculum == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict);
                }

                // Validate all columns
                bool isValid = ValidateColumns(dt);
                if (!isValid)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.ExpectationFailed);
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

                // Check if term already imported of this student course
                var query_registrationresults_term = db.registration_results.Where(s => s.term_id
                == termId && s.student_course_id == studentCourseId).FirstOrDefault();
                if (query_registrationresults_term != null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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

                        var query_classstudent = db.class_student.Where(s => s.id == studentClassId).FirstOrDefault();
                        if (query_classstudent == null)
                        {
                            // Add class student
                            db.class_student.Add(new class_student
                            {
                                id = studentClassId,
                                student_course_id = studentCourseId
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
                            query_student.gender = studentGender;
                            query_student.faculty = studentFaculty;
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
            if (ContainColumn("Mã SV", dt) && ContainColumn("Họ tên SV", dt) && ContainColumn("Email SV", dt)
                && ContainColumn("Ngày sinh", dt) && ContainColumn("Giới tính", dt) && ContainColumn("Thuộc Lớp", dt)
                && ContainColumn("Thuộc Khoa", dt) && ContainColumn("Mã HP", dt) && ContainColumn("Mã LHP", dt)
                && ContainColumn("Tên HP", dt) && ContainColumn("Số TC", dt) && ContainColumn("HT Đăng Ký", dt)
                && ContainColumn("Ngày ĐK", dt) && ContainColumn("Người ĐK", dt) && ContainColumn("Mã giảng viên", dt)
                && ContainColumn("Giảng viên", dt) && ContainColumn("Thời khóa biểu", dt))
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

        public ActionResult DataTableToJson(DataTable table)
        {
            // Convert datatable to Json
            string jsonString = JsonConvert.SerializeObject(table);
            return Json(jsonString, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int studentCourseId, string termId)
        {
            try
            {
                // Delete all records for re-import
                db.registration_results.RemoveRange(db.registration_results.Where(c => c.term_id
                == termId && c.student_course_id == studentCourseId));
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