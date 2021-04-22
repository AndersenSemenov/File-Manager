using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace File_Manager
{
    public partial class RenameForm : Form
    {
        public string currentText = "";
        public RenameForm()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                currentText = textBox1.Text;
                this.Close();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }
    }
}