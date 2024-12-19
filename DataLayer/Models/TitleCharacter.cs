using System.ComponentModel.DataAnnotations;
//using WebApi.DTOs;

namespace DataLayer.Models
{
    public class TitleCharacter
    {

        [Key]
        public string NConst { get; set; }
        public string? TConst { get; set; }
        public string? Character { get; set; }
        public int Ordering { get; set; }
        public TitleBasic TitleBasic { get; set; }

    }
}
