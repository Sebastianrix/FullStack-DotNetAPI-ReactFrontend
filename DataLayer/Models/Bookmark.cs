using DataLayer.Models;
using System;

namespace DataLayer.Models
{
  public class Bookmark
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? TConst { get; set; }
    public string? NConst { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}