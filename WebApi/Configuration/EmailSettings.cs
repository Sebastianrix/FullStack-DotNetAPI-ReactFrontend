namespace WebApi.Configuration
{
    //public class EmailSettings
    //{
    //    public string Mail { get; set; }
    //    public string Password { get; set; }
    //    public string Host { get; set; }
    //    public int Port { get; set; }
    //    public bool EnableSsl { get; set; }
    //}

    public class EmailSettings
    {
        public string Username { get; set; }     
        public string Password { get; set; }    
        public string Host { get; set; }   
        public int Port { get; set; }            
        public bool EnableSsl { get; set; }      
        public string FromAddress { get; set; }
    }
}
