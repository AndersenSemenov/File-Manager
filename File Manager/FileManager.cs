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
using System.Windows.Controls;
using ListView = System.Windows.Forms.ListView;
using ListViewItem = System.Windows.Forms.ListViewItem;
using TextBox = System.Windows.Forms.TextBox;
using System.Diagnostics;
using System.IO.Compression;

namespace File_Manager
{
    public partial class FileManager : Form
    {
        static bool flag = true;
        public User user = null;

        public FileManager()
        {
            InitializeComponent();
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    listView1.Items.Add(drive.Name);
                    listView2.Items.Add(drive.Name);
                }
            }
            ReDraw();
        }

        private void openDirectory(string name, ListView listView)
        {
            if (name != "")
            {
                listView.Items.Add("...");
                DirectoryInfo di = new DirectoryInfo(name);
                foreach (var directory in di.GetDirectories())
                {
                    ListViewItem item = new ListViewItem(directory.Name);
                    item.SubItems.Add("dir");
                    item.SubItems.Add("");
                    item.SubItems.Add(directory.LastWriteTime.ToString());
                    listView.Items.Add(item);
                }
                foreach (var file in di.GetFiles())
                {
                    ListViewItem item = new ListViewItem(file.Name);
                    item.SubItems.Add(file.Extension);
                    item.SubItems.Add(file.Length.ToString());
                    item.SubItems.Add(file.LastWriteTime.ToString());
                    listView.Items.Add(item);
                }
            }
            else
            {
                foreach (var drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)
                    {
                        listView.Items.Add(drive.Name);
                    }
                }
            }
        }

        private string ShortCutPath(string path)
        {
            int i = path.Length - 1;
            if (path[i] == 92)
            {
                return "";
            }
            while (i >= 0 && path[i] != 92)
            {
                i--;
            }
            if (path[i - 1] == ':')
            {
                return path.Substring(0, i + 1);
            }
            else
            {
                return path.Substring(0, i);
            }
        }

        private void MyDoubleClick(ListView listView, TextBox textBox)
        {
            if (listView.SelectedItems[0].Text == "...")
            {
                textBox.Text = ShortCutPath(textBox.Text);
                listView.Items.Clear(); 
                openDirectory(textBox.Text, listView);
            }
            else
            {
                string fullPath = Path.Combine(textBox.Text, listView.SelectedItems[0].Text);
                if (File.Exists(fullPath))
                {
                    Process.Start(fullPath);
                }
                else if (Directory.Exists(fullPath))
                {
                    listView.Items.Clear();
                    textBox.Text = fullPath;
                    openDirectory(textBox.Text, listView);
                }
            }
            ReDraw();
        }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MyDoubleClick(listView1, textBox1);
        }

        private void listView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MyDoubleClick(listView2, textBox2);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            var currentBox = flag ? textBox1 : textBox2;
            var currentList = flag ? listView1 : listView2;
            if (currentList.SelectedItems.Count != 0)
            {

                var fullPath = Path.Combine(currentBox.Text, currentList.SelectedItems[0].Text);
                var CurrentExtension = Path.GetExtension(fullPath);
                if (CurrentExtension == "")
                {
                    Directory.Delete(fullPath, true);
                    MessageBox.Show($"Directory {currentList.SelectedItems[0].Text} was successfuly deleted");
                }
                else
                {
                    File.Delete(fullPath);
                    MessageBox.Show($"File {currentList.SelectedItems[0].Text} was successfuly deleted");
                }
                ReDraw();
            }
            else
            {
                MessageBox.Show("Choose a file to delete");
            }
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            var currentBox = flag ? textBox1 : textBox2;
            var currentList = flag ? listView1 : listView2;
            if (currentList.SelectedItems.Count != 0)
            {
                var currentPath = Path.Combine(currentBox.Text, currentList.SelectedItems[0].Text);
                RenameForm form2 = new RenameForm();
                form2.ShowDialog();
                string newName = form2.currentText;
                var CurrentExtension = Path.GetExtension(currentPath);
                if (CurrentExtension == "")
                {
                    Directory.Move(currentPath, Path.Combine(currentBox.Text, newName));
                    MessageBox.Show($"Directory {currentList.SelectedItems[0].Text} was successfuly renamed to {newName}");
                }
                else
                {
                    File.Move(currentPath, Path.Combine(currentBox.Text, newName) + CurrentExtension);
                    MessageBox.Show($"File {currentList.SelectedItems[0].Text} was successfuly renamed to {newName + CurrentExtension}");
                }
                ReDraw();
            }
            else
            {
                MessageBox.Show("Choose an element to rename first!");
            }
        }

        private void listView1_Enter(object sender, EventArgs e)
        {
            flag = true;
        }

        private void listView2_Enter(object sender, EventArgs e)
        {
            flag = false;
        }

        private void ReDraw()
        {
            listView1.Items.Clear();
            openDirectory(textBox1.Text, listView1);
            listView2.Items.Clear();
            openDirectory(textBox2.Text, listView2);
            if (user != null)
            {
                this.SettingsButton.Visible = true;
                this.LogOutButton.Visible = true;
                this.textBox3.Visible = true;
                this.textBox4.Visible = true;
                this.SignUpButton.Visible = false;
                this.SignInButton.Visible = false;
                this.textBox4.Text = user.Login;
                this.Text = user.AppName;
                ChangeFontSize(user.FontSize, user.FontSizeButton);
            }
            else
            {
                this.SettingsButton.Visible = false;
                this.LogOutButton.Visible = false;
                this.textBox3.Visible = false;
                this.textBox4.Visible = false;
                this.SignUpButton.Visible = true;
                this.SignInButton.Visible = true;
                this.Text = "File Manager";
                ChangeFontSize(12f, 16f);
            }
        }


        private void ChangeFontSize(float size1, float size2)
        {
            this.listView1.Font = new Font("Microsoft Sans Serif", size1);
            this.listView2.Font = new Font("Microsoft Sans Serif", size1);

            this.DeleteButton.Font = new Font("Microsoft Sans Serif", size2);
            this.RenameButton.Font = new Font("Microsoft Sans Serif", size2);
            this.CopyButton.Font = new Font("Microsoft Sans Serif", size2);
            this.ArchiveButton.Font = new Font("Microsoft Sans Serif", size2);
            this.MoveButton.Font = new Font("Microsoft Sans Serif", size2);
            this.CreateButton.Font = new Font("Microsoft Sans Serif", size2);
            this.SettingsButton.Font = new Font("Microsoft Sans Serif", size2);
            this.LogOutButton.Font = new Font("Microsoft Sans Serif", size2);
        }

        private void ArchiveButton_Click(object sender, EventArgs e)
        {
            var currentBox = flag ? textBox1 : textBox2;
            var currentList = flag ? listView1 : listView2;
            if (currentList.SelectedItems.Count != 0)
            {
                var boxTo = flag ? textBox2 : textBox1;
                var currentPath = Path.Combine(currentBox.Text, currentList.SelectedItems[0].Text);
                var CurrentExtension = Path.GetExtension(currentPath);
                if (CurrentExtension == "")
                {
                    string zipPath = boxTo.Text + "\\" + currentList.SelectedItems[0].Text + ".zip";
                    ZipFile.CreateFromDirectory(currentPath, zipPath);
                    Directory.Delete(currentPath, true);
                    MessageBox.Show($"Directory {currentList.SelectedItems[0].Text} was successfuly archived to {zipPath}");
                }
                else
                {
                    MessageBox.Show("Choose a directory to archive!");
                }
                ReDraw();
            }
            else
            {
                MessageBox.Show("Choose a directory to archive!");
            }
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            var currentBox = flag ? textBox1 : textBox2;
            var currentList = flag ? listView1 : listView2;
            if (currentList.SelectedItems.Count != 0)
            {
                var boxTo = flag ? textBox2 : textBox1;
                var currentPath = Path.Combine(currentBox.Text, currentList.SelectedItems[0].Text);
                var CurrentExtension = Path.GetExtension(currentPath);
                if (CurrentExtension == "")
                {
                    string newPath = Path.Combine(boxTo.Text, currentList.SelectedItems[0].Text);
                    Directory.CreateDirectory(newPath);
                    DirectoryCopy(currentPath, newPath, true);
                    MessageBox.Show($"Directory {currentList.SelectedItems[0].Text} was successfuly copied to {boxTo.Text}");
                }
                else
                {
                    File.Copy(currentPath, Path.Combine(boxTo.Text, currentList.SelectedItems[0].Text), true);
                    MessageBox.Show($"File {currentList.SelectedItems[0].Text} was successfuly copied to {boxTo.Text}");
                }
                ReDraw();
            }
            else
            {
                MessageBox.Show("Choose an element to copy first!");
            }
        }

        private void MoveButton_Click(object sender, EventArgs e)
        {
            var currentBox = flag ? textBox1 : textBox2;
            var currentList = flag ? listView1 : listView2;
            if (currentList.SelectedItems.Count != 0)
            {
                var boxTo = flag ? textBox2 : textBox1;
                var currentPath = Path.Combine(currentBox.Text, currentList.SelectedItems[0].Text);
                var CurrentExtension = Path.GetExtension(currentPath);
                if (CurrentExtension == "")
                {
                    Directory.Move(currentPath, Path.Combine(boxTo.Text, currentList.SelectedItems[0].Text));
                    MessageBox.Show($"Directory {currentList.SelectedItems[0].Text} was successfuly moved to {boxTo.Text}");
                }
                else
                {
                    File.Move(currentPath, Path.Combine(boxTo.Text, currentList.SelectedItems[0].Text));
                    MessageBox.Show($"File {currentList.SelectedItems[0].Text} was successfuly moved to {boxTo.Text}");
                }
                ReDraw();
            }
            else
            {
                MessageBox.Show("Choose an element to move first!");
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            var currentBox = flag ? textBox1 : textBox2;
            var currentList = flag ? listView1 : listView2;
            Create createForm = new Create();
            createForm.ShowDialog();
            if (createForm.Extension == "")
            {
                string newName = createForm.currentText;
                string newPath = Path.Combine(currentBox.Text, newName);
                Directory.CreateDirectory(newPath);
            }
            else
            {
                File.Create(Path.Combine(currentBox.Text, createForm.currentText) + createForm.Extension);
            }
            ReDraw();
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }
            DirectoryInfo[] dirs = dir.GetDirectories();       
            Directory.CreateDirectory(destDirName);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, false);
            }
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }


        private void SignUpButton_Click(object sender, EventArgs e)
        {
            LoginForm form3 = new LoginForm();
            form3.label1.Text = "Sign up into a system:";
            form3.button1.Text = "Sign up!";
            form3.ShowDialog();
            user = form3.currentUser;
            ReDraw();
        }

        private void SignInButton_Click(object sender, EventArgs e)
        {
            LoginForm form3 = new LoginForm();
            form3.label1.Text = "Sign in into a system:";
            form3.button1.Text = "Sign in!";
            form3.ShowDialog();
            user = form3.currentUser;
            ReDraw();
        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            user = null;
            ReDraw();
            MessageBox.Show("You have successfully logged out");
        }

        private void SettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.ShowDialog();
            Users users = Serialization.Deserialize();
            foreach (User forUser in users.usersList)
            {
                if (forUser.Login == user.Login && forUser.Password == user.Password)
                {
                    forUser.Login = settingsForm.Login != "" ? settingsForm.Login : forUser.Login;
                    forUser.Password = settingsForm.Password != "" ? settingsForm.Password : forUser.Password;
                    forUser.AppName = settingsForm.AppName != "" ? settingsForm.AppName : forUser.AppName;
                    forUser.FontSize = settingsForm.FontSize != 0 ? settingsForm.FontSize : forUser.FontSize;
                    forUser.FontSizeButton = settingsForm.FontSizeButton != 0? settingsForm.FontSizeButton : forUser.FontSizeButton;
                }
            }
            users = Serialization.Refresh(users);
            user.Login = settingsForm.Login != "" ? settingsForm.Login : user.Login;
            this.textBox4.Text = settingsForm.Login;
            user.Password = settingsForm.Password != "" ? settingsForm.Password : user.Password;
            user.AppName = settingsForm.AppName != "" ? settingsForm.AppName : user.AppName;
            user.FontSize = settingsForm.FontSize != 0 ? settingsForm.FontSize : user.FontSize;
            user.FontSizeButton = settingsForm.FontSizeButton != 0 ? settingsForm.FontSizeButton : user.FontSizeButton;
            ReDraw();
        }

        private void FileManager_Load(object sender, EventArgs e)
        {

        }
    }
}