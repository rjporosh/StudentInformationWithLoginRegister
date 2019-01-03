using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_Register
{
    public partial class Main : Form
    {
        string uType;
        string uName;
        private DataTable dt;
        string connection = "data source=MYLAPTOP\\SQLEXPRESS12;initial catalog=LoginRegister;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
        tblStudentInformation model = new tblStudentInformation();
       
        public Main(string name,string type)
        {
            InitializeComponent();
            uName=name;
            uType = type;
            userTableOnly();
            clear();
        }

        void userTableOnly()
        {
            SqlConnection con = new SqlConnection(connection);
            if (uType == "Admin")
            {
                SqlDataAdapter sda = new SqlDataAdapter("Select * from tblStudentInformation  ", con);
                dt = new DataTable();
                sda.Fill(dt);
                DataTable tempDT = new DataTable();
                tempDT = dt.DefaultView.ToTable(true, "Id", "StudentName", "Class", "Roll","Section" ,"Age","AddedBy");
                //Now bind this to DataGridView
                dgvMain.DataSource = tempDT;
                con.Close();
            }
            else
            {
                SqlDataAdapter sda = new SqlDataAdapter("Select * from tblStudentInformation where UserName = '" + uName + "' and Type = '" + uType + "' ", con);
                dt = new DataTable();
                sda.Fill(dt);
                DataTable tempDT = new DataTable();
                tempDT = dt.DefaultView.ToTable(true, "Id", "StudentName", "Class", "Roll","Section" ,"Age","AddedBy");
                //Now bind this to DataGridView
                dgvMain.DataSource = tempDT;
                con.Close();
            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            model.AddedBy = uName;
            model.Age = txtAge.Text;
            model.StudentName = txtStudentName.Text;
            model.Class = txtClass.Text;
            model.Section = txtSection.Text;
            model.Roll = txtRoll.Text;

            using (Entities db = new Entities())
            {
                if (model.Id == 0) //Insert
                {
                    db.tblStudentInformations.Add(model);
                    db.SaveChanges();
                    userTableOnly();
                    clear();

                    MessageBox.Show("Saved new Successfully");
                }
                else //Update
                {
                    db.Entry(model).State = EntityState.Modified;
                    db.SaveChanges();
                    userTableOnly();
                    clear();
                    MessageBox.Show("Updated Successfully");
                }
            }
        }

        void clear()
        {
            txtStudentName.Text = "";
            txtClass.Text = "";
            txtRoll.Text = "";
            txtSection.Text = "";
            txtAge.Text = "";
            btnSave.Text = "Add New";
            userTableOnly();
            btnDelete.Enabled = false;
            model.Id = 0;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure to Delete this Information ?", uName+"'s Student Information", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (Entities db = new Entities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.tblStudentInformations.Attach(model);
                    db.tblStudentInformations.Remove(model);
                    db.SaveChanges();
                    userTableOnly();
                    clear();
                    MessageBox.Show("Deleted Successfully");
                }
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home h = new Home();
            h.ShowDialog();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            userTableOnly();
            clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvMain.CurrentRow.Index != -1)
            {
                model.Id = Convert.ToInt32(dgvMain.CurrentRow.Cells["Id"].Value);
                using (Entities db = new Entities())
                {
                    model = db.tblStudentInformations.Where(x => x.Id == model.Id).FirstOrDefault();
                    txtStudentName.Text = model.StudentName;
                    txtClass.Text = model.Class;
                    txtRoll.Text = model.Roll;
                    txtSection.Text = model.Section;
                    txtAge.Text = model.Age;
                    uName = model.AddedBy;
                    
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }

        }
    }
}
