using ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Data;
using ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;

namespace ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Controllers
{
    public class EncryptController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EncryptController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
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
        public IActionResult Encrypt(Messages obj)
        {
            ModelState.Remove("EncryptedText");
            ModelState.Remove("PrivateKey");
            ModelState.Remove("IV");
            if (ModelState.IsValid)
            {
                // Generate RSA key pair
                string publicKey, privateKey;
                GenerateRSAKeyPair(out publicKey, out privateKey);

                // Generate AES key
                var aesKey = GenerateAESKey();

                // Encrypt message with AES
                var message = obj.PlainText;
                var iv = new byte[16];
                new Random().NextBytes(iv);
                var encryptedMessage = EncryptMessageWithAES(message, aesKey, iv);

                // Encrypt AES key with RSA
                var encryptedAESKey = EncryptMessageWithRSA(Convert.ToBase64String(aesKey), publicKey);

                // Display encrypted message and encrypted AES key in label
                string encryptedMessageEl = Convert.ToBase64String(encryptedMessage);
                string encryptedAESKEY = encryptedAESKey;
                obj.PublicKey = publicKey;
                obj.EncryptedAESKey = encryptedAESKEY;
                obj.EncryptedText = encryptedMessageEl;
                string hexIVString = BitConverter.ToString(iv).Replace("-", "");

                string filePath = "C:\\Users\\HP\\Downloads\\encrypted-" + DateTime.Now.ToString("yyyy-dd-MM-@hh-mm")+".txt";
                // Save the encrypted key, encrypted text, and private key to a file
                using (var writer = new StreamWriter(filePath))
                {
                    
                    // Write the encrypted text to the file
                    writer.WriteLine("\nThis is the encypted Message: \n" + encryptedMessageEl);

                    // Write the encrypted key to the file
                    writer.WriteLine("\nThis is the encypted Key: \n" + encryptedAESKEY);

                    // Write the private key to the file
                    writer.WriteLine("\nThis is the private Key: \n" + privateKey);

                    //Write the IV to the file
                    writer.WriteLine("\nThis is the IV:\n"+ Encoding.ASCII.GetString(iv));

                }
                TempData["encryptedText"] = encryptedMessageEl;
                TempData["encryptedAESKey"] = encryptedAESKEY;
                TempData["privateKey"] = privateKey;
                TempData["filePath"] = filePath;
                DownloadFile(filePath);
                ViewData["Result"] = encryptedAESKEY;
                _db.Messages.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "New Message encrypted successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult DownloadFile(string fileName)
        {
            // Set the file path
            string filePath = fileName;

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                // If the file does not exist, return a 404 error
                return NotFound();
            }

            // Set the file content type
            string contentType = "text/plain";

            // Read the file into a byte array
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            fileName = fileName.Remove(0, fileName.LastIndexOf("\\"));
            // Return the file to the user
            return File(fileBytes, contentType, fileName);
        }
        private static void GenerateRSAKeyPair(out string publicKey, out string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                publicKey = rsa.ToXmlString(false);
                privateKey = rsa.ToXmlString(true);
            }
        }
        private static string EncryptMessageWithRSA(string message, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.FromXmlString(publicKey);
                var messageBytes = Encoding.UTF8.GetBytes(message);
                var encryptedMessageBytes = rsa.Encrypt(messageBytes, RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedMessageBytes);
            }
        }
        private static byte[] GenerateAESKey()
        {
            using (var aes = Aes.Create())
            {
                return aes.Key;
            }
        }
        private static byte[] EncryptMessageWithAES(string message, byte[] key, byte[] iv)
        {
            // Prepend the IV to the encrypted message
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] encryptedMessage = new byte[iv.Length + messageBytes.Length];
            Array.Copy(iv, encryptedMessage, iv.Length);
            Array.Copy(messageBytes, 0, encryptedMessage, iv.Length, messageBytes.Length);

            // Encrypt the message
            using (var aes = Aes.Create())
            using (var encryptor = aes.CreateEncryptor(key, iv))
            using (var memoryStream = new MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                cryptoStream.Write(encryptedMessage, 0, encryptedMessage.Length);
                cryptoStream.FlushFinalBlock();
                return memoryStream.ToArray();
            }
        }
    }
}