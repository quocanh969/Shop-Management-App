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
using System.Windows.Shapes;

namespace _1612018_FinalProject
{
    /// <summary>
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Window
    {
        private DateTime ngayNhap { get; set; } 

        public Product()
        {
            InitializeComponent();
            InitCMBMaLoai();

            Calendar.SelectedDate = DateTime.Today;

            // Xem chi tiết sản phẩm
            if(GlobalItem.isLooking == true)
            {// Xem
                // Vô hiệu hóa khả năng chỉnh sữa thông tin trong chế độ xem
                //bằng cách dựng lên màn che trong suốt
                Man_Che.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Hidden;

                // Cập nhật thông tin
                txtBoxTenSP.Text = GlobalItem.selectedProduct.TenSP;
                txtBoxMaSP.Text = GlobalItem.selectedProduct.MaSP;
                txtBoxSoLuong.Text = GlobalItem.selectedProduct.SoLuong.ToString();
                txtBoxDonGia.Text = GlobalItem.selectedProduct.DonGia.ToString();
                txtBoxTenNCC.Text = GlobalItem.selectedProduct.NhaCungCap;
                //  Cập nhật tình trạng
                if (GlobalItem.selectedProduct.TinhTrang == true)
                {
                    chckContinue.IsChecked = true;
                }
                else
                {
                    chckPause.IsChecked = false;
                }
                // Cập nhật ngày nhập hàng
                ngayNhap = GlobalItem.selectedProduct.NgayNhap.Value;
                StringBuilder dayPick = new StringBuilder();                
                dayPick.Append(ngayNhap.Day);
                dayPick.Append("/");
                dayPick.Append(ngayNhap.Month);
                dayPick.Append("/");
                dayPick.Append(ngayNhap.Year);

                dateImport.Text = dayPick.ToString();
                // Cập nhật mã loại
                foreach(var t in Data_handle.Data_interface.database.LOAIs.ToList())
                {
                    if(t.MaLoai == GlobalItem.selectedProduct.MaLoai)
                    {
                        cmbMaLoai.SelectedItem = t;
                        break;
                    }
                }

                
            }
            else if(GlobalItem.isEditing == true) // Chỉnh sữa sản phẩm
            {// Sửa
                // Cập nhật thông tin
                txtBoxTenSP.Text = GlobalItem.selectedProduct.TenSP;                
                txtBoxMaSP.Text = GlobalItem.selectedProduct.MaSP;
                txtBoxMaSP.IsEnabled = false;
                txtBoxSoLuong.Text = GlobalItem.selectedProduct.SoLuong.ToString();
                txtBoxDonGia.Text = GlobalItem.selectedProduct.DonGia.ToString();
                txtBoxTenNCC.Text = GlobalItem.selectedProduct.NhaCungCap;
                //  Cập nhật tình trạng
                if (GlobalItem.selectedProduct.TinhTrang == true)
                {
                    chckContinue.IsChecked = true;
                }
                else
                {
                    chckPause.IsChecked = false;
                }
                // Cập nhật ngày nhập hàng
                Calendar.SelectedDate = GlobalItem.selectedProduct.NgayNhap;
                ngayNhap = GlobalItem.selectedProduct.NgayNhap.Value;
                StringBuilder dayPick = new StringBuilder();
                dayPick.Append(ngayNhap.Day);
                dayPick.Append("/");
                dayPick.Append(ngayNhap.Month);
                dayPick.Append("/");
                dayPick.Append(ngayNhap.Year);

                dateImport.Text = dayPick.ToString();
                // Cập nhật mã loại
                foreach (var t in Data_handle.Data_interface.database.LOAIs.ToList())
                {
                    if (t.MaLoai == GlobalItem.selectedProduct.MaLoai)
                    {
                        cmbMaLoai.SelectedItem = t;
                        break;
                    }
                }
            }
            else
            {// Thêm
                //do nothing
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {// Thêm hoặc sữa thông tin confirm  

            ErrorNotice.Visibility = Visibility.Hidden;// Kiểm tra lại


            if (GlobalItem.isLooking == true)
            {// Xem
                this.Close();
            }
            else if(GlobalItem.isEditing == true)
            {// Sửa
                    var edittedProduct = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == GlobalItem.selectedProduct.MaSP).SingleOrDefault();

                    edittedProduct.TenSP = txtBoxTenSP.Text;
                    edittedProduct.MaSP = txtBoxMaSP.Text;
                    edittedProduct.MaLoai = Data_handle.Data_interface.database.LOAIs.ToList()[cmbMaLoai.SelectedIndex].MaLoai;
                    edittedProduct.NgayNhap = ngayNhap;
                    edittedProduct.SoLuong = 0;
                    edittedProduct.DonGia = 0;
                    edittedProduct.TinhTrang = chckContinue.IsChecked;
                    edittedProduct.NhaCungCap = txtBoxTenNCC.Text;

                    // Kiểm tra và tạo số lượng của sản phẩm
                    int temp = 0;
                    if (int.TryParse(txtBoxSoLuong.Text, out temp) == false)
                    {
                        // Xử lý lỗi
                        ErrorNotice.Content = "*Định dạng số lượng không đúng";
                        ErrorNotice.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                    {
                        edittedProduct.SoLuong = temp;
                    }

                    // Kiểm tra và tạo đơn giá cho số lượng
                    float temp_1 = 0;
                    if (float.TryParse(txtBoxDonGia.Text, out temp_1) == false)
                    {
                        // Xử lý lỗi
                        ErrorNotice.Content = "*Định dạng đơn giá không đúng";
                        ErrorNotice.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                    {
                        edittedProduct.DonGia = temp_1;
                    }
               
                    Data_handle.Data_interface.database.SaveChanges();               
            }
            else
            {// Thêm
                // Kiểm tra Mã sản phẩm
                var check = KiemTraMaSP(txtBoxMaSP.Text);
                if (check == true)
                {

                    // Tạo biến sản phẩm mới
                    SANPHAM newProduct = new SANPHAM()
                    {
                        TenSP = txtBoxTenSP.Text,
                        MaSP = txtBoxMaSP.Text,
                        MaLoai = Data_handle.Data_interface.database.LOAIs.ToList()[cmbMaLoai.SelectedIndex].MaLoai,
                        NgayNhap = ngayNhap,
                        SoLuong = 0,
                        DonGia = 0,
                        TinhTrang = chckContinue.IsChecked,
                        NhaCungCap = txtBoxTenNCC.Text,
                    };

                    // Kiểm tra và tạo số lượng của sản phẩm
                    int temp = 0;
                    if (int.TryParse(txtBoxSoLuong.Text, out temp) == false)
                    {
                        // Xử lý lỗi
                        ErrorNotice.Content = "*Định dạng số lượng không đúng";
                        ErrorNotice.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                    {
                        newProduct.SoLuong = temp;
                    }

                    // Kiểm tra và tạo đơn giá cho số lượng
                    float temp_1 = 0;
                    if (float.TryParse(txtBoxDonGia.Text, out temp_1) == false)
                    {
                        // Xử lý lỗi
                        ErrorNotice.Content = "*Định dạng đơn giá không đúng";
                        ErrorNotice.Visibility = Visibility.Visible;
                        return;
                    }
                    else
                    {
                        newProduct.DonGia = temp_1;                        
                    }
                                        
                    // Thêm mới
                    // vào database
                    Data_handle.Data_interface.database.SANPHAMs.Add(newProduct);
                    Data_handle.Data_interface.database.SaveChanges();
                    // vào danh sách hiển thị
                    GlobalItem.lstSanPham.Insert(0, newProduct);                    
                }
                else
                {
                    // Xử lý lỗi
                    ErrorNotice.Content = "*Mã sản phẩm này đã tồn tại";
                    ErrorNotice.Visibility = Visibility.Visible;
                    
                }               
            }

            // Tắt màn hình khi không có lỗi
            if (ErrorNotice.Visibility == Visibility.Hidden)
            {
                this.Close();
            }
        }
        
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InitCMBMaLoai()
        {
            cmbMaLoai.ItemsSource = Data_handle.Data_interface.database.LOAIs.ToList();
            cmbMaLoai.SelectedIndex = 0;
        }

        private bool KiemTraMaSP(string masp)
        {// Kiểm tra có tồn tại masp trong database khong
            foreach(var t in Data_handle.Data_interface.database.SANPHAMs)
            {
                if(t.MaSP == masp)
                {
                    return false;
                }
            }
            return true;
        }

        private void PopupBox_Closed(object sender, RoutedEventArgs e)
        {// Chọn ngày thành công

            ngayNhap = Calendar.SelectedDate.Value;

            StringBuilder dayPick = new StringBuilder();
        // Cập nhật ngày tháng
            dayPick.Append(ngayNhap.Day);            
            dayPick.Append("/");
            dayPick.Append(ngayNhap.Month);
            dayPick.Append("/");
            dayPick.Append(ngayNhap.Year);

            dateImport.Text = dayPick.ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GlobalItem.isLooking = false;
            GlobalItem.isEditing = false;
            ErrorNotice.Visibility = Visibility.Hidden;
        }
    }
}
