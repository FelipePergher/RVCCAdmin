﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class DoctorViewModel
    {
        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Name { get; set; }

        public string CRM { get; set; }
    }
}