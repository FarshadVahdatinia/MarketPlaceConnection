using System.Security.Cryptography;
using System.Text;

namespace GPOS.MarketPlaceApi.Helper
{
    public static class HmacAuthorize
    {
        public static bool HmacAuthorization(string Header)
        {
            try
            {
                var timeStamp = Header.Split(':')[1];
                Encoding encoding = Encoding.ASCII;
                var secret = "********";
                var secretByte = encoding.GetBytes(secret);
                var message = "POST" + timeStamp;
                byte[] messageBytes = encoding.GetBytes(message);
                using (HMACSHA256 hmac = new HMACSHA256(secretByte))
                {
                    byte[] hashmessage = hmac.ComputeHash(messageBytes);
                    var hashedHexMessage = Convert.ToHexString(hashmessage).ToLower();
                    var hashedByteMessage = encoding.GetBytes(hashedHexMessage);
                    var key = Convert.ToBase64String(hashedByteMessage);
                    var model = $"***************";
                    if (model == Header)
                    {
                        return true;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
