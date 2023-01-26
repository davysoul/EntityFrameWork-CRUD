using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFCRUDAPP
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            txtAddress.Clear();
            txtCity.Clear();
            txtFirstname.Clear();
            txtLastname.Clear();
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            model.FirstName = txtFirstname.Text.Trim();
            model.LastName = txtLastname.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();
            using (EFDBEntities db = new EFDBEntities())
            {
               
                db.Database.Connection.Open();
                if (model.CustomerID==0)
                {
                    db.Customer.Add(model);
                }
                else
                {
                    db.Entry(model).State = EntityState.Modified;
                }
               
                
                db.SaveChanges();
                
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Submitted successfully");
            
        }
        private void PopulateDataGridView()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using (EFDBEntities db = new EFDBEntities())
            {
                db.Database.Connection.Open();
                dgvCustomer.DataSource = db.Customer.ToList<Customer>();
            }
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["CustomerID"].Value);
                using (EFDBEntities db = new EFDBEntities())
                {
                    model = db.Customer.Where(c => c.CustomerID.Equals(model.CustomerID)).FirstOrDefault();
                    txtAddress.Text = model.Address;
                    txtCity.Text = model.City;
                    txtFirstname.Text = model.FirstName;
                    txtLastname.Text = model.LastName;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are sure to delete this record?","EF Crud Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (EFDBEntities db = new EFDBEntities())
                {
                    var entry = db.Entry(model);
                    Console.WriteLine(entry);
                    if (entry.State == EntityState.Detached)
                        db.Customer.Attach(model);
                    db.Customer.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Record was deleted successfully");
                }
            }
          
        }
    }
}
