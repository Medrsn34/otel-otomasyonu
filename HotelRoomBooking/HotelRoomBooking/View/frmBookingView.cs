using HotelRoomBooking.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace HotelRoomBooking.View
{
    public partial class frmBookingView : Sample
    {
        private PrintDocument printDocument;
        private DataGridViewRow currentPrintRow;

        public frmBookingView()
        {
            InitializeComponent();
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void frmBookingView_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            ListBox lb = new ListBox();
            lb.Items.Add(dgvID);
            lb.Items.Add(dgvcusID);
            lb.Items.Add(dgvCustomer);
            lb.Items.Add(dgvRoomID);
            lb.Items.Add(dgvRoom);
            lb.Items.Add(dgvIn);
            lb.Items.Add(dgvOut);
            lb.Items.Add(dgvDay);
            lb.Items.Add(dgvRate);
            lb.Items.Add(dgvAmount);
            lb.Items.Add(dgvRece);
            lb.Items.Add(dgvChange);
            lb.Items.Add(dgvStatus);

            string qry = @"
select 
 b.bookID,
 b.customerID,
 c.cName,
 b.roomID,
 r.rName,
 b.CheckInDate,
 b.bCheckOutDate,
 b.days,
 b.rate,
 b.amount,
 b.received,
 b.change,
 b.status
from Bookings b
inner join customers c on c.cusID = b.customerID
inner join Room r on r.roomID = b.roomID
where c.cName like '%" + txtSearch.Text + "%'";

            MainClass.loadData(qry, guna2DataGridView1, lb);
        }

        // Güncelleme: BookID ile doğru satırı bul
        public void UpdateCurrentRow(int bookID, decimal amount, decimal received, decimal change)
        {
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.Cells["dgvID"].Value != null && Convert.ToInt32(row.Cells["dgvID"].Value) == bookID)
                {
                    row.Cells["dgvAmount"].Value = amount;
                    row.Cells["dgvRece"].Value = received;
                    row.Cells["dgvChange"].Value = change;
                    break;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmBookingAdd frm = new frmBookingAdd();
            frm.Owner = this;
            MainClass.BlurBackground(frm);
            LoadData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvedit")
            {
                frmBookingAdd frm = new frmBookingAdd();
                frm.Owner = this;

                frm.id = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                frm.custID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvcusID"].Value);
                frm.roomID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvRoomID"].Value);

                if (DateTime.TryParse(guna2DataGridView1.CurrentRow.Cells["dgvIn"].Value?.ToString(), out DateTime cin))
                    frm.txtcheckIN.Text = cin.ToString("dd.MM.yyyy");

                if (DateTime.TryParse(guna2DataGridView1.CurrentRow.Cells["dgvOut"].Value?.ToString(), out DateTime cout))
                    frm.txtcheckout.Text = cout.ToString("dd.MM.yyyy");

                frm.txtDays.Text = guna2DataGridView1.CurrentRow.Cells["dgvDay"].Value.ToString();
                frm.txtRate.Text = guna2DataGridView1.CurrentRow.Cells["dgvRate"].Value.ToString();
                frm.txtAmount.Text = guna2DataGridView1.CurrentRow.Cells["dgvAmount"].Value.ToString();
                frm.txtRece.Text = guna2DataGridView1.CurrentRow.Cells["dgvRece"].Value.ToString();
                frm.cbStatus.Text = guna2DataGridView1.CurrentRow.Cells["dgvStatus"].Value.ToString();

                MainClass.BlurBackground(frm);
            }

            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvDel")
            {
                int bookID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvID"].Value);
                int roomID = Convert.ToInt32(guna2DataGridView1.CurrentRow.Cells["dgvRoomID"].Value);

                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Warning;

                if (guna2MessageDialog1.Show("Bu rezervasyonu silmek istiyor musunuz?") == DialogResult.Yes)
                {
                    string delQry = "DELETE FROM Bookings WHERE BookID=@id";
                    Hashtable ht = new Hashtable();
                    ht.Add("@id", bookID);
                    MainClass.SQL(delQry, ht);

                    string roomQry = "UPDATE Room SET rStatus='Available' WHERE roomID=@rid";
                    Hashtable ht2 = new Hashtable();
                    ht2.Add("@rid", roomID);
                    MainClass.SQL(roomQry, ht2);

                    guna2MessageDialog1.Show("Silindi");
                    LoadData();
                }
            }

            if (guna2DataGridView1.Columns[e.ColumnIndex].Name == "dgvPrint")
            {
                currentPrintRow = guna2DataGridView1.CurrentRow;
                PrintDialog pd = new PrintDialog { Document = printDocument };
                if (pd.ShowDialog() == DialogResult.OK)
                    printDocument.Print();
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (currentPrintRow == null) return;

            Font title = new Font("Arial", 18, FontStyle.Bold);
            Font h = new Font("Arial", 12, FontStyle.Bold);
            Font n = new Font("Arial", 10);

            float y = 50;
            float x = 50;

            e.Graphics.DrawString("HOTEL BOOKING INVOICE", title, Brushes.Black, x, y);
            y += 40;

            e.Graphics.DrawString($"Customer: {currentPrintRow.Cells["dgvCustomer"].Value}", n, Brushes.Black, x, y); y += 20;
            e.Graphics.DrawString($"Room: {currentPrintRow.Cells["dgvRoom"].Value}", n, Brushes.Black, x, y); y += 20;
            e.Graphics.DrawString($"Check In: {currentPrintRow.Cells["dgvIn"].Value}", n, Brushes.Black, x, y); y += 20;
            e.Graphics.DrawString($"Check Out: {currentPrintRow.Cells["dgvOut"].Value}", n, Brushes.Black, x, y); y += 20;
            e.Graphics.DrawString($"Days: {currentPrintRow.Cells["dgvDay"].Value}", n, Brushes.Black, x, y); y += 20;
            e.Graphics.DrawString($"Rate: {currentPrintRow.Cells["dgvRate"].Value} TL", h, Brushes.Black, x, y); y += 20;
            e.Graphics.DrawString($"Amount: {currentPrintRow.Cells["dgvAmount"].Value} TL", h, Brushes.Black, x, y); y += 20;
            e.Graphics.DrawString($"Received: {currentPrintRow.Cells["dgvRece"].Value} TL", h, Brushes.Black, x, y); y += 20;
            e.Graphics.DrawString($"Change: {currentPrintRow.Cells["dgvChange"].Value} TL", h, Brushes.Black, x, y); y += 20;

            e.HasMorePages = false;
        }
    }
}
