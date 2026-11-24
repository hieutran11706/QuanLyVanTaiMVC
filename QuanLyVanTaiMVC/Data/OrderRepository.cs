using Oracle.ManagedDataAccess.Client;
using QuanLyVanTaiMVC.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace QuanLyVanTaiMVC.Data
{
    public class OrderRepository
    {
        public List<OrderListItem> GetOrders(string keyword, string status)
        {
            const string sql = @"
SELECT d.MA_DH, k.TEN_KH, d.NGAY_DAT, d.TRANG_THAI, d.TONG_TIEN,
       a.TEN_TINH || ' → ' || b.TEN_TINH AS TUYEN,
       x.BIEN_SO
FROM DONHANG d
JOIN KHACHHANG k ON k.MA_KH = d.MA_KH
LEFT JOIN CHUYENXE c ON c.MA_CHUYEN = d.MA_CHUYEN
LEFT JOIN TUYENDUONG td ON td.MA_TUYEN = c.MA_TUYEN
LEFT JOIN TINHTHANH a ON a.MA_TINH = td.MA_TINH_DI
LEFT JOIN TINHTHANH b ON b.MA_TINH = td.MA_TINH_DEN
LEFT JOIN XE x ON x.MA_XE = c.MA_XE
WHERE (:p_status IS NULL OR d.TRANG_THAI = :p_status)
  AND (:p_keyword IS NULL OR TO_CHAR(d.MA_DH) LIKE '%' || :p_keyword || '%'
       OR UPPER(k.TEN_KH) LIKE '%' || UPPER(:p_keyword) || '%')
ORDER BY d.NGAY_DAT DESC";
            var table = OracleDb.Query(sql,
                new OracleParameter("p_status", (object)status ?? DBNull.Value),
                new OracleParameter("p_keyword", (object)keyword ?? DBNull.Value));
            var list = new List<OrderListItem>();
            foreach (DataRow r in table.Rows)
            {
                list.Add(new OrderListItem {
                    MaDh = Convert.ToDecimal(r["MA_DH"]),
                    TenKh = Convert.ToString(r["TEN_KH"]),
                    NgayDat = Convert.ToDateTime(r["NGAY_DAT"]),
                    TrangThai = Convert.ToString(r["TRANG_THAI"]),
                    TongTien = Convert.ToDecimal(r["TONG_TIEN"]),
                    Tuyen = r["TUYEN"] == DBNull.Value ? "Chưa phân tuyến" : Convert.ToString(r["TUYEN"]),
                    BienSo = r["BIEN_SO"] == DBNull.Value ? "Chưa phân xe" : Convert.ToString(r["BIEN_SO"])
                });
            }
            return list;
        }

        public DataTable GetTracking(decimal maDh)
        {
            return OracleDb.Query(@"
SELECT d.MA_DH, d.TRANG_THAI, d.NGAY_DAT, d.TONG_TIEN,
       k.TEN_KH, k.SDT, d.THONG_TIN_MO_RONG
FROM DONHANG d JOIN KHACHHANG k ON k.MA_KH = d.MA_KH
WHERE d.MA_DH = :p_id",
                new OracleParameter("p_id", maDh));
        }

        public DataTable GetHistory(decimal maDh)
        {
            return OracleDb.Query(@"
SELECT TRANG_THAI, THOI_DIEM, GHI_CHU
FROM LICHSUTRANGTHAI
WHERE MA_DH = :p_id
ORDER BY THOI_DIEM DESC",
                new OracleParameter("p_id", maDh));
        }

        public void UpdateStatus(decimal maDh, string status, string note)
        {
            using (var connection = OracleDb.CreateConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                using (var command = new OracleCommand("CAP_NHAT_TRANG_THAI", connection))
                {
                    command.Transaction = transaction;
                    command.CommandType = CommandType.StoredProcedure;
                    command.BindByName = true;
                    command.Parameters.Add("P_MA_DH", OracleDbType.Decimal).Value = maDh;
                    command.Parameters.Add("P_TRANG_THAI", OracleDbType.Varchar2, 30).Value = status;
                    command.Parameters.Add("P_GHI_CHU", OracleDbType.Varchar2, 500).Value =
                        string.IsNullOrWhiteSpace(note) ? (object)DBNull.Value : note.Trim();
                    try
                    {
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
