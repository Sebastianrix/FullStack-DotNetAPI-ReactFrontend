using DataLayer.Models;
using System;

namespace DataLayer.Models
{
  public class SearchHistory
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string SearchQuery { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}