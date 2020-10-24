using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace MovieBookingProgram
{
    public partial class Book : Form
    {
        //different file path
        public string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Uni\CMPG223\Project\CMPG Assignment\2020_10_17_Basic_Layout\CMPG 223 Group Project\MovieBookingProgram\MovieBookingProgram\DBTheater.mdf;Integrated Security=True";

        public SqlConnection conn;
        public SqlCommand cmd;
        public DataSet ds;
        public SqlDataAdapter adap;

        public void ConnectDatabase()
        {
            conn = new SqlConnection(constr);
            conn.Open();
            adap = new SqlDataAdapter();
            ds = new DataSet();

        }

        private void Reset()
        {
            ConnectDatabase();
            GrabDataCbx(@"SELECT Special_Id FROM Specials", cbxSpecial);
            GrabDataCbx(@"SELECT Movie_Name FROM Movies", cbxMovie);
            GrabDataCbx(@"SELECT Description FROM Genres", cbxGenre);
            conn.Close();

            //User guidance
            btnBook.Enabled = false;
            gbxDateTime.Enabled = true;
            gbxMovie.Enabled = true;
            gbxTicket.Enabled = false;
        }

        private void GrabDataCbx(string sql, ComboBox cbx)
        {
            ConnectDatabase();
            cmd = new SqlCommand(sql, conn);
            adap.SelectCommand = cmd;
            adap.Fill(ds, "Data");
            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                cbx.Items.Add(dataReader.GetValue(1));
            }
            conn.Close();
        }
        private void GrabDataRtbx(string sql, RichTextBox rtbx)
        {
            ConnectDatabase();
            cmd = new SqlCommand(sql, conn);
            adap.SelectCommand = cmd;
            adap.Fill(ds, "Data");
            SqlDataReader dataReader = cmd.ExecuteReader();

            rtbx.Text = dataReader.GetString(1);
            conn.Close();
        }
        private void GrabDataLbl(string sql, Label lbl)
        {
            ConnectDatabase();
            cmd = new SqlCommand(sql, conn);
            adap.SelectCommand = cmd;
            adap.Fill(ds, "Data");
            SqlDataReader dataReader = cmd.ExecuteReader();

            lbl.Text = dataReader.GetString(1);
            conn.Close();
        }

        private string GrabData(string sql)
        {
            ConnectDatabase();
            cmd = new SqlCommand(sql, conn);
            adap.SelectCommand = cmd;
            adap.Fill(ds, "Data");
            SqlDataReader dataReader = cmd.ExecuteReader();

            return dataReader.GetString(1);
            conn.Close();
        }

        public Book()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void gbxTime_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Book_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome user!");
            Reset();

        }

        private void cbxMovie_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load movie description from database
            GrabDataRtbx(@"SELECT Description FROM Movies WHERE Movie_Name = " + cbxMovie.SelectedItem, rtbxMovie); //NB: no such column yet
            //load restriction
            GrabDataLbl(@"SELECT Age_Restrict FROM Movies WHERE Movie_Name = " + cbxMovie.SelectedItem, lblRestriction);
            //unload genre
            cbxGenre.SelectedIndex = 0;
            //allow booking
            btnBook.Enabled = true;
        }

        private void cbxGenre_SelectedIndexChanged(object sender, EventArgs e)
        {
            //load movie names in genre
            GrabDataCbx(@"SELECT Movie_Name FROM Movies WHERE Genre_Id = " + cbxGenre.SelectedItem, cbxMovie);
        }

        private void cbxSpecial_SelectedIndexChanged(object sender, EventArgs e)
        {

            //check first method
            //GrabDataCbx(@"SELECT Movie_Name FROM Movies WHERE Genre_Id = " + cbxSpecial.SelectedItem, cbxMovie);
            GrabDataRtbx(@"SELECT Description FROM Specials", rtbxSpecial);
            GrabDataLbl(@"SELECT Price FROM Specials WHERE Special_Id = " + cbxSpecial.SelectedItem, lblSpecialPrice);

        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            //discuss how to "check" username, and generate new ones

            DialogResult result = MessageBox.Show("Do you want to check out?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                gbxDateTime.Enabled = false;
                gbxMovie.Enabled = false;
                gbxTicket.Enabled = true;

                //discuss how to generate the following:

                //Booking_Id
                string Booking_Id;
                //Client_Id - see username comment ^

                //Special_Id
                string Special_Id = cbxSpecial.SelectedItem.ToString();
                //Booking_For
                string Booking_For = monthCalendar2.SelectionStart.ToString() + cbxTime.SelectedItem.ToString();
                //Movie_Id
                string Movie_Id = GrabData(@"SELECT Movie_Id FROM Movies WHERE Movie_Name = " + cbxMovie.SelectedItem);
                //Seat_Code - how should we keep track of available seats


                //write to rich text - format?


                //Payment
                    //Dont have required hardware to present over zoom meeting, rtf would be printed on "slips" to give to the customer?
                    //Future expansion - sms "slips" to customer?
                    //discuss what we should do with the slips futjer

                //write to database
                    
                

            }
        }
    }
}
