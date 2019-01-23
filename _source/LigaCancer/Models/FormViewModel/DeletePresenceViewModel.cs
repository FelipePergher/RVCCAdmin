using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormViewModel
{
    public class DeletePresenceViewModel
    {
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}
