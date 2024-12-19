using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class SearchTitle
    {
        public string TConst { get; set; }
        public string PrimaryTitle { get; set; }
        public string? Poster { get; set; }
        public int? StartYear { get; set; }
        public string? Genre { get; set; }
        public decimal? Rating { get; set; }
    }
}
