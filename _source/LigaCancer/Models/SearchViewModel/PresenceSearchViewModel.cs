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
        public string Patient { get; set; }

        [Display(Name = "Data Inicial"), 
            DataType(DataType.Date), 
            DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Data Final"),
            DataType(DataType.Date),
            DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTo { get; set; }
    }
}
