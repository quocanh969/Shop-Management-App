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
    /// Interaction logic for Dat_hang.xaml
    /// </summary>
    public partial class Dat_hang : UserControl
    {
        private DateTime ngayDat { get; set; }
        private DateTime ngayNhan { get; set; }
        private DATHANG newOrder;

        public Dat_hang()
        {
            InitializeComponent();
            InitCMBMaLoai();
            initEvent();

            // Cập nhật ngày
            ngayDat = DateTime.Today;
            ngayNhan = DateTime.Today;
            OrderCalendar.SelectedDate = DateTime.Today;
            ReceiveCalendar.SelectedDate = DateTime.Today;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayDat.Day);
            dayPick.Append("/");
            dayPick.Append(ngayDat.Month);
            dayPick.Append("/");
            dayPick.Append(ngayDat.Year);

            dateOrder.Text = dayPick.ToString();
            dateReceive.Text = dayPick.ToString();
        }

        private void ngayDatPopUp_Closed(object sender, RoutedEventArgs e)
        {// Cập nhật ngày đặt hàng            
            ngayDat = OrderCalendar.SelectedDate.Value;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayDat.Day);
            dayPick.Append("/");
            dayPick.Append(ngayDat.Month);
            dayPick.Append("/");
            dayPick.Append(ngayDat.Year);

            dateOrder.Text = dayPick.ToString();
        }

        private void ngayNhanPopUp_Closed(object sender, RoutedEventArgs e)
        {// Cập nhật ngày nhận hàng
            ngayNhan = ReceiveCalendar.SelectedDate.Value;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayNhan.Day);
            dayPick.Append("/");
            dayPick.Append(ngayNhan.Month);
            dayPick.Append("/");
            dayPick.Append(ngayNhan.Year);

            dateReceive.Text = dayPick.ToString();
        }

        private void cmbLoai_LostFocus(object sender, RoutedEventArgs e)
        {// Thao tác để sau kiếm tra rằng co chọn được Loai chưa
            //phải chọn được loại thì mới có thể chọn sản phẩm
            cmbTenSP.IsEnabled = true;

            // Khởi tạo combo Box Tên Sản phẩm sau khi chọn được loại
            var temp = (LOAI)cmbLoai.SelectedItem;
            InitCMBTenSP(temp.MaLoai);
        }

        private void btnNhapHD_Click(object sender, RoutedEventArgs e)
        {// Xử lý btn nhập
            int test;
            ErrorNotice.Visibility = Visibility.Hidden; // khởi tạo lại trang thái notice error

            if (int.TryParse(txtBoxSoLuong.Text, out test) == true)
            {

                newOrder = new DATHANG()
                {
                    MaSP = null,
                    MaLoai = null,
                    SoLuong = test,
                    NgayDat = ngayDat,
                    NgayNhan = ngayNhan,
                    TenKH = txtBoxTenKH.Text,//
                    Sdt = txtBoxSdtKH.Text,//
                    TienThu = 0,
                    MaEvent = null,
                    DiaChi = txtBoxDiaChiKH.Text,  //                
                    TraTruoc = 0,
                    HinhThuc = cmbHinhThucThanhToan.SelectedIndex,
                    GhiChu = txtBoxGhiChu.Text,
                };

                // Cập nhật MaLoai
                if (cmbLoai.SelectedItem != null)
                {
                    var t = (LOAI)cmbLoai.SelectedItem;
                    newOrder.MaLoai = t.MaLoai;
                }
                else
                {
                    ErrorNotice.Content = "*Vẫn chưa chọn loại sản phẩm";
                    ErrorNotice.Visibility = Visibility.Visible;
                    return;
                }

                // Cập nhật MaSanPham
                if (cmbTenSP.SelectedItem != null)
                {
                    var t = (SANPHAM)cmbTenSP.SelectedItem;
                    newOrder.MaSP = t.MaSP;
                }
                else
                {
                    ErrorNotice.Content = "*Vẫn chưa chọn sản phẩm";
                    ErrorNotice.Visibility = Visibility.Visible;
                    return;
                }

                // Cập nhật tiền trả trước
                double temp = 0;
                if (double.TryParse(txtBoxTraTruoc.Text, out temp) == true)
                {
                    newOrder.TraTruoc = temp;
                }
                else
                {
                    ErrorNotice.Content = "*Định dạng số tiền trả trước không phù hợp";
                    ErrorNotice.Visibility = Visibility.Visible;
                    return;
                }

                // Kiểm tra và cập nhật thông tin khách hàng
                if (txtBoxTenKH.Text == null || txtBoxSdtKH.Text == null || txtBoxDiaChiKH.Text == null
                    || txtBoxTenKH.Text == "" || txtBoxSdtKH.Text == "" || txtBoxDiaChiKH.Text == "")
                {

                    ErrorNotice.Content = "*Thông tin khách hàng không phù hợp";
                    ErrorNotice.Visibility = Visibility.Visible;
                    return;
                }

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
                        newOrder.MaEvent = selected.MaKM;
                    }
                    else
                    {
                        newOrder.MaEvent = null;
                    }
                    newOrder.TienThu = TongTienThu;

                    // Kết thúc quá trình làm hóa đơn, hiển thị cho người quản lý xem
                    Man_che.Visibility = Visibility.Visible;
                    btnNhapHD.Visibility = Visibility.Hidden;
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
                return;
            }
        }

        private void btnNhap_vao_data_Click(object sender, RoutedEventArgs e)
        {// btn nhập vào data
            try
            {
                Data_handle.Data_interface.database.DATHANGs.Add(newOrder);
                Data_handle.Data_interface.database.SaveChanges();

                refresh();
                updateProduct();
                Man_che.Visibility = Visibility.Hidden;
                btnNhapHD.Visibility = Visibility.Visible;
                lblHeader.Content = "ĐẶT HÀNG";
            }
            catch
            {
                MessageBox.Show("Lỗi khi nạp hóa đơn", "Thông báo");
            }
        }

        private void btnQuay_lai_Click(object sender, RoutedEventArgs e)
        {// btn quay lại
            Man_che.Visibility = Visibility.Hidden;
            btnNhapHD.Visibility = Visibility.Visible;
            lblHeader.Content = "ĐẶT HÀNG";
        }

        // Khởi tạo data comboBoxMaLoai
        private void InitCMBMaLoai()
        {
            cmbLoai.ItemsSource = Data_handle.Data_interface.database.LOAIs.ToList();
            cmbLoai.SelectedIndex = 0;
        }

        // Khởi tạo data comboBoxTenSP
        private void InitCMBTenSP(string maloai)
        {
            // Cập nhật cmbTenSp sau khi chọn được loại
            List<SANPHAM> tempLstSP = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaLoai == maloai && x.TinhTrang == true).ToList();
            if (tempLstSP.Count() > 0)
            {
                cmbTenSP.ItemsSource = tempLstSP;
                cmbTenSP.SelectedIndex = 0;
                txtBoxDonGia.Text = tempLstSP[0].DonGia.ToString();
            }
            else
            {
                cmbTenSP.ItemsSource = null;
                txtBoxDonGia.Text = null;
            }
        }

        private void initEvent()
        {

            List<KHUYENMAI> lstEventAvailable = Data_handle.Data_interface.database.KHUYENMAIs.Where(x => x.TrangThai == true && x.NgayBD <= DateTime.Today).ToList();
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

        // Hàm keim63 tra số lượng
        private bool checkSoLuong(ref int soLuong)
        {
            if (soLuong == 0)
            {
                MessageBox.Show("Hết mặt hàng này rồi", "Thống báo");
                return false;
            }
            else
            {
                var t = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == newOrder.MaSP && x.TinhTrang == true).SingleOrDefault();
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

        // Hàm refresh
        public void refresh()
        {
            cmbLoai.SelectedIndex = 0;
            cmbLoai.Items.Refresh();
            cmbTenSP.ItemsSource = null;
            cmbTenSP.IsEnabled = false;
            txtBoxTienGiam.Text = null;
            txtBoxSoLuong.Text = null;
            txtBoxThanhTien.Text = null;
            txtBoxDonGia.Text = null;
            txtBoxTraTruoc.Text = null;
            txtBoxTenKH.Text = null;
            txtBoxSdtKH.Text = null;
            txtBoxDiaChiKH.Text = null;
            cmbHinhThucThanhToan.SelectedIndex = 0;
            cmbHinhThucThanhToan.Items.Refresh();
            ngayDat = DateTime.Today;
            ngayNhan = DateTime.Today;
            dateOrder.Text = null;
            dateReceive.Text = null;
            txtBoxGhiChu.Text = null;
        }

        // Cập nhật dữ liệu lên product
        private void updateProduct()
        {
            try
            {
                var t = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == newOrder.MaSP).SingleOrDefault();
                t.SoLuong -= newOrder.SoLuong;

                Data_handle.Data_interface.database.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Lỗi khi cập nhật lại số lượng sản phẩm", "Thông báo");
            }
        }

        // Xửu l1y button refresh
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            refresh();
        }
    }
}
