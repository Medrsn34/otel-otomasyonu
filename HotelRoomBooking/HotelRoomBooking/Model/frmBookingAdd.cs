using HotelRoomBooking.Model;
using HotelRoomBooking.View;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace HotelRoomBooking.Model
{
    public partial class frmBookingAdd : Sample
    {
        public frmBookingAdd()
        {
            InitializeComponent();

            // Event handlerlar burada bağlanıyor
            txtRece.TextChanged += txtRece_TextChanged;
            txtAmount.TextChanged += txtAmount_TextChanged;
            txtcheckIN.Leave += txtcheckIN_Leave;
            txtcheckout.Leave += txtcheckout_Leave;
            cbRoom.SelectedIndexChanged += cbRoom_SelectedIndexChanged;
        }

        public int id = 0;
        public int custID = 0;
        public int roomID = 0;

        private void frmBookingAdd_Load(object sender, EventArgs e)
        {
            string qry = @"select cusID 'id', cName 'name' from customers";
            MainClass.CBFill(qry, cbCustomer);

            string qry2 = @"select RoomID 'id', rName 'name' from Room";
            MainClass.CBFill(qry2, cbRoom);

            txtRate.Text = "";

            if (id > 0)
            {
                cbRoom.SelectedValue = roomID;
                cbCustomer.SelectedValue = custID;
            }

            HesaplaChange();
        }

        // Amount ve Received değerlerine göre Change hesaplama
        private void HesaplaChange()
        {
            decimal amount = 0;
            decimal received = 0;

            decimal.TryParse(txtAmount.Text.Trim(), out amount);
            decimal.TryParse(txtRece.Text.Trim(), out received);

            decimal change = received - amount;
            if (change < 0) change = 0;

            txtChange.Text = change.ToString("0.00");
        }

        // Gün sayısı ve amount hesaplama
        private void calDays()
        {
            if (string.IsNullOrWhiteSpace(txtcheckIN.Text) || string.IsNullOrWhiteSpace(txtcheckout.Text))
                return;

            DateTime d1, d2;
            if (!DateTime.TryParseExact(txtcheckIN.Text, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out d1) ||
                !DateTime.TryParseExact(txtcheckout.Text, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out d2))
            {
                MessageBox.Show("Tarih formatı gg.aa.yyyy olmalıdır");
                return;
            }

            int days = (d2 - d1).Days + 1;
            if (days < 1) days = 1;
            txtDays.Text = days.ToString();

            double rate = 0;
            double.TryParse(txtRate.Text.Trim(), out rate);

            decimal amount = (decimal)(days * rate);
            txtAmount.Text = amount.ToString("0.00");

            HesaplaChange();
        }

        private void txtRece_TextChanged(object sender, EventArgs e)
        {
            HesaplaChange();
        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            HesaplaChange();
        }

        private void txtcheckIN_Leave(object sender, EventArgs e)
        {
            calDays();
        }

        private void txtcheckout_Leave(object sender, EventArgs e)
        {
            calDays();
        }

        private void cbRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRoom.SelectedIndex != -1)
            {
                string qry = @"select rRate from Room where roomID =" + cbRoom.SelectedValue;
                SqlCommand cmd = new SqlCommand(qry, MainClass.con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    txtRate.Text = dt.Rows[0]["rRate"].ToString();
                    calDays();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string qry = "";

            if (id == 0)
            {
                qry = @"INSERT INTO Bookings
        (customerID, roomID, CheckInDate, bCheckOutDate, Status, days, rate, amount, received, [change])
        VALUES
        (@customerID, @roomID, @CheckInDate, @bCheckOutDate, @status, @days, @rate,
         @amount, @received, @change)";
            }
            else
            {
                qry = @"UPDATE Bookings SET 
        customerID=@customerID,
        roomID=@roomID,
        CheckInDate=@CheckInDate,
        bCheckOutDate=@bCheckOutDate,
        Status=@status,
        days=@days,
        rate=@rate,
        amount=@amount,
        received=@received,
        [change]=@change
        WHERE bookID = @id";
            }

            DateTime checkIn, checkOut;

            if (!DateTime.TryParseExact(txtcheckIN.Text, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out checkIn) ||
                !DateTime.TryParseExact(txtcheckout.Text, "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out checkOut))
            {
                MessageBox.Show("Tarih formatı gg.aa.yyyy olmalıdır");
                return;
            }

            decimal amount = 0;
            decimal received = 0;
            decimal change = 0;

            if (!decimal.TryParse(txtAmount.Text, out amount) ||
                !decimal.TryParse(txtRece.Text, out received))
            {
                MessageBox.Show("Amount veya Received geçerli bir sayı değil.");
                return;
            }

            change = received - amount;
            if (change < 0) change = 0;

            Hashtable ht = new Hashtable();
            ht.Add("@id", id);
            ht.Add("@customerID", Convert.ToInt32(cbCustomer.SelectedValue));
            ht.Add("@roomID", Convert.ToInt32(cbRoom.SelectedValue));
            ht.Add("@CheckInDate", checkIn);
            ht.Add("@bCheckOutDate", checkOut);
            ht.Add("@status", cbStatus.Text);
            ht.Add("@days", Convert.ToInt32(txtDays.Text));
            ht.Add("@rate", Convert.ToDouble(txtRate.Text));
            ht.Add("@amount", amount);
            ht.Add("@received", received);
            ht.Add("@change", change);

            int r = MainClass.SQL(qry, ht);

            string qry2 = "";
            if (cbStatus.Text == "Checked In")
                qry2 = @"UPDATE Room SET rStatus='Occupied' WHERE roomID=" + cbRoom.SelectedValue;
            else
                qry2 = @"UPDATE Room SET rStatus='Available' WHERE roomID=" + cbRoom.SelectedValue;

            MainClass.SQL(qry2, new Hashtable());

            if (r > 0)
            {
                MainClass.ClearAll(this);
                guna2MessageDialog1.Show("Saved Successfully");
                id = 0;

                // Parent DataGridView’i yenile
                if (Owner is frmBookingView parent)
                {
                    parent.LoadData();
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
