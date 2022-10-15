namespace App.Services.Security
{
    public class SecurityDetails
    {
        public IDictionary<string, string> Header { get; set; }
        public IDictionary<string, string> Payload { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
