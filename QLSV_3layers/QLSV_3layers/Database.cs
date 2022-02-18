using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// PHAN CHÍ TRUNG - 1800005414
namespace QLSV_3layers
{
    public class Database
    {
        //---Khai báo connection để kết nối CSDL 
        // Đối với những máy khác thì cần chỉnh sửa Data Source = ??? ; Initial Catalog = ??? ; User ID = ??? ; Password= ???
        // Phần chấm hỏi tùy theo cấu hình máy

        private string connetionString = @"Data Source=LAPTOP-G0I3AISP; Initial Catalog = DACN_QLSV; User ID = Trung; Password=Trung123";
        private SqlConnection conn;
        private DataTable dt;
        private SqlCommand cmd;
        public Database()
        {
            try
            {
                conn = new SqlConnection(connetionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối thất bại: " + ex.Message);
            }
        }

        public DataTable SelectData(string sql, List<CustomParameter> lstPara)
        {
            try
            {
                conn.Open();
                cmd = new SqlCommand(sql, conn);//nội dung sql đc truyền vào
                cmd.CommandType = CommandType.StoredProcedure;//set command type cho cmd
                foreach(var para in lstPara)//gán các tham số cho cmd
                {
                    cmd.Parameters.AddWithValue(para.key,para.value);
                }
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataRow Select(string sql)
        {
            try
            {
                conn.Open();//mở kết nối
                cmd = new SqlCommand(sql, conn);//truyền giá trị vào cmd
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());//thực thi câu lệnh
                return dt.Rows[0];//trả về kết quả
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load thông tin chi tiết: "+ex.Message);
                return null;
            }
            finally
            {
                conn.Close();//cuối cùng đóng kết nối
            }
        }

        public int ExeCute(string sql,List<CustomParameter> lstPara)
        {
            try
            {
                //cần sửa lại hàm execute như sau
                //string sql,List<CustomParameter> lstPara là tham số truyền vào 
                //CustomParameter ....
                conn.Open();//mở kết nối
                cmd = new SqlCommand(sql, conn);//thực thi câu lệnh sql
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var p in lstPara)//gán các tham số cho cmd
                {
                    cmd.Parameters.AddWithValue(p.key,p.value);
                }
                var rs = cmd.ExecuteNonQuery();//lấy kết quả thực thi truy vấn
                return (int)rs;//trả về kết quả
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thực thi câu lệnh: "+ex.Message);
                return -100;
            }
            finally
            {
                conn.Close();//cuối cùng đóng kết nối
            }
        }
    }
}
