using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qlBanHang
{
    public class HoaDonDTO
    {
        public string MaHD { get; set; }
        public string TenKH { get; set; }
        public string TenSP { get; set; }
        public DateTime? NgayLapHD { get; set; }
        public int? SoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}
