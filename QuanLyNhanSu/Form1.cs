using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNhanSu.Db;

namespace QuanLyNhanSu
{
    public partial class Form1 : Form
    {
        ConnectDB cdb = new ConnectDB();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Bạn có chắc muốn thoát?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dg == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LayDSNS();
            cbxMaPB.Items.Clear();
            LayDSTenPB();
            cbxMaPB.Items.Add("Tất cả");
        }

        private void LayDSTenPB()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataSet ds = new DataSet();
            SqlConnection con = cdb.GetConnection();
            try
            {
                con.Open();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = "SP_LayDSTenPB";
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Connection = con;
                da.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    cbxMaPB.Items.Add(ds.Tables[0].Rows[i][0].ToString());
                con.Close();
                if (cbxMaPB.Items.Count > 0)
                {
                    cbxMaPB.Text = (cbxMaPB.Items[0]).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LayDSNS()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            SqlConnection con = cdb.GetConnection();
            try
            {
                con.Open();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.CommandText = "SP_LayDSNV";
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Connection = con;
                da.Fill(dt);
                dtgDSNS.DataSource = dt;
                con.Close();
                dtgDSNS.Columns[0].Width = 35;
                dtgDSNS.Columns[0].HeaderText = "ID";
                dtgDSNS.Columns[1].Width = 130;
                dtgDSNS.Columns[1].HeaderText = "Họ Tên";
                dtgDSNS.Columns[2].Width = 80;
                dtgDSNS.Columns[2].HeaderText = "Giới tính";
                dtgDSNS.Columns[3].Width = 80;
                dtgDSNS.Columns[3].HeaderText = "Ngày sinh";
                dtgDSNS.Columns[4].Width = 135;
                dtgDSNS.Columns[4].HeaderText = "Email";
                dtgDSNS.Columns[5].Width = 80;
                dtgDSNS.Columns[5].HeaderText = "Địa chỉ";
                dtgDSNS.Columns[6].Width = 70;
                dtgDSNS.Columns[6].HeaderText = "Phone";
                dtgDSNS.Columns[7].Width = 70;
                dtgDSNS.Columns[7].HeaderText = "Tên PB";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string rowFilter = string.Format("{0} like '{1}'", "HoTen", "*" + txtTimKiem.Text + "*");
            (dtgDSNS.DataSource as DataTable).DefaultView.RowFilter = rowFilter;
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {
            if (txtTimKiem.Text == "")
            {
                LayDSNS();
            }
        }

        private void Reset()
        {
            txtDiaChi.Text = "";
            txtEmail.Text = "";
            txtHoTen.Text = "";
            txtID.Text = "";
            txtPhone.Text = "";
            rdoNam.Checked = false;
            rdoNu.Checked = false;
            dtpNgaySinh.Value = DateTime.Now;
        }

        private bool KiemTraThongTin()
        {
            if (txtHoTen.Text == "")
            {
                MessageBox.Show("Vui lòng điền họ và tên nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtHoTen.Focus();
                return false;
            }
            if (txtDiaChi.Text == "")
            {
                MessageBox.Show("Vui lòng điền địa chỉ của nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDiaChi.Focus();
                return false;
            }
            if (txtEmail.Text == "")
            {
                MessageBox.Show("Vui lòng điền Email của nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmail.Focus();
                return false;
            }
            if (rdoNam.Checked == false && rdoNu.Checked == false)
            {
                MessageBox.Show("Vui lòng chọn giới tính cho nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            Int32 SDT;
            bool isNumeric = Int32.TryParse(txtPhone.Text, out SDT);  // 10 chữ số
            if (txtPhone.Text == "")
            {
                MessageBox.Show("Vui lòng điền số điện thoại của nhân viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPhone.Focus();
                return false;
            }
            else if (isNumeric == false)
            {
                MessageBox.Show("Số điện thoại phải là 10 chữ số!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtPhone.Focus();
                return false;
            }
            return true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (KiemTraThongTin())
            {
                try
                {
                    SqlConnection conn = cdb.GetConnection();
                    SqlCommand cmd = new SqlCommand();
                    SqlCommand cmd1 = new SqlCommand();

                    cmd1.CommandText = "SP_LayMaPB";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@TenPB", SqlDbType.NVarChar).Value = cbxMaPB.SelectedItem.ToString();
                    cmd1.Connection = conn;
                    conn.Open();
                    string strMaPB = (string)cmd1.ExecuteScalar();
                    conn.Close();        

                    cmd.CommandText = "SP_ThemNhanVien";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar).Value = txtHoTen.Text;
                    if(rdoNam.Checked == true)
                    {
                        cmd.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = rdoNam.Text;
                    }
                    else
                    {
                        cmd.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = rdoNu.Text;
                    }
                    cmd.Parameters.Add("@NgaySinh", SqlDbType.Date).Value = dtpNgaySinh.Text;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = txtDiaChi.Text;
                    cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = txtPhone.Text;
                    cmd.Parameters.Add("@MaPB", SqlDbType.VarChar).Value = strMaPB;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LayDSNS();
                    MessageBox.Show("Thêm mới nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); 
                    Reset();
                }
                catch (Exception)
                {
                    
                }
            }
        }

        private void txtID_Enter(object sender, EventArgs e)
        {
            if (txtID.Text == "Thêm mới không cần ID")
            {
                txtID.Clear();
                txtID.ForeColor = SystemColors.Highlight;
            }
        }


        private void txtID_Leave(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                txtID.Text = "Thêm mới không cần ID";
                txtID.ForeColor = SystemColors.InactiveCaption;
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if(txtID.Text == "" || txtID.Text == "Thêm mới không cần ID")
            {
                MessageBox.Show("Vui lòng điền ID nhân viên cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
                txtID.SelectAll();
            }
            else if (KiemTraThongTin())
            {
                try
                {
                    SqlConnection conn = cdb.GetConnection();
                    SqlCommand cmd = new SqlCommand();
                    SqlCommand cmd1 = new SqlCommand();

                    cmd1.CommandText = "SP_LayMaPB";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@TenPB", SqlDbType.NVarChar).Value = cbxMaPB.SelectedItem.ToString();
                    cmd1.Connection = conn;
                    conn.Open();
                    string strMaPB = (string)cmd1.ExecuteScalar();
                    conn.Close();

                    cmd.CommandText = "SP_SuaNhanVien";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(txtID.Text);
                    cmd.Parameters.Add("@HoTen", SqlDbType.NVarChar).Value = txtHoTen.Text;
                    if (rdoNam.Checked == true)
                    {
                        cmd.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = rdoNam.Text;
                    }
                    else
                    {
                        cmd.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = rdoNu.Text;
                    }
                    cmd.Parameters.Add("@NgaySinh", SqlDbType.Date).Value = dtpNgaySinh.Text;
                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = txtEmail.Text;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = txtDiaChi.Text;
                    cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = txtPhone.Text;
          
                    cmd.Parameters.Add("@MaPB", SqlDbType.VarChar).Value = strMaPB;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LayDSNS();
                    MessageBox.Show("Sửa nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                catch (Exception)
                {
                   
                }
            }
        }

        private void dtgDSNS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = new DataGridViewRow();
                row = dtgDSNS.Rows[e.RowIndex];
                txtID.Text = Convert.ToString(row.Cells["ID"].Value);
                txtHoTen.Text = Convert.ToString(row.Cells["HoTen"].Value);
                dtpNgaySinh.Text = Convert.ToString(row.Cells["NgaySinh"].Value);
                txtDiaChi.Text = Convert.ToString(row.Cells["DiaChi"].Value);
                txtEmail.Text = Convert.ToString(row.Cells["Email"].Value);
                string GioiTinh = Convert.ToString(row.Cells["GioiTinh"].Value);
                if (GioiTinh.Trim() == "Nu")
                {
                    rdoNu.Checked = true;
                }
                else
                {
                    rdoNam.Checked = true;
                }
                txtPhone.Text = Convert.ToString(row.Cells["Phone"].Value);
                cbxMaPB.Text = Convert.ToString(row.Cells["TenPB"].Value);
            }
            catch (Exception)
            {

            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "Thêm mới không cần ID" || txtID.Text == "")
            {
                MessageBox.Show("Vui lòng điền ID nhân viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtID.Focus();
            }
            else
            {
                DialogResult result = MessageBox.Show("Bạn có muốn xóa hay không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        SqlConnection conn = cdb.GetConnection();
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "SP_XoaNhanVien";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = Convert.ToInt32(txtID.Text);

                        cmd.Connection = conn;
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        LayDSNS();
                        MessageBox.Show("Xóa nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Reset();
                    }
                    catch (Exception)
                    {
                        
                    }
                }
            }
        }


        private void btnHienThi_Click(object sender, EventArgs e)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();

                SqlConnection conn = cdb.GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlCommand cmd1 = new SqlCommand();

                if (cbxMaPB.SelectedItem.ToString() == @"Tất cả")
                {
                    LayDSNS();
                }
                else
                {
                    cmd1.CommandText = "SP_LayMaPB";
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@TenPB", SqlDbType.NVarChar).Value = cbxMaPB.SelectedItem.ToString();
                    cmd1.Connection = conn;
                    conn.Open();
                    string strMaPB = (string)cmd1.ExecuteScalar();
                    conn.Close();


                    conn.Open();
                    da.SelectCommand = new SqlCommand();
                    da.SelectCommand.CommandText = "SP_LayDSNVPB";
                    da.SelectCommand.Parameters.Add("@MaPB", SqlDbType.VarChar).Value = strMaPB;
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Connection = conn;
                    da.Fill(dt);
                    dtgDSNS.DataSource = dt;
                    conn.Close();
                    dtgDSNS.Columns[0].Width = 35;
                    dtgDSNS.Columns[0].HeaderText = "ID";
                    dtgDSNS.Columns[1].Width = 130;
                    dtgDSNS.Columns[1].HeaderText = "Họ Tên";
                    dtgDSNS.Columns[2].Width = 80;
                    dtgDSNS.Columns[2].HeaderText = "Giới tính";
                    dtgDSNS.Columns[3].Width = 80;
                    dtgDSNS.Columns[3].HeaderText = "Ngày sinh";
                    dtgDSNS.Columns[4].Width = 135;
                    dtgDSNS.Columns[4].HeaderText = "Email";
                    dtgDSNS.Columns[5].Width = 80;
                    dtgDSNS.Columns[5].HeaderText = "Địa chỉ";
                    dtgDSNS.Columns[6].Width = 70;
                    dtgDSNS.Columns[6].HeaderText = "Phone";
                    dtgDSNS.Columns[7].Width = 70;
                    dtgDSNS.Columns[7].HeaderText = "Tên PB";
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }
        private void dtgDSNS_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = new DataGridViewRow();
                row = dtgDSNS.Rows[e.RowIndex];
                txtID.Text = Convert.ToString(row.Cells["ID"].Value);
                txtHoTen.Text = Convert.ToString(row.Cells["HoTen"].Value);
                dtpNgaySinh.Text = Convert.ToString(row.Cells["NgaySinh"].Value);
                txtDiaChi.Text = Convert.ToString(row.Cells["DiaChi"].Value);
                txtEmail.Text = Convert.ToString(row.Cells["Email"].Value);
                string GioiTinh = Convert.ToString(row.Cells["GioiTinh"].Value);
                if (GioiTinh.Trim() == "Nu")
                {
                    rdoNu.Checked = true;
                }
                else
                {
                    rdoNam.Checked = true;
                }
                txtPhone.Text = Convert.ToString(row.Cells["Phone"].Value);
                cbxMaPB.Text = Convert.ToString(row.Cells["TenPB"].Value);
            }
            catch (Exception)
            {

            }
        }

    }
}
