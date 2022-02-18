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
    public partial class frmLopHoc : Form
    {
        public frmLopHoc(string malophoc)
        {
            this.malophoc = malophoc;
            InitializeComponent();
        }
        private string malophoc;
        private Database db;
        private string nguoithuchien = "admin";
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmLopHoc_Load(object sender, EventArgs e)
        {
            db = new Database();
            List<CustomParameter> lst = new List<CustomParameter>()
            {
                new CustomParameter()
                {
                    key = "@tukhoa",
                    value=""
                }
            };
            //load dữ liệu cho 2 combobox môn học và giáo viên
            cbbMonhoc.DataSource = db.SelectData("selectAllMonHoc", lst);
            cbbMonhoc.DisplayMember = "tenmonhoc";//thuộc tính hiển thị của combobox
            cbbMonhoc.ValueMember = "mamonhoc";//giá trị (key) của combobox
            cbbMonhoc.SelectedIndex = -1;

            cbbGiaoVien.DataSource = db.SelectData("selectAllGV",lst);
            cbbGiaoVien.DisplayMember = "hoten";
            cbbGiaoVien.ValueMember = "magiaovien";
            cbbGiaoVien.SelectedIndex = -1;//set combobox không chọn giá trị nào


            if (string.IsNullOrEmpty(malophoc))
            {
                this.Text = "Thêm mới lớp học";
            }
            else
            {
                this.Text = "Cập nhật lớp học";
                var r = db.Select("exec selectLopHoc '"+malophoc+"'");
                cbbGiaoVien.SelectedValue = r["magiaovien"].ToString();
                cbbMonhoc.SelectedValue = r["mamonhoc"].ToString();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string sql = "";

            //ràng buộc điều kiện
            //phải chọn môn học và giáo viên giảng dạy mới tiếp tục thực hiện các câu lệnh phía dưới
            if (cbbMonhoc.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn môn học");
                return;
            }
            if (cbbGiaoVien.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn giáo viên");
                return;
            }//kết thúc ràng buộc

            List<CustomParameter> lst = new List<CustomParameter>();
            if(string.IsNullOrEmpty(malophoc))
            {
                sql = "insertLopHoc";
                lst.Add(new CustomParameter()
                {
                    key = "@nguoitao",
                    value = nguoithuchien
                });
            }
            else
            {
                sql = "updateLopHoc";
                lst.Add(new CustomParameter()
                {
                    key = "@nguoicapnhat",
                    value = nguoithuchien
                });
                lst.Add(new CustomParameter()
                {
                    key = "@malophoc",
                    value = malophoc
                });
            }



            lst.Add(new CustomParameter()
            {
                key = "@mamonhoc",
                value = cbbMonhoc.SelectedValue.ToString()//lấy giá trị đc chọn của combobox môn học
            });

            lst.Add(new CustomParameter()
            {
                key = "@magiaovien",
                value = cbbGiaoVien.SelectedValue.ToString()//lấy giá trị đc chọn của combobox giáo viên
            });

            var kq = db.ExeCute(sql, lst);
            if(kq == 1)
            {

                if(string.IsNullOrEmpty(malophoc))
                {
                    MessageBox.Show("Thêm mới lớp học thành công");
                }else
                {
                    MessageBox.Show("Cập nhật lớp học thành công");
                }

                this.Dispose();


            }else
            {
                MessageBox.Show("Lưu dữ liệu thất bại");

            }

        }


        /// <summary>
        /// Khi thực hiện việc xóa một lớp học nào đó với điều kiện là lớp học đã kết thúc rồi !!!!!!!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnXoa_Click(object sender, EventArgs e)
        {
            //  Xác nhận lại 1 lần trước khi xóa lớp học
            if (
                    DialogResult.Yes ==
                    MessageBox.Show(
                                    "Bạn muốn xóa lớp học [" +  cbbMonhoc.Text+ "]?",
                                    "Xác nhận xóa lớp học",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question
                                    )
                 )
            {
                string sql = "";

                List<CustomParameter> lstPara = new List<CustomParameter>();
                if (string.IsNullOrEmpty(malophoc))//nếu không có mã lớp học
                {
                    MessageBox.Show("Không có dữ liệu mã lớp học");
                }

                else//nếu xóa lớp học thì thực hiện những câu lệnh tiếp theo sau
                {
                    sql = "deleteLH";//gọi tới procedure xóa lớp học
                    lstPara.Add(new CustomParameter()
                    {
                        key = "@malophoc",
                        value = malophoc
                    });

                    var rs = new Database().ExeCute(sql, lstPara);//truyền 2 tham số là câu lệnh sql
                                                                  //và danh sách các tham số
                    if (rs == 1)//nếu thực hiện thành công
                    {
                        if (string.IsNullOrEmpty(malophoc))//nếu không có dữ liệu truyền vào là mã lớp học
                        {
                            MessageBox.Show("Không có dữ liệu");
                        }
                        else//nếu xóa lớp học
                        {
                            MessageBox.Show("Xóa lớp học này thành công");
                        }
                        this.Dispose();//đóng form sau khi xóa lớp học thành công
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
