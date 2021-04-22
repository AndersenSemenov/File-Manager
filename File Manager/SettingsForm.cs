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
    public partial class SettingsForm : Form
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string AppName { get; set; }
        public float FontSize { get; set; }
        public float FontSizeButton { get; set; }
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            Login = textBox1.Text;
            Password = textBox2.Text;
            AppName = textBox3.Text;
            FontSize = (float)numericUpDown1.Value;
            FontSizeButton = (float)numericUpDown2.Value;
            this.Close();
        }
    }
}