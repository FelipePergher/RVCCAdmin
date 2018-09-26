using System;
using System.Collections.Generic;
using System.Text;
using LigaCancer.Data.Models.Patient;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LigaCancer.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ActivePatient> ActivePatients { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Attachments> Attachments { get; set; }

        public DbSet<CancerType> CancerTypes { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Family> Families { get; set; }

        public DbSet<FamilyMembers> FamilyMembers { get; set; }

        public DbSet<FileAttachment> FileAttachments { get; set; }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<Naturality> Naturalities { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<PatientInformation> PatientInformation { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<Profession> Professions { get; set; }

        public DbSet<Residence> Residences { get; set; }

        public DbSet<ResidenceType> ResidenceTypes { get; set; }

        public DbSet<TreatmentPlace> TreatmentPlaces { get; set; }
    }
}
