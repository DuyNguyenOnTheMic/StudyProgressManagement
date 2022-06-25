using StudyProgressManagement.Models;
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
        sep_team03Entities db = new sep_team03Entities();

        // GET: Faculty/StudyProgram
        public ActionResult Index()
        {
            return View();
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

           /* int studentcourse_id = int.Parse(postedStudentCourse);
            var query_studentcourse_curriculum = db.studentcourse_curriculum.Where(s => s.student_course_id == studentcourse_id).FirstOrDefault();
            if (query_studentcourse_curriculum != null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/


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

                //Insert records to database table.
                foreach (DataRow row in dt.Rows)
                {
                    string knowledge_type = row["Mã loại kiến thức"].ToString();
                    var query_knowledge_type = db.knowledge_type.Where(k => k.id == knowledge_type).FirstOrDefault();
                    if (query_knowledge_type == null)
                    {
                        db.knowledge_type.Add(new knowledge_type
                        {
                            id = SetNullOnEmpty(row["Mã loại kiến thức"].ToString()),
                            name = SetNullOnEmpty(row["Tên loại kiến thức"].ToString())
                        });
                    }
                    db.SaveChanges();

                    string curriculum = row["Mã học phần"].ToString();
                    var query_curriculum = db.curricula.Where(c => c.id == curriculum).FirstOrDefault();
                    if (query_curriculum == null)
                    {
                        db.curricula.Add(new curriculum
                        {
                            id = SetNullOnEmpty(row["Mã học phần"].ToString()),
                            name = SetNullOnEmpty(row["Tên học phần (Tiếng Việt)"].ToString()),
                            name_english = SetNullOnEmpty(row["Tên học phần (Tiếng Anh)"].ToString()),
                            credits = (int)ToNullableInt(row["TC"].ToString()),
                            theoretical_hours = ToNullableInt(row["LT"].ToString()),
                            practice_hours = ToNullableInt(row["TH"].ToString()),
                            internship_hours = ToNullableInt(row["TT"].ToString()),
                            project_hours = ToNullableInt(row["DA"].ToString()),
                            compulsory_or_optional = SetNullOnEmpty(row["Bắt buộc/ Tự chọn"].ToString()),
                            prerequisites = SetNullOnEmpty(row["Điều kiện tiên quyết"].ToString()),
                            learn_before = SetNullOnEmpty(row["Học trước – học sau"].ToString()),
                            editing_notes = SetNullOnEmpty(row["Ghi chú chỉnh sửa"].ToString()),
                            knowledge_type_id = SetNullOnEmpty(row["Mã loại kiến thức"].ToString())
                        });
                    }

                    db.studentcourse_curriculum.Add(new studentcourse_curriculum
                    {
                        student_course_id = int.Parse(postedStudentCourse),
                        curriculum_id = SetNullOnEmpty(row["Mã học phần"].ToString())
                    });
                }
                db.SaveChanges();
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