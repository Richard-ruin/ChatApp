using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Sender")]
        public int SenderId { get; set; }

        public virtual User Sender { get; set; }

        // For direct messages. Null for group messages
        [ForeignKey("Recipient")]
        public int? RecipientId { get; set; }

        public virtual User Recipient { get; set; }

        // For group messages. Null for direct messages
        [ForeignKey("ChatRoom")]
        public int? ChatRoomId { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }

        [Required]
        public string Content { get; set; }

        // Store encrypted content
        [Required]
        public string EncryptedContent { get; set; }

        public DateTime SentAt { get; set; }

        public bool IsRead { get; set; }
    }
}