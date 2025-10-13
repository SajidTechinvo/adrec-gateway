using ADREC.Common.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ADREC.Common.JWT
{
    internal class JwtTokenGenerator(IOptions<JwtSettings> options) : IJwtTokenGenerator
    {
        #region Private Fields

        private readonly JwtSettings _jwtSettings = options.Value;

        #endregion Private Fields

        #region Methods

        public string GenerateToken(string email, TimeSpan expiry)
        {
            var credentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));

            var tokenHandler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Email,email),
                ]),
                Expires = DateTime.UtcNow.Add(expiry),
                SigningCredentials = new SigningCredentials(credentials, SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }

        #endregion Methods
    }
}