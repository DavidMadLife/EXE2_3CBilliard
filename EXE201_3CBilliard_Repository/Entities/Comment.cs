using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EXE201_3CBilliard_Repository.Entities
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public long PostId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        /*[ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }*/

        [ForeignKey("PostId")]
        [JsonIgnore]
        public Post Post { get; set; }
    }
}
