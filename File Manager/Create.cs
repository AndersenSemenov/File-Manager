using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_Manager
{
    public partial class Create : Form
    {
        public string currentText = "";
        public string Extension = "";
        public bool ready = false;
        public Create()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentText = textBox1.Text;
            Extension = comboBox1.Text == "Directory" ?  "" : ".txt";
            ready = true;
            this.Close();
        }

        private void Create_Load(object sender, EventArgs e)
        {

        }
    }
}
