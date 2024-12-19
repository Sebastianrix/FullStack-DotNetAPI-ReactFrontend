using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class CoPlayer
    {
        public string NConst { get; set; }
        public string PrimaryName { get; set; }
        public int? Frequency { get; set; }
    }
}