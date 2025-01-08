using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using wdpr_project.Data;

namespace wdpr_project.Models;

public class Expert : User
{
    public bool ContactByPhone { get; set; }
    public bool ContactByThirdParty { get; set; }
    public List<Disability> Disabilities { get; set; } = new List<Disability>();
    public List<DisabilityAid> Aids { get; set; } = new List<DisabilityAid>();
    public PersonalData PersonalData { get; set; } = new PersonalData();
    public PersonalData? Caretaker { get; set; }
    public ResearchExpert? ResearchExperts { get; set; } = new ResearchExpert();// Represents many-to-many relationship
    
    public Expert(){}

    public Expert(string id)
    {
        Id = id;
        PersonalData = null!; // prevent the creation of new objects that EF will attempt to add to the DB
    }

    public async Task<ActionResult?> UpdateFields(ExpertFullDTO dto, ApplicationDbContext dbContext) //TODO: Unit tests
    {
        Password = dto.Password;
        ContactByPhone = dto.ContactByPhone;
        ContactByThirdParty = dto.ContactByThirdParty;
        PersonalData.UpdateFields(dto.PersonalData);
        if (Caretaker is not null){
            if (dto.Caretaker is null)
            {
                Caretaker = null;
                if (Caretaker is not null)
                { 
                    dbContext.PersonalData.Remove(Caretaker!);
                }
            }
            else
            {
                Caretaker.UpdateFields(dto.Caretaker);
            }
        }
        else
        {
            Caretaker = dto.Caretaker;
        }

        return await SyncExpert(dto, dbContext);
    }

    public async Task<ActionResult?> SyncExpert(ExpertFullDTO dto, ApplicationDbContext dbContext) //TODO: Unit tests
    {
        string? errorMessage = null;
        
        Disabilities.Clear();
        Aids.Clear();

        if (dto.DisabilityIds.Count > 0)
        {
            List<int> invalidIds = new List<int>();
            List<Disability> availableDisabilities = await dbContext.Disabilities.ToListAsync();
            foreach (int id in dto.DisabilityIds)
            {
                Disability? disability = availableDisabilities.FirstOrDefault(d => d.Id == id);

                if (disability is null)
                {
                    invalidIds.Add(id);
                    continue;
                }
                
                Disabilities.Add(disability);
            }

            if (invalidIds.Count > 0)
            {
                string errorString = $"Unable to find disabilities with the following id(s): [{string.Join<int>(",",invalidIds)}]";
                if (errorMessage is null)
                {
                    errorMessage = errorString;
                }
                else
                {
                    errorMessage += "\n" + errorString;
                }
            }
        }

        if (dto.DisabilityAidIds.Count > 0)
        {
            List<int> invalidIds = new List<int>();
            List<DisabilityAid> availableAids = await dbContext.DisabilityAids.ToListAsync();
            foreach (int id in dto.DisabilityAidIds)
            {
                DisabilityAid? disabilityAid = availableAids.FirstOrDefault(d => d.Id == id);

                if (disabilityAid is null)
                {
                    invalidIds.Add(id);
                    continue;
                }
                
                Aids.Add(disabilityAid);
            }

            if (invalidIds.Count > 0)
            {
                string errorString = $"Unable to find disabilities aids with the following id(s): [{string.Join<int>(",",invalidIds)}]";
                if (errorMessage is null)
                {
                    errorMessage = errorString;
                }
                else
                {
                    errorMessage += "\n" + errorString;
                }
            }
        }

        if (errorMessage is not null)
        {
            return new ConflictObjectResult(errorMessage);
        }

        return null;
    }
}

public class ExpertBaseDTO
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Firstname { get; set; }
    public string? Middlenames { get; set; }
    public string Lastname { get; set; }
}

public class ExpertDetailDTO
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Firstname { get; set; }
    public string? Middlenames { get; set; }
    public string Lastname { get; set; }
    public bool Contactbyphone { get; set; }
    public bool Contactbythirdparty { get; set; }
    public List<DisabilityDTO> Disabilities { get; set; }
    public List<DisabilityAidDTO> Aids { get; set; }
    public string? Emailaddress { get; set; }
    public string? Phonenumber { get; set; }
    public PersonalDataNameDTO? Caretaker { get; set; }
}

public class ExpertFullDTO
{
   public string Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool ContactByPhone { get; set; }
    public bool ContactByThirdParty { get; set; }
    public List<int> DisabilityIds { get; set; }
    public List<int> DisabilityAidIds { get; set; }
    public PersonalData PersonalData { get; set; }
    public PersonalData? Caretaker { get; set; }
}

public class ExpertProfile : Profile
{
    public ExpertProfile()
    {
        CreateMap<Expert, ExpertBaseDTO>()
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.PersonalData.Firstname))
            .ForMember(dest => dest.Middlenames, opt => opt.MapFrom(src => src.PersonalData.Middlenames))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.PersonalData.Lastname));

        CreateMap<Expert, ExpertDetailDTO>()
            .ForMember(dest => dest.Firstname, opt => opt.MapFrom(src => src.PersonalData.Firstname))
            .ForMember(dest => dest.Middlenames, opt => opt.MapFrom(src => src.PersonalData.Middlenames))
            .ForMember(dest => dest.Lastname, opt => opt.MapFrom(src => src.PersonalData.Lastname))
            .ForMember(dest => dest.Contactbyphone, opt => opt.MapFrom(src => src.ContactByPhone))
            .ForMember(dest => dest.Contactbythirdparty, opt => opt.MapFrom(src => src.ContactByThirdParty))
            .ForMember(dest => dest.Disabilities, opt => opt.MapFrom(src => src.Disabilities))
            .ForMember(dest => dest.Aids, opt => opt.MapFrom(src => src.Aids))
            .ForMember(dest => dest.Emailaddress,
                opt => opt.MapFrom(src => src.ContactByThirdParty ? src.PersonalData.Emailaddress : null))
            .ForMember(dest => dest.Phonenumber,
                opt => opt.MapFrom(src =>
                    src.ContactByThirdParty && src.ContactByPhone ? src.PersonalData.Phonenumber : null))
            .ForMember(dest => dest.Caretaker, opt => opt.MapFrom(src => src.Caretaker));

        CreateMap<Expert, ExpertFullDTO>()
            .ForMember(dest => dest.DisabilityIds, opt => opt.MapFrom(src => src.Disabilities.Select(d => d.Id).ToList()))
            .ForMember(dest => dest.DisabilityIds, opt => opt.MapFrom(src => src.Aids.Select(d => d.Id).ToList()));
        CreateMap<ExpertFullDTO, Expert>()
            .ForMember(dest => dest.Aids, opt => opt.Ignore())
            .ForMember(dest => dest.Disabilities, opt => opt.Ignore());
    }
}