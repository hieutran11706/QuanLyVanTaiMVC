using QuanLyVanTaiMVC.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace QuanLyVanTaiMVC.Data
{
    public class DashboardRepository
    {
        private int Count(string sql)
        {
            var t = OracleDb.Query(sql);
            return Convert.ToInt32(t.Rows[0][0]);
        }

        public DashboardViewModel GetDashboard()
        {
            var vm = new DashboardViewModel();
            using (var c = OracleDb.CreateConnection())
            {
                c.Open();
                vm.DatabaseStatus = "Đã kết nối Oracle";
            }

            vm.Customers = Count("SELECT COUNT(*) FROM KHACHHANG");
            vm.Products = Count("SELECT COUNT(*) FROM HANGHOA");
            vm.Warehouses = Count("SELECT COUNT(*) FROM KHOHANG");
            vm.Vehicles = Count("SELECT COUNT(*) FROM XE");
            vm.Drivers = Count("SELECT COUNT(*) FROM TAIXE");
            vm.Trips = Count("SELECT COUNT(*) FROM CHUYENXE");
            vm.Orders = Count("SELECT COUNT(*) FROM DONHANG");
            vm.NewOrders = Count("SELECT COUNT(*) FROM DONHANG WHERE TRANG_THAI = 'MOI_TAO'");
            vm.ProcessingOrders = Count("SELECT COUNT(*) FROM DONHANG WHERE TRANG_THAI = 'DANG_XU_LY'");
            vm.ShippingOrders = Count("SELECT COUNT(*) FROM DONHANG WHERE TRANG_THAI = 'DANG_VAN_CHUYEN'");
            vm.DeliveredOrders = Count("SELECT COUNT(*) FROM DONHANG WHERE TRANG_THAI = 'DA_GIAO'");
            var revenue = OracleDb.Query("SELECT NVL(SUM(TONG_TIEN),0) FROM DONHANG WHERE TRANG_THAI = 'DA_GIAO'");
            vm.Revenue = Convert.ToDecimal(revenue.Rows[0][0]);
            return vm;
        }

        public string TestConnection()
        {
            using (var c = OracleDb.CreateConnection())
            {
                c.Open();
                using (var cmd = new OracleCommand("SELECT SYS_CONTEXT('USERENV','CON_NAME') FROM DUAL", c))
                    return Convert.ToString(cmd.ExecuteScalar());
            }
        }
    }
}
