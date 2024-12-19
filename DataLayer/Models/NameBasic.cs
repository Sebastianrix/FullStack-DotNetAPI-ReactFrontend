using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class NameBasic
    {

        public string ActualName { get; set; }
        [Key]
        public string NConst { get; set; }
        public string? BirthYear { get; set; }
        public string? DeathYear { get; set; }
        public double? NRating { get; set; }
    }
}
