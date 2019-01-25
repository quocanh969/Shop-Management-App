using _1612018_FinalProject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _1612018_FinalProject
{
    /// <summary>
    /// Interaction logic for Doanh_thu.xaml
    /// </summary>
    public partial class Doanh_thu : UserControl
    {
        // Biến phần chọn thời điểm
        List<Type> lstType = new List<Type>();
        List<Thang> lstThang;
        List<Nam> lstNam;
        List<Quy> lstQuy;

        private DateTime ngayTK { get; set; }

        // Biến phần chọn thời gian
        private DateTime ngayBDTK { get; set; }
        private DateTime ngayKTTK { get; set; }

        // Trang thái
        private bool ThongKeTheoThoiDiem = true;

        // Danh sach sản phâm
        List<DoanhThuSanPham> lstDoanhThu = new List<DoanhThuSanPham>();
        List<DoanhThuSanPham> lstDoanhXuat = new List<DoanhThuSanPham>();
        List<KeyValuePair<DateTime, Double>> lstThuNgay = new List<KeyValuePair<DateTime, Double>>();
        List<KeyValuePair<DateTime, Double>> lstXuatNgay = new List<KeyValuePair<DateTime, Double>>();

        public Doanh_thu()
        {
            InitializeComponent();
            initType();           

            // Khởi tạo ngày hom nay
            ngayTK = DateTime.Today;
            ngayBDTK = DateTime.Today;
            ngayKTTK = DateTime.Today;

            DayCalendar.SelectedDate = DateTime.Today;
            StartCalendar.SelectedDate = DateTime.Today;
            EndCalendar.SelectedDate = DateTime.Today;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayBDTK.Day);
            dayPick.Append("/");
            dayPick.Append(ngayBDTK.Month);
            dayPick.Append("/");
            dayPick.Append(ngayBDTK.Year);

            dayStatistic.Text = dayPick.ToString();
            start.Text = dayPick.ToString();
            end.Text = dayPick.ToString();
        }

        private void ngayBDThongKe_Closed(object sender, RoutedEventArgs e)
        {// Chọn ngày thành công
            ngayBDTK = StartCalendar.SelectedDate.Value;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayBDTK.Day);
            dayPick.Append("/");
            dayPick.Append(ngayBDTK.Month);
            dayPick.Append("/");
            dayPick.Append(ngayBDTK.Year);

            start.Text = dayPick.ToString();
        }

        private void ngayKTThongKe_Closed(object sender, RoutedEventArgs e)
        {// Chọn ngày thành công
            ngayKTTK = EndCalendar.SelectedDate.Value;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayKTTK.Day);
            dayPick.Append("/");
            dayPick.Append(ngayKTTK.Month);
            dayPick.Append("/");
            dayPick.Append(ngayKTTK.Year);

            end.Text = dayPick.ToString();
        }

        // khởi tạo combo box lựa chọn
        private void initType()
        {
            lstType.Add(new Type() { luachon = "Ngày" });
            lstType.Add(new Type() { luachon = "Tháng" });
            lstType.Add(new Type() { luachon = "Năm" });
            lstType.Add(new Type() { luachon = "Quý" });

            // Cập nhật combo box
            cmbType.ItemsSource = lstType;
            cmbType.SelectedIndex = 0;
        }

        // khởi tạo combo box thang
        private void initThang()
        {
            lstThang = new List<Thang>();
            for (int i = 1; i <= 12; i++)
            {
                Thang temp = new Thang() { value = i };
                lstThang.Add(temp);
            }
        }

        // khởi tạo combo box Nam
        private void initNam()
        {
            lstNam = new List<Nam>();
            for (int i = 2000; i <= 2030; i++)
            {
                Nam temp = new Nam() { value = i };
                lstNam.Add(temp);
            }
        }

        // khởi tạo combo box Quý
        private void initQuy()
        {
            lstQuy = new List<Quy>();
            for (int i = 1; i <= 4; i++)
            {
                Quy temp = new Quy() { value = $"Quý {i}" };
                lstQuy.Add(temp);
            }
        }

        // --- Lớp ---
        class Type
        {
            public String luachon { get; set; }
        }
        class Thang
        {
            public int value { get; set; }
        }
        class Nam
        {
            public int value { get; set; }
        }
        class Quy
        {
            public String value { get; set; }
        }

        // Xử lý khi mà chọn lựa chọn xong
        private void cmbType_LostFocus(object sender, RoutedEventArgs e)
        {
            if (cmbType.SelectedIndex == 0)
            {
                chonNgay.IsEnabled = true;
                cmbTypeValue.IsEnabled = false;
            }
            else
            {
                chonNgay.IsEnabled = false;
                cmbTypeValue.IsEnabled = true;
                if (cmbType.SelectedIndex == 1)
                {// chọn tháng
                    initThang();
                    cmbTypeValue.ItemsSource = lstThang;
                    cmbTypeValue.SelectedIndex = 0;
                }
                else if (cmbType.SelectedIndex == 2)
                {// chọn năm
                    initNam();
                    cmbTypeValue.ItemsSource = lstNam;
                    cmbTypeValue.SelectedIndex = 0;
                }
                else
                {// Chọn quý
                    initQuy();
                    cmbTypeValue.ItemsSource = lstQuy;
                    cmbTypeValue.SelectedIndex = 0;
                }
            }
        }

        private void ngayThongKe_Closed(object sender, RoutedEventArgs e)
        {
            // Chọn ngày thành công
            ngayTK = DayCalendar.SelectedDate.Value;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayTK.Day);
            dayPick.Append("/");
            dayPick.Append(ngayTK.Month);
            dayPick.Append("/");
            dayPick.Append(ngayTK.Year);

            dayStatistic.Text = dayPick.ToString();
        }

        private void btnThongKe_Click(object sender, RoutedEventArgs e)
        {
            if (ThongKeTheoThoiDiem == true)
            {// Thời điểm
                // refresh
                lstDoanhThu.Clear();
                lstDoanhXuat.Clear();

                if (cmbType.SelectedIndex == 0) // Thống kê theo ngày
                {
                    // ---------------------------------------------
                    Bieu_do_thu.Title = $"Biểu đồ thống kế doanh thu sản phẩm trong ngày {dayStatistic.Text}";

                    // Chỉ cập nhật những sản phẩm có bán được trong ngày hôm đó thôi, để tránh gây rối

                    // Bán hàng trực tiếp
                    // Lưu tên các sản phẩm
                    var lstTemp = Data_handle.Data_interface.database.HOADONs.Where(x => x.NgayLapHD == ngayTK)
                        .GroupBy(x => x.MaSP).ToList();
                    if (lstTemp.Count() > 0)
                    {
                        foreach (var c in lstTemp)
                        {
                            string _TenSP = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == c.Key).Select(x => x.TenSP).SingleOrDefault();
                            lstDoanhThu.Add(new DoanhThuSanPham() { tenSP = _TenSP, maSP = c.Key, doanhThu = 0 });
                        }

                        // Cập nhật số lượng bán được trong một ngày ở cả "BanHang"
                        foreach (var t in lstDoanhThu)
                        {
                            double tienThu = 0;
                            tienThu = Data_handle.Data_interface.database.HOADONs.Where(x => x.MaSP == t.maSP && x.NgayLapHD == ngayTK)
                                    .Sum(x => x.TienThu).Value;
                            t.doanhThu = tienThu;
                        }
                    }

                    // ---------------------------------------------
                    var lstTemp_1 = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.NgayNhap == ngayTK)
                        .GroupBy(x=>x.MaSP).ToList();
                    if (lstTemp_1.Count() > 0)
                    {
                        foreach (var c in lstTemp_1)
                        {
                            var _SP = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == c.Key).SingleOrDefault();                            
                            lstDoanhXuat.Add(new DoanhThuSanPham() { tenSP = _SP.TenSP, maSP = c.Key, maLoai = _SP.MaLoai, soLuong = 0, doanhThu = 0 });
                        }

                        // Cập nhật hàng hoa nhập
                        foreach (var t in lstDoanhXuat)
                        {
                            double tienTra = 0;
                            int soluong = 0;
                            tienTra = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.MaSP == t.maSP && x.NgayNhap == ngayTK)
                                    .Sum(x => x.TienTra).Value;
                            soluong = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.MaSP == t.maSP && x.NgayNhap == ngayTK)
                                    .Sum(x => x.SoLuong).Value;
                            t.soLuong = soluong;
                            t.doanhThu = tienTra;
                        }
                    }
                }
                else if (cmbType.SelectedIndex == 1) // Thống kê theo tháng
                {
                    // ---------------------------------------------
                    Bieu_do_thu.Title = $"Biểu đồ thống kế doanh thu sản phẩm trong tháng {cmbTypeValue.SelectedIndex + 1}";

                    // Chỉ cập nhật những sản phẩm có bán được trong ngày hôm đó thôi, để tránh gây rối

                    // Bán hàng trực tiếp
                    // Lưu tên các sản phẩm
                    var lstTemp = Data_handle.Data_interface.database.HOADONs.Where(x => x.NgayLapHD.Value.Month == cmbTypeValue.SelectedIndex + 1)
                        .GroupBy(x => x.MaSP).ToList();
                    if (lstTemp.Count() > 0)
                    {
                        foreach (var c in lstTemp)
                        {
                            string _TenSP = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == c.Key).Select(x => x.TenSP).SingleOrDefault();
                            lstDoanhThu.Add(new DoanhThuSanPham() { tenSP = _TenSP, maSP = c.Key, doanhThu = 0 });
                        }

                        // Cập nhật số lượng bán được trong một ngày ở cả "BanHang"
                        foreach (var t in lstDoanhThu)
                        {
                            double tienThu = 0;
                            tienThu = Data_handle.Data_interface.database.HOADONs.Where(x => x.MaSP == t.maSP && x.NgayLapHD.Value.Month == cmbTypeValue.SelectedIndex + 1)
                                    .Sum(x => x.TienThu).Value;
                            t.doanhThu = tienThu;
                        }
                    }

                    // ---------------------------------------------
                    var lstTemp_1 = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.NgayNhap.Value.Month == cmbTypeValue.SelectedIndex + 1)
                        .GroupBy(x => x.MaSP).ToList();
                    if (lstTemp_1.Count() > 0)
                    {
                        foreach (var c in lstTemp_1)
                        {
                            var _SP = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == c.Key).SingleOrDefault();
                            lstDoanhXuat.Add(new DoanhThuSanPham() { tenSP = _SP.TenSP, maSP = c.Key, maLoai = _SP.MaLoai, soLuong = 0, doanhThu = 0 });
                        }

                        // Cập nhật hàng hoa nhập
                        foreach (var t in lstDoanhXuat)
                        {
                            double tienTra = 0;
                            int soluong = 0;
                            tienTra = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.MaSP == t.maSP && x.NgayNhap.Value.Month == cmbTypeValue.SelectedIndex + 1)
                                    .Sum(x => x.TienTra).Value;
                            soluong = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.MaSP == t.maSP && x.NgayNhap.Value.Month == cmbTypeValue.SelectedIndex + 1)
                                    .Sum(x => x.SoLuong).Value;
                            t.soLuong = soluong;
                            t.doanhThu = tienTra;
                        }
                    }
                }
                else if (cmbType.SelectedIndex == 2) // Thống kê theo năm
                {
                    // ---------------------------------------------
                    Bieu_do_thu.Title = $"Biểu đồ thống kế doanh thu sản phẩm trong năm {cmbTypeValue.SelectedIndex + 2000}";

                    // Chỉ cập nhật những sản phẩm có bán được trong ngày hôm đó thôi, để tránh gây rối

                    // Bán hàng trực tiếp
                    // Lưu tên các sản phẩm
                    var lstTemp = Data_handle.Data_interface.database.HOADONs.Where(x => x.NgayLapHD.Value.Year == cmbTypeValue.SelectedIndex + 2000)
                        .GroupBy(x => x.MaSP).ToList();
                    if (lstTemp.Count() > 0)
                    {
                        foreach (var c in lstTemp)
                        {
                            string _TenSP = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == c.Key).Select(x => x.TenSP).SingleOrDefault();
                            lstDoanhThu.Add(new DoanhThuSanPham() { tenSP = _TenSP, maSP = c.Key, doanhThu = 0 });
                        }

                        // Cập nhật số lượng bán được trong một ngày ở cả "BanHang"
                        foreach (var t in lstDoanhThu)
                        {
                            double tienThu = 0;
                            tienThu = Data_handle.Data_interface.database.HOADONs.Where(x => x.MaSP == t.maSP && x.NgayLapHD.Value.Year == cmbTypeValue.SelectedIndex + 2000)
                                    .Sum(x => x.TienThu).Value;
                            t.doanhThu = tienThu;
                        }
                    }

                    // ---------------------------------------------
                    var lstTemp_1 = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.NgayNhap.Value.Year == cmbTypeValue.SelectedIndex + 2000)
                        .GroupBy(x => x.MaSP).ToList();
                    if (lstTemp_1.Count() > 0)
                    {
                        foreach (var c in lstTemp_1)
                        {
                            var _SP = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == c.Key).SingleOrDefault();
                            lstDoanhXuat.Add(new DoanhThuSanPham() { tenSP = _SP.TenSP, maSP = c.Key, maLoai = _SP.MaLoai, soLuong = 0, doanhThu = 0 });
                        }

                        // Cập nhật hàng hoa nhập
                        foreach (var t in lstDoanhXuat)
                        {
                            double tienTra = 0;
                            int soluong = 0;
                            tienTra = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.MaSP == t.maSP && x.NgayNhap.Value.Year == cmbTypeValue.SelectedIndex + 2000)
                                    .Sum(x => x.TienTra).Value;
                            soluong = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.MaSP == t.maSP && x.NgayNhap.Value.Year == cmbTypeValue.SelectedIndex + 2000)
                                    .Sum(x => x.SoLuong).Value;
                            t.soLuong = soluong;
                            t.doanhThu = tienTra;
                        }
                    }
                }
                else // Thống kê theo quý
                {

                    // ---------------------------------------------
                    Bieu_do_thu.Title = $"Biểu đồ thống kế doanh thu sản phẩm trong quý {cmbTypeValue.SelectedIndex + 1}";

                    // Chỉ cập nhật những sản phẩm có bán được trong ngày hôm đó thôi, để tránh gây rối

                    // Bán hàng trực tiếp
                    // Lưu tên các sản phẩm
                    var lstTemp = Data_handle.Data_interface.database.HOADONs.Where(x => (x.NgayLapHD.Value.Month - 1) / 4 == cmbTypeValue.SelectedIndex)
                        .GroupBy(x => x.MaSP).ToList();
                    if (lstTemp.Count() > 0)
                    {
                        foreach (var c in lstTemp)
                        {
                            string _TenSP = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == c.Key).Select(x => x.TenSP).SingleOrDefault();
                            lstDoanhThu.Add(new DoanhThuSanPham() { tenSP = _TenSP, maSP = c.Key, doanhThu = 0 });
                        }

                        // Cập nhật số lượng bán được trong một ngày ở cả "BanHang"
                        foreach (var t in lstDoanhThu)
                        {
                            double tienThu = 0;
                            tienThu = Data_handle.Data_interface.database.HOADONs.Where(x => x.MaSP == t.maSP && (x.NgayLapHD.Value.Month - 1 )/ 4 == cmbTypeValue.SelectedIndex)
                                    .Sum(x => x.TienThu).Value;
                            t.doanhThu = tienThu;
                        }
                    }

                    // ---------------------------------------------
                    var lstTemp_1 = Data_handle.Data_interface.database.NHAPHANGs.Where(x => (x.NgayNhap.Value.Month - 1) / 4 == cmbTypeValue.SelectedIndex)
                        .GroupBy(x => x.MaSP).ToList();
                    if (lstTemp_1.Count() > 0)
                    {
                        foreach (var c in lstTemp_1)
                        {
                            var _SP = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == c.Key).SingleOrDefault();
                            lstDoanhXuat.Add(new DoanhThuSanPham() { tenSP = _SP.TenSP, maSP = c.Key, maLoai = _SP.MaLoai, soLuong = 0, doanhThu = 0 });
                        }

                        // Cập nhật hàng hoa nhập
                        foreach (var t in lstDoanhXuat)
                        {
                            double tienTra = 0;
                            int soluong = 0;
                            tienTra = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.MaSP == t.maSP && (x.NgayNhap.Value.Month - 1) / 4 == cmbTypeValue.SelectedIndex)
                                    .Sum(x => x.TienTra).Value;
                            soluong = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.MaSP == t.maSP && (x.NgayNhap.Value.Month - 1) / 4 == cmbTypeValue.SelectedIndex)
                                    .Sum(x => x.SoLuong).Value;
                            t.soLuong = soluong;
                            t.doanhThu = tienTra;
                        }
                    }
                }

                // Vẽ biểu đồ
                Bieu_do_tron_thu.ItemsSource = lstDoanhThu;
                Bieu_do_tron_thu.Refresh();
                // Datagrid
                dataGridImport.ItemsSource = lstDoanhXuat;
                dataGridImport.Items.Refresh();

                // Tổng thu
                Double tongThu = 0;
                foreach(var t in lstDoanhThu)
                {
                    tongThu += t.doanhThu;
                }
                txtBlckTongThu.Text = tongThu.ToString();

                // Tổng xuất
                Double tongXuat = 0;
                foreach(var t in lstDoanhXuat)
                {
                    tongXuat += t.doanhThu;
                }
                txtBlckTongXuat.Text = tongXuat.ToString();                
            }
            else
            {// Khoảng thời gian
                // Kiểm tra khoảng thời gian nhập
                if(ngayBDTK > ngayKTTK)
                {
                    MessageBox.Show("Khoảng thời gian nhập vào không hợp Lý", "Thông báo");
                    return;
                }
                else
                {
                    double tienThu = 0, tienXuat = 0;                    

                    for (var day = ngayBDTK; day <= ngayKTTK; day = day.AddDays(1))
                    {
                        // Thu
                        if (Data_handle.Data_interface.database.HOADONs.Where(x => x.NgayLapHD == day).ToList().Count() == 0)
                        {
                            tienThu = 0;
                        }
                        else
                        {
                            tienThu = Data_handle.Data_interface.database.HOADONs.Where(x => x.NgayLapHD == day)
                                .Sum(x => x.TienThu).Value;
                        }
                        lstThuNgay.Add(new KeyValuePair<DateTime, Double>(day, tienThu));

                        // Xuất
                        if (Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.NgayNhap == day).ToList().Count() == 0)
                        {
                            tienXuat = 0;
                        }
                        else
                        {
                            tienXuat = Data_handle.Data_interface.database.NHAPHANGs.Where(x => x.NgayNhap == day)
                                .Sum(x => x.TienTra).Value;
                        }
                        lstXuatNgay.Add(new KeyValuePair<DateTime, Double>(day, tienXuat));
                    }                    

                    Bieu_do_duong_doanh_thu.ItemsSource = lstThuNgay.ToArray();
                    Bieu_do_duong_doanh_thu.Refresh();
                    Bieu_do_duong_doanh_xuat.ItemsSource = lstXuatNgay.ToArray();
                    Bieu_do_duong_doanh_xuat.Refresh();
                }
            }
        }

        private void Thoi_diem_Checked(object sender, RoutedEventArgs e)
        {// Khi tick chọn thống kê theo thời điểm
            ThongKeTheoThoiDiem = true;
            Thong_ke_thoi_diem.Visibility = Visibility.Visible;
            Thong_ke_thoi_gian.Visibility = Visibility.Hidden;
        }

        private void Thoi_gian_Checked(object sender, RoutedEventArgs e)
        {// Khi tick chọn thống kê theo khoảng thời gian
            ThongKeTheoThoiDiem = false;
            Thong_ke_thoi_diem.Visibility = Visibility.Hidden;
            Thong_ke_thoi_gian.Visibility = Visibility.Visible;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Bieu_do_tron_thu.Refresh();
            Bieu_do_duong_doanh_thu.Refresh();
            Bieu_do_duong_doanh_xuat.Refresh();
        }
    }

    class DoanhThuSanPham
    {
        public string tenSP { get; set; }
        public string maSP { get; set; }
        public string maLoai { get; set; }
        public int soLuong { get; set; }
        public double doanhThu { get; set; }
    }
}
