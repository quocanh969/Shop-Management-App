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
    /// Interaction logic for Doi_mat_khau.xaml
    /// </summary>
    public partial class Doi_mat_khau : Window
    {
        public Doi_mat_khau()
        {
            InitializeComponent();
            if (GlobalItem.isUserAdding == false)
            {
                txtBoxUserName.Text = GlobalItem.username;
                txtBoxUserName.IsEnabled = false;
                cmbRole.Visibility = Visibility.Hidden;
                lblCurrntePass.Visibility = Visibility.Visible;
                txtBoxCurrentPass.Visibility = Visibility.Visible;
                lblHeader.Content = "CHANG PASSWORD";
            }
            else
            {
                cmbRole.Visibility = Visibility.Visible;
                lblCurrntePass.Visibility = Visibility.Hidden;
                txtBoxCurrentPass.Visibility = Visibility.Hidden;
                lblHeader.Content = "CREATE USER";
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {// Xử lý button OK
            if (GlobalItem.isUserAdding == false)
            {
                if (txtBoxCurrentPass.Password == GlobalItem.password)
                {
                    if (txtBoxNewPass.Password == txtBoxConfirm.Password)
                    {
                        var handle = Data_handle.Data_interface.database.USERS.Where(x => x.Username == GlobalItem.username).SingleOrDefault();
                        handle.Password = txtBoxNewPass.Password;
                        Data_handle.Data_interface.database.SaveChanges();

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu xác nhận (Confirm) không trùng nhau", "Thông báo");
                    }
                }
                else
                {
                    MessageBox.Show("Mật khẩu (Current Pass) bạn nhập không đúng", "Thông báo");
                }
            }
            else
            {
                var check = kiemTraUser(txtBoxUserName.Text);
                if(check == true)
                {
                    if (txtBoxNewPass.Password == txtBoxConfirm.Password)
                    {
                        USER newUser = new USER()
                        {
                            Username = txtBoxUserName.Text,
                            Password = txtBoxNewPass.Password,
                            MaNV = Data_handle.Data_interface.database.NHANVIENs.Count(),
                            PhanQuyen = "admin",
                        };
                        
                        // Cập nhật lại phân quyền
                        if(cmbRole.SelectedIndex == 0)
                        {
                            newUser.PhanQuyen = "Staff";
                        }

                        GlobalItem.createdUser = newUser;                      

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu xác nhận (Confirm) không trùng nhau", "Thông báo");
                    }                    
                }
                else
                {
                    MessageBox.Show("Đã tồn tại tài khoản", "Thông báo");
                    txtBoxUserName.Text = "";
                }
            }
        }

        private bool kiemTraUser(string username)
        {
            foreach (var t in Data_handle.Data_interface.database.USERS)
            {
                if (t.Username == username) return false;
            }
            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {// Xử lý button Cancel
            this.Close();
        }
    }
}
