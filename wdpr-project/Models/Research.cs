using AutoMapper;

namespace wdpr_project.Models;

public class Research
{
    public int Id {get;set;}
    public string Title {get;set;}
    public string Description {get;set;}
    public int Reward {get;set;}
    public int Capacity {get;set;}
    public bool Status {get;set;}
    public Business business {get;set;}
    public ICollection<ResearchExpert> ResearchExperts { get; set; } 
    public ResearchCriterium ResearchCriterium { get; set; }
}


public class ResearchDTO{
    public int Id {get;set;}
    public string Title {get;set;}
    public string Description {get;set;}
    public int Reward {get;set;}
    public int Capacity {get;set;}
    public bool Status {get;set;}
    public string businessId {get;set;}
   public ICollection<string> ExpertIds { get; set; } // Represents multiple expert IDs
    public ResearchCriterium ResearchCriterium { get; set; }

}

public class ResearchProfile : Profile
{
    public ResearchProfile()
    {
        CreateMap<Research, ResearchDTO>();
    }
}