using DE1_BLL;
using DE1_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DE1_GUI
{
    public partial class Form1 : Form
    {
        private SinhvienBLL sinhvienBLL = new SinhvienBLL();
        private LopBLL lopBLL = new LopBLL();
        public Form1()
        {
            InitializeComponent();
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void SetupListView()
        {
            lvSinhvien.View = View.Details; // Chế độ xem chi tiết
            lvSinhvien.FullRowSelect = true; // Chọn toàn bộ hàng
            lvSinhvien.GridLines = true; // Hiển thị đường kẻ bảng

            // Thêm các cột vào ListView
            lvSinhvien.Columns.Add("Mã SV", 100);
            lvSinhvien.Columns.Add("Họ và tên", 200);
            lvSinhvien.Columns.Add("Ngày sinh", 150);
            lvSinhvien.Columns.Add("Lớp", 150);
        }
        private void LoadLopList()
        {
            var lops = lopBLL.GetAllLop();
            cboLop.Items.Clear();

            foreach (var lop in lops)
            {
                cboLop.Items.Add(lop.TenLop);
            }
        }
        private void LoadSinhvienList()
        {
            lvSinhvien.Items.Clear(); // Xóa dữ liệu cũ trong ListView

            var sinhviens = sinhvienBLL.GetAllSinhvien();
            foreach (var sv in sinhviens)
            {
                var lop = lopBLL.GetLopById(sv.MaLop); // Lấy lớp dựa trên mã lớp của sinh viên

                var listItem = new ListViewItem(sv.MaSV); // Cột Mã SV
                listItem.SubItems.Add(sv.HoTenSV); // Cột Họ và tên
                listItem.SubItems.Add(sv.NgaySinh?.ToString("dd/MM/yyyy")); // Cột Ngày sinh
                listItem.SubItems.Add(lop?.TenLop); // Hiển thị tên lớp thay vì mã lớp
                lvSinhvien.Items.Add(listItem); // Thêm dòng vào ListView
            }
        }



        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {

        }

        private void dtNgaysinh_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtHoten_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị hộp thoại xác nhận trước khi xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                // Nếu người dùng chọn Yes, thực hiện xóa
                if (result == DialogResult.Yes)
                {
                    string maSV = txtMaSV.Text;
                    bool success = sinhvienBLL.DeleteSinhvien(maSV);

                    if (success)
                    {
                        MessageBox.Show("Xóa sinh viên thành công!");
                        LoadSinhvienList(); // Tải lại danh sách sau khi xóa
                    }
                    else
                    {
                        MessageBox.Show("Xóa sinh viên thất bại.");
                    }
                }
                // Nếu người dùng chọn No, không làm gì thêm
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }

        }


        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboLop.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy mã lớp từ tên lớp được chọn
                string tenLop = cboLop.SelectedItem.ToString();
                string maLop = lopBLL.GetMaLopByTenLop(tenLop);
                if (maLop == null)
                {
                    MessageBox.Show("Không tìm thấy mã lớp tương ứng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SinhVien sv = new SinhVien
                {
                    MaSV = txtMaSV.Text,
                    HoTenSV = txtHoten.Text,
                    NgaySinh = dtNgaysinh.Value,
                    MaLop = maLop // Lưu mã lớp vào cơ sở dữ liệu
                };

                bool success = sinhvienBLL.UpdateSinhvien(sv);
                if (success)
                {
                    MessageBox.Show("Cập nhật sinh viên thành công!");
                    LoadSinhvienList();
                }
                else
                {
                    MessageBox.Show("Cập nhật sinh viên thất bại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng chọn Yes, thoát chương trình
            if (result == DialogResult.Yes)
            {
                Application.Exit(); // Thoát chương trình
            }
        }

        private void bt_Tim_Click(object sender, EventArgs e)
        {
            try
            {
                string tenSV = txtTim.Text.ToLower(); // Lấy tên sinh viên từ TextBox và chuyển về chữ thường
                var sinhviens = sinhvienBLL.GetAllSinhvien().Where(sv => sv.HoTenSV.ToLower().Contains(tenSV)).ToList(); // Tìm sinh viên có tên chứa từ khóa tìm kiếm
                // Xóa dữ liệu cũ trong ListView
                lvSinhvien.Items.Clear();

                // Nếu tìm thấy sinh viên thì hiển thị
                if (sinhviens.Count > 0)
                {
                    foreach (var sv in sinhviens)
                    {
                        var lop = lopBLL.GetLopById(sv.MaLop); // Lấy lớp dựa trên mã lớp của sinh viên

                        var listItem = new ListViewItem(sv.MaSV); // Cột Mã SV
                        listItem.SubItems.Add(sv.HoTenSV); // Cột Họ và tên
                        listItem.SubItems.Add(sv.NgaySinh?.ToString("dd/MM/yyyy")); // Cột Ngày sinh
                        listItem.SubItems.Add(lop?.TenLop); // Hiển thị tên lớp thay vì mã lớp
                        lvSinhvien.Items.Add(listItem); // Thêm dòng vào ListView
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên."); // Thông báo nếu không tìm thấy sinh viên
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message); // Xử lý ngoại lệ
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSinhvienList();
            LoadLopList();
            SetupListView();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận khi người dùng bấm nút Close (X)
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Nếu người dùng chọn Yes, tiếp tục thoát
            if (result == DialogResult.Yes)
            {
                e.Cancel = false; // Tiếp tục thoát
            }
            // Nếu người dùng chọn No, hủy bỏ sự kiện đóng cửa sổ
            else
            {
                e.Cancel = true; // Hủy bỏ sự kiện đóng cửa sổ, không thoát
            }
        }

        private void lvSinhvien_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lvSinhvien.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvSinhvien.SelectedItems[0];
                string maSV = selectedItem.SubItems[0].Text; // Lấy mã sinh viên từ dòng đã chọn
                SinhVien sv = sinhvienBLL.GetSinhvienById(maSV);
                if (sv != null)
                {
                    txtMaSV.Text = sv.MaSV;
                    txtHoten.Text = sv.HoTenSV;
                    dtNgaysinh.Value = sv.NgaySinh ?? DateTime.Now;

                    var lop = lopBLL.GetLopById(sv.MaLop); // Lấy thông tin lớp từ mã lớp của sinh viên
                    if (lop != null)
                    {
                        cboLop.SelectedItem = lop.TenLop; // Hiển thị tên lớp trong ComboBox
                    }
                }
            }
        }

        private void btThem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (cboLop.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy mã lớp từ tên lớp được chọn
                string tenLop = cboLop.SelectedItem.ToString();
                string maLop = lopBLL.GetMaLopByTenLop(tenLop);
                if (maLop == null)
                {
                    MessageBox.Show("Không tìm thấy mã lớp tương ứng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SinhVien sv = new SinhVien
                {
                    MaSV = txtMaSV.Text,
                    HoTenSV = txtHoten.Text,
                    NgaySinh = dtNgaysinh.Value,
                    MaLop = maLop // Lưu mã lớp vào cơ sở dữ liệu
                };

                bool success = sinhvienBLL.AddSinhvien(sv);
                if (success)
                {
                    MessageBox.Show("Thêm sinh viên thành công!");
                    LoadSinhvienList();
                }
                else
                {
                    MessageBox.Show("Thêm sinh viên thất bại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboLop.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn lớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Lấy mã lớp từ tên lớp được chọn
                string tenLop = cboLop.SelectedItem.ToString();
                string maLop = lopBLL.GetMaLopByTenLop(tenLop);
                if (maLop == null)
                {
                    MessageBox.Show("Không tìm thấy mã lớp tương ứng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Tạo đối tượng sinh viên
                SinhVien sv = new SinhVien
                {
                    MaSV = txtMaSV.Text,
                    HoTenSV = txtHoten.Text,
                    NgaySinh = dtNgaysinh.Value,
                    MaLop = maLop // Lưu mã lớp vào cơ sở dữ liệu
                };

                // Cập nhật sinh viên vào CSDL
                bool success = sinhvienBLL.UpdateSinhvien(sv);
                if (success)
                {
                    MessageBox.Show("Lưu thông tin sinh viên thành công!");
                    LoadSinhvienList();
                }
                else
                {
                    MessageBox.Show("Lưu thông tin sinh viên thất bại.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnKLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra xem có sinh viên nào được chọn không
                if (lvSinhvien.SelectedItems.Count > 0)
                {
                    ListViewItem selectedItem = lvSinhvien.SelectedItems[0];
                    string maSV = selectedItem.SubItems[0].Text; // Lấy mã sinh viên từ dòng đã chọn

                    // Lấy thông tin sinh viên từ BLL
                    SinhVien sv = sinhvienBLL.GetSinhvienById(maSV);
                    if (sv != null)
                    {
                        // Khôi phục dữ liệu về trạng thái ban đầu
                        txtMaSV.Text = sv.MaSV;
                        txtHoten.Text = sv.HoTenSV;
                        dtNgaysinh.Value = sv.NgaySinh ?? DateTime.Now;

                        // Lấy và hiển thị thông tin lớp
                        var lop = lopBLL.GetLopById(sv.MaLop);
                        if (lop != null)
                        {
                            cboLop.SelectedItem = lop.TenLop;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn sinh viên từ danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn hủy bỏ các thay đổi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                // Thực hiện logic không lưu
            }
        }
    }
}
