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
    /// Interaction logic for Trang_chu.xaml
    /// </summary>
    public partial class Trang_chu : UserControl
    {
        // Phân trang
        private int soPhanTu = 7;
        private int trangHienTai = 0;
        private int soTrang = 0;

        private bool isSearching = false;
        private bool firstFilter = true;
        private List<item> lstType = new List<item>();

        private List<SANPHAM> lstCurrentProduct = new List<SANPHAM>();

        public Trang_chu()
        {
            InitializeComponent();
            
            initData();

            // Xác định số trang
            int ssp = Data_handle.Data_interface.database.SANPHAMs.Count();
            soTrang = ssp / soPhanTu;
            if(ssp % soPhanTu > 0)
            {
                soTrang++;
            }

            lblTotalPage.Content = soTrang.ToString();
            lblCurrentPage.Content = (trangHienTai+1).ToString();

            // Thống kê sơ bộ số hàng hóa còn abn1
            lblTong_hang.Content = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.TinhTrang == true).Sum(x => x.SoLuong);

            // Thống kê sơ bộ số mặt hàng
            lblSo_hang.Content = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.TinhTrang == true).Count();

            // Thống kê sơ bộ doanh thu
            lblSo_loai.Content = Data_handle.Data_interface.database.LOAIs.Count();


            // HIển thị button update
            if (GlobalItem.isAdmin == true)
            {
                btnUpdate.Visibility = Visibility.Visible;

            }
            else
            {
                btnUpdate.Visibility = Visibility.Hidden;

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút Add
            if(isSearching == true)
            {
                GlobalItem.lstSanPham = Data_handle.Data_interface.database.SANPHAMs.ToList();
                dataGridProduct.ItemsSource = GlobalItem.lstSanPham;
                isSearching = false;
            }

                Product addProduct = new Product();
                addProduct.ShowDialog();
                updateSematic();
            trangHienTai = 0;
            soTrang = GlobalItem.lstSanPham.Count() / soPhanTu;
            if (GlobalItem.lstSanPham.Count() % soPhanTu > 0)
            {
                soTrang++;
            }
            updateListSanPham();
            dataGridProduct.ItemsSource = lstCurrentProduct;
            dataGridProduct.Items.Refresh();
            
        }


        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút update
            if (GlobalItem.selectedProduct != null)
            {
                GlobalItem.isEditing = true;

                Product editProduct = new Product();
                editProduct.ShowDialog();
                updateSematic();
                soTrang = GlobalItem.lstSanPham.Count() / soPhanTu;
                if (GlobalItem.lstSanPham.Count() % soPhanTu > 0)
                {
                    soTrang++;
                }
                updateListSanPham();
                dataGridProduct.ItemsSource = lstCurrentProduct;
                dataGridProduct.Items.Refresh();
            }
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {// Xử lý nút Delete         
            if (GlobalItem.selectedProduct != null)
            {
                if (isSearching == true)
                {
                    GlobalItem.lstSanPham = Data_handle.Data_interface.database.SANPHAMs.ToList();
                    dataGridProduct.ItemsSource = GlobalItem.lstSanPham;
                    isSearching = false;
                }

                var selectedItem = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.MaSP == GlobalItem.selectedProduct.MaSP).SingleOrDefault();

                selectedItem.TinhTrang = false;

                Data_handle.Data_interface.database.SaveChanges();

                int i = 0;
                while (GlobalItem.selectedProduct.MaSP != GlobalItem.lstSanPham[i].MaSP)
                {
                    i++;
                }

                GlobalItem.lstSanPham.RemoveAt(i);

                updateSematic();
                soTrang = GlobalItem.lstSanPham.Count() / soPhanTu;
                if (GlobalItem.lstSanPham.Count() % soPhanTu > 0)
                {
                    soTrang++;
                }
                if(trangHienTai == soTrang)
                {
                    trangHienTai--;
                }
                updateListSanPham();
                dataGridProduct.ItemsSource = lstCurrentProduct;
                dataGridProduct.Items.Refresh();
            }
        }

        // KHởi tạo data cho bảng
        private void initData()
        {
            trangHienTai = 0;
            GlobalItem.lstSanPham = Data_handle.Data_interface.database.SANPHAMs.Where(x=>x.TinhTrang == true).ToList();
            lstCurrentProduct = GlobalItem.lstSanPham.Skip(trangHienTai*soPhanTu).Take(soPhanTu).ToList();
            dataGridProduct.ItemsSource = lstCurrentProduct;
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {// Xem chi tiết khi click vào một cell
            GlobalItem.isLooking = true;            
            Product showProduct = new Product();
            showProduct.ShowDialog();
        }

        private void dataGridProduct_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {// Xác định dòng cell nào đang được chọn         
            DataGrid DG = (DataGrid)sender;         
            GlobalItem.selectedProduct = (SANPHAM)DG.SelectedItem;
        }

        // Hàm cập nhật nội dung label và datagrid
        public void updateSematic()
        {

            // Thống kê sơ bộ số hàng hóa còn bán
            lblTong_hang.Content = Data_handle.Data_interface.database.SANPHAMs.Where(x=>x.TinhTrang == true).Sum(x => x.SoLuong);

            // Thống kê sơ bộ số mặt hàng còn bán
            lblSo_hang.Content = Data_handle.Data_interface.database.SANPHAMs.Where(x => x.TinhTrang == true).Count();

            // Thống kê sơ bộ doanh thu
            lblSo_loai.Content = Data_handle.Data_interface.database.LOAIs.Count();
        }

        public void updateData()
        {
            dataGridProduct.Items.Refresh();
        }

        // Xử lý button Search
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(txtBoxSearch.Text == null || txtBoxSearch.Text == "")
            {
                MessageBox.Show("Khung search còn trống", "Thống báo");
            }
            else
            {
                isSearching = true;
                trangHienTai = 0;
                GlobalItem.lstSanPham = Data_handle.Data_interface.database.SANPHAMs
                    .Where(x => x.TenSP.Contains(txtBoxSearch.Text) && x.TinhTrang == true).ToList();
                if (GlobalItem.lstSanPham.Count() > 0)
                {                    
                    soTrang = GlobalItem.lstSanPham.Count() / soPhanTu;
                    if(GlobalItem.lstSanPham.Count() % soPhanTu > 0)
                    {
                        soTrang++;
                    }
                    updateListSanPham();
                    dataGridProduct.ItemsSource = lstCurrentProduct;
                    dataGridProduct.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Không tồn tại sản phẩm cần tìm", "Thông báo");
                    GlobalItem.lstSanPham = Data_handle.Data_interface.database.SANPHAMs.Where(x=>x.TinhTrang == true).ToList();
                    soTrang = GlobalItem.lstSanPham.Count() / soPhanTu;
                    if (GlobalItem.lstSanPham.Count() % soPhanTu > 0)
                    {
                        soTrang++;
                    }
                    updateListSanPham();
                    dataGridProduct.ItemsSource = lstCurrentProduct;
                    txtBoxSearch.Text = null;
                }
            }
        }

        // Xử lý button refresh
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if(isSearching == true)
            {
                isSearching = false;
                trangHienTai = 0;                
                GlobalItem.lstSanPham = Data_handle.Data_interface.database.SANPHAMs.Where(x=>x.TinhTrang==true).ToList();
                soTrang = GlobalItem.lstSanPham.Count() / soPhanTu;
                if (GlobalItem.lstSanPham.Count() % soPhanTu > 0)
                {
                    soTrang++;
                }
                updateListSanPham();
                dataGridProduct.ItemsSource = lstCurrentProduct;
                dataGridProduct.Items.Refresh();

                

                // refresh tool bar
                txtBoxSearch.Text = null;
                cmbFilter.SelectedItem = null;
                cmbFilterValue.SelectedItem = null;
            }
        }

        // Xử lý sự kiện sau khi chọn bộ lọc
        private void cmbFilter_DropDownClosed(object sender, EventArgs e)
        {
            lstType.Clear();
            cmbFilterValue.ItemsSource = null;

            if (cmbFilter.SelectedIndex == 0)
            {
                foreach (var t in Data_handle.Data_interface.database.SANPHAMs.GroupBy(x=>x.MaLoai))
                {
                    lstType.Add(new item() { value = t.Key });
                }
            }
            else if (cmbFilter.SelectedIndex == 1)
            {
                foreach (var t in Data_handle.Data_interface.database.SANPHAMs.GroupBy(x=>x.DonGia))
                {
                    lstType.Add(new item() { value = t.Key.ToString()});
                }
            }
            else if (cmbFilter.SelectedIndex == 2)
            {
                StringBuilder dayPick = new StringBuilder();
                foreach (var t in Data_handle.Data_interface.database.SANPHAMs.GroupBy(x=>x.NgayNhap))
                {
                    // Cập nhật ngày tháng
                    dayPick.Append(t.Key.Value.Day);
                    dayPick.Append("/");
                    dayPick.Append(t.Key.Value.Month);
                    dayPick.Append("/");
                    dayPick.Append(t.Key.Value.Year);

                    lstType.Add(new item() { value = dayPick.ToString(), day = t.Key.Value });
                    dayPick.Clear();
                }
            }
            else
            {
                foreach (var t in Data_handle.Data_interface.database.SANPHAMs.GroupBy(x=>x.NhaCungCap))
                {
                    lstType.Add(new item() { value = t.Key });
                }
            }

            cmbFilterValue.ItemsSource = lstType;
            if (firstFilter == false)
            {
                cmbFilterValue.Items.Refresh();
            }
            else
            {
                firstFilter = true;
            }
        }

        // Xử lý button lọc
        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            if (cmbFilter.SelectedItem == null || cmbFilterValue.SelectedItem == null)
            {
                MessageBox.Show("Bạn vẫn chưa chọn hình thức lọc", "Thống báo");
            }
            else
            {
                isSearching = true;
                trangHienTai = 0;
                item t = (item)cmbFilterValue.SelectedItem;
                if (cmbFilter.SelectedIndex == 0)
                {// Lọc theo loại
                    GlobalItem.lstSanPham = Data_handle.Data_interface.database.SANPHAMs
                    .Where(x => x.MaLoai == t.value && x.TinhTrang == true).ToList();
                }
                else if (cmbFilter.SelectedIndex == 1)
                {// Lọc theo đợn giá
                    GlobalItem.lstSanPham = Data_handle.Data_interface.database.SANPHAMs
                    .Where(x => x.DonGia.ToString() == t.value && x.TinhTrang == true).ToList();
                }
                else if (cmbFilter.SelectedIndex == 2)
                {// Lọc theo ngày nhập
                    
                    GlobalItem.lstSanPham = Data_handle.Data_interface.database.SANPHAMs
                    .Where(x => x.NgayNhap == t.day && x.TinhTrang == true).ToList();
                }
                else
                {// Lọc theo nhà cung cấp
                    GlobalItem.lstSanPham = Data_handle.Data_interface.database.SANPHAMs
                     .Where(x => x.NhaCungCap == t.value && x.TinhTrang == true).ToList();
                }

                soTrang = GlobalItem.lstSanPham.Count() / soPhanTu;
                if (GlobalItem.lstSanPham.Count() % soPhanTu > 0)
                {
                    soTrang++;
                }
                updateListSanPham();
                dataGridProduct.ItemsSource = lstCurrentProduct;
                dataGridProduct.Items.Refresh();
            }
        }

        // Trang trước
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            // Giao diện
            trangHienTai--;
            lblCurrentPage.Content = (trangHienTai + 1).ToString();
            btnNext.Visibility = Visibility.Visible;
            if (trangHienTai == 0)
            {
                btnPrev.Visibility = Visibility.Hidden;
            }
            // Xử lý
            lstCurrentProduct = GlobalItem.lstSanPham.Skip(trangHienTai * soPhanTu).Take(soPhanTu).ToList();
            dataGridProduct.ItemsSource = lstCurrentProduct;
            dataGridProduct.Items.Refresh();
        }

        // Trang sau
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            // Giao diện
            trangHienTai++;
            if(trangHienTai == soTrang)
            {
                return;
            }
            lblCurrentPage.Content = (trangHienTai + 1).ToString();
            btnPrev.Visibility = Visibility.Visible;
            if(trangHienTai == soTrang - 1)
            {
                btnNext.Visibility = Visibility.Hidden;
            }

            // Xử lý
            lstCurrentProduct = GlobalItem.lstSanPham.Skip(trangHienTai * soPhanTu).Take(soPhanTu).ToList();
            dataGridProduct.ItemsSource = lstCurrentProduct;
            dataGridProduct.Items.Refresh();
        }

        private void updateListSanPham()
        {
            if(soTrang == 1)
            {
                btnNext.Visibility = Visibility.Hidden;
                btnPrev.Visibility = Visibility.Hidden;
            }
            else if(trangHienTai == 0)
            {
                btnPrev.Visibility = Visibility.Hidden;
                btnNext.Visibility = Visibility.Visible;
            }
            else if(trangHienTai == soTrang - 1)
            {
                btnPrev.Visibility = Visibility.Visible;
                btnNext.Visibility = Visibility.Hidden;
            }
            else
            {
                btnNext.Visibility = Visibility.Visible;
                btnPrev.Visibility = Visibility.Visible;
            }
            lstCurrentProduct = GlobalItem.lstSanPham.Skip(trangHienTai * soPhanTu).Take(soPhanTu).ToList();
            lblCurrentPage.Content = (trangHienTai + 1).ToString();
            lblTotalPage.Content = soTrang.ToString();
        }
    }

    class item
    {
        public string value { get; set; }
        public DateTime day { get; set; }
    }
}
