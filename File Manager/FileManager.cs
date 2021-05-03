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
using System.Collections;

namespace File_Manager
{
    public partial class FileManager : Form
    {
        static bool flag = true;
        public User user = null;
        private bool IsFile = true;
        private ListViewColumnSorter lvwColumnSorter;
        private DataComparer dc;

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
            DrawFiles();
        }

        private void openDirectory(string name, ListView listView)
        {
            if (name != "")
            {
                ListViewItem first = new ListViewItem("...");
                first.SubItems.Add("");
                first.SubItems.Add("");
                first.SubItems.Add("");
                listView.Items.Add(first);
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
            if (IsFile)
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
                DrawFiles();
            }
            else
            {
                string href = listView.SelectedItems[0].Tag.ToString();
                Process.Start(href);
            }
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
                DrawFiles();
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
                DrawFiles();
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


        private void DrawFiles()
        {
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
            this.listView2.ListViewItemSorter = lvwColumnSorter;

            listView1.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("Name", 380);
            listView1.Columns.Add("Extension", 150);
            listView1.Columns.Add("Size");
            listView1.Columns.Add("Last Change", 180);
            openDirectory(textBox1.Text, listView1);

            listView2.Items.Clear();
            listView2.Columns.Clear();
            listView2.Columns.Add("Name", 380);
            listView2.Columns.Add("Extension", 150);
            listView2.Columns.Add("Size");
            listView2.Columns.Add("Last Change", 180);
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

        private void DrawBooks(List<Book> books)
        { 

            listView1.Items.Clear();
            listView2.Items.Clear();
            listView1.Columns.Clear();
            listView1.Columns.Add("Name", 350);
            listView1.Columns.Add("Cost");
            listView1.Columns.Add("Author", 180);
            listView1.Columns.Add("Pages");
            listView1.Columns.Add("Date", 85);

            listView2.Columns.Clear();
            listView2.Columns.Add("Name", 350);
            listView2.Columns.Add("Cost");
            listView2.Columns.Add("Author", 180);
            listView2.Columns.Add("Pages");
            listView2.Columns.Add("Date", 85);

            foreach (var book in books)
            {
                ListViewItem item1 = new ListViewItem(book.Name);
                item1.SubItems.Add(book.Cost);
                item1.SubItems.Add(book.Author);
                item1.SubItems.Add(book.Pages);
                item1.SubItems.Add(book.Date);
                item1.Tag = book.Reference;
                listView1.Items.Add(item1);
                listView2.Items.Add(item1.Clone() as ListViewItem);
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
                DrawFiles();
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
                DrawFiles();
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
                DrawFiles();
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
            DrawFiles();
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
            DrawFiles();
        }

        private void SignInButton_Click(object sender, EventArgs e)
        {
            LoginForm form3 = new LoginForm();
            form3.label1.Text = "Sign in into a system:";
            form3.button1.Text = "Sign in!";
            form3.ShowDialog();
            user = form3.currentUser;
            DrawFiles();
        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            user = null;
            DrawFiles();
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
                    forUser.FontSizeButton = settingsForm.FontSizeButton != 0 ? settingsForm.FontSizeButton : forUser.FontSizeButton;
                }
            }
            users = Serialization.Refresh(users);
            user.Login = settingsForm.Login != "" ? settingsForm.Login : user.Login;
            this.textBox4.Text = settingsForm.Login;
            user.Password = settingsForm.Password != "" ? settingsForm.Password : user.Password;
            user.AppName = settingsForm.AppName != "" ? settingsForm.AppName : user.AppName;
            user.FontSize = settingsForm.FontSize != 0 ? settingsForm.FontSize : user.FontSize;
            user.FontSizeButton = settingsForm.FontSizeButton != 0 ? settingsForm.FontSizeButton : user.FontSizeButton;
            DrawFiles();
        }

        private void Books_Click(object sender, EventArgs e)
        {
            lvwColumnSorter = new ListViewColumnSorter();
            this.listView1.ListViewItemSorter = lvwColumnSorter;
            this.listView2.ListViewItemSorter = lvwColumnSorter;
            dc = new DataComparer(SortOrder.None);
            IsFile = false;
            BookSearcher bookSearcher = new BookSearcher();
            bookSearcher.ShowDialog();
            List<Book> books = Parser.Parse(bookSearcher.BookName, bookSearcher.Count);
            DrawBooks(books);
        }

        private void FilesButton_Click(object sender, EventArgs e)
        {
            IsFile = true;
            DrawFiles();
        }

        private void listView1_ColumnClick(object o, ColumnClickEventArgs e)
        {
            ColumnSort(listView1, e);
        }

        private void listView2_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ColumnSort(listView2, e);
        }

        private void ColumnSort(ListView lv, ColumnClickEventArgs e)
        {

            lvwColumnSorter.SortColumn = e.Column;
            if (e.Column == 4 && !IsFile)
            {
                dc.Order = dc.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
                lv.ListViewItemSorter = new DataComparer(dc.Order);
            }
            else
            {
                lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
                lv.ListViewItemSorter = lvwColumnSorter;
                lv.Sort();
            }
        }
    }         
}