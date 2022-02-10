using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Kiwi_review.Interfaces.IServices;
using Kiwi_review.Models.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Kiwi_review.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<JwtConfig> _options;

        public TokenService(IOptions<JwtConfig> options)
        {
            _options = options;
        }

        public TnToken? CreateToken(Dictionary<string, string?> keyValuePairs)
        {
            var claims = keyValuePairs.Select(
                item => new Claim(item.Key, item.Value)).ToList();
            return CreateTokenString(claims);
        }

        private TnToken? CreateTokenString(IEnumerable<Claim> claims)
        {
            var now = DateTime.Now;
            var expires = now.Add(TimeSpan.FromMinutes(_options.Value.AccessTokenExpiresMinutes));
            var token = new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.IssuerSigningKey)),
                    SecurityAlgorithms.HmacSha256));
            return new TnToken {TokenStr = new JwtSecurityTokenHandler().WriteToken(token), Expires = expires};
        }

        public TnToken? GetEntireToken(string? tkStr)
        {
            try
            {
                var jwtArr = tkStr?.Split('.');
                if (jwtArr is null or { Length: < 3})
                {
                    return null;
                }

                var payLoad =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[1]));
                if (payLoad is null)
                {
                    return null;
                }
                var exp = double.Parse(payLoad["exp"]);
                var startTime = new DateTime(1970, 1, 1);
                var dateTime = startTime.AddSeconds(exp);
                var token = new TnToken
                {
                    TokenStr = tkStr,
                    Expires = dateTime
                };
                return token;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static long ToUnixEpochDate(DateTime date)
        {
            return (long) Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                .TotalSeconds);
        }

        public TokenType ValidateTokenState(string? encodeJwt)
        {
            try
            {
                var jwtArr = encodeJwt?.Split('.');
                if (jwtArr is null or { Length: < 3})
                {
                    return TokenType.Fail;
                }
                var payLoad =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(Base64UrlEncoder.Decode(jwtArr[1]));
                if (payLoad is null) return TokenType.Fail;
                var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(_options.Value.IssuerSigningKey));
                var success = string.Equals(jwtArr[2],
                    Base64UrlEncoder.Encode(
                        hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1])))));
                if (!success) return TokenType.Fail;
                var now = ToUnixEpochDate(DateTime.UtcNow);
                success = (now >= long.Parse(payLoad["nbf"]) && now < long.Parse(payLoad["exp"]));
                return !success ? TokenType.Expired : TokenType.Ok;
            }
            catch (Exception)
            {
                return TokenType.Fail;
            }
        }
    }
}