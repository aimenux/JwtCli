namespace App.Services.Security
{
    public class SecurityDetails
    {
        public IDictionary<string, string> Header { get; init; }
        public IDictionary<string, string> Payload { get; init; }
        public string Audience { get; init; }
        public string Issuer { get; init; }
        public DateTime ValidFrom { get; init; }
        public DateTime ValidTo { get; init; }
    }
}
