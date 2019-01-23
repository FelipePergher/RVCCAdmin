using LigaCancer.Data.Models.ManyToManyModels;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LigaCancer.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Many to Many Relations
            //Patient Information and Cancer Type
            builder.Entity<PatientInformationCancerType>()
                .HasKey(bc => new { bc.PatientInformationId, bc.CancerTypeId });

            builder.Entity<PatientInformationCancerType>()
                .HasOne(bc => bc.PatientInformation)
                .WithMany(b => b.PatientInformationCancerTypes)
                .HasForeignKey(bc => bc.PatientInformationId);

            builder.Entity<PatientInformationCancerType>()
                .HasOne(bc => bc.CancerType)
                .WithMany(c => c.PatientInformationCancerTypes)
                .HasForeignKey(bc => bc.CancerTypeId);

            //Patient Information and Doctor
            builder.Entity<PatientInformationDoctor>()
               .HasKey(bc => new { bc.PatientInformationId, bc.DoctorId });

            builder.Entity<PatientInformationDoctor>()
                .HasOne(bc => bc.PatientInformation)
                .WithMany(b => b.PatientInformationDoctors)
                .HasForeignKey(bc => bc.PatientInformationId);

            builder.Entity<PatientInformationDoctor>()
                .HasOne(bc => bc.Doctor)
                .WithMany(c => c.PatientInformationDoctors)
                .HasForeignKey(bc => bc.DoctorId);

            //Patient Information and Treatment Place
            builder.Entity<PatientInformationTreatmentPlace>()
               .HasKey(bc => new { bc.PatientInformationId, bc.TreatmentPlaceId });

            builder.Entity<PatientInformationTreatmentPlace>()
                .HasOne(bc => bc.PatientInformation)
                .WithMany(b => b.PatientInformationTreatmentPlaces)
                .HasForeignKey(bc => bc.PatientInformationId);

            builder.Entity<PatientInformationTreatmentPlace>()
                .HasOne(bc => bc.TreatmentPlace)
                .WithMany(c => c.PatientInformationTreatmentPlaces)
                .HasForeignKey(bc => bc.TreatmentPlaceId);

            //Patient Information and Medicine
            builder.Entity<PatientInformationMedicine>()
              .HasKey(bc => new { bc.PatientInformationId, bc.MedicineId });

            builder.Entity<PatientInformationMedicine>()
                .HasOne(bc => bc.PatientInformation)
                .WithMany(b => b.PatientInformationMedicines)
                .HasForeignKey(bc => bc.PatientInformationId);

            builder.Entity<PatientInformationMedicine>()
                .HasOne(bc => bc.Medicine)
                .WithMany(c => c.PatientInformationMedicines)
                .HasForeignKey(bc => bc.MedicineId);

            #endregion

            #region Entity Filters

            builder.Entity<Doctor>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<ActivePatient>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Address>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<CancerType>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Doctor>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Family>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<FamilyMember>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<FileAttachment>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Medicine>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Naturality>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Patient>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<PatientInformation>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Phone>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<TreatmentPlace>().HasQueryFilter(p => !p.IsDeleted);

            #endregion

            #region Unique

            builder.Entity<CancerType>().HasIndex(p => p.Name).IsUnique();
            builder.Entity<Doctor>().HasIndex(p => p.CRM).IsUnique();
            builder.Entity<Medicine>().HasIndex(p => p.Name).IsUnique();
            builder.Entity<Patient>().HasIndex(p => p.RG).IsUnique();
            builder.Entity<Patient>().HasIndex(p => p.CPF).IsUnique();
            builder.Entity<TreatmentPlace>().HasIndex(p => p.City).IsUnique();

            #endregion
        }

        public DbSet<ActivePatient> ActivePatients { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<FileAttachment> FileAttachments { get; set; }

        public DbSet<CancerType> CancerTypes { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Family> Families { get; set; }

        public DbSet<FamilyMember> FamilyMembers { get; set; }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<Naturality> Naturalities { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<PatientInformation> PatientInformation { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<TreatmentPlace> TreatmentPlaces { get; set; }

        public DbSet<Presence> Presences { get; set; }
    }
}
