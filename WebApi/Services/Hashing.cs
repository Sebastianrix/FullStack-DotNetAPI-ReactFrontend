using System.Security.Cryptography;
using System.Text;

namespace WebApi.Services
{
    public class Hashing
    {
       
            protected const int saltBitesize = 64;
            protected const byte saltBytesize = saltBitesize / 8;
            protected const int hashBitsize = 256;
            protected const int hashBytesize = hashBitsize / 8;

            private HashAlgorithm sha256 = SHA256.Create();
            protected RandomNumberGenerator rand = RandomNumberGenerator.Create();

            
            //called from Authenticator.register()
            //both salt and hashed password are returned for storage in the database

        public (string hash, string salt) Hash(string password)
            {
                byte[] salt = new byte[saltBytesize];
                rand.GetBytes(salt);
                string saltString = Convert.ToHexString(salt);
                string hash = HashSHA256(password, saltString);
                return (hash, saltString);
            }
            //Varify is called from Authenticator.Login()
            public bool Verify(string LoginPassword, string hashedRegisteredPassword, string saltString)
            {
                string hashedLoginPassword = HashSHA256(LoginPassword, saltString);
                if (hashedRegisteredPassword == hashedLoginPassword) return true;
                else return false;
            }

            // The hashing
            private string HashSHA256(string password, string saltString)
            {
                byte[] hashInput = Encoding.UTF8.GetBytes(saltString + password);
                byte[] hashOutput = sha256.ComputeHash(hashInput);
                return Convert.ToHexString(hashOutput);
            }

        

    }
}
