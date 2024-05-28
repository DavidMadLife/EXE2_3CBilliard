

using EXE201_3CBilliard_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Repository
{
    public interface IUnitOfWork
    {
        public IGenericRepository<BidaClub> BidaClubRepository { get; }
        public IGenericRepository<BidaTable> BidaTableRepository { get; }
        public IGenericRepository<BidaTable_Slot> BidaTableSlotRepository { get; }
        public IGenericRepository<Booking> BookingRepository { get; }
        public IGenericRepository<Comment> CommentRepository { get; }
        public IGenericRepository<Feedback> FeedbackRepository { get; }
        public IGenericRepository<JwtCode> JwtCodeRepository { get; }
        public IGenericRepository<Notificate> NotificateRepository { get; }
        public IGenericRepository<Post> PostRepository { get; }
        public IGenericRepository<Role> RoleRepository { get; }
        public IGenericRepository<Slot> SlotRepository { get; }
        public IGenericRepository<User> UserRepository { get; }
        public IGenericRepository<Bill> BillRepository { get; }
        public int Save();
    }
}
