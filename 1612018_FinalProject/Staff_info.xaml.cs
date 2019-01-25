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
    /// Interaction logic for Staff_info.xaml
    /// </summary>
    public partial class Staff_info : Window
    {
        List<YearOfBirth> lstYear = new List<YearOfBirth>();
        public Staff_info()
        {            
            InitializeComponent();
            InitYearOfBirth();

            // nạp dữ liệu vào
            if (GlobalItem.isUserLooking == true)
            {// Xem
                Man_che.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Hidden;
                // Có 2 chế độ là xem thông tin bản thân và xem thông tin người khác
                if (GlobalItem.isUser == false)
                {
                    Setting.Visibility = Visibility.Hidden;
                    txtBoxTenNV.Text = GlobalItem.selectedStaff.TenNV;
                    if (GlobalItem.selectedStaff.GioiTinh == "Nam       ")// Vì format trong SQL server nên phải có nheiu62 space
                    {
                        chckNam.IsChecked = true;
                        chckNu.IsChecked = false;
                    }
                    else
                    {
                        chckNu.IsChecked = true;
                        chckNam.IsChecked = false;
                    }
                    // Cập nhật vai trò
                    if(GlobalItem.selectedStaff.VaiTro == "admin ")
                    {
                        cmbRole.SelectedIndex = 1;
                    }
                    else
                    {
                        cmbRole.SelectedIndex = 0;
                    }
                    txtBoxTel.Text = GlobalItem.selectedStaff.Sdt;
                    cmbNamSinh.SelectedIndex = GlobalItem.selectedStaff.NamSinh.Value - 1980;
                    txtBoxEmail.Text = GlobalItem.selectedStaff.Email;
                    txtBoxCMND.Text = GlobalItem.selectedStaff.CMND;
                    txtBoxDiaChi.Text = GlobalItem.selectedStaff.DiaChi;
                }
                else
                {
                    txtBoxTenNV.Text = GlobalItem.user.TenNV;
                    if(GlobalItem.user.GioiTinh == "Nam       ")
                    {
                        chckNam.IsChecked = true;
                    }
                    else
                    {
                        chckNu.IsChecked = true;
                    }
                    // Cập nhật vai trò
                    if (GlobalItem.user.VaiTro == "admin ")
                    {
                        cmbRole.SelectedIndex = 1;
                    }
                    else
                    {
                        cmbRole.SelectedIndex = 0;
                    }
                    txtBoxTel.Text = GlobalItem.user.Sdt;                    
                    cmbNamSinh.SelectedIndex = GlobalItem.user.NamSinh.Value - 1980;
                    txtBoxEmail.Text = GlobalItem.user.Email;
                    txtBoxCMND.Text = GlobalItem.user.CMND;
                    txtBoxDiaChi.Text = GlobalItem.user.DiaChi;
                }
            }
            else if(GlobalItem.isUserEditting == true)
            {// Sữa
                btnSetting.Visibility = Visibility.Hidden;
                txtBoxTenNV.Text = GlobalItem.selectedStaff.TenNV;
                if (GlobalItem.selectedStaff.GioiTinh == "Nam       ")// Vì format trong SQL server nên phải có nheiu62 space
                {
                    chckNam.IsChecked = true;
                    chckNu.IsChecked = false;
                }
                else
                {
                    chckNu.IsChecked = true;
                    chckNam.IsChecked = false;
                }
                // Cập nhật vai trò
                if (GlobalItem.selectedStaff.VaiTro == "admin ")
                {
                    cmbRole.SelectedIndex = 1;
                }
                else
                {
                    cmbRole.SelectedIndex = 0;
                }
                txtBoxTel.Text = GlobalItem.selectedStaff.Sdt;
                cmbNamSinh.SelectedIndex = GlobalItem.selectedStaff.NamSinh.Value - 1980;
                txtBoxEmail.Text = GlobalItem.selectedStaff.Email;
                txtBoxCMND.Text = GlobalItem.selectedStaff.CMND;
                txtBoxDiaChi.Text = GlobalItem.selectedStaff.DiaChi;
            }
            else
            {// Thêm
                if(GlobalItem.createdUser.PhanQuyen == "admin ")
                {
                    cmbRole.SelectedIndex = 1;
                }
                else
                {
                    cmbRole.SelectedIndex = 0;
                }
                cmbRole.IsEnabled = false;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {// Xử lý khi thoát chương trình
            GlobalItem.isUser = false;
            GlobalItem.isUserLooking = false;
            GlobalItem.isUserEditting = false;
            GlobalItem.isUserAdding = false;
            GlobalItem.createdUser = null;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {// Xứ lý khi nhấn OK
            // kHởi tạo lại giá trị của ErrorNOtice
            ErrorNotice.Visibility = Visibility.Hidden;

            if(GlobalItem.isUserLooking == true)
            {// Xem
                this.Close();
            }
            else if(GlobalItem.isUserEditting == true)
            {// Sửa
                var edittedStaff = Data_handle.Data_interface.database.NHANVIENs.Where(x => x.MaNV == GlobalItem.selectedStaff.MaNV).SingleOrDefault();

                edittedStaff.TenNV = txtBoxTenNV.Text;
                if (chckNam.IsChecked == true)// Vì format trong SQL server nên phải có nheiu62 space
                {
                    edittedStaff.GioiTinh = "Nam       ";
                }
                else
                {
                    edittedStaff.GioiTinh = "Nữ        ";
                }
                // Vai trò
                if(cmbRole.SelectedIndex == 0)
                {
                    edittedStaff.VaiTro = "staff ";
                }
                else
                {
                    edittedStaff.VaiTro = "admin ";
                }
                edittedStaff.Sdt = txtBoxTel.Text;
                edittedStaff.NamSinh = cmbNamSinh.SelectedIndex + 1980;
                edittedStaff.Email = txtBoxEmail.Text;
                edittedStaff.CMND = txtBoxCMND.Text;
                edittedStaff.DiaChi = txtBoxDiaChi.Text;

                Data_handle.Data_interface.database.SaveChanges();
            }
            else
            {// Thêm
             // Tạo biến NHANVIEN mới
                NHANVIEN newStaff = new NHANVIEN()
                {
                    TenNV = txtBoxTenNV.Text,
                    GioiTinh = "Nam       ",
                    VaiTro = "admin ",
                    NamSinh = cmbNamSinh.SelectedIndex + 1980,
                    Email = txtBoxEmail.Text,
                    DiaChi = txtBoxDiaChi.Text,
                    CMND = txtBoxCMND.Text,
                    Sdt = txtBoxTel.Text,
                    TrangThai = true,
                };

                // Cập nhật giới tính               
                if (chckNu.IsChecked == true)
                {
                    newStaff.GioiTinh = "Nữ        ";
                }

                // Cập nhật lại vai trò
                if(cmbRole.SelectedIndex == 0)
                {
                    newStaff.VaiTro = "staff ";
                }

                // Thêm mới
                // vào user ( phải thêm vào lúc này vì có thể hủy giữa chừng )
                Data_handle.Data_interface.database.USERS.Add(GlobalItem.createdUser);                
                // vào database
                Data_handle.Data_interface.database.NHANVIENs.Add(newStaff);
                Data_handle.Data_interface.database.SaveChanges();
                // vào danh sách hiển thị
                GlobalItem.lstNhanVien.Insert(0, newStaff);
            }

            if(ErrorNotice.Visibility == Visibility.Hidden)
            {

                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {// Xử lý khi nhấn Cancel
            this.Close();
        }

        private void InitYearOfBirth()
        {
            for(int i = 1980;i<=2020;i++)
            {
                YearOfBirth temp = new YearOfBirth() { year = i};
                lstYear.Add(temp);
            }

            // Cập nhật combo box
            cmbNamSinh.ItemsSource = lstYear;
            cmbNamSinh.SelectedIndex = 0;
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            Doi_mat_khau changePass = new Doi_mat_khau();
            changePass.ShowDialog();
        }               

    }



    class YearOfBirth
    {
        public int year { get; set; }
    }
}
