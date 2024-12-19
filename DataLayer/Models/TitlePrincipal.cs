using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class TitlePrincipal
    {
        public string TConst { get; set; }
        public string NConst { get; set; }
        public int? Ordering { get; set; }
        public string? Name { get; set; }
        public string? Roles { get; set; }
        public string? Title { get; set; }
        public string? Poster { get; set; }
    }

}