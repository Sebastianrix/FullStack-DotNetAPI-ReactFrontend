using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
  public class TitleRating
  {
    public string TConst { get; set; }
    public double AverageRating { get; set; }
    public int NumVotes { get; set; }
  }
}