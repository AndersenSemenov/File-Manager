using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace File_Manager
{
    class DataComparer : IComparer
    {
        public SortOrder Order { get; set; }

        public DataComparer(SortOrder Order)
        {
            this.Order = Order;
        }
        private int GetMonth(string s)
        {
            string month = s.Substring(0, s.IndexOf(' '));
            Dictionary<string, int> months = new Dictionary<string, int>();
            months.Add("Jan", 1);
            months.Add("Feb", 2);
            months.Add("Mar", 3);
            months.Add("Apr", 4);
            months.Add("May", 5);
            months.Add("Jun", 6);
            months.Add("Jul", 7);
            months.Add("Aug", 8);
            months.Add("Sep", 9);
            months.Add("Oct", 10);
            months.Add("Nov", 11);
            months.Add("Dec", 12);
            return months[month];
        }

        private int GetYear(string s)
        {
            int index1 = s.IndexOf(' ') + 1;
            string ss = s.Substring(index1);
            return Convert.ToInt32(ss);
        }

        public int Compare(object x, object y)
        {
            string X, Y;

            X = ((ListViewItem)x).SubItems[4].Text;
            Y = ((ListViewItem)y).SubItems[4].Text;
            DateTime date1 = new DateTime(GetYear(X), GetMonth(X), 1);
            DateTime date2 = new DateTime(GetYear(Y), GetMonth(Y), 1);
            int compareResult =  date1.CompareTo(date2);
            if (Order != SortOrder.Ascending)
            {
                return compareResult;
            }
            else
            {
                return (-compareResult);
            }
        }
    }
}
