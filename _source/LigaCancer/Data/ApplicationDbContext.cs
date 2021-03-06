// <copyright file="ApplicationDbContext.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RVCC.Business.Services;
using RVCC.Data.Models;
using RVCC.Data.Models.RelationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RVCC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly UserResolverService _userResolverService;

        public ApplicationDbContext(DbContextOptions options, UserResolverService userResolverService)
            : base(options)
        {
            this._userResolverService = userResolverService;
        }

        public DbSet<ActivePatient> ActivePatients { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<FileAttachment> FileAttachments { get; set; }

        public DbSet<CancerType> CancerTypes { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<FamilyMember> FamilyMembers { get; set; }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<Naturality> Naturalities { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<PatientInformation> PatientInformation { get; set; }

        public DbSet<Phone> Phones { get; set; }

        public DbSet<TreatmentPlace> TreatmentPlaces { get; set; }

        public DbSet<Presence> Presences { get; set; }

        public DbSet<Benefit> Benefits { get; set; }

        public DbSet<PatientBenefit> PatientBenefits { get; set; }

        public DbSet<Stay> Stays { get; set; }

        public DbSet<AdminInfo> AdminInfos { get; set; }

        #region Shirt Sale 2020

        public DbSet<SaleShirt2020> SalesShirt2020 { get; set; }

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

            return base.SaveChanges();
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

            return await base.SaveChangesAsync(cancellationToken);
        }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
    }
}
