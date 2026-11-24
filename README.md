# QuanLyVanTaiMVC - ASP.NET MVC 5 / .NET Framework 4.8

Đây là bộ khung giai đoạn 1 cho đề tài **Hệ thống quản lý vận tải hàng hóa liên tỉnh**.
CSDL mục tiêu: Oracle Database 23ai, schema `QL_VANTAI`.

## Mở dự án
1. Giải nén ZIP.
2. Mở `QuanLyVanTaiMVC.sln` bằng Visual Studio 2022.
3. Khôi phục NuGet packages (Restore NuGet Packages).
4. Mở `Web.config`, sửa `OracleDb` theo tài khoản Oracle của bạn.
5. Kiểm tra Oracle container đang chạy và service `FREEPDB1` truy cập được.
6. Nhấn F5.

## Chuỗi kết nối
Mẫu đang để:
`User Id=QL_VANTAI;Password=THAY_MAT_KHAU;Data Source=localhost:1521/FREEPDB1;`

Không đưa mật khẩu thật lên GitHub hoặc gửi công khai.

## Phạm vi hiện có
- Giao diện quản trị responsive.
- Dashboard đọc số liệu thật từ 12 bảng.
- Kiểm tra kết nối Oracle.
- Trang danh sách đơn hàng có JOIN khách hàng/chuyến/tuyến.
- Trang theo dõi trạng thái và lịch sử đơn hàng.
- Repository dùng bind parameters.
- Có transaction khi gọi procedure cập nhật trạng thái.

## Lưu ý quan trọng
- Chưa thể xác nhận build/chạy với Oracle trên máy bạn vì cần môi trường Oracle và Visual Studio cục bộ.
- Script SQL của nhóm có một thiếu sót dữ liệu danh mục: một số tỉnh như `Da Nang`, `Can Tho`, `Hai Phong` được dùng ở phần tạo kho/tuyến nhưng chưa được thêm vào `TINHTHANH`. Hãy chạy phần bổ sung tỉnh ở `Database/01_BoSungTinhThanh.sql` trước khi tạo kho/tuyến mẫu.
- Chạy script tạo bảng/procedure/index hiện tại của nhóm trước. Không chạy lại `CREATE TABLE` trên schema đã có bảng.
- Website không dùng tài khoản `SYS`; đăng nhập bằng schema nghiệp vụ `QL_VANTAI`.
