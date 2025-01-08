using wdpr_project.Models;

public class ResearchExpert
    {
        public int Id {get;set;}
        public int ResearchId { get; set; }
        public Research Research { get; set; }
        
        public string ExpertId { get; set; }
        public Expert Expert { get; set; }
    }