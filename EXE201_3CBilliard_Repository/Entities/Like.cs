using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EXE201_3CBilliard_Repository.Entities
{
    [Table("Like")]
    public class Like
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public long UserId { get; set; }

        [Required]
        public long PostId { get; set; }

        /*[ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }*/

        [ForeignKey("PostId")]
        [JsonIgnore]
        public Post Post { get; set; }
    }
}
