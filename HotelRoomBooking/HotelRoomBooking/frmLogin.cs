using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelRoomBooking
{
    public partial class frmLogin : Sample
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifre giriniz.", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MainClass.IsValidUser(txtName.Text, txtPass.Text) == false)
            {
                MessageBox.Show("Geçersiz kullanıcı adı veya şifre!\n\nVarsayılan kullanıcı:\nKullanıcı Adı: admin\nŞifre: admin\n\nEğer hala giriş yapamıyorsanız, veritabanında kullanıcı olmayabilir.", "Giriş Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            frmMain frm = new frmMain();
            frm.Show();
            this.Hide();

        }
    }
}
