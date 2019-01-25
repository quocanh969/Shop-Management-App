using _1612018_FinalProject.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1612018_FinalProject
{
    class GlobalItem
    {
        public static NHANVIEN user;
        public static string username;
        public static string password;
        public static bool isAdmin;
                

        // Danh sách database
        public static List<SANPHAM> lstSanPham = new List<SANPHAM>();
        public static List<SANPHAM> lstSanPhamRac = new List<SANPHAM>();
        public static List<NHANVIEN> lstNhanVien = new List<NHANVIEN>();
        public static List<NHANVIEN> lstNhanVienBiDuoi = new List<NHANVIEN>();
        public static List<KHUYENMAI> lstKhuyenMai = new List<KHUYENMAI>();
        public static List<KHUYENMAI> lstKhuyenMaiKetThuc = new List<KHUYENMAI>();

        public static List<HOADON> gioHang = new List<HOADON>();
        public static List<matHang> lstMatHang = new List<matHang>();

        // Tab_trang_chu
        public static bool isLooking = false;
        public static bool isEditing = false;        
        
        public static SANPHAM selectedProduct;             
        

        // Tab_khuyen_mai
        public static bool isEventLooking = false;
        public static bool isEventEditting = false;
        public static bool isEventAdding = false;
        public static KHUYENMAI selectedEvent;

        // Tab_quan_ly
        public static bool isUser = false;
        public static bool isUserLooking = false;
        public static bool isUserEditting = false;
        public static bool isUserAdding = false;

        public static USER createdUser = null;

        public static NHANVIEN selectedStaff;
    }
}
