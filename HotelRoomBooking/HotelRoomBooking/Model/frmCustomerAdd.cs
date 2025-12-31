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
    public partial class frmCustomerAdd : Sample
    {
        public frmCustomerAdd()
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
                qry = @"Insert into customers Values(@cName,@cPhone,@cEmail)";
            }
            else
            {
                qry = @"Update customers SET cName=@cName,cPhone = @cPhone,cEmail=@cEmail
                               where cusID = @id ";
            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@cName", txtName.Text);
            ht.Add("@cPhone", txtPhone.Text);
            ht.Add("@cEmail", txtEmail.Text);
           

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
