using AutoMapper;

namespace wdpr_project.Models;

public class Business : User
{
    public string? URL { get; set; }
    public string Name { get; set; }
    public Address Address { get; set; }

    
    public Business(){}
    public Business(string id)
    {
        Id = id;
    }
    public Business(string UserName, string password, string name, string url = "") : base(UserName, password)
    {
        URL = url;
        Name = name;
    }
}

public class BusinessDTO
{
   // public int Id { get; set; }
    public string Username { get; set; }
    public string Name { get; set; }
    public AddressDTO Adress { get; set; }
    public string URL { get; set; }
    public ICollection<Research> researches {get;set;} 

}

public class BusinessProfile : Profile
{
    public BusinessProfile()
    {
        CreateMap<Business, BusinessDTO>()
            .ForMember(dest => dest.Adress, opt => opt.MapFrom(src => src.Address));
    }
}