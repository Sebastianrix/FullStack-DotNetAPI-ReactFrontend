namespace DataLayer.Models
{
	public class EmailSettings
	{
		public string Mail { get; set; }
		public string Password { get; set; }
		public string Host { get; set; }
		public int Port { get; set; }
		public bool EnableSsl { get; set; }
	}
}
