using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Models
{
    public class ChatRoom
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }

        public virtual User Creator { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<ChatRoomMember> Members { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }

    public class ChatRoomMember
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("ChatRoom")]
        public int ChatRoomId { get; set; }

        public virtual ChatRoom ChatRoom { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}