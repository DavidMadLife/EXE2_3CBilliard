using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace EXE201_3CBilliard_Repository.Entities
{
    public class MyDBContext : DbContext
    {
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {

        }
        public DbSet<BidaClub> BidaClubs { get; set; }
        public DbSet<BidaTable> BidaTables { get; set; }
        public DbSet<BidaTable_Slot> BidaTable_Slots { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<JwtCode> JwtCodes { get; set; }
        public DbSet<Notificate> Notifications { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<Bill> Bills { get; set; }



    }
}
