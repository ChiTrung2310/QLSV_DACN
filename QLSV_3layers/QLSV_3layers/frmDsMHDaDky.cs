using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV_3layers
{
    public partial class frmDsMHDaDky : Form
    {
        private string masv;
        public frmDsMHDaDky(string masv)
        {
            this.masv = masv;
            InitializeComponent();
        }

        private void frmDsMHDaDky_Load(object sender, EventArgs e)
        {
            LoadMonDky();
        }

        private void LoadMonDky()
        {
            List<CustomParameter> lst = new List<CustomParameter>()
            {
                new CustomParameter()
                {
                    key = "@masinhvien",
                    value = masv
                }
            };

            dgvDSMHDky.DataSource = new Database().SelectData("monDaDKy", lst);

            // đặt lại tên các cột hiển thị
            dgvDSMHDky.Columns["malophoc"].HeaderText = "Mã lớp học";
            dgvDSMHDky.Columns["tenmonhoc"].HeaderText = "Tên môn học";
            dgvDSMHDky.Columns["gvien"].HeaderText = "Giáo viên";
            dgvDSMHDky.Columns["sotinchi"].HeaderText = "Số TC";
            dgvDSMHDky.Columns["diemthilan1"].HeaderText = "Điểm thi lần 1";
            dgvDSMHDky.Columns["diemthilan2"].HeaderText = "Điểm thi lần 2";
        }

        private void btnDkyMoi_Click(object sender, EventArgs e)
        {
            new frmDangkyMonhoc(masv).ShowDialog();
            LoadMonDky(); // sau khi đăng ký thành công hiện lên trên gridview đã đăng ký
        }

    }
}
