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
        private Table<SANPHAM> SANPHAMs;
        private Table<HOADON> HOADONs;
        private Table<KHACHHANG> KHACHHANGs;
        public Window2()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(Windown_load);

            btnTinhTien.Click += new RoutedEventHandler(TinhTien);

            dataGrid.SelectionChanged += new SelectionChangedEventHandler(data_click);
        }

        //private void TinhTien(object sender, RoutedEventArgs e)
        //{
        //    if (!int.TryParse(txtMaHD.Text, out int maHD))
        //    {
        //        MessageBox.Show("Ma hoa don phai la so nguyen!");
        //        return;
        //    }
        //    var query = from h in HOADONs
        //                join s in SANPHAMs on h.MaSP equals s.MaSP
        //                join k in KHACHHANGs on h.MaKH equals k.MaKH
        //                where h.MaHD == maHD
        //                select new HoaDonDTO
        //                {
        //                    MaHD = h.MaHD.ToString(),
        //                    TenKH = k.HoTen,
        //                    TenSP = s.TenSP,
        //                    NgayLapHD = h.NgayLapHD,
        //                    SoLuong = h.SoLuong,
        //                    DonGia = s.DonGia,
        //                    ThanhTien = h.SoLuong.GetValueOrDefault() * s.DonGia.GetValueOrDefault()
        //                };
        //    dataGrid.ItemsSource = query.ToList(); // Cập nhật DataGrid với kết quả truy vấn
        //}

        private void TinhTien(object sender, RoutedEventArgs e)
        {
            if(!int.TryParse(txtMaHD.Text, out int maHD))
            {
                MessageBox.Show("Ma hoa don phai la so nguyen!");
                return;
            }
            string tenSPCanTinh = "dau an"; // c2: string tenSPCanTinh = txtTenSP.Text.Trim();
            var query = from h in HOADONs
                        join s in SANPHAMs on h.MaSP equals s.MaSP
                        join k in KHACHHANGs on h.MaKH equals k.MaKH
                        where h.MaHD == maHD && s.TenSP.ToLower().Contains(tenSPCanTinh.ToLower())
                        select new HoaDonDTO
                        {
                            MaHD = h.MaHD.ToString(),
                            TenKH = k.HoTen,
                            TenSP = s.TenSP,
                            NgayLapHD = h.NgayLapHD,
                            SoLuong = h.SoLuong,
                            DonGia = s.DonGia,
                            ThanhTien = h.SoLuong.GetValueOrDefault() * s.DonGia.GetValueOrDefault()
                        };
            var sp = query.FirstOrDefault();
            if (sp != null)
            {
                // Hiển thị kết quả lên TextBox
                txtTenSP.Text = sp.TenSP;
                txtSoLuong.Text = sp.SoLuong?.ToString() ?? "0";
                txtDonGia.Text = sp.DonGia?.ToString("0.00") ?? "0.00";
                txtThanhTien.Text = sp.ThanhTien.ToString("0.00");

                // Optionally show in MessageBox
                MessageBox.Show($"Thành tiền của sản phẩm '{sp.TenSP}' là {sp.ThanhTien:N0} đ", "Kết quả", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Không tìm thấy sản phẩm '{tenSPCanTinh}' trong hóa đơn {maHD}.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //private void data_click(object sender, SelectionChangedEventArgs e)
        //{
        //    HoaDonDTO item = dataGrid.SelectedItem as HoaDonDTO;

        //    if (item != null)
        //    {
        //        txtMaHD.Text = item.MaHD;
        //        txtTenKH.Text = item.TenKH;
        //        txtTenSP.Text = item.TenSP;
        //        dtpNgayLapHD.SelectedDate = item.NgayLapHD;
        //        txtSoLuong.Text = item.SoLuong?.ToString() ?? "0";
        //        txtDonGia.Text = item.DonGia?.ToString("0.00") ?? "0.00";
        //        txtThanhTien.Text = item.ThanhTien.ToString("0.00");
        //    }

        //}

        private void data_click(object sender, SelectionChangedEventArgs e)
        {
            if (dataGrid.SelectedItem is HoaDonDTO item)
            {
                // Hiển thị dữ liệu lên các TextBox
                txtMaHD.Text = item.MaHD;
                txtTenKH.Text = item.TenKH;
                txtTenSP.Text = item.TenSP;
                dtpNgayLapHD.SelectedDate = item.NgayLapHD;
                txtSoLuong.Text = item.SoLuong?.ToString() ?? "0";
                txtDonGia.Text = item.DonGia?.ToString("0.00") ?? "0.00";
                txtThanhTien.Text = item.ThanhTien.ToString("0.00");

                // Lấy MaHD cần tìm
                if (int.TryParse(item.MaHD, out int maHD))
                {
                    // Tính tổng tiền cho tất cả sản phẩm thuộc cùng hóa đơn
                    var tongTien = (from h in HOADONs
                                    join s in SANPHAMs on h.MaSP equals s.MaSP
                                    where h.MaHD == maHD
                                    select h.SoLuong.GetValueOrDefault() * s.DonGia.GetValueOrDefault()).Sum();

                    // Hiển thị tổng tiền (ví dụ lên MessageBox hoặc thêm 1 TextBox riêng)
                    MessageBox.Show($"Tổng tiền hóa đơn {maHD} là: {tongTien:N0} đ", "Tổng tiền", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Hoặc hiển thị vào một TextBox:
                    // txtTongTien.Text = tongTien.ToString("N0");
                }
            }
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
                        select new HoaDonDTO
                        {
                            MaHD = hd.MaHD.ToString(),
                            TenKH = kh.HoTen,
                            TenSP = sp.TenSP,
                            NgayLapHD = hd.NgayLapHD,
                            SoLuong = hd.SoLuong,
                            DonGia = sp.DonGia,
                            ThanhTien = hd.SoLuong.GetValueOrDefault() * sp.DonGia.GetValueOrDefault()
                        };

            dataGrid.ItemsSource = query.ToList(); // .ToList() giúp tránh deferred execution và binding lỗi

        }

    }
}
