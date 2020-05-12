// <copyright file="Naturality.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models
{
    public class Naturality : RegisterData
    {
        [Key]
        public int NaturalityId { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}