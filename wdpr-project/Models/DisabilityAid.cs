using AutoMapper;

namespace wdpr_project.Models;

public class DisabilityAid
{
    public int Id { get; set; }
    public string Description { get; set; }
    public List<Expert> AidUsers { get; set; } = new List<Expert>();

    public DisabilityAid(){}
    public DisabilityAid(string description)
    {
        Description = description;
    }

    public void UpdateFields(DisabilityAidFullDTO dto)
    {
        if (dto.Id != Id)
        {
            return; //TODO: Throw error?
        }
        Description = dto.Description;
    }

    public void UpdateFields(DisabilityAidDTO dto)
    {
        Description = dto.Description;
    }
}

public class DisabilityAidDTO
{
    public string Description { get; set; }
}

public class DisabilityAidFullDTO
{
    public int Id { get; set; }
    public string Description { get; set; }
    public List<int> AidUserIds { get; set; }
}

public class DisabilityAidProfile : Profile
{
    public DisabilityAidProfile()
    {
        CreateMap<DisabilityAid, DisabilityAidDTO>();
        CreateMap<DisabilityAid, DisabilityAidFullDTO>()
            .ForMember(dest => dest.AidUserIds,
                opt => opt.MapFrom(src => src.AidUsers.Select(e => e.Id).ToList()));
    }
}