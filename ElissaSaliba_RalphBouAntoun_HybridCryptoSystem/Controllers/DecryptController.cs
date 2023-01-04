using ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Data;
using ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Controllers
{
    public class DecryptController : Controller
    {

        private readonly ApplicationDbContext _db;

        public DecryptController(ApplicationDbContext db)
        {
            _db = db;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Decrypt(Messages obj)
        {
            if (ModelState.IsValid)
            {
                // Decrypt AES key with RSA
                var encryptedAESKey = obj.EncryptedAESKey;
                var privateKey = obj.PrivateKey;
                var encryptedText = obj.EncryptedText;
                // Extract the IV from the beginning of the encrypted message
                byte[] iv = new byte[16];
                //var encryptedMessage = Convert.FromBase64String(encryptedText);
                

                var aesKeyString = DecryptMessageWithRSA(encryptedAESKey, privateKey);
                var aesKey = Convert.FromBase64String(aesKeyString);

                // Decrypt message with AES
                var encryptedMessage = Convert.FromBase64String(encryptedText);
                Array.Copy(encryptedMessage, iv, 16);
                byte[] payload = new byte[encryptedMessage.Length - 16];
                Array.Copy(encryptedMessage, 16, payload, 0, payload.Length);
                //var iv = new byte[16];
                var plainMessage = DecryptMessageWithAES(payload, aesKey, iv);
                TempData["decryptedText"] = plainMessage;
            }
            return RedirectToAction("Index");
        }

        private static string DecryptMessageWithRSA(string encryptedMessage, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(privateKey);
                var encryptedMessageBytes = Convert.FromBase64String(encryptedMessage);
                var messageBytes = rsa.Decrypt(encryptedMessageBytes, RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(messageBytes);
            }
        }
        private static string DecryptMessageWithAES(byte[] encryptedMessage, byte[] key, byte[] iv)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = key;
                aes.IV = iv;

                // Decrypt message
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream(encryptedMessage))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
