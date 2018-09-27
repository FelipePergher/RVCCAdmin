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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure entity filters
            builder.Entity<Doctor>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<ActivePatient>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Address>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Attachments>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<CancerType>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Doctor>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Family>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<FamilyMembers>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<FileAttachment>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Medicine>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Naturality>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Patient>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<PatientInformation>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Phone>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Profession>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Residence>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<ResidenceType>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<TreatmentPlace>().HasQueryFilter(p => !p.IsDeleted);
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
