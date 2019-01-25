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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Lưu tab hiện tại đang mở
        UserControl CurrentTab = new UserControl();
        Staff_info info_screen;

        public MainWindow()
        {
            InitializeComponent();


            // Chỉ admin mới có thể quản lý nhân viên
            if(GlobalItem.isAdmin == true)
            {
                btnManagement.Visibility = Visibility.Visible;
                txtBox_user.Text = "Admin";
            }
            else
            {
                btnManagement.Visibility = Visibility.Hidden;
                txtBox_user.Text = "Staff";
            }

            // Thiết lập Tab ban đầu là Tab_trang_chu
            CurrentTab = Tab_Trang_chu;
        }

        private void btnSystem_Click(object sender, RoutedEventArgs e)
        {// Xử lý nhấn button System
            this.Background = new SolidColorBrush(Color.FromRgb(9,132,227));
            lblScreenName.Content = "Hệ thống quản lý hàng hóa";

            // Thiết lập và cập nhật lại trang tab
            if (CurrentTab != Tab_Trang_chu)
            {
                Tab_Trang_chu.updateSematic();
                Tab_Trang_chu.updateData();

                CurrentTab.Visibility = Visibility.Hidden;
                Tab_Trang_chu.Visibility = Visibility.Visible;
                CurrentTab = Tab_Trang_chu;
            }
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {// Xử lý nhấn button Sell
            this.Background = new SolidColorBrush(Color.FromRgb(111, 178, 141));
            lblScreenName.Content = "Bán hàng";

            // Thiết lập và cập nhật lại trang tab
            if (CurrentTab != Tab_Ban_hang)
            {                
                CurrentTab.Visibility = Visibility.Hidden;
                Tab_Ban_hang.Visibility = Visibility.Visible;
                CurrentTab = Tab_Ban_hang;
            }
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {// Xử lý nhấn button Import
            this.Background = new SolidColorBrush(Color.FromRgb(72, 78, 212));
            lblScreenName.Content = "Nhập hàng mới";

            // Thiết lập và cập nhật lại trang tab
            if (CurrentTab != Tab_Nhap_hang)
            {
                CurrentTab.Visibility = Visibility.Hidden;
                Tab_Nhap_hang.Visibility = Visibility.Visible;
                CurrentTab = Tab_Nhap_hang;
            }
        }

        private void btnStatistic_Click(object sender, RoutedEventArgs e)
        {// Xử lý nhấn button Statistic
            this.Background = new SolidColorBrush(Color.FromRgb(52, 205, 184));
            lblScreenName.Content = "Thống kê hàng hóa";

            // Thiết lập và cập nhật lại trang tab
            if (CurrentTab != Tab_Thong_ke)
            {
                CurrentTab.Visibility = Visibility.Hidden;
                Tab_Thong_ke.Visibility = Visibility.Visible;
                CurrentTab = Tab_Thong_ke;
            }
        }

        private void btnPromotions_Click(object sender, RoutedEventArgs e)
        {// Xử lý nhấn button Promotions
            this.Background = new SolidColorBrush(Color.FromRgb(251, 83, 48));
            lblScreenName.Content = "Chương trình khuyến mãi";

            // Thiết lập và cập nhật lại trang tab
            if (CurrentTab != Tab_Khuyen_mai)
            {
                CurrentTab.Visibility = Visibility.Hidden;
                Tab_Khuyen_mai.Visibility = Visibility.Visible;
                CurrentTab = Tab_Khuyen_mai;
            }
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {// Xử lý khi nhấn button "Thông tin cá nhân"          
            GlobalItem.isUser = true;
            GlobalItem.isUserLooking = true;
            info_screen = new Staff_info();
            info_screen.ShowDialog();
            
        }

        private void btnLog_out_Click(object sender, RoutedEventArgs e)
        {// Xử lý nhấn button Logout
            Login login_screen = new Login();
            login_screen.Show();
            this.Close();
        }

        private void btnManagement_Click(object sender, RoutedEventArgs e)
        {// Xử lý khi nhấn button Management
            this.Background = new SolidColorBrush(Color.FromRgb(41, 149, 166));
            lblScreenName.Content = "Quản lý nhân viên";

            // Thiết lập và cập nhật lại trang tab
            if (CurrentTab != Tab_Quan_ly)
            {
                CurrentTab.Visibility = Visibility.Hidden;
                Tab_Quan_ly.Visibility = Visibility.Visible;
                CurrentTab = Tab_Quan_ly;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // do nothing
        }

        // ---------------------

    }
}
