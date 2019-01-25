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
    /// Interaction logic for Gio_hang.xaml
    /// </summary>
    public partial class Gio_hang : Window
    {
        private matHang selected;
        private int rowOfProduct;
        private List<matHang> lstMatHang = new List<matHang>();
        public Gio_hang()
        {
            InitializeComponent();
            initData();
        }

        private void initData()
        {          
            dataGiaHang.ItemsSource = GlobalItem.lstMatHang;            
        }

        private void dataGiaHang_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid DG = (DataGrid)sender;
            selected = (matHang)DG.SelectedItem;
            rowOfProduct = DG.SelectedIndex;
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {// Xóa sản phẩm
            GlobalItem.lstMatHang.Remove(selected);
            // Tìm và xóa sản phẩm trong giỏ hàng

            GlobalItem.gioHang.RemoveAt(rowOfProduct);

            dataGiaHang.Items.Refresh();
        }

        // button OK
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }    
}
