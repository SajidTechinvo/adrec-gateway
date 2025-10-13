namespace ADREC.Common.JWT
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(string email, TimeSpan expiry);
    }
}