using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelRoomBooking.Model
{
    public partial class frmUserAdd : Sample
    {
        public frmUserAdd()
        {
            InitializeComponent();
        }
        public int id = 0;

        private void btnSave_Click(object sender, EventArgs e)
        {
            string qry = "";
            if (id == 0)
            {
                // Insert operation
                qry = @"Insert into users Values(@uName,@uUsername,@uPass,@Phone)";
            }
            else
            {
                qry = @"Update users SET uName=@uName,uUsername=@uUsername,uPass=@uPass,uPhone = @Phone
                               where userID = @id ";
            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@uName", txtName.Text);
            ht.Add("@uUsername", txtUser.Text);
            ht.Add("@uPass", txtPass.Text);
            ht.Add("@Phone", txtPhone.Text);
           
            int r = MainClass.SQL(qry, ht);
            if (r > 0)
            {
                MainClass.ClearAll(this);
                txtName.Focus();
                guna2MessageDialog1.Show("Saved Successfully");
                id = 0;
              
            }

        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
