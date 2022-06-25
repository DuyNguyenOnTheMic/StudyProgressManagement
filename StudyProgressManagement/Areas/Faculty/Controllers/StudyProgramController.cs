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
            var postedStudentCourse = Request.Form["student_course"].ToString();
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
                        var query_knowledge_type = db.knowledge_type.Where(k => k.id == knowledge_type).FirstOrDefault();
                        if (query_knowledge_type == null)
                        {
                            db.knowledge_type.Add(new knowledge_type
                            {
                                id = row["Mã loại kiến thức"].ToString().Trim(),
                                name = row["Tên loại kiến thức"].ToString().Trim()
                            });
                        }
                        db.SaveChanges();

                        string curriculum = row["Mã học phần"].ToString();
                        var query_curriculum = db.curricula.Where(c => c.id == curriculum).FirstOrDefault();
                        if (query_curriculum == null)
                        {
                            db.curricula.Add(new curriculum
                            {
                                id = row["Mã học phần"].ToString().Trim(),
                                name = row["Tên học phần (Tiếng Việt)"].ToString().Trim(),
                                name_english = row["Tên học phần (Tiếng Anh)"].ToString().Trim(),
                                credits = string.IsNullOrEmpty(row["TC"].ToString()) ? 0 : int.Parse(row["TC"].ToString().Trim()),
                                theoretical_hours = string.IsNullOrEmpty(row["LT"].ToString()) ? 0 : int.Parse(row["TC"].ToString().Trim()),
                                practice_hours = string.IsNullOrEmpty(row["TH"].ToString()) ? 0 : int.Parse(row["TC"].ToString().Trim()),
                                internship_hours = string.IsNullOrEmpty(row["TT"].ToString()) ? 0 : int.Parse(row["TC"].ToString().Trim()),
                                project_hours = string.IsNullOrEmpty(row["DA"].ToString()) ? 0 : int.Parse(row["TC"].ToString().Trim()),
                                compulsory_or_optional = row["Bắt buộc/ Tự chọn"].ToString().Trim(),
                                prerequisites = row["Điều kiện tiên quyết"].ToString().Trim(),
                                learn_before = row["Học trước – học sau"].ToString().Trim(),
                                editing_notes = row["Ghi chú chỉnh sửa"].ToString().Trim(),
                                knowledge_type_id = row["Mã loại kiến thức"].ToString().Trim(),
                            });
                        }                      
                    }
                    db.SaveChanges();
                }
            }
            ViewBag.majors = db.majors.ToList();
            return View();
        }
    }
}