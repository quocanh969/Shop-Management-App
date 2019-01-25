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
    /// Interaction logic for Thong_ke.xaml
    /// </summary>
    public partial class Thong_ke : UserControl
    {
        public Thong_ke()
        {
            InitializeComponent();

        }

        private void btnDoanhThu_Click(object sender, RoutedEventArgs e)
        {
            // Chọn Tab_Doanh_Thu
            Tab_Doanh_Thu.Visibility = Visibility.Visible;
            btnDoanhThu.Background = Brushes.White;
            Tab_Mat_Hang_Ban_Chay.Visibility = Visibility.Hidden;
            btnBanChay.Background = Brushes.LightGray;
        }

        private void btnBanChay_Click(object sender, RoutedEventArgs e)
        {
            // Chọn Tab_Mat_Hang_Ban_Chay
            Tab_Mat_Hang_Ban_Chay.Visibility = Visibility.Visible;
            btnBanChay.Background = Brushes.White;
            Tab_Doanh_Thu.Visibility = Visibility.Hidden;
            btnDoanhThu.Background = Brushes.LightGray;
        }
    }
}
