using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace LvivDotNet.Common
{
    /// <summary>
    /// Security helpers.
    /// </summary>
    public static class SecurityHelpers
    {
        /// <summary>
        /// Password hasing.
        /// </summary>
        /// <param name="password"> Password. </param>
        /// <param name="salt"> Hashing salt. </param>
        /// <returns> Hashed, using salt, password. </returns>
        public static string GetPasswordHash(string password, byte[] salt)
            => Convert.ToBase64String(
                        KeyDerivation.Pbkdf2(
                            password: password,
                            salt: salt,
                            prf: KeyDerivationPrf.HMACSHA256,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8));

        /// <summary>
        /// Random bytes generator.
        /// </summary>
        /// <param name="length"> Length or random bytes array. </param>
        /// <returns> Returns array of random bytes. </returns>
        public static byte[] GetRandomBytes(int length)
        {
            using (var randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var salt = new byte[length];

                randomNumberGenerator.GetBytes(salt);

                return salt;
            }
        }

        /// <summary>
        /// Jwt token generator.
        /// </summary>
        /// <param name="userId"> User Id. </param>
        /// <param name="secret"> Secret. </param>
        /// <param name="role"> User role. </param>
        /// <returns> Returns Jwt token base on user id and role. </returns>
        public static string GenerateJwtToken(int userId, string secret, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", userId.ToString(CultureInfo.CurrentCulture.NumberFormat)),
                    new Claim(ClaimTypes.Role, role),
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor) as JwtSecurityToken;

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Jwt token decoder.
        /// </summary>
        /// <param name="token"> Jwt token. </param>
        /// <returns> Returns decoded Jwt token. </returns>
        public static JwtSecurityToken DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadToken(token) as JwtSecurityToken;
        }
    }
}