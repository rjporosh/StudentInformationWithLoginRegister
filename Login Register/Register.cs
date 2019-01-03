using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_Register
{
    public partial class Register : Form
    {
        string connection = "data source=MYLAPTOP\\SQLEXPRESS12;initial catalog=LoginRegister;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
        int count =1;
        int Available = 1;
        tblLoginRegister modelRegister = new tblLoginRegister();
        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            CheckUsernameIsAvailable();
            var isOk = isValid(txtPassword, label3) &
                       isValid(txtType, label4) &
                       isValid(txtFullName, label1) &
                       isValid(txtUserName, label2);
            if (isOk && Available==0 )
            {
              
                
                modelRegister.FullName = txtFullName.Text.Trim();
                modelRegister.UserName = txtUserName.Text.Trim();
                modelRegister.Password = txtPassword.Text.Trim();
                modelRegister.Type = txtType.Text.Trim();
                using (Entities db = new Entities())
                {
                    if (modelRegister.Id == 0 && Available == 0 ) //Insert
                    {

                        db.tblLoginRegisters.Add(modelRegister);
                        db.SaveChanges();
                        MessageBox.Show("Registered Successfully", "Registered");


                    }
                    else //Update
                    {
                        MessageBox.Show("Registration failed", "Error");
                    }

                }
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home h = new Home();
            h.ShowDialog();
        }

        private void picShow_Click(object sender, EventArgs e)
        {
            picShow.Image = Login_Register.Properties.Resources.hide;

            if (count % 2 == 0)
            {
                picShow.Image = Login_Register.Properties.Resources.show;
                txtPassword.PasswordChar = '*';
            }
            else
            {
                picShow.Image = Login_Register.Properties.Resources.hide;
                txtPassword.PasswordChar = '\0';
            }
            count++;
        }

        void CheckUsernameIsAvailable()
        {
            SqlConnection con = new SqlConnection(connection);
            SqlDataAdapter sda =
                new SqlDataAdapter(
                    "Select * from tblLoginRegister Where UserName= '" + txtUserName.Text + "' And Type= '" + txtType.Text + "'", con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                picAvailable.Visible = true;
                MessageBox.Show("User Exist");
                picAvailable.Image = Login_Register.Properties.Resources.X;

                txtUserName.Text = "";
                txtUserName.BackColor = System.Drawing.Color.Red;
                txtUserName.ForeColor = System.Drawing.Color.Black;
                label2.ForeColor = System.Drawing.ColorTranslator.FromHtml("#1aff0000");
                Available = 1;
            }
            else
            {
               
                picAvailable.Image = Login_Register.Properties.Resources.Available;
                txtUserName.BackColor = System.Drawing.Color.LightGray;
                label2.ForeColor = System.Drawing.Color.LightGray;
                picAvailable.Visible = true;
                Available = 0;

            }

        }

        private bool isValid(System.Windows.Forms.TextBox textBox, System.Windows.Forms.Label label)
        {
            var validity = true;
            if (textBox.Text == "")
            {
                validity = false;
                label.ForeColor = System.Drawing.ColorTranslator.FromHtml("#1aff0000");
                // textBox.ForeColor = System.Drawing.Color.Red;
                textBox.BackColor = System.Drawing.Color.Red;
                textBox.ForeColor = System.Drawing.Color.LightGray;

            }
            else
            {
                validity = true;
                label.ForeColor = System.Drawing.Color.LightGray;
                //textBox.ForeColor = System.Drawing.Color.LightGray;
                textBox.BackColor = System.Drawing.Color.LightGray;

            }


            return validity;
        }

        private bool isValid(System.Windows.Forms.ComboBox comboBox, System.Windows.Forms.Label label)
        {
            var validity = true;
            if (comboBox.Text == "")
            {
                validity = false;
                label.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                validity = true;
                label.ForeColor = System.Drawing.Color.LightGray;
            }

            return validity;
        }
    }
}
