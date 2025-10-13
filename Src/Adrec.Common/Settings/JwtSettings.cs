namespace ADREC.Common.Settings
{
    public class JwtSettings()
    {
        public const string SectionName = "JwtSettings";
        public string Secret { get; set; }
        public int ExpiryMinutes { get; set; }
    }
}