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
    public partial class frmRoomAdd : Sample
    {
        public frmRoomAdd()
        {
            InitializeComponent();
        }
        public int id = 0;
        public int typeID = 0;
        private void btnSave_Click(object sender, EventArgs e)
        {
            string qry = "";
            if (id == 0)
            {
                // Insert operation
                qry = @"Insert into Room Values(@rName,@TypeID,@rRate,@rStatus)";
            }
            else
            {
                qry = @"Update Room SET rName=@rName,rTypeID=@TypeID,rRate=@rRate,rStatus=@rStatus
                               where roomID = @id ";
            }
            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@rName", txtName.Text);
            ht.Add("@TypeID", Convert.ToInt32(cbType.SelectedValue));
            ht.Add("@rRate", Convert.ToDouble(txtRate.Text));
            ht.Add("@rStatus", cbStatus.Text);

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

        private void frmRoomAdd_Load(object sender, EventArgs e)
        {
            string qry = @"Select typeID 'id' , tName 'name' from RoomType";
            MainClass.CBFill(qry, cbType);
            if (id > 0)
            {
                cbType.SelectedValue = typeID;
            }
        }
    }
}
