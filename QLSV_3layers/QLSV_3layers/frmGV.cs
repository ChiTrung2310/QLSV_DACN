using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSV_3layers
{
    public partial class frmGV : Form
    {
        public frmGV(string mgv)
        {
            this.mgv = mgv;
            InitializeComponent();
        }
        private string mgv;
        private string nguoithucthi = "admin";
        private void frmGV_Load(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(mgv))
            {
                this.Text = "Thêm mới giáo viên";
            }else
            {
                this.Text = "Cập nhật giáo viên";
                var r = new Database().Select("selectGV '" + int.Parse(mgv) + "'");
                txtHo.Text = r["ho"].ToString();
                txtTendem.Text = r["tendem"].ToString();
                txtTen.Text = r["ten"].ToString();
                rbtNam.Checked = r["gioitinh"].ToString() == "1" ? true : false;
                mtbNgaysinh.Text = r["ngsinh"].ToString();
                txtDienthoai.Text = r["dienthoai"].ToString();
                txtEmail.Text = r["email"].ToString();
                txtDiachi.Text = r["diachi"].ToString();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql = "";
            DateTime ngaysinh;
            List<CustomParameter> lstPara = new List<CustomParameter>();
            try
            {
                ngaysinh = DateTime.ParseExact(mtbNgaysinh.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch 
            {
                MessageBox.Show("Ngày sinh không hợp lệ");
                mtbNgaysinh.Select();
                return;
            }

            if(string.IsNullOrEmpty(mgv)) // nếu không có tham số truyền vào thì là thêm mới giáo viên
            {
                sql = "insertGV";
                lstPara.Add(new CustomParameter()
                {
                    key = "@nguoitao",
                    value = nguoithucthi
                });
            }
            else
            {
                sql = "updateGV";
                lstPara.Add(new CustomParameter()
                {
                    key = "@nguoicapnhat",
                    value = nguoithucthi
                });
                lstPara.Add(new CustomParameter() {
                    key = "@magiaovien",
                    value = mgv
                });
            }

            lstPara.Add(new CustomParameter()
            {
                key = "@ho",
                value = txtHo.Text
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@tendem",
                value = txtTendem.Text
            });
            lstPara.Add(new CustomParameter()
            {
                key = "@ten",
                value = txtTen.Text
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@ngaysinh",
                value = ngaysinh.ToString("yyyy-MM-dd")
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@gioitinh",
                value = rbtNam.Checked?"1":"0"
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@email",
                value = txtEmail.Text
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@dienthoai",
                value = txtDienthoai.Text
            });

            lstPara.Add(new CustomParameter()
            {
                key = "@diachi",
                value = txtDiachi.Text
            });


            var rs = new Database().ExeCute(sql,lstPara);
            if(rs == 1)
            {
                if(string.IsNullOrEmpty(mgv))
                {
                    MessageBox.Show("Thêm mới giáo viên thành công");
                }
                else
                {
                    MessageBox.Show("Cập nhật giáo viên thành công");
                }
                this.Dispose();
            }
            else
            {
                MessageBox.Show("Thực thi truy vấn thất bại");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Dispose(); // thoát form
        }

        /// <summary>
        /// Khi thực hiện việc xóa giáo viên
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXoa_Click(object sender, EventArgs e)
        {
            //  Xác nhận lại 1 lần trước khi xóa giáo viên
            if (
                    DialogResult.Yes ==
                    MessageBox.Show(
                                    "Bạn muốn xóa giáo viên [" + txtHo.Text + txtTendem.Text + txtTen.Text + "]?",
                                    "Xác nhận xóa giáo viên",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question
                                    )
                 )
            {
                string sql = "";

                List<CustomParameter> lstPara = new List<CustomParameter>();
                if (string.IsNullOrEmpty(mgv))//nếu không có mã giáo viên
                {
                    MessageBox.Show("Không có dữ liệu mã giáo viên");
                }

                else//nếu xóa giáo viên thì thực hiện những câu lệnh tiếp theo sau
                {
                    sql = "deleteGV";//gọi tới procedure xóa sinh viên
                    lstPara.Add(new CustomParameter()
                    {
                        key = "@magiaovien",
                        value = mgv
                    });

                    var rs = new Database().ExeCute(sql, lstPara);//truyền 2 tham số là câu lệnh sql
                                                                  //và danh sách các tham số
                    if (rs == 1)//nếu thực hiện thành công
                    {
                        if (string.IsNullOrEmpty(mgv))//nếu không có dữ liệu truyền vào là mã giáo viên
                        {
                            MessageBox.Show("Không có dữ liệu");
                        }
                        else//nếu xóa sinh viên
                        {
                            MessageBox.Show("Xóa thông tin giáo viên thành công");
                        }
                        this.Dispose();//đóng form sau khi xóa giáo viên thành công
                    }
                    else//nếu không thành công
                    {
                        MessageBox.Show("Thực thi thất bại");
                    }
                }
            }
        }
    }
}
