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
    public partial class frmDsLopHoc : Form
    {
        public frmDsLopHoc()
        {
            InitializeComponent();
        }
        private string tukhoa = "";
        private void frmDsLopHoc_Load(object sender, EventArgs e)
        {
            loadDSLH();
        }
        private void loadDSLH()
        {
            string sql = "allLopHoc";
            List<CustomParameter> lstPara = new List<CustomParameter>()
            {
                new CustomParameter()
                {
                    key = "@tukhoa",
                    value = tukhoa
                }
            };
            dgvLopHoc.DataSource = new Database().SelectData(sql,lstPara);

            //đặt tên cột
            dgvLopHoc.Columns["malophoc"].HeaderText = "Mã lớp học";
            dgvLopHoc.Columns["mamonhoc"].HeaderText = "Mã môn học";
            dgvLopHoc.Columns["tenmonhoc"].HeaderText = "Tên môn học";
            dgvLopHoc.Columns["sotinchi"].HeaderText = "Số TC";
            dgvLopHoc.Columns["gvien"].HeaderText = "Giáo viên";
            dgvLopHoc.Columns["trangthai"].HeaderText = "Trạng thái";

        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            tukhoa = txtTimKiem.Text;
            loadDSLH();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            new frmLopHoc(null).ShowDialog();
            loadDSLH();
        }

        /// <summary>
        /// Khi trực tiếp double click vào datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLopHoc_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                new frmLopHoc(dgvLopHoc.Rows[e.RowIndex].Cells["malophoc"].Value.ToString()).ShowDialog();
                loadDSLH();
            }
        }
    }
}
