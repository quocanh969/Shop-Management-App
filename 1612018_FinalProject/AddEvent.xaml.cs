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
    /// Interaction logic for AddEvent.xaml
    /// </summary>
    public partial class AddEvent : Window
    {
        private DateTime ngayBD { get; set; }
        private DateTime ngayKT { get; set; }

        public AddEvent()
        {
            InitializeComponent();

            // Xem chi tiết sản phẩm
            if (GlobalItem.isEventLooking == true)
            {// Xem
                // Vô hiệu hóa khả năng chỉnh sữa thông tin trong chế độ xem
                //bằng cách dựng lên màn che trong suốt
                Man_che.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Hidden;

                // Cập nhật thông tin
                txtBoxTenEvent.Text = GlobalItem.selectedEvent.TenKM;
                CheckState.IsChecked = GlobalItem.selectedEvent.TrangThai;

                // ngày bắt đầu
                ngayBD = GlobalItem.selectedEvent.NgayBD.Value;
                StringBuilder dayPick_1 = new StringBuilder();
                // Cập nhật ngày tháng
                dayPick_1.Append(ngayBD.Day);
                dayPick_1.Append("/");
                dayPick_1.Append(ngayBD.Month);
                dayPick_1.Append("/");
                dayPick_1.Append(ngayBD.Year);

                dateStart.Text = dayPick_1.ToString();

                // ngày kết thúc
                ngayKT = GlobalItem.selectedEvent.NgayKT.Value;
                StringBuilder dayPick_2 = new StringBuilder();
                // Cập nhật ngày tháng
                dayPick_2.Append(ngayKT.Day);
                dayPick_2.Append("/");
                dayPick_2.Append(ngayKT.Month);
                dayPick_2.Append("/");
                dayPick_2.Append(ngayKT.Year);

                dateEnd.Text = dayPick_2.ToString();

                txtBoxSoLuong.Text = GlobalItem.selectedEvent.SoLuongCanMua.ToString();
                txtBoxPhanTram.Text = GlobalItem.selectedEvent.PhanTram.ToString();
                txtBoxGhiChu.Text = GlobalItem.selectedEvent.GhiChu;
            }
            else if (GlobalItem.isEventEditting == true) // Chỉnh sữa sản phẩm
            {// Sửa
                // Cập nhật thông tin
                txtBoxTenEvent.Text = GlobalItem.selectedEvent.TenKM;
                CheckState.IsChecked = GlobalItem.selectedEvent.TrangThai;

                // ngày bắt đầu
                ngayBD = GlobalItem.selectedEvent.NgayBD.Value;
                StringBuilder dayPick_1 = new StringBuilder();
                // Cập nhật ngày tháng
                dayPick_1.Append(ngayBD.Day);
                dayPick_1.Append("/");
                dayPick_1.Append(ngayBD.Month);
                dayPick_1.Append("/");
                dayPick_1.Append(ngayBD.Year);

                dateStart.Text = dayPick_1.ToString();

                // ngày kết thúc
                ngayKT = GlobalItem.selectedEvent.NgayKT.Value;
                StringBuilder dayPick_2 = new StringBuilder();
                // Cập nhật ngày tháng
                dayPick_2.Append(ngayKT.Day);
                dayPick_2.Append("/");
                dayPick_2.Append(ngayKT.Month);
                dayPick_2.Append("/");
                dayPick_2.Append(ngayKT.Year);

                dateEnd.Text = dayPick_2.ToString();

                txtBoxSoLuong.Text = GlobalItem.selectedEvent.SoLuongCanMua.ToString();
                txtBoxPhanTram.Text = GlobalItem.selectedEvent.PhanTram.ToString();
                txtBoxGhiChu.Text = GlobalItem.selectedEvent.GhiChu;
            }
            else
            {// Thêm
                ngayBD = DateTime.Today;
                ngayKT = DateTime.Today;
                StartCalendar.SelectedDate = DateTime.Today;
                EndCalendar.SelectedDate = DateTime.Today;
            }
        }



        private void btnOK_Click(object sender, RoutedEventArgs e)
        {// Xử lý button OK

            ErrorNotice.Visibility = Visibility.Hidden;// Kiểm tra lại

            // Kiểm tra ngày
            if(ngayBD > ngayKT)
            {
                ErrorNotice.Content = "*Ngày kết thúc ở trước ngày bắt đầu";
                ErrorNotice.Visibility = Visibility.Visible;
                return;
            }

            if (ngayKT < DateTime.Today)
            {
                ErrorNotice.Content = "*Sự kiện đã kết thúc trước hiện tại";
                ErrorNotice.Visibility = Visibility.Visible;
                return;
            }

            if (GlobalItem.isEventLooking == true)
            {// Xem
                this.Close();
            }
            else if (GlobalItem.isEventEditting == true)
            {// Sửa
                var edittedEvent = Data_handle.Data_interface.database.KHUYENMAIs.Where(x => x.MaKM == GlobalItem.selectedEvent.MaKM).SingleOrDefault();

                edittedEvent.TenKM = txtBoxTenEvent.Text;
                edittedEvent.TrangThai = CheckState.IsChecked;

                edittedEvent.NgayBD = ngayBD;
                edittedEvent.NgayKT = ngayKT;

                // Kiểm tra và tạo số lượng sản phẩm cần mua của event
                int temp = 0;
                if (int.TryParse(txtBoxSoLuong.Text, out temp) == false)
                {
                    // Xử lý lỗi
                    ErrorNotice.Content = "*Định dạng số lượng không đúng";
                    ErrorNotice.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    edittedEvent.SoLuongCanMua = temp;
                }

                // Kiểm tra và tạo phần trăm giảm giá
                int temp_1 = 0;
                if (int.TryParse(txtBoxPhanTram.Text, out temp_1) == false)
                {
                    // Xử lý lỗi
                    ErrorNotice.Content = "*Định dạng phầm trăm giảm giá không đúng";
                    ErrorNotice.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    edittedEvent.PhanTram = temp_1;
                }

                edittedEvent.GhiChu = txtBoxGhiChu.Text;

                    Data_handle.Data_interface.database.SaveChanges();                
            }
            else
            {// Thêm

                // Tạo biến sản phẩm mới
                KHUYENMAI newEvent = new KHUYENMAI()
                {
                    TenKM = txtBoxTenEvent.Text,
                    TrangThai = CheckState.IsChecked,
                    NgayBD = ngayBD,
                    NgayKT = ngayKT,
                    SoLuongCanMua = 0,
                    PhanTram = 0,
                    GhiChu = txtBoxGhiChu.Text,
                };

                // Kiểm tra và tạo số lượng sản phẩm cần mua của event
                int temp = 0;
                if (int.TryParse(txtBoxSoLuong.Text, out temp) == false)
                {
                    // Xử lý lỗi
                    ErrorNotice.Content = "*Định dạng số lượng không đúng";
                    ErrorNotice.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    newEvent.SoLuongCanMua = temp;
                }

                // Kiểm tra và tạo phần trăm giảm giá
                int temp_1 = 0;
                if (int.TryParse(txtBoxPhanTram.Text, out temp_1) == false)
                {
                    // Xử lý lỗi
                    ErrorNotice.Content = "*Định dạng phần trăm giảm giá không đúng";
                    ErrorNotice.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    newEvent.PhanTram = temp_1;
                }

                // Thêm mới
                // vào database
                    Data_handle.Data_interface.database.KHUYENMAIs.Add(newEvent);
                    Data_handle.Data_interface.database.SaveChanges();
                // vào danh sách hiển thị vào đầu danh sach
                GlobalItem.lstKhuyenMai.Insert(0, newEvent);
               
            }
            

            // Tắt màn hình khi không có lỗi
            if (ErrorNotice.Visibility == Visibility.Hidden)
            {
                this.Close();
            }
        }



        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Chọn ngày
        private void ngayBDPopUp_Closed(object sender, RoutedEventArgs e)
        {// Cập nhật ngày bắt đầu sự kiên lên textbox
            ngayBD = StartCalendar.SelectedDate.Value;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayBD.Day);
            dayPick.Append("/");
            dayPick.Append(ngayBD.Month);
            dayPick.Append("/");
            dayPick.Append(ngayBD.Year);

            dateStart.Text = dayPick.ToString();
        }

        private void ngayKTPopUp_Closed(object sender, RoutedEventArgs e)
        {// Cập nhật ngày kết thúc sự kiện lên textbox
            ngayKT = EndCalendar.SelectedDate.Value;

            StringBuilder dayPick = new StringBuilder();
            // Cập nhật ngày tháng
            dayPick.Append(ngayKT.Day);
            dayPick.Append("/");
            dayPick.Append(ngayKT.Month);
            dayPick.Append("/");
            dayPick.Append(ngayKT.Year);

            dateEnd.Text = dayPick.ToString();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            GlobalItem.isEventLooking = false;
            GlobalItem.isEventEditting = false;
            GlobalItem.isEventAdding = false;
            ErrorNotice.Visibility = Visibility.Hidden;
        }
    }
}
