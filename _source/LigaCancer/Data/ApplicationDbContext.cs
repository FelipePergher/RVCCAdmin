// <copyright file="ApplicationDbContext.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Audit.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RVCC.Business.Services;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.Domain;
using RVCC.Data.Models.RelationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RVCC.Data
{
    [AuditDbContext]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly UserResolverService _userResolverService;
        private readonly DbContextHelper _helper = new DbContextHelper();
        private readonly IAuditDbContext _auditContext;

        public ApplicationDbContext(DbContextOptions options, UserResolverService userResolverService)
            : base(options)
        {
            _userResolverService = userResolverService;
            _auditContext = new DefaultAuditContext(this);
            _helper.SetConfig(_auditContext);
        }

        #region Patient

        public DbSet<Patient> Patients { get; set; }

        public DbSet<ActivePatient> ActivePatients { get; set; }

        public DbSet<PatientInformation> PatientInformation { get; set; }

        public DbSet<Naturality> Naturalities { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<FamilyMember> FamilyMembers { get; set; }

        public DbSet<FileAttachment> FileAttachments { get; set; }

        public DbSet<PatientExpenseType> PatientExpenseTypes { get; set; }

        public DbSet<PatientBenefit> PatientBenefits { get; set; }

        #endregion

        #region General Models

        public DbSet<CancerType> CancerTypes { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<TreatmentPlace> TreatmentPlaces { get; set; }

        public DbSet<Benefit> Benefits { get; set; }

        public DbSet<ExpenseType> ExpenseTypes { get; set; }

        public DbSet<ServiceType> ServiceTypes { get; set; }

        public DbSet<Stay> Stays { get; set; }

        public DbSet<Setting> Settings { get; set; }

        #endregion

        #region Shirt Sale 2020

        public DbSet<SaleShirt2020> SalesShirt2020 { get; set; }

        #endregion

        #region Audit Models

        public DbSet<AuditDoctor> AuditDoctors { get; set; }

        public DbSet<AuditTreatmentPlace> AuditTreatmentPlaces { get; set; }

        public DbSet<AuditCancerType> AuditCancerTypes { get; set; }

        public DbSet<AuditMedicine> AuditMedicines { get; set; }

        public DbSet<AuditStay> AuditStays { get; set; }

        public DbSet<AuditSetting> AuditSettings { get; set; }

        public DbSet<AuditBenefit> AuditBenefits { get; set; }

        public DbSet<AuditPatient> AuditPatients { get; set; }

        public DbSet<AuditPatientInformation> AuditPatientInformations { get; set; }

        public DbSet<AuditNaturality> AuditNaturalities { get; set; }

        public DbSet<AuditPhone> AuditPhones { get; set; }

        public DbSet<AuditAddress> AuditAddresses { get; set; }

        public DbSet<AuditFamilyMember> AuditFamilyMembers { get; set; }

        public DbSet<AuditFileAttachment> AuditFileAttachments { get; set; }

        public DbSet<AuditPatientBenefit> AuditPatientBenefits { get; set; }

        public DbSet<AuditPatientInformationTreatmentPlace> AuditPatientInformationTreatmentPlaces { get; set; }

        public DbSet<AuditPatientInformationDoctor> AuditPatientInformationDoctors { get; set; }

        public DbSet<AuditPatientInformationCancerType> AuditPatientInformationCancerTypes { get; set; }

        public DbSet<AuditPatientInformationMedicine> AuditPatientInformationMedicines { get; set; }

        public DbSet<AuditPatientInformationServiceType> AuditPatientInformationServiceTypes { get; set; }

        public DbSet<AuditExpenseType> AuditExpenseTypes { get; set; }

        public DbSet<AuditPatientExpenseType> AuditPatientExpenseTypes { get; set; }

        public DbSet<AuditServiceType> AuditServiceTypes { get; set; }

        public DbSet<AuditSaleShirt2020> AuditSaleShirt2020s { get; set; }

        #endregion

        #region Override Save Methods

        public override int SaveChanges()
        {
            IEnumerable<EntityEntry> entries = this.ChangeTracker.Entries().Where(e => e.Entity is RegisterData && (e.State == EntityState.Added || e.State == EntityState.Modified));
            var user = _userResolverService.GetUserName();

            foreach (EntityEntry entityEntry in entries)
            {
                var registerData = (RegisterData)entityEntry.Entity;
                registerData.UpdatedTime = DateTime.Now;
                registerData.UpdatedBy = user;

                if (entityEntry.State == EntityState.Added)
                {
                    registerData.RegisterTime = DateTime.Now;
                    registerData.CreatedBy = user;
                }
            }

            return _helper.SaveChanges(_auditContext, () => base.SaveChanges());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<EntityEntry> entries = this.ChangeTracker.Entries().Where(e => e.Entity is RegisterData && (e.State == EntityState.Added || e.State == EntityState.Modified));
            var user = _userResolverService.GetUserName();

            foreach (EntityEntry entityEntry in entries)
            {
                var registerData = (RegisterData)entityEntry.Entity;
                registerData.UpdatedTime = DateTime.Now;
                registerData.UpdatedBy = user;

                if (entityEntry.State == EntityState.Added)
                {
                    registerData.UpdatedTime = DateTime.Now;
                    registerData.CreatedBy = user;
                }
            }

            return await _helper.SaveChangesAsync(_auditContext, () => base.SaveChangesAsync(cancellationToken));
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Changing collationg from fields that can have accent

            // Todo not better change the database collation at once?
            builder.Entity<Patient>().Property(c => c.FirstName).UseCollation("Latin1_General_CI_AI");
            builder.Entity<Patient>().Property(c => c.Surname).UseCollation("Latin1_General_CI_AI");
            builder.Entity<Patient>().Property(c => c.RG).UseCollation("Latin1_General_CI_AI");
            builder.Entity<Patient>().Property(c => c.CPF).UseCollation("Latin1_General_CI_AI");
            builder.Entity<Medicine>().Property(c => c.Name).UseCollation("Latin1_General_CI_AI");
            builder.Entity<Doctor>().Property(c => c.Name).UseCollation("Latin1_General_CI_AI");
            builder.Entity<TreatmentPlace>().Property(c => c.City).UseCollation("Latin1_General_CI_AI");
            builder.Entity<CancerType>().Property(c => c.Name).UseCollation("Latin1_General_CI_AI");
            builder.Entity<ApplicationUser>().Property(c => c.Name).UseCollation("Latin1_General_CI_AI");
            builder.Entity<Stay>().Property(c => c.City).UseCollation("Latin1_General_CI_AI");
            #endregion

            #region Many to Many Relations

            // Patient Information and Cancer Type
            builder.Entity<PatientInformationCancerType>()
                .HasKey(bc => new { bc.PatientInformationId, bc.CancerTypeId });

            builder.Entity<PatientInformationCancerType>()
                .HasOne(bc => bc.PatientInformation)
                .WithMany(b => b.PatientInformationCancerTypes)
                .HasForeignKey(bc => bc.PatientInformationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PatientInformationCancerType>()
                .HasOne(bc => bc.CancerType)
                .WithMany(c => c.PatientInformationCancerTypes)
                .HasForeignKey(bc => bc.CancerTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Patient Information and Doctor
            builder.Entity<PatientInformationDoctor>()
               .HasKey(bc => new { bc.PatientInformationId, bc.DoctorId });

            builder.Entity<PatientInformationDoctor>()
                .HasOne(bc => bc.PatientInformation)
                .WithMany(b => b.PatientInformationDoctors)
                .HasForeignKey(bc => bc.PatientInformationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PatientInformationDoctor>()
                .HasOne(bc => bc.Doctor)
                .WithMany(c => c.PatientInformationDoctors)
                .HasForeignKey(bc => bc.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Patient Information and Treatment Place
            builder.Entity<PatientInformationTreatmentPlace>()
               .HasKey(bc => new { bc.PatientInformationId, bc.TreatmentPlaceId });

            builder.Entity<PatientInformationTreatmentPlace>()
                .HasOne(bc => bc.PatientInformation)
                .WithMany(b => b.PatientInformationTreatmentPlaces)
                .HasForeignKey(bc => bc.PatientInformationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PatientInformationTreatmentPlace>()
                .HasOne(bc => bc.TreatmentPlace)
                .WithMany(c => c.PatientInformationTreatmentPlaces)
                .HasForeignKey(bc => bc.TreatmentPlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Patient Information and Medicine
            builder.Entity<PatientInformationMedicine>()
              .HasKey(bc => new { bc.PatientInformationId, bc.MedicineId });

            builder.Entity<PatientInformationMedicine>()
                .HasOne(bc => bc.PatientInformation)
                .WithMany(b => b.PatientInformationMedicines)
                .HasForeignKey(bc => bc.PatientInformationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PatientInformationMedicine>()
                .HasOne(bc => bc.Medicine)
                .WithMany(c => c.PatientInformationMedicines)
                .HasForeignKey(bc => bc.MedicineId)
                .OnDelete(DeleteBehavior.Cascade);

            // Patient Information and Cancer Type
            builder.Entity<PatientInformationServiceType>()
                .HasKey(bc => new { bc.PatientInformationId, bc.ServiceTypeId });

            builder.Entity<PatientInformationServiceType>()
                .HasOne(bc => bc.PatientInformation)
                .WithMany(b => b.PatientInformationServiceTypes)
                .HasForeignKey(bc => bc.PatientInformationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PatientInformationServiceType>()
                .HasOne(bc => bc.ServiceType)
                .WithMany(c => c.PatientInformationServiceTypes)
                .HasForeignKey(bc => bc.ServiceTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Unique

            builder.Entity<CancerType>().HasIndex(p => p.Name).IsUnique();
            builder.Entity<Doctor>().HasIndex(p => p.CRM).IsUnique();
            builder.Entity<Medicine>().HasIndex(p => p.Name).IsUnique();
            builder.Entity<Patient>().HasIndex(p => p.RG).IsUnique();
            builder.Entity<Patient>().HasIndex(p => p.CPF).IsUnique();
            builder.Entity<TreatmentPlace>().HasIndex(p => p.City).IsUnique();
            builder.Entity<ExpenseType>().HasIndex(p => p.Name).IsUnique();
            builder.Entity<ServiceType>().HasIndex(p => p.Name).IsUnique();

            #endregion
        }
    }
}
