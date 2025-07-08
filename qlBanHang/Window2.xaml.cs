using System;
using System.Collections.Generic;
using System.Data.Linq;
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

namespace qlBanHang
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        SanPhamDataContext dc = new SanPhamDataContext();
        Table<SANPHAM> SANPHAMs;
        Table<HOADON> HOADONs;
        Table<KHACHHANG> KHACHHANGs;
        public Window2()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(Windown_load);
        }

        private void Windown_load(object sender, RoutedEventArgs e)
        {
            loadHD();
        }

        private void loadHD()
        {
            HOADONs = dc.GetTable<HOADON>();
            SANPHAMs = dc.GetTable<SANPHAM>();
            KHACHHANGs = dc.GetTable<KHACHHANG>();

            var query = from hd in HOADONs
                        join sp in SANPHAMs on hd.MaSP equals sp.MaSP
                        join kh in KHACHHANGs on hd.MaKH equals kh.MaKH
                        select new
                        {
                            MaHD = hd.MaHD,
                            TenKH = kh.HoTen,
                            TenSP = sp.TenSP,
                            NgayLapHD = hd.NgayLapHD,
                            SoLuong = hd.SoLuong,
                            DonGia = sp.DonGia,
                            ThanhTien = hd.SoLuong * sp.DonGia
                        };

            dataGrid.ItemsSource = query.ToList(); // .ToList() giúp tránh deferred execution và binding lỗi

        }

    }
}
