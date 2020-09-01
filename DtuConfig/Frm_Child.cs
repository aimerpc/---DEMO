using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DtuConfig
{
    public partial class Frm_Child : Form
    {
        public Frm_Child()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.strApnData = "\"" + textBoxApnName.Text + "\",\"" + textBoxApnAccount.Text + "\",\"" + textBoxApnKey.Text + "\"";
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
