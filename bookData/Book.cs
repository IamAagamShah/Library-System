using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bookData
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string PublishDate { get; set; }
        public int PageCount { get; set; }
        public double Rating { get; set; }
        // Add other properties fetched from the API...

        public override string ToString()
        {
            return $"ID: {Id}, Title: {Title}, Author: {Author}";
        }
    }

}
