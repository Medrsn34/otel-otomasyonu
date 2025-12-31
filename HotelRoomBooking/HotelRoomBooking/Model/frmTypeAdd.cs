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
    public partial class frmTypeAdd : Sample
    {
        public frmTypeAdd()
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
                qry = @"Insert into RoomType Values(@tName,@tDescription)";
            }
            else
            {
                qry = @"Update RoomType SET tName=@tName,tDescription=@tDescription
                               where typeID = @id ";
            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@tName", txtName.Text);
            ht.Add("@tDescription", txtDes.Text);


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
