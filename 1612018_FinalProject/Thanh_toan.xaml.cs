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
    /// Interaction logic for Thanh_toan.xaml
    /// </summary>
    public partial class Thanh_toan : UserControl
    {
        private DATHANG selectedDathang;

        public Thanh_toan()
        {
            InitializeComponent();
        }

        private void initData()
        {
            // Cập nhật danh sách hóa đơn bán hàng trực tiếp
            dataGridBanHangTrucTiep.ItemsSource = Data_handle.Data_interface.database.HOADONs.ToList();
            dataGridBanHangTrucTiep.Items.Refresh();

            // Cập nhật danh sách hóa đơn đặt hàng
            dataGridDatHang.ItemsSource = Data_handle.Data_interface.database.DATHANGs.ToList();
            dataGridDatHang.Items.Refresh();
        }

        private void Truc_tiep_Checked(object sender, RoutedEventArgs e)
        {
            GridTrucTiep.Visibility = Visibility.Visible;
            GridDatHang.Visibility = Visibility.Hidden;
        }

        private void Dat_hang_Checked(object sender, RoutedEventArgs e)
        {
            GridTrucTiep.Visibility = Visibility.Hidden;
            GridDatHang.Visibility = Visibility.Visible;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            dataGridBanHangTrucTiep.ItemsSource = null;
            dataGridDatHang.ItemsSource = null;
            initData();            
        }

        // Cập nhật dòng code
        private void dataGridDatHang_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            selectedDathang = (DATHANG)dataGridDatHang.SelectedItem;
        }

        // nút thanh toán đơn đặt hàng
        private void btnDaThanhToan_Click(object sender, RoutedEventArgs e)
        {
            // Tạo hóa đơn
            HOADON newBill = new HOADON()
            {
                MaSP = selectedDathang.MaSP,
                MaLoai = selectedDathang.MaLoai,
                SoLuong = selectedDathang.SoLuong,
                NgayLapHD = selectedDathang.NgayNhan,
                TienThu = selectedDathang.TienThu,
                MaEvent = selectedDathang.MaEvent,
                HinhThuc = selectedDathang.HinhThuc,
                GhiChu = $"Đặt hàng đã thanh toán\n Khách hàng: {selectedDathang.TenKH}\n Địa chỉ: {selectedDathang.DiaChi}\n Số điện thoại: {selectedDathang.Sdt}",
            };

            Data_handle.Data_interface.database.DATHANGs.Remove(selectedDathang);
            Data_handle.Data_interface.database.HOADONs.Add(newBill);
            Data_handle.Data_interface.database.SaveChanges();

            dataGridBanHangTrucTiep.ItemsSource = null;
            dataGridDatHang.ItemsSource = null;
            initData();
        }
    }
}
