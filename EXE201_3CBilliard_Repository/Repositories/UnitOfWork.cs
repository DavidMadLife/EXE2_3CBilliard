



using EXE201_3CBilliard_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDBContext _context;
        private IGenericRepository<BidaClub> _bidaClubRepo;
        private IGenericRepository<BidaTable> _bidaTableRepo;
        private IGenericRepository<BidaTable_Slot> _bidaTableSlotRepo;
        private IGenericRepository<Booking> _bookingRepo;
        private IGenericRepository<Comment> _commentRepo;
        private IGenericRepository<Feedback> _feedbackRepo;
        private IGenericRepository<JwtCode> _jwtCodeRepo;
        private IGenericRepository<Notificate> _notificateRepo;
        private IGenericRepository<Post> _postRepo;
        private IGenericRepository<Role> _roleRepo;
        private IGenericRepository<Slot> _slotRepo;
        private IGenericRepository<User> _userRepo;
        private IGenericRepository<Bill> _billRepo;
        private IGenericRepository<Like> _likeRepo;


        public UnitOfWork(MyDBContext context)
        {
            _context = context;
        }

        

        public IGenericRepository<BidaClub> BidaClubRepository
        {
            get { return _bidaClubRepo ??= new GenericRepository<BidaClub>(_context); }
        }
        public IGenericRepository<BidaTable> BidaTableRepository
        {
            get { return _bidaTableRepo ??= new GenericRepository<BidaTable>(_context); }
        }

        public IGenericRepository<BidaTable_Slot> BidaTableSlotRepository
        {
            get { return _bidaTableSlotRepo ??= new GenericRepository<BidaTable_Slot>(_context); }
        }

        public IGenericRepository<Booking> BookingRepository
        {
            get { return _bookingRepo ??= new GenericRepository<Booking>(_context); }
        }

        public IGenericRepository<Comment> CommentRepository
        {
            get { return _commentRepo ??= new GenericRepository<Comment>(_context); }
        }

        public IGenericRepository<Feedback> FeedbackRepository
        {
            get { return _feedbackRepo ??= new GenericRepository<Feedback>(_context); }
        }

        public IGenericRepository<JwtCode> JwtCodeRepository
        {
            get { return _jwtCodeRepo ??= new GenericRepository<JwtCode>(_context); }
        }

        public IGenericRepository<Notificate> NotificateRepository
        {
            get { return _notificateRepo ??= new GenericRepository<Notificate>(_context); }
        }

        public IGenericRepository<Post> PostRepository
        {
            get { return _postRepo ??= new GenericRepository<Post>(_context); }
        }

        public IGenericRepository<Role> RoleRepository
        {
            get { return _roleRepo ??= new GenericRepository<Role>(_context); }
        }

        public IGenericRepository<Slot> SlotRepository
        {
            get { return _slotRepo ??= new GenericRepository<Slot>(_context); }
        }

        public IGenericRepository<User> UserRepository
        {
            get { return _userRepo ??= new GenericRepository<User>(_context); }
        }
        public IGenericRepository<Bill> BillRepository
        {
            get { return _billRepo ??= new GenericRepository<Bill>(_context); }
        }
        public IGenericRepository<Like> LikeRepository
        {
            get { return _likeRepo ??= new GenericRepository<Like>(_context); }
        }
        public int Save()
        {
            return _context.SaveChanges();
        }

       
    }
}
