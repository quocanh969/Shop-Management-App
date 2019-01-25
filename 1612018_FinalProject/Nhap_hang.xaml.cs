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
    /// Interaction logic for Nhap_hang.xaml
    /// </summary>
    public partial class Nhap_hang : UserControl
    {
        private DateTime ngayNhap { get; set; }
        private int tong { get; set; }
        private NHAPHANG newImport;

        private SANPHAM newProduct;
        private LOAI newCategory;

        private int TruongHop;
        // 1 - mã loại mới, sản phẩm mới
        // 2 - mã loại cũ, sản phẩm mới
        // 3 - mã loại cũ, sản phẩm cũ
        // 4 - mã loại và sản phẩm không phù hợp

        public Nhap_hang()
        {
            InitializeComponent();

            // Cập nhật ngày
            ngayNhap = DateTime.Today;
            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayNhap.Day);
            dayPick.Append("/");
            dayPick.Append(ngayNhap.Month);
            dayPick.Append("/");
            dayPick.Append(ngayNhap.Year);

            datePicking.Text = dayPick.ToString();
        }

        private void PopupBox_Closed(object sender, RoutedEventArgs e)
        {// Xử lý cập nhật thời gian
            ngayNhap = Calendar.SelectedDate.Value;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayNhap.Day);
            dayPick.Append("/");
            dayPick.Append(ngayNhap.Month);
            dayPick.Append("/");
            dayPick.Append(ngayNhap.Year);

            datePicking.Text = dayPick.ToString();
        }

        private void btnNhapHD_Click(object sender, RoutedEventArgs e)
        {// xử lý btn nhập
            ErrorNotice.Visibility = Visibility.Hidden;

            int checkCategory;
            int checkProduct;

            string maloai = txtBoxMaLoai.Text;
            string tenloai = txtBoxLoaiSP.Text;
            string tensp = txtBoxTenSP.Text;
            string masp = txtBoxMaSP.Text;
            int soluong = 0;
            double dongia, tienxuat;

            // Kiểm tra nhà cung cấp
            if (txtBoxNCC.Text == null || txtBoxNCC.Text == "")
            {
                ErrorNotice.Content = "*Thông tin nhà cung cấp không phù hợp";
                ErrorNotice.Visibility = Visibility.Visible;
                return;
            }

            // Kiểm tra số lượng
            
            if(int.TryParse(txtBoxSoLuong.Text,out soluong) == false)
            {
                ErrorNotice.Content = "*Định dạng số lượng không phù hợp";
                ErrorNotice.Visibility = Visibility.Visible;
                return;
            }

            // Kiêm tra đợn giá và tiền xuất
            
            if(double.TryParse(txtBoxDonGia.Text,out dongia) == false)
            {
                ErrorNotice.Content = "*Thông tin đơn giá không phù hợp";
                ErrorNotice.Visibility = Visibility.Visible;
                return;
            }

            tienxuat = dongia * soluong;
            txtBoxThanhTien.Text = tienxuat.ToString(); 
                                 
            // Kiểm tra và cập nhật mã loại
            if (txtBoxMaSP.Text == null || txtBoxMaSP.Text == ""
                || txtBoxTenSP.Text == null || txtBoxTenSP.Text == ""
                || txtBoxMaLoai.Text == null || txtBoxMaLoai.Text == ""
                || txtBoxLoaiSP.Text == null || txtBoxLoaiSP.Text == "")
            {
                ErrorNotice.Content = "*Định dạng thông tin sản phẩm không phù hợp";
                ErrorNotice.Visibility = Visibility.Visible;
                return;
            }            
            else
            {                
                checkCategory = checkLoai(maloai, tenloai);    
                // kiểm tra loại
                if(checkCategory == 1)
                {// nếu loại mới
                    TruongHop = 1; // loại mới, sản phẩm mới

                    newCategory = new LOAI()
                    {
                        MaLoai = txtBoxMaLoai.Text,
                        TenLoai = txtBoxLoaiSP.Text,
                    };

                    // Tiếp tục kiểm tra và nạp thêm vào sản phẩm
                    checkProduct = checkSP(masp, tensp);
                    if(checkProduct == 1)
                    {                        
                        newProduct = new SANPHAM()
                        {
                            MaSP = masp,
                            TenSP = tensp,
                            MaLoai = maloai,
                            SoLuong = soluong,
                            DonGia = dongia,
                            NgayNhap = ngayNhap,
                            TinhTrang = true,
                            NhaCungCap = txtBoxNCC.Text,
                        };                        
                    }
                    else
                    {// trường hợp còn lại
                        ErrorNotice.Content = "*Một sản phẩm không thể có thuộc nhiều loại";
                        ErrorNotice.Visibility = Visibility.Visible;
                        return;
                    }

                }
                else if(checkCategory == 0)
                {// bổ sung loại cũ
                    checkProduct = checkSP(masp, tensp);
                    if(checkProduct == 1)
                    {// sản phẩm mới
                        TruongHop = 2;
                        newProduct = new SANPHAM()
                        {
                            MaSP = masp,
                            TenSP = tensp,
                            MaLoai = maloai,
                            SoLuong = soluong,
                            DonGia = dongia,
                            NgayNhap = ngayNhap,
                            TinhTrang = true,
                            NhaCungCap = txtBoxNCC.Text,
                        };
                    }
                    else if(checkProduct == 0)
                    {// bổ sung sản phẩm cũ
                        TruongHop = 3;
                        // Cập nhật vào CSDL
                    }
                    else
                    {
                        ErrorNotice.Content = "*Không tồn tại sản phẩm hợp với mã này";
                        ErrorNotice.Visibility = Visibility.Visible;
                        return;
                    }
                }
                else
                {
                    ErrorNotice.Content = "*Tên loại và mã loại không phù hợp";
                    ErrorNotice.Visibility = Visibility.Visible;
                    return;
                }
            }

            // Sau khi kiểm tra thành công hết thì tạo hóa đợn
            newImport = new NHAPHANG()
                {
                    MaSP = masp,
                    MaLoai = maloai,
                    SoLuong = soluong,
                    DonGia = dongia,
                    NgayNhap = ngayNhap,
                    TienTra = tienxuat,
                    NhaCungCap = txtBoxNCC.Text,
                    GhiChu = txtBoxGhiChu.Text,
                };

            // Kết thúc quá trình làm hóa đơn, hiển thị cho người quản lý xem
            Man_che.Visibility = Visibility.Visible;
            btnNhapHD.Visibility = Visibility.Hidden;
            lblHeader.Content = "XEM LẠI HÓA ĐƠN";
        }
        
        private int checkLoai(string maloai, string tenloai)
        {// Nếu loại mới thì return 1
            // Nếu bổ sung loai cũ thì return 0
            // Nếu không tồn tại tên đúng với loại thì return -1
            List<LOAI> lstLoai = Data_handle.Data_interface.database.LOAIs.ToList();

            foreach(var t in lstLoai)
            {
                if(t.MaLoai == maloai)
                {
                    if (t.TenLoai == tenloai) return 0;
                    else return -1;
                }
            }
            return 1;
        }

        private int checkSP(string masp, string tensp)
        {// Nếu loại mới thì return 1
            // Nếu bổ sung loai cũ thì return 0
            // Nếu không tồn tại tên đúng với loại thì return -1
            List<SANPHAM> lstSP = Data_handle.Data_interface.database.SANPHAMs.Where(x=>x.TinhTrang == true).ToList();

            foreach (var t in lstSP)
            {
                if (t.MaSP == masp)
                {
                    if (t.TenSP == tensp) return 0;
                    else return -1;
                }
            }
            return 1;
        }

        // btn Refresh
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            refresh();
        }

        // btn Nhập vào data
        private void btnNhap_vao_data_Click(object sender, RoutedEventArgs e)
        {
            try
            {           
                if(TruongHop == 1)// loại mới, sản phẩm mới
                {
                    // Cập nhật loại vào CSDL
                    Data_handle.Data_interface.database.LOAIs.Add(newCategory);
                    Data_handle.Data_interface.database.SaveChanges();
                    // Cập nhật sản phẩm vào CSDL
                    Data_handle.Data_interface.database.SANPHAMs.Add(newProduct);
                    Data_handle.Data_interface.database.SaveChanges();
                    GlobalItem.lstSanPham.Add(newProduct);
                }
                else if(TruongHop == 2) // loại cũ, sản phẩm mới
                {
                    // Cập nhật sản phẩm vào CSDL
                    Data_handle.Data_interface.database.SANPHAMs.Add(newProduct);
                    Data_handle.Data_interface.database.SaveChanges();
                    GlobalItem.lstSanPham.Add(newProduct);
                }
                else if(TruongHop == 3) // loại cũ, sản phẩm cũ
                {
                    var t = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == txtBoxMaSP.Text).SingleOrDefault();
                    t.SoLuong = t.SoLuong + int.Parse(txtBoxSoLuong.Text);
                    Data_handle.Data_interface.database.SaveChanges();
                }
                else
                {
                    // do nothing
                }
                Data_handle.Data_interface.database.NHAPHANGs.Add(newImport);
                Data_handle.Data_interface.database.SaveChanges();

                refresh();
                updateProduct();
                Man_che.Visibility = Visibility.Hidden;
                btnNhapHD.Visibility = Visibility.Visible;
                lblHeader.Content = "HÓA ĐƠN";
            }
            catch
            {
                MessageBox.Show("Lỗi khi nạp hóa đơn", "Thông báo");
            }
        }

        // btn Nhập vào nút quay lại
        private void btnQuay_lai_Click(object sender, RoutedEventArgs e)
        {
            Man_che.Visibility = Visibility.Hidden;
            btnNhapHD.Visibility = Visibility.Visible;
            lblHeader.Content = "HÓA ĐƠN";
        }

        // button update data
        private void updateProduct()
        {
            try
            {
                if(TruongHop == 3)
                {
                    var t = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == newImport.MaSP).SingleOrDefault();
                    t.SoLuong += newImport.SoLuong;
                    Data_handle.Data_interface.database.SaveChanges();
                }
            }
            catch
            {
                MessageBox.Show("Lỗi khi cập nhật lại số lượng sản phẩm", "Thông báo");
            }
        }

        // button refresh
        public void refresh()
        {
            txtBoxLoaiSP.Text = null;
            txtBoxMaLoai.Text = null;
            txtBoxTenSP.Text = null;
            txtBoxMaSP.Text = null;
            txtBoxNCC.Text = null;
            txtBoxDonGia.Text = null;
            txtBoxSoLuong.Text = null;
            txtBoxThanhTien.Text = null;
            ngayNhap = DateTime.Today;
            datePicking.Text = null;
        }
    }
}
