using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class SimilarMovie
    {
        public string TConst { get; set; }
        public string PrimaryTitle { get; set; }
        public int? NumVotes { get; set; }
        public int? MatchingLanguages { get; set; }
        public string? Poster { get; set; }
    }
}
