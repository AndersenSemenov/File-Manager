using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Manager
{
    class Book
    {
        public string Reference { get; set; }
        public string Name { get; set; }
        public string Cost { get; set; }
        public string Author { get; set; }
        public string Pages { get; set; }
        public string Date { get; set; }
        public Book(string Reference, string Name, string Cost, string Author, string Pages, string Date)
        {
            this.Reference = Reference;
            this.Name = Name;
            this.Cost = Cost;
            this.Author = Author;
            this.Pages = Pages;
            this.Date = Date;
        }
    }
}