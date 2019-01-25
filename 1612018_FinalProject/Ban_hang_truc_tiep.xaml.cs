using _1612018_FinalProject.model;
using MaterialDesignThemes.Wpf;
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
    /// Interaction logic for Ban_hang_truc_tiep.xaml
    /// </summary>
    public partial class Ban_hang_truc_tiep : UserControl
    {
        private DateTime ngayLapHD { get; set; }
        private HOADON newBill;        

        public Ban_hang_truc_tiep()
        {
            InitializeComponent();
            InitCMBMaLoai();
            initEvent();

            // Tự động tạo ngày
            ngayLapHD = DateTime.Today;
            Calendar.SelectedDate = DateTime.Today;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayLapHD.Day);
            dayPick.Append("/");
            dayPick.Append(ngayLapHD.Month);
            dayPick.Append("/");
            dayPick.Append(ngayLapHD.Year);

            datePicking.Text = dayPick.ToString();
        }

        // Sự kiện sau khi chọn xong trong comboxBoxMaLoai
        private void cmbLoai_LostFocus(object sender, RoutedEventArgs e)
        {// Xử lý sự kiên sau khi chọn xong Loai            
            cmbTenSP.IsEnabled = true;

            // Khởi tạo combo Box Tên Sản phẩm sau khi chọn được loại
            var temp = (LOAI)cmbLoai.SelectedItem;
            InitCMBTenSP(temp.MaLoai);

            // Cập nhật MaLoai
            txtBoxMaLoai.Text = temp.MaLoai;
        }                     

        // Cập nhật ngày làm hoa đơn
        private void PopupBox_Closed(object sender, RoutedEventArgs e)
        {// Chọn ngày thành công
            ngayLapHD = Calendar.SelectedDate.Value;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayLapHD.Day);            
            dayPick.Append("/");
            dayPick.Append(ngayLapHD.Month);
            dayPick.Append("/");
            dayPick.Append(ngayLapHD.Year);

            datePicking.Text = dayPick.ToString();
            
        }

        private void btnNhapHD_Click(object sender, RoutedEventArgs e)
        {// Xử lý button Nhập hóa đơn
            int test;
            ErrorNotice.Visibility = Visibility.Hidden; // khởi tạo lại trang thái notice error

            if(int.TryParse(txtBoxSoLuong.Text,out test) == true)
            {

                newBill = new HOADON()
                {
                    MaSP = txtBoxMaSP.Text,
                    MaLoai = txtBoxMaLoai.Text,
                    SoLuong = test,
                    NgayLapHD = ngayLapHD, 
                    TienThu = 0,//
                    MaEvent = null,//
                    HinhThuc = cmbHinhThucThanhToan.SelectedIndex,
                    GhiChu = txtBoxGhiChu.Text,
                };

                if (checkSoLuong(ref test) == true)
                {
                    double donGia = double.Parse(txtBoxDonGia.Text);
                    double TongTienThu = test * donGia;

                    double luongTienGiam = 0;
                    // Có sự kiện
                    KHUYENMAI selected = (KHUYENMAI)cmbKhuyenMai.SelectedItem;

                    int soLUongCanMua = 0;

                    if (selected != null)
                    {
                        soLUongCanMua = selected.SoLuongCanMua.Value;
                        if (test >= soLUongCanMua)
                        {
                            for (int i = 0; i < test / soLUongCanMua; i++)
                            {
                                luongTienGiam += ((100 - selected.PhanTram.Value) * 1.0 / 100) * soLUongCanMua * donGia;
                            }
                        }
                    }
                    TongTienThu -= luongTienGiam;

                    // Cập nhật giao diện
                    txtBoxTienGiam.Text = luongTienGiam.ToString();
                    txtBoxThanhTien.Text = TongTienThu.ToString();

                    // Cập nhật newBill
                    if (selected != null)
                    {
                        newBill.MaEvent = selected.MaKM;
                    }
                    else
                    {
                        newBill.MaEvent = null;
                    }
                    newBill.TienThu = TongTienThu;

                    // Kết thúc quá trình làm hóa đơn, hiển thị cho người quản lý xem
                    Man_che.Visibility = Visibility.Visible;
                    Nhap_Thanh.Visibility = Visibility.Hidden;
                    lblHeader.Content = "XEM LẠI HÓA ĐƠN";
                }
                else
                {
                   // Do nothing
                }
            }
            else
            {
                ErrorNotice.Content = "*Định dạng số lượng không phù hợp";
                ErrorNotice.Visibility = Visibility.Visible;
            }
            
        }

        // Khởi tạo data comboBoxMaLoai
        private void InitCMBMaLoai()
        {
            cmbLoai.ItemsSource = Data_handle.Data_interface.database.LOAIs.ToList();
            cmbLoai.SelectedIndex = 0;
            txtBoxMaLoai.Text = "COAT";
        }

        // Khởi tạo data comboBoxTenSP
        private void InitCMBTenSP(string maloai)
        {
            // Cập nhật cmbTenSp sau khi chọn được loại
            List<SANPHAM>tempLstSP = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaLoai == maloai && x.TinhTrang == true).ToList();
            if (tempLstSP.Count() > 0)
            {
                cmbTenSP.ItemsSource = tempLstSP;
                cmbTenSP.SelectedIndex = 0;

                txtBoxMaSP.Text = tempLstSP[0].MaSP;
                txtBoxDonGia.Text = tempLstSP[0].DonGia.ToString();
            }
            else
            {
                cmbTenSP.ItemsSource = null;                

                txtBoxMaSP.Text = null;
                txtBoxDonGia.Text = null;
            }
        }

        // Sự kiện sau khi chọn xong trong comboxBoxTenSP
        private void cmbTenSP_LostFocus(object sender, RoutedEventArgs e)
        {
            var temp = (SANPHAM)cmbTenSP.SelectedItem;
            if (temp != null)
            {
                // Cập nhật MaSP
                txtBoxMaSP.Text = temp.MaSP;
                // Cập nhật đơn giá
                txtBoxDonGia.Text = temp.DonGia.ToString();
            }
        }

        private void initEvent()
        {
            
            List<KHUYENMAI> lstEventAvailable = Data_handle.Data_interface.database.KHUYENMAIs.Where(x=>x.TrangThai == true && x.NgayBD <= DateTime.Today).ToList();
            if (lstEventAvailable.Count > 0)
            {
                cmbKhuyenMai.IsEnabled = true;
                cmbKhuyenMai.ItemsSource = lstEventAvailable;
                cmbKhuyenMai.SelectedIndex = 0;
            }
            else
            {
                cmbKhuyenMai.Text = "Không có sự kiện phù hợp";
                cmbKhuyenMai.IsEnabled = false;
            }
        }

        public void refresh()
        {
            cmbLoai.SelectedIndex = 0;
            cmbLoai.Items.Refresh();
            txtBoxMaLoai.Text = "COAT";
            cmbTenSP.ItemsSource = null;
            cmbTenSP.IsEnabled = false;
            txtBoxMaSP.Text = null;            
            txtBoxTienGiam.Text = null;
            txtBoxSoLuong.Text = null;
            txtBoxThanhTien.Text = null;
            txtBoxDonGia.Text = null;
            cmbHinhThucThanhToan.SelectedIndex = 0;
            cmbHinhThucThanhToan.Items.Refresh();
            ngayLapHD = DateTime.Today;
            datePicking.Text = null;
            txtBoxGhiChu.Text = null;
        }

        // Kiểm tra lại hóa đơn bước cuối rồi nhập vào database
        private void btnNhap_vao_data_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalItem.gioHang.Count() > 0)
            {
                ThanhToan();
            }
            else
            {
                MessageBox.Show("Bạn vẫn chưa mua hàng", "Thông báo");
            }
        }

        // hàm thanh toán
        private void ThanhToan()
        {
            try
            {
                foreach (var t in GlobalItem.gioHang)
                {
                    Data_handle.Data_interface.database.HOADONs.Add(t);
                }
                Data_handle.Data_interface.database.SaveChanges();
                refresh();
                GlobalItem.gioHang.Clear();
                updateProduct();
                Man_che.Visibility = Visibility.Hidden;
                Nhap_Thanh.Visibility = Visibility.Visible;
                lblHeader.Content = "HÓA ĐƠN";
                MessageBox.Show("Mua hàng thành công", "Thông báo");
            }
            catch
            {
                MessageBox.Show("Lỗi khi nạp hóa đơn", "Thông báo");
            }
        }

        private void btnQuay_lai_Click(object sender, RoutedEventArgs e)
        {
            Man_che.Visibility = Visibility.Hidden;
            Nhap_Thanh.Visibility = Visibility.Visible;
            lblHeader.Content = "HÓA ĐƠN";
        }

        private void updateProduct()
        {
            try
            {
                foreach (var temp in GlobalItem.gioHang)
                {
                    var t = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == temp.MaSP).SingleOrDefault();
                    t.SoLuong -= temp.SoLuong;
                }

                Data_handle.Data_interface.database.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Lỗi khi cập nhật lại số lượng sản phẩm", "Thông báo");                
            }
        }

        private bool checkSoLuong(ref int soLuong)
        {
            if (soLuong == 0)
            {
                MessageBox.Show("Hết mặt hàng này rồi", "Thống báo");
                return false;
            }
            else
            {
                var t = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == newBill.MaSP && x.TinhTrang == true).SingleOrDefault();
                if (t != null)
                {
                    if (soLuong - t.SoLuong > 0)
                    {
                        MessageBox.Show($"Hiện cửa hàng chỉ còn {t.SoLuong} mặt hàng này thôi", "Thống báo");
                        soLuong = t.SoLuong.Value;
                        return true;
                    }                   
                    
                }
                else
                {
                    MessageBox.Show("Không tồn tại mặt hàng này", "Thông báo");
                    return false;
                }
            }

            return true;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {// Xử l1y button refresh
            refresh();
            GlobalItem.lstMatHang.Clear();
        }

        // button tiếp tục mua hàng
        private void btnTiepTuc_Click(object sender, RoutedEventArgs e)
        {
            GlobalItem.gioHang.Add(newBill);

            // Cập nhật giỏ hàng
            var sp = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.TinhTrang == true && x.MaSP == newBill.MaSP).Select(x => x.TenSP).SingleOrDefault();
            // chắc chắn tìm được
            GlobalItem.lstMatHang.Add(new matHang() { tenSP = sp, maLoai = newBill.MaLoai, soLuong = newBill.SoLuong, thanhTien = newBill.TienThu.Value });

            Man_che.Visibility = Visibility.Hidden;
            Nhap_Thanh.Visibility = Visibility.Visible;
            lblHeader.Content = "HÓA ĐƠN";

            refresh();
        }

        private void btnGioHang_Click(object sender, RoutedEventArgs e)
        {
            Gio_hang gh = new Gio_hang();
            gh.ShowDialog();
        }

        // nút thanh toán
        private void btnThanhToan_Click(object sender, RoutedEventArgs e)
        {
            if(GlobalItem.gioHang.Count() > 0)
            {
                ThanhToan();
            }
            else
            {
                MessageBox.Show("Bạn vẫn chưa mua hàng", "Thông báo");
            }
        }
    }

    class matHang
    {
        public string tenSP { get; set; }
        public string maLoai { get; set; }
        public int soLuong { get; set; }
        public double thanhTien { get; set; }
    }
}
