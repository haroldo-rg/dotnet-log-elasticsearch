namespace LogElastic.Configuration
{
    public class ElasticConfiguration
    {
        public string Uri { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public ElasticConfiguration(string uri, string userName, string password)
        {
            this.Uri = uri;
            this.UserName = userName;
            this.Password = password;
        }
    }
}