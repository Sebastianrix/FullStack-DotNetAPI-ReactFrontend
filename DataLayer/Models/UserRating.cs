using DataLayer.Models;
using System;

namespace DataLayer.Models
{
  public class UserRating
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string TConst { get; set; }
    public int Rating { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}