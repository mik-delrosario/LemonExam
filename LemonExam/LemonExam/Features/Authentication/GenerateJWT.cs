using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using LemonExam.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LemonExam.Model.Master;

namespace LemonExam.Features.Authentication
{
    public class GenerateJWT
    {
        #region Private Fields

        private readonly JwtIssuerOptions _options;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("SSkey4#_OnceAdeveloperalwaysAdevelopersotheysayanyway!"));


        #endregion

        #region Constructor

        public GenerateJWT(IOptions<JwtIssuerOptions> options)
        {
            _options = options.Value;
            throwOnInvalidOptions(_options);
            _serializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
        }
        #endregion
        public string CreateToken(UserAccount user)
        {
            var now = DateTime.Now;
            string responseStr = string.Empty;

            try
            {
                var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.SiteName.ToString()),
                new Claim(JwtRegisteredClaimNames.Nonce, user.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, user.SitePassword.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, _options.IssuedAt.ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Typ, user.ID.ToString())
            };
                // Create the JWT and write it to a string
                var jwt = new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    notBefore: _options.NotBefore,
                    expires: now.Add(_options.ValidFor),
                    signingCredentials: _options.SigningCredentials);
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var response = new
                {
                    bearer = encodedJwt,
                    expires = (int)_options.ValidFor.TotalSeconds
                };
                responseStr = JsonConvert.SerializeObject(response, _serializerSettings);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return responseStr;
        }


        private void throwOnInvalidOptions(JwtIssuerOptions options)
        {
            if (options.ID == 0)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.ID));
            }

            if (string.IsNullOrEmpty(options.Issuer))
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.Issuer));
            }

            if (string.IsNullOrEmpty(options.Audience))
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.Audience));
            }

            if (options.ValidFor == TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.Expiration));
            }

            //if (options.IdentityResolver == null) {
            //    throw new ArgumentNullException(nameof(JwtIssuerOptions.IdentityResolver));
            //}

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.NonceGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.NonceGenerator));
            }
        }
        private long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
    }
}
