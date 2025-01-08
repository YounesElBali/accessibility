using AutoMapper;
using wdpr_project.DTOs;
using wdpr_project.Models;

namespace wdpr_project.DTOs
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public ICollection<UserDTO> Users { get; set; }
        public ICollection<MessageDTO> Messages { get; set; }
    }

    public class MessageDTO
    {
        public int Id { get; set; }
        public string message { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }

        public int ChatId { get; set; }
        public DateTime Date { get; set; }
    }

    public class UserDTO
    {
        public string UserId { get; set; }
        public string Username { get; set; } 
    }


    public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Chat, ChatDTO>();
        CreateMap<Message, MessageDTO>();
        CreateMap<User, UserDTO>();
    }
}
}



