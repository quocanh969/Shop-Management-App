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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút Cancel
            this.Close();

        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                OK_Event();
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút OK            
            OK_Event();
        }

        // Xử lý sự kiện đăng nhập
        private void OK_Event()
        {
            // Kiểm tra người dùng nhập rỗng
            if (txtBox_UserName.Text == "")
            {
                txtBox_notice.Text = "*Your Username box is empty";
                txtBox_notice.Visibility = Visibility.Visible;
            }
            else if (passBox_Password.Password == "")
            {
                txtBox_notice.Text = "*Your Password box is empty";
                txtBox_notice.Visibility = Visibility.Visible;
            }
            else
            {
                if (loginCheck(txtBox_UserName.Text, passBox_Password.Password) == true)
                {
                    try
                    {
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Lỗi không khởi tạo được màn hình", "Thông báo");
                        return;
                    }
                }
                else
                {
                    // xóa ký tự trong ô đăng nhập
                    txtBox_UserName.Text = "";
                    passBox_Password.Password = "";
                    txtBox_notice.Text = "*Wrong account";
                    txtBox_notice.Visibility = Visibility.Visible;
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {// Drag Window
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool loginCheck(string user, string pass)
        {            
            // Đếm xem coi có bao nhiêu tài khoản thỏa user và pass
            // Lưu ý là do database nên không có chuyện đăng kí trùng user name nên toi616 đa là 1 tài khoảng
            var existUser = Data_handle.Data_interface.database.USERS.Where(x => x.Username == user
            && x.Password == pass);

            if(existUser.Count() > 0)
            {
                foreach (var temp in existUser)
                {
                    var t = Data_handle.Data_interface.database.NHANVIENs.Where(x => x.MaNV == temp.MaNV);                    
                    GlobalItem.user = t.First();

                    if (GlobalItem.user.TrangThai == false)
                    {// Nếu user bị xóa rồi thì cũng không được
                        return false;
                    }
                    else
                    {
                        GlobalItem.username = txtBox_UserName.Text;
                        GlobalItem.password = passBox_Password.Password;
                        // Kiểm tra phân quyền
                        if (temp.PhanQuyen == "admin")
                        {
                            GlobalItem.isAdmin = true;
                        }
                        else
                        {
                            GlobalItem.isAdmin = false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }

        }

    }
}
