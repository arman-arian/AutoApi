using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace TServer.Api.Helper
{
    public static class TokenHelper
    {
        private const string Secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        public static string Encode(Dictionary<string, object> payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, Secret);
            return token;
        }

        public static IDictionary<string, object> Decode(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var payload = decoder.DecodeToObject<IDictionary<string, object>>(token, Secret, true);
                return payload;
            }
            catch (TokenExpiredException)
            {
                // Token has expired
                return null;
            }
            catch (SignatureVerificationException)
            {
                // Token has invalid signature
                return null;
            }
        }
    }
}
