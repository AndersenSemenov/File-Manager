using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace File_Manager
{
    class Serialization
    {

        public static void Serialize(Users users)
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(@"D:\Binary.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            bf.Serialize(stream, users);
            stream.Close();
        }

        public static Users Deserialize()
        {
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(@"D:\Binary.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            if (stream.Length != 0)
            {
                Users users = (Users)bf.Deserialize(stream);
                stream.Close();
                return users;
            }
            else
            {
                stream.Close();
                return new Users();
            }
        }

        public static Users Refresh(Users users)
        {
            Serialize(users);
            return Deserialize();
        }
    }
}
