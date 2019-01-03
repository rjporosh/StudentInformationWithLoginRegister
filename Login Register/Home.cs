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
   
    public partial class Home : Form
    {
        string connection = "data source=MYLAPTOP\\SQLEXPRESS12;initial catalog=LoginRegister;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
        int count =1;
        public Home()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
          var isOk = isValid(txtPassword, label2) &
                       isValid(txtType, label3) &
                       isValid(txtUserName, label1);
            if (isOk)
            {
               SqlConnection con = new SqlConnection(connection);
                SqlDataAdapter sda = new SqlDataAdapter("Select Type from tblLoginregister Where UserName= '" + txtUserName.Text + "'and Password ='" + txtPassword.Text + " ' and Type ='"+txtType.Text+" ' ", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    //MessageBox.Show("Login Success", "Congrates");
                    if (checkRemember.Checked)
                    {
                        Properties.Settings.Default.UserName = txtUserName.Text;
                        Properties.Settings.Default.Password = txtPassword.Text;
                        Properties.Settings.Default.Type = txtType.Text;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        Properties.Settings.Default.UserName = "";
                        Properties.Settings.Default.Password = "";
                        Properties.Settings.Default.Type = "";
                        Properties.Settings.Default.Save();
                    }

                    pictureBox1.Image = Login_Register.Properties.Resources.unlocking;
                    MessageBox.Show("Login Success", "Congrates");
                    MessageBox.Show("WelCome dear " + txtUserName.Text);
                    Main m = new Main(txtUserName.Text,txtType.Text);
                    this.Hide();
                    m.ShowDialog();
                    con.Close();

                }
                else
                {
                    pictureBox1.Image = Login_Register.Properties.Resources.lockFinal;

                    MessageBox.Show("Incorrect User Name Or Password or Type");
                }
              
            }
            else
            {
                MessageBox.Show("Fill All the field.");
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register r = new Register();
            r.ShowDialog();
        }

        private bool isValid(System.Windows.Forms.TextBox textBox, System.Windows.Forms.Label label)
        {
            var validity = true;
            if (textBox.Text == "")
            {
                validity = false;
                label.ForeColor = System.Drawing.ColorTranslator.FromHtml("#1aff0000");
                textBox.BackColor = System.Drawing.Color.Red;
                // textBox.ForeColor = System.Drawing.Color.Red;

            }
            else
            {
                validity = true;
                label.ForeColor = System.Drawing.Color.LightGray;
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

        private void Home_Load(object sender, EventArgs e)
        {
            txtUserName.Text = Properties.Settings.Default.UserName;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtType.Text = Properties.Settings.Default.Type;
        }
    }
}
