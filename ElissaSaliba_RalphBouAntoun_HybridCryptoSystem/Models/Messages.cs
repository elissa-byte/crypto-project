using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Models
{
    public class Messages
    {
        [Key]
        public int Id { get; set; }
        public string EncryptedText { get; set; }
        [DisplayName("Encrypted Key")]
        public string? EncryptedAESKey { get; set; }
        [DisplayName("Public Key")]
        public string? PublicKey { get; set; }
        [NotMapped]
        public string PrivateKey { get; set; }
        [Required]
        [Display(Name ="Enter email")]
        public string? SenderEmail { get; set; }
        [ValidateNever]
        public DateTime EncryptionDate { get; set; } = DateTime.Now;
        [NotMapped]
        [Display(Name = "Enter your message")]
        public string? PlainText { get; set; }
    }
}
