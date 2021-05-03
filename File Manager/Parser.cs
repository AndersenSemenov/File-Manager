using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using System.Web;

namespace File_Manager
{
    class Parser
    {
        public static List<Book> Parse(string Request, int Count)
        {
            Regex RegexBook = new Regex(@"<div class=?.card h-100?.(.|\n)*?(?=(<div class=?.card h-100?.|::after))");
            Regex RegexRef = new Regex(@"<a.*href=.(?'refer'.*).*?(?=..class)");
            Regex RegexName = new Regex(@"<b>(?'name'.*)</b>");
            Regex RegexCost = new Regex(@"class=.price-inline.*\n.*\n\s *\$(?'cost'.*)");
            Regex RegexAuthor = new Regex(@"<div class=.author-names.*\n\s*<p.By (?'author'.*)</p>");
            Regex RegexPageDate = new Regex(@"<div class=.product-meta.*\n\s*<p class=.page_count.>(?'pages'.*) pages</p>\s*<p>(?'date'.*)</p>");
            
            List<Book> books = new List<Book>();
            int current = 0;
            int page = 1;
            while (current < Count)
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--headless");
                ChromeDriver driver = new ChromeDriver(@"D:\С++", options);
                driver.Navigate().GoToUrl("https://www.packtpub.com/catalogsearch/result?q=" + HttpUtility.UrlEncode(Request) + "&product_type_filter=Book&released=Available" + "&page=" + page.ToString());
                string s = driver.PageSource;
                var matches = RegexBook.Matches(s);

                foreach (Match match in matches)
                {
                    string Reference = RegexRef.Match(match.Value).Groups["refer"].ToString();
                    string Name = RegexName.Match(match.Value).Groups["name"].ToString();
                    string Cost = RegexCost.Match(match.Value).Groups["cost"].ToString();
                    string Author = RegexAuthor.Match(match.Value).Groups["author"].ToString();
                    string Pages = RegexPageDate.Match(match.Value).Groups["pages"].ToString();
                    string Date = RegexPageDate.Match(match.Value).Groups["date"].ToString();

                    books.Add(new Book(Reference, Name, Cost, Author, Pages, Date));
                    current++;
                    if (current == Count)
                    {
                        break;
                    }
                }
                page++;
            }
            return books;
        }
    }
}
