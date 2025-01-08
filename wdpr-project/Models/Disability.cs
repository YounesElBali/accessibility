using AutoMapper;

namespace wdpr_project.Models;

public class Disability
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public List<Expert> DisabledExperts { get; set; } = new List<Expert>();

    public Disability(){}
    public Disability(string type, string description)
    {
        Type = type;
        Description = description;
    }

    public void UpdateFields(DisabilityFullDTO dto)
    {
        if (dto.Id != Id)
        {
            return; //TODO: Throw error?
        }
        Type = dto.Type;
        Description = dto.Description;
    }
    
    public void UpdateFields(DisabilityDTO dto)
    {
        Type = dto.Type;
        Description = dto.Description;
    }
}

public class DisabilityDTO
{
    public string Type { get; set; }
    public string Description { get; set; }
}

public class DisabilityFullDTO
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public List<int> DisabledExpertIds { get; set; }
}

public class DisabilityProfile : Profile
{
    public DisabilityProfile()
    {
        CreateMap<Disability, DisabilityDTO>();
        CreateMap<Disability, DisabilityFullDTO>()
            .ForMember(dest => dest.DisabledExpertIds,
                opt => opt.MapFrom(src => src.DisabledExperts.Select(e => e.Id).ToList()));
    }
}