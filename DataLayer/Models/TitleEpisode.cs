using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
  public class TitleEpisode
  {
    public string TConst { get; set; }
    public string ParentTConst { get; set; }
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }
  }
}