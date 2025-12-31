using HotelRoomBooking.Model;
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

namespace HotelRoomBooking.View
{
    public partial class frmRoomView : Sample
    {
        public frmRoomView()
        {
            InitializeComponent();
        }

        private void frmRoomView_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void LoadData()
        {
            ListBox lb = new ListBox();
            //lb.Items.Add(dgvSr);
            lb.Items.Add(dgvID);
            lb.Items.Add(dgvName);
            lb.Items.Add(dgvtypeID);
            lb.Items.Add(dgvType);
            lb.Items.Add(dgvRate);
            lb.Items.Add(dgvStatus);
            string qry = @"Select roomID,rName,rTypeID,tName,rRate,rStatus From Room inner join RoomType on typeID =rTypeID where rName like'%" + txtSearch.Text + "%'";

            MainClass.loadData(qry, guna2DataGridView1, lb);
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            MainClass.BlurBackground(new frmRoomAdd());
            LoadData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvedit" && e.RowIndex != -1)
            {
                frmRoomAdd frm = new frmRoomAdd();
                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                frm.txtName.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvName"].Value);
                frm.txtRate.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvRate"].Value);
                frm.cbStatus.Text = Convert.ToString(guna2DataGridView1.CurrentRow.Cells["dgvStatus"].Value);
                frm.typeID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvtypeID"].Value);
                MainClass.BlurBackground(frm);
                LoadData();

            }
            if (guna2DataGridView1.CurrentCell.OwningColumn.Name == "dgvDel" && e.RowIndex != -1)
            {
                DataGridViewRow row = guna2DataGridView1.CurrentRow;
                int ID = Convert.ToInt32(row.Cells["dgvID"].Value);
                if (ID != 0)
                {
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                    if (guna2MessageDialog1.Show("Are you sure you want to delete") == DialogResult.Yes)
                    {
                        try
                        {
                            string qry = "DELETE FROM users WHERE userID = " + ID + " ";
                            Hashtable ht = new Hashtable();
                            int r = MainClass.SQL(qry, ht);
                            if (r > 0)
                            {
                                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                                guna2MessageDialog1.Show("Deleted Successfully");
                                ID = 0;
                                LoadData();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                            MainClass.con.Close();
                        }

                    }
                }

            }
        }
    }
}
