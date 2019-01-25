using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1612018_FinalProject.model
{
    class Data_handle
    {
        // Đây là biến chịu trách nhiệm lưu trữ Data chung
        // mục tiêu là tạo data 1 lần thôi
        private static Data_handle Data_layer;

        // Tạo biến public static để có thể tham chiếu trực tiếp từ code vào data
        public static Data_handle Data_interface
        {
            get
            {
                if(Data_layer == null)
                {
                    Data_layer = new Data_handle();                    
                }
                return Data_layer;
            }
            set
            {
                Data_layer = value;
            }
        }

        // Tham chiếu database từ SQL server
        public QUAN_LY_CUA_HANG_QUAN_AOEntities database { get; set; }        
        

        Data_handle()
        {
            database = new QUAN_LY_CUA_HANG_QUAN_AOEntities();            
        }
    }
}
