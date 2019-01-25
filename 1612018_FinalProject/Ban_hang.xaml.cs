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
    /// Interaction logic for Ban_hang.xaml
    /// </summary>
    public partial class Ban_hang : UserControl
    {
        private UserControl current_Tab = new UserControl();
        public Ban_hang()
        {
            InitializeComponent();
            current_Tab = Tab_Truc_Tiep;
        }

        private void btnBanHangTrucTiep_Click(object sender, RoutedEventArgs e)
        {// Chọn tabBan_Hang_Truc_Tiep
            if (current_Tab != Tab_Truc_Tiep)
            {
                current_Tab = Tab_Truc_Tiep;

                Tab_Truc_Tiep.Visibility = Visibility.Visible;
                btnBanHangTrucTiep.Background = Brushes.White;
                Tab_Dat_Hang.Visibility = Visibility.Hidden;
                btnDatHang.Background = Brushes.LightGray;
                Tab_Thanh_Toan.Visibility = Visibility.Hidden;
                btnThanhToan.Background = Brushes.LightGray;
            }
        }

        private void btnDatHang_Click(object sender, RoutedEventArgs e)
        {// Chọn Tab_Đặt_Hàng
            if (current_Tab != Tab_Dat_Hang)
            {
                current_Tab = Tab_Dat_Hang;

                Tab_Truc_Tiep.Visibility = Visibility.Hidden;
                btnBanHangTrucTiep.Background = Brushes.LightGray;
                Tab_Dat_Hang.Visibility = Visibility.Visible;
                btnDatHang.Background = Brushes.White;
                Tab_Thanh_Toan.Visibility = Visibility.Hidden;
                btnThanhToan.Background = Brushes.LightGray;
            }
        }

        private void btnThanhToan_Click(object sender, RoutedEventArgs e)
        {// Chọn tab thanh toán
            if (current_Tab != Tab_Thanh_Toan)
            {
                current_Tab = Tab_Thanh_Toan;

                Tab_Truc_Tiep.Visibility = Visibility.Hidden;
                btnBanHangTrucTiep.Background = Brushes.LightGray;
                Tab_Dat_Hang.Visibility = Visibility.Hidden;
                btnDatHang.Background = Brushes.LightGray;
                Tab_Thanh_Toan.Visibility = Visibility.Visible;
                btnThanhToan.Background = Brushes.White;
            }
        }
    }
}
