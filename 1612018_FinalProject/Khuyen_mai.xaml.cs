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
    /// Interaction logic for Khuyen_mai.xaml
    /// </summary>
    public partial class Khuyen_mai : UserControl
    {
        private bool isSearching = false;

        public Khuyen_mai()
        {
            InitializeComponent();
            initData();

            // HIển thị button update
            if (GlobalItem.isAdmin == true)
            {
                ToolBar.Visibility = Visibility.Visible;
            }
            else
            {
                ToolBar.Visibility = Visibility.Hidden;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút Add
            if (isSearching == true)
            {
                GlobalItem.lstKhuyenMai = Data_handle.Data_interface.database.KHUYENMAIs.ToList();
                dataGridEvent.ItemsSource = GlobalItem.lstKhuyenMai;
                isSearching = false;
            }

            GlobalItem.isEventAdding = true;
            AddEvent addEvent = new AddEvent();
            addEvent.ShowDialog();
            dataGridEvent.Items.Refresh();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút Delete            
            if (GlobalItem.selectedEvent != null)
            {
                if (isSearching == true)
                {
                    GlobalItem.lstKhuyenMai = Data_handle.Data_interface.database.KHUYENMAIs.ToList();
                    dataGridEvent.ItemsSource = GlobalItem.lstKhuyenMai;
                    isSearching = false;
                }

                var selectedItem = Data_handle.Data_interface.database.KHUYENMAIs.Where(x => x.MaKM == GlobalItem.selectedEvent.MaKM).SingleOrDefault();

                selectedItem.TrangThai = false;

                Data_handle.Data_interface.database.SaveChanges();

                int i = 0;
                while (GlobalItem.selectedEvent.MaKM != GlobalItem.lstKhuyenMai[i].MaKM)
                {
                    i++;
                }

                GlobalItem.lstKhuyenMai.RemoveAt(i);

                dataGridEvent.Items.Refresh();
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút update
            if (GlobalItem.selectedEvent != null)
            {
                GlobalItem.isEventEditting = true;

                AddEvent editEvent = new AddEvent();
                editEvent.ShowDialog();
                dataGridEvent.Items.Refresh();
            }
        }

        private void dataGridEvent_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {// Xác định dòng cell nào đang được chọn         
            DataGrid DG = (DataGrid)sender;
            GlobalItem.selectedEvent = (KHUYENMAI)DG.SelectedItem;
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {// Xem chi tiết khi click vào một cell
            GlobalItem.isEventLooking = true;
            AddEvent showEvent = new AddEvent();
            showEvent.ShowDialog();
        }

        // KHởi tạo data cho bảng
        private void initData()
        {
            int makm = -1;
            var lstEvent = Data_handle.Data_interface.database.KHUYENMAIs.Where(x => x.TrangThai == true).ToList();
            foreach (var t in lstEvent)
            {
                if (t.NgayKT < DateTime.Today)
                {
                    makm = t.MaKM;
                    var temp = Data_handle.Data_interface.database.KHUYENMAIs.Where(x => x.MaKM == makm).SingleOrDefault();
                    temp.TrangThai = false;
                    Data_handle.Data_interface.database.SaveChanges();
                }
            }

            GlobalItem.lstKhuyenMai = Data_handle.Data_interface.database.KHUYENMAIs.Where(x => x.TrangThai == true).ToList();
            dataGridEvent.ItemsSource = GlobalItem.lstKhuyenMai;
        }

        // Xử lý button refresh
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (isSearching == true)
            {
                GlobalItem.lstKhuyenMai = Data_handle.Data_interface.database.KHUYENMAIs.Where(x=>x.TrangThai==true).ToList();
                dataGridEvent.ItemsSource = GlobalItem.lstKhuyenMai;
                dataGridEvent.Items.Refresh();
            }
        }

        // Xử lý button Search
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (txtBoxSearch.Text == null || txtBoxSearch.Text == "")
            {
                MessageBox.Show("Khung search còn trống", "Thống báo");
            }
            else
            {
                isSearching = true;

                GlobalItem.lstKhuyenMai = Data_handle.Data_interface.database.KHUYENMAIs
                    .Where(x => x.TenKM.Contains(txtBoxSearch.Text) && x.TrangThai == true).ToList();
                if (GlobalItem.lstKhuyenMai.Count() > 0)
                {
                    dataGridEvent.ItemsSource = GlobalItem.lstKhuyenMai;
                    dataGridEvent.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Không tồn tại sự kiện cần tìm", "Thông báo");
                    GlobalItem.lstKhuyenMai = Data_handle.Data_interface.database.KHUYENMAIs.Where(x=>x.TrangThai==true).ToList();
                    txtBoxSearch.Text = null;
                }
            }
        }
    }
}
