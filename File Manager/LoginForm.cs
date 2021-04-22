using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace File_Manager
{
    public partial class LoginForm : Form
    {
        public User currentUser;
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Sign up!") // sign up
            {
                User newUser = new User(textBox1.Text, textBox2.Text);
                Users users = Serialization.Deserialize();
                bool IsNew = true;
                foreach (User user in users.usersList)
                {
                    if(newUser.Login == user.Login && newUser.Password == user.Password)
                    {
                        IsNew = false;
                    }
                }
                if (IsNew)
                {
                    users.usersList.Add(newUser);
                    users = Serialization.Refresh(users);
                    currentUser = users.usersList[users.usersList.Count - 1];
                    MessageBox.Show("You have successfully signed up");
                }
                else
                {
                    MessageBox.Show("You have already been signed up, use button 'sign in'!");
                }
            }
            else if (button1.Text == "Sign in!") // sign in
            {
                User userEnter = new User(textBox1.Text, textBox2.Text);
                Users users = Serialization.Deserialize();
                bool Exists = false;
                foreach (User user in users.usersList)
                {
                    if (userEnter.Login == user.Login && userEnter.Password == user.Password)
                    {
                        currentUser = user;
                        Exists = true;
                        MessageBox.Show("You have successfully signed in");
                    }
                }
                if (!Exists)
                {
                    MessageBox.Show("There is no such user, use button 'sign up' first!");
                }
            }
        }
    }
}
