using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.SearchViewModel
{
    public class PresenceSearchViewModel
    {
        public PresenceSearchViewModel()
        {
            DateFrom = DateTime.Now.AddDays(-7);
            DateTo = DateTime.Now;
        }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Sobrenome")]
        public string Surname { get; set; }

        [Display(Name = "Data Inicial"), 
            DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Data Final"),
            DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { get; set; }
    }
}
