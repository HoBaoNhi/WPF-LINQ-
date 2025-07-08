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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        SanPhamDataContext dc = new SanPhamDataContext();
        Table<SANPHAM> SANPHAMs;
        Table<HOADON> HOADONs;
        Table<KHACHHANG> KHACHHANGs;
        public Window1()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(Windown_load);

            btnThem.Click += new RoutedEventHandler(ThemSP);
            btnXoa.Click += new RoutedEventHandler(XoaSP);
            btnCapNhat.Click += new RoutedEventHandler(CapNhatSP);
            btnLamMoi.Click += new RoutedEventHandler(LamMoi);



            dataGrid.SelectionChanged += new SelectionChangedEventHandler(data_click);
        }

            private void LamMoi(object sender, RoutedEventArgs e)
            {
                txtMaSP.Clear();
                txtTenSP.Clear();
                txtDVT.Clear();
                txtDG.Clear();
                txtMaSP.Focus();
                loadSP();
            }

        private void CapNhatSP(object sender, RoutedEventArgs e)
        {
            string maSP = txtMaSP.Text;
            var sp = dc.SANPHAMs.SingleOrDefault(n => n.MaSP == maSP);
            if (sp != null)
            {
                sp.TenSP = txtTenSP.Text;
                sp.DonViTinh = txtDVT.Text;
                decimal donGia;
                if (!decimal.TryParse(txtDG.Text, out donGia) || donGia < 0)
                {
                    MessageBox.Show("Đơn giá không hợp lệ!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                sp.DonGia = donGia;
            }
            // gan entity
            // var doadon = dc.HOADONs.SingleOrDefault(n=>n.MaHD==txtTenSP)
            dc.SubmitChanges();
            loadSP();
            MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void XoaSP(object sender, RoutedEventArgs e)
        {
            var query = from sp in SANPHAMs
                        where sp.MaSP == txtMaSP.Text
                        select sp;
            foreach (var s in query)
            {
                SANPHAMs.DeleteOnSubmit(s);
            }
            dc.SubmitChanges();
            loadSP();
        }

        private void data_click(object sender, SelectionChangedEventArgs e)
        {
            SANPHAM sp = dataGrid.SelectedItem as SANPHAM;
            if (sp != null)
            {
                txtMaSP.Text = sp.MaSP.ToString();
                txtTenSP.Text = sp.TenSP.ToString();
                txtDVT.Text = sp.DonViTinh.ToString();
                txtDG.Text = sp.DonGia.ToString();
            }
        }

        private void ThemSP(object sender, RoutedEventArgs e)
        {
            // Kiem tra MaSP khong duoc trung 
            string maSP = txtMaSP.Text.Trim();
            if (SANPHAMs.Any(n => n.MaSP == maSP))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Kiem tra TenSP khong duoc de trong
            string tenSP = txtTenSP.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenSP))
            {
                MessageBox.Show("Tên sản phẩm không được để trống!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // Kiem tra DonGia phai la so va >= 0
            decimal donGia;
            if (!decimal.TryParse(txtDG.Text, out donGia) || donGia < 0)
            {
                MessageBox.Show("Đơn giá không hợp lệ!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            SANPHAM sp = new SANPHAM();
            sp.MaSP = txtMaSP.Text;
            sp.TenSP = txtTenSP.Text;
            sp.DonViTinh = txtDVT.Text;
            sp.DonGia = donGia;

            SANPHAMs.InsertOnSubmit(sp);
            dc.SubmitChanges();
            loadSP();
        }

        public void loadSP()
        {
            SANPHAMs = dc.GetTable<SANPHAM>();
            var query = from sp in SANPHAMs
                        select sp;
            dataGrid.ItemsSource = query;
        }
        private void Windown_load(object sender, RoutedEventArgs e)
        {
            loadSP();
        }

    }
}
