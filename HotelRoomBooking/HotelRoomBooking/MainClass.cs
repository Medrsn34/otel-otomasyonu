using Guna.UI2.WinForms;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace HotelRoomBooking
{
    internal class MainClass
    {
        public static readonly string cons = "   Data Source=EDIZABI\\SQLEXPRESS;Initial Catalog=HotelRoomBooking1;Integrated Security=True";
        public static SqlConnection con = new SqlConnection(cons);
        public static string user;

        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }

        public static bool IsValidUser(string user, string pass)
        {
            bool isValid = false;
            try
            {
                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
                {
                    return false;
                }

                string qry = "Select * from users where uUsername = @user and uPass = @pass";
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@pass", pass);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    isValid = true;
                    USER = dt.Rows[0]["uName"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + ex.Message + "\n\nLütfen:\n1. SQL Server'ın çalıştığından emin olun\n2. Veritabanının mevcut olduğunu kontrol edin\n3. Connection string'i kontrol edin", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isValid = false;
            }
            return isValid;
        }

        public static void BlurBackground(Form Model)
        {
            Form Background = new Form();

            using (Model)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = frmMain.Instance.Size;
                Background.Location = frmMain.Instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                Model.Owner = Background;
                Model.ShowDialog(Background);
                Background.Dispose();
            }
        }

        // Load Data
        public static void loadData(string qry, DataGridView gv, ListBox lb)
        {
            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < lb.Items.Count; i++)
                {
                    string colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colNam1].DataPropertyName = dt.Columns[i].ToString();
                }

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
        }

        private static void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0;
            foreach (DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        // SQL Command
        public static int SQL(string qry, Hashtable ht)
        {
            int res = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;

                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }
                if (con.State == ConnectionState.Closed) { con.Open(); }
                res = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open) { con.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
            return res;
        }

        // ComboBox Doldur
        public static void CBFill(string qry, ComboBox cb)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cb.DisplayMember = "Name";
                cb.ValueMember = "id";
                cb.DataSource = dt;

                if (dt.Rows.Count > 0)
                {
                    cb.SelectedIndex = -1;
                }
                else
                {
                    cb.DataSource = null;
                    cb.Items.Clear();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("ComboBox doldurma hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Form Temizle
        public static void ClearAll(Form f)
        {
            foreach (Control c in f.Controls)
            {
                Type type = c.GetType();
                if (type == typeof(Guna2TextBox))
                {
                    ((Guna2TextBox)c).Text = "";
                }
                else if (type == typeof(Guna2ComboBox))
                {
                    ((Guna2ComboBox)c).SelectedIndex = -1;
                }
                else if (type == typeof(Guna2CheckBox))
                {
                    ((Guna2CheckBox)c).Checked = false;
                }
            }
        }
    }
}
