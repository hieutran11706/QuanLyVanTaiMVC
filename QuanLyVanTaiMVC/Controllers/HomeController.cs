using QuanLyVanTaiMVC.Data;
using System;
using System.Web.Mvc;

namespace QuanLyVanTaiMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly DashboardRepository repository = new DashboardRepository();

        public ActionResult Index()
        {
            try { return View(repository.GetDashboard()); }
            catch (Exception ex)
            {
                ViewBag.DbError = ex.Message;
                return View("Index", new Models.DashboardViewModel { DatabaseStatus = "Chưa kết nối được Oracle" });
            }
        }

        public ActionResult Connection()
        {
            try
            {
                ViewBag.Status = "Thành công";
                ViewBag.Pdb = repository.TestConnection();
            }
            catch (Exception ex)
            {
                ViewBag.Status = "Thất bại";
                ViewBag.Error = ex.Message;
            }
            return View();
        }
    }
}
