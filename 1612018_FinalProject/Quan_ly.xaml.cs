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
    /// Interaction logic for Quan_ly.xaml
    /// </summary>
    public partial class Quan_ly : UserControl
    {
        private bool isSearching = false;

        public Quan_ly()
        {
            InitializeComponent();
            initData();

            // Thống kê sợ bộ
            updateSematic();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút Add
            if (isSearching == true)
            {
                GlobalItem.lstNhanVien = Data_handle.Data_interface.database.NHANVIENs.Where(x=>x.TrangThai==true).ToList();
                dataGridStaff.ItemsSource = GlobalItem.lstNhanVien;
                isSearching = false;
            }

            GlobalItem.isUserAdding = true;
            Doi_mat_khau addUser = new Doi_mat_khau();
            addUser.ShowDialog();

                if (GlobalItem.createdUser != null)
                {// Chỉ khi tạo user mới thì mới tạo nhân viên mới
                    Staff_info addStaff = new Staff_info();
                    addStaff.ShowDialog();
                    updateSematic();
                    //dataGridStaff.ItemsSource = GlobalItem.lstNhanVien;                
                    dataGridStaff.Items.Refresh();
                }            
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút Delete            
            if (GlobalItem.selectedStaff != null)
            {
                if (isSearching == true)
                {
                    GlobalItem.lstNhanVien = Data_handle.Data_interface.database.NHANVIENs.ToList();
                    dataGridStaff.ItemsSource = GlobalItem.lstNhanVien;
                    isSearching = false;
                }
                var selectedItem = Data_handle.Data_interface.database.NHANVIENs.Where(x => x.MaNV == GlobalItem.selectedStaff.MaNV).SingleOrDefault();

                selectedItem.TrangThai = false;

                Data_handle.Data_interface.database.SaveChanges();

                int i = 0;
                while (GlobalItem.selectedStaff.MaNV != GlobalItem.lstNhanVien[i].MaNV)
                {
                    i++;
                }

                GlobalItem.lstNhanVien.RemoveAt(i);

                updateSematic();
                dataGridStaff.Items.Refresh();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút update
            if (GlobalItem.selectedStaff != null)
            {                
                GlobalItem.isUserEditting = true;

                Staff_info editStaff = new Staff_info();
                editStaff.ShowDialog();
                updateSematic();
                dataGridStaff.Items.Refresh();
            }
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {// Xem chi tiết khi click vào một cell
            GlobalItem.isUserLooking = true;           
            Staff_info showStaff = new Staff_info();
            showStaff.ShowDialog();
        }

        // Khởi tạo dữ liệu
        private void initData()
        {
            GlobalItem.lstNhanVien = Data_handle.Data_interface.database.NHANVIENs.Where(x => x.TrangThai == true).ToList();
            dataGridStaff.ItemsSource = GlobalItem.lstNhanVien;
        }

        private void dataGridProduct_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {// Xác định dòng cell nào đang được chọn         
            DataGrid DG = (DataGrid)sender;
            GlobalItem.selectedStaff = (NHANVIEN)DG.SelectedItem;            
        }

        private void updateSematic()
        {
            // Thống kê sơ bộ số nhân viên
            lblSo_nhan_vien.Content = Data_handle.Data_interface.database.NHANVIENs.Where(x => x.VaiTro == "staff" && x.TrangThai == true).Count();

            // Thống kê sơ bộ số admin
            lblSo_Admin.Content = Data_handle.Data_interface.database.NHANVIENs.Where(x => x.VaiTro == "admin" && x.TrangThai == true).Count();

            // Thống kê sơ bộ doanh thu
            lblSo_nguoi.Content = Data_handle.Data_interface.database.NHANVIENs.Where(x=>x.TrangThai==true).Count();
        }

        // Search
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxSearch.Text == null || txtBoxSearch.Text == "")
            {
                MessageBox.Show("Khung search còn trống", "Thống báo");
            }
            else
            {
                isSearching = true;

                GlobalItem.lstNhanVien = Data_handle.Data_interface.database.NHANVIENs
                    .Where(x => x.TenNV.Contains(txtBoxSearch.Text) && x.TrangThai == true).ToList();

                if (GlobalItem.lstNhanVien.Count() > 0)
                {
                    dataGridStaff.ItemsSource = GlobalItem.lstNhanVien;
                    dataGridStaff.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Không tồn tại nhân viên cần tìm", "Thông báo");
                    GlobalItem.lstNhanVien = Data_handle.Data_interface.database.NHANVIENs.ToList();
                    txtBoxSearch.Text = null;
                }
            }
        }

        // Button refresh
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (isSearching == true)
            {
                GlobalItem.lstNhanVien = Data_handle.Data_interface.database.NHANVIENs.ToList();
                dataGridStaff.ItemsSource = GlobalItem.lstNhanVien;
                dataGridStaff.Items.Refresh();
            }
        }
    }
}
