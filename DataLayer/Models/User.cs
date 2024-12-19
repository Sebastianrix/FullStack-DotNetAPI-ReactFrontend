using DataLayer.Models;
using System;

namespace DataLayer.Models
{
  public class User
  {
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Salt { get; set; }
    public string Role { get; set; }
    public string Name { get; set; }

        // public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
    }
}