using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography; 
namespace Encrypt_Decrypt
{
    class Program
    {
        public static RSAParameters publickey;
        public static RSAParameters privatekey;
        static string CONTAINER_NAME = "MyContainerName";

        public enum KeySizes
        {
            SIZE_512=512,
            SIZE_1024=1024,
            SIZE_2048=2048,
            SIZE_952=952,
            SIZE_1369=1369

        }
        static void Main(string[] args)
        {
            
            string Message = "Oliver Kipkemei is the great";
            generateKeys();
            byte[] encrypted = Encrypt(Encoding.UTF8.GetBytes(Message));
            byte[] decrypted = Decrypt(encrypted);
            Console.WriteLine("Original Message \n" + Message +"\n");
            Console.WriteLine("Encrypted message \n" +  BitConverter.ToString(encrypted).Replace("-","") + "\n \n");
            Console.WriteLine("Decrypted Message \n" + Encoding.UTF8.GetString(decrypted));
            Console.ReadLine();
        }
        static void generateKeys()
        {
            
            using (var rsa = new RSACryptoServiceProvider(2048)) // using instance of RSA provider/ specifying the key length
            {
                rsa.PersistKeyInCsp = false;// dont store key in container
                publickey = rsa.ExportParameters(false);// generate the public key
                privatekey = rsa.ExportParameters(true); // generate the private key
            }
        }
        static byte[] Encrypt(byte[] input)
        {
            byte[] encrypted;
            using(var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false; // don't store the keys on container
                rsa.ImportParameters(publickey); // import the created key
                encrypted = rsa.Encrypt(input,true); //encrypt the string with rsa pkcs#1v 1.5 padding
            }
            return encrypted;
        }
        static byte[] Decrypt(byte[] input)
        {
            byte[] decrypted;
            using(var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(privatekey);
                decrypted = rsa.Decrypt(input, true); //decrypt the text with private key

            }
            return decrypted;
        }
    }
}
