using AutoMapper;
using EXE201_3CBilliard_Model.Models.Request;
using EXE201_3CBilliard_Model.Models.Respone;
using EXE201_3CBilliard_Model.Models.Response;
using EXE201_3CBilliard_Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXE201_3CBilliard_Model.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //Request
            CreateMap<BidaClubRequest, BidaClub>().ReverseMap();
            CreateMap<BidaTableRequest, BidaTable>().ReverseMap();
            CreateMap<NotificateRequest, Notificate>().ReverseMap();
            CreateMap<CommentRequest, Comment>().ReverseMap();
            CreateMap<PostRequest, Post>().ReverseMap();
            CreateMap<FeedbackRequset, Feedback>().ReverseMap();
            CreateMap<RoleRequest, Role>().ReverseMap();
            CreateMap<SlotRequest, Slot>().ReverseMap();
            CreateMap<BookingRequest, Booking>().ReverseMap();

            //Reponse
            CreateMap<BidaClubReponse, BidaClub>().ReverseMap();
            CreateMap<BidaTableResponse, BidaTable>().ReverseMap();
            CreateMap<NotificateResponse, Notificate>().ReverseMap();
            CreateMap<CommentResponse, Comment>().ReverseMap();
            CreateMap<PostResponse, Post>().ReverseMap();
            CreateMap<FeedbackResponse, Feedback>().ReverseMap();
            CreateMap<RoleResponse, Role>().ReverseMap();
            CreateMap<SlotResponse, Slot>().ReverseMap();
            CreateMap<BookingResponse, Booking>().ReverseMap();
        }
    }
}
