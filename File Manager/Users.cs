using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace File_Manager
{
    [Serializable]
    public class Users
    {
        public List<User> usersList { get; set;} = new List<User>();
    }

    [Serializable]
    public class User
    {
        public string Login;
        public string Password;
        public string AppName;
        public float FontSize;
        public float FontSizeButton;

        public User() { }

        public User(string Login, string Password)
        {
            this.Login = Login;
            this.Password = Password;
            this.AppName = "File Manager";
            this.FontSize = 12;
            this.FontSizeButton = 16;
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext contex)
        {
            Login = Crypt.Encrypt(Login);
            Password = Crypt.Encrypt(Password);
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext contex)
        {
            Login = Crypt.Decrypt(Login);
            Password = Crypt.Decrypt(Password);
        }
    }
}
