using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using MetroFramework.Forms;
using System.Security.Cryptography;


namespace TutteeFrame
{
    public partial class frmAccountManagement : MetroForm
    {
        SqlConnection conn;
        SqlCommand cmd;
        //path tùy chỉnh  trên từng máy khác nhau.
        string path = @"Data Source=DESKTOP-A4CIEO2\SQLEXPRESS;Initial Catalog=TutteeFrame;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table;
        static private string CryptonMethod(string input)
        {
            MD5 md = MD5.Create();
            byte[] inputstr = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md.ComputeHash(inputstr);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        void LoadDataTodgvAccount()
        {
            using (conn = new SqlConnection(path))
            {
                using (cmd = conn.CreateCommand())
                {
                    using (table = new DataTable())
                    {
                        cmd.CommandText = "SELECT * FROM ACCOUNT";
                        adapter.SelectCommand = cmd;
                        adapter.Fill(table);
                        dgvListAccount.DataSource = table;
                    }
                }
            }
 
        }
        void LoadDatatodgvTeacher()
        {
            using (conn = new SqlConnection(path))
            {
                using (cmd = conn.CreateCommand())
                {
                    using (table = new DataTable())
                    {
                        cmd.CommandText = "SELECT * FROM TEACHER";
                        adapter.SelectCommand = cmd;
                        adapter.Fill(table);
                        dgvDetailTeacher.DataSource = table;
                    }
                }
            }

        }
        public frmAccountManagement()
        {
            InitializeComponent();
        }

        private void frmAccountManagement_Load(object sender, EventArgs e)
        {
            LoadDataTodgvAccount();

        }
        private void dgvListAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvDetailTeacher.DataSource = null;
            int i;
            i = dgvListAccount.CurrentRow.Index;
            string TeacherID = dgvListAccount.Rows[i].Cells[0].Value.ToString();
            using(conn = new SqlConnection(path))
            {
                using(cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT * FROM TEACHER ORDER BY CASE WHEN TeacherID = '{TeacherID}' THEN 1 ELSE 2 END, TeacherID";
                    adapter.SelectCommand = cmd;
                    using(table = new DataTable())
                    {
                        adapter.Fill(table);
                        dgvDetailTeacher.DataSource = table;
                        tbTeacherID.Text = dgvDetailTeacher.Rows[0].Cells[0].Value.ToString();
                        tbTeacherSurName.Text = dgvDetailTeacher.Rows[0].Cells[1].Value.ToString();
                        tbTeacherFistName.Text = dgvDetailTeacher.Rows[0].Cells[2].Value.ToString();
                        tbTeacherAddress.Text = dgvDetailTeacher.Rows[0].Cells[3].Value.ToString();
                        tbTeacherMail.Text = dgvDetailTeacher.Rows[0].Cells[4].Value.ToString();
                        tbTeacherPhoneNumber.Text = dgvDetailTeacher.Rows[0].Cells[5].Value.ToString();
                        tbTeacherSubjectID.Text = dgvDetailTeacher.Rows[0].Cells[6].Value.ToString();
                        tbUserName.Text = dgvListAccount.Rows[i].Cells[1].Value.ToString();
                        tbPassword.Text = null;
                        //Dấu paaword không hiện.
                        //tbPassword.Text = dgvListAccount.Rows[i].Cells[2].Value.ToString();
                        if(dgvDetailTeacher.Rows[0].Cells[7].Value.ToString()=="True")
                        {
                            cbTeacherPosition.SelectedIndex = 1;
                        }
                        else
                        {
                            if(dgvDetailTeacher.Rows[0].Cells[8].Value.ToString()=="True")
                            {
                                cbTeacherPosition.SelectedIndex = 0;
                            }
                            else
                            {
                                cbTeacherPosition.SelectedIndex = 2;
                            }
                        }
                    }
                }
            }
        }

        private void btNewTeacher_Click(object sender, EventArgs e)
        {
            //Create a new teacher.
            using(conn = new SqlConnection(path))
            {
                conn.Open();
                using(cmd = conn.CreateCommand())
                {
                    bool is_Admin = false;
                    bool is_Ministry = false;
                    if(cbTeacherPosition.SelectedItem.ToString() =="Admin")
                    {
                        is_Admin = true;
                    }
                    else
                    {
                        if(cbTeacherPosition.SelectedItem.ToString() =="Ministry")
                        {
                            is_Ministry = true;
                        }
                        else
                        {

                        }
                    }
                    cmd.CommandText = $"insert into TEACHER values('{tbTeacherID.Text}','{tbTeacherSurName.Text}','{tbTeacherFistName.Text}','{tbTeacherAddress.Text}','{tbTeacherMail.Text}','{tbTeacherPhoneNumber.Text}','{tbTeacherSubjectID.Text}','{is_Ministry}','{is_Admin}','{cbTeacherPosition.Text}')";
                    cmd.ExecuteNonQuery();
                }

                using (cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"INSERT INTO ACCOUNT VALUES('{tbTeacherID.Text}','{tbUserName.Text}','{CryptonMethod(tbPassword.Text)}')";
                    cmd.ExecuteNonQuery();
                }
            }
            LoadDataTodgvAccount();
            conn.Close();
        }
    }
}
