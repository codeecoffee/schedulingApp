using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace schedulingApp
{
    public partial class EditCustomerForn : Form
    {
        public EditCustomerForn()
        {
            InitializeComponent();
        }

        private void bttnDeleteCustomer_Click(object sender, EventArgs e)
        {
            //TODO!
            //MessageBox to confirm deletion
            // Use DB Aux to delete from DB
            //Reload the Customer data grid
            //MessageBox to confirm that user is deleted from DB

        }

        private void bttnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
