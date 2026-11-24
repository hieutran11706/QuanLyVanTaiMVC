using QuanLyVanTaiMVC.Data;
using System;
using System.Web.Mvc;

namespace QuanLyVanTaiMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderRepository repository = new OrderRepository();

        public ActionResult Index(string keyword, string status)
        {
            ViewBag.Keyword = keyword;
            ViewBag.Status = status;
            try { return View(repository.GetOrders(keyword, status)); }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new System.Collections.Generic.List<Models.OrderListItem>());
            }
        }

        public ActionResult Tracking(decimal id)
        {
            try
            {
                var order = repository.GetTracking(id);
                if (order.Rows.Count == 0) return HttpNotFound("Không tìm thấy đơn hàng.");
                ViewBag.Order = order;
                ViewBag.History = repository.GetHistory(id);
                return View();
            }
            catch (Exception ex) { return Content("Không thể tải dữ liệu: " + Server.HtmlEncode(ex.Message)); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(decimal maDh, string trangThai, string ghiChu)
        {
            string[] allowed = { "MOI_TAO", "DANG_XU_LY", "DANG_VAN_CHUYEN", "DA_GIAO", "DA_HUY" };
            if (Array.IndexOf(allowed, trangThai) < 0)
            {
                TempData["Error"] = "Trạng thái không hợp lệ.";
                return RedirectToAction("Tracking", new { id = maDh });
            }
            try
            {
                repository.UpdateStatus(maDh, trangThai, ghiChu);
                TempData["Success"] = "Đã cập nhật trạng thái và ghi lịch sử.";
            }
            catch (Exception ex) { TempData["Error"] = ex.Message; }
            return RedirectToAction("Tracking", new { id = maDh });
        }
    }
}
