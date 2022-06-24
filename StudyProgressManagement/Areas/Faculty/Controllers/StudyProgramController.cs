using StudyProgressManagement.Models;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
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
            foreach (var imageFile in Request.Files)
            {
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
                        var query = db.knowledge_type.Where(k => k.id == knowledge_type).FirstOrDefault();
                        if (query == null)
                        {
                            db.knowledge_type.Add(new knowledge_type
                            {
                                id = row["Mã loại kiến thức"].ToString(),
                                name = row["Tên loại kiến thức"].ToString()
                            });
                        }
                        db.curricula.Add(new curriculum
                        {
                            no = int.Parse(row["STT"].ToString()),
                            id = row["Mã học phần"].ToString(),
                            name = row["Tên học phần (Tiếng Việt)"].ToString(),
                            name_english = row["Tên học phần (Tiếng Anh)"].ToString(),
                            credits = int.Parse(row["TC"].ToString()),
                            theoretical_hours = int.Parse(row["LT"].ToString()),
                            practice_hours = int.Parse(row["TH"].ToString()),
                            internship_hours = int.Parse(row["TT"].ToString()),
                            project_hours = int.Parse(row["DA"].ToString()),
                            compulsory_or_optional = row["Bắt buộc/ Tự chọn"].ToString(),
                            prerequisites = row["Điều kiện tiên quyết"].ToString(),
                            learn_before = row["Học trước – học sau"].ToString(),
                            editing_notes = row["Ghi chú chỉnh sửa"].ToString(),
                            knowledge_type_id = row["Mã loại kiến thức"].ToString(),
                            student_course_id = int.Parse(postedStudentCourse)
                        });
                    }
                    db.SaveChanges();
                }
            }
            return View();
        }
    }
}