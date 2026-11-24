namespace QuanLyVanTaiMVC.Models
{
    public class DashboardViewModel
    {
        public int Customers { get; set; }
        public int Products { get; set; }
        public int Warehouses { get; set; }
        public int Vehicles { get; set; }
        public int Drivers { get; set; }
        public int Trips { get; set; }
        public int Orders { get; set; }
        public int NewOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int ShippingOrders { get; set; }
        public int DeliveredOrders { get; set; }
        public decimal Revenue { get; set; }
        public string DatabaseStatus { get; set; }
    }
}
