using System;

namespace QuanLyVanTaiMVC.Models
{
    public class OrderListItem
    {
        public decimal MaDh { get; set; }
        public string TenKh { get; set; }
        public DateTime NgayDat { get; set; }
        public string TrangThai { get; set; }
        public decimal TongTien { get; set; }
        public string Tuyen { get; set; }
        public string BienSo { get; set; }
    }
}
