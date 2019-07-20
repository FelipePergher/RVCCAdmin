﻿// <auto-generated />
using System;
using LigaCancer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LigaCancer.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LigaCancer.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.ActivePatient", b =>
                {
                    b.Property<int>("ActivePatientId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<bool>("Death");

                    b.Property<DateTime>("DeathDate");

                    b.Property<bool>("Discharge");

                    b.Property<DateTime>("DischargeDate");

                    b.Property<int>("PatientId");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("ActivePatientId");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.ToTable("ActivePatients");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City");

                    b.Property<string>("Complement");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("HouseNumber");

                    b.Property<double>("MonthlyAmmountResidence");

                    b.Property<string>("Neighborhood");

                    b.Property<string>("ObservationAddress");

                    b.Property<int>("PatientId");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<int?>("ResidenceType");

                    b.Property<string>("Street");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("AddressId");

                    b.HasIndex("PatientId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.CancerType", b =>
                {
                    b.Property<int>("CancerTypeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Name");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("CancerTypeId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("CancerTypes");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.Doctor", b =>
                {
                    b.Property<int>("DoctorId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CRM");

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Name");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("DoctorId");

                    b.HasIndex("CRM")
                        .IsUnique()
                        .HasFilter("[CRM] IS NOT NULL");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.FamilyMember", b =>
                {
                    b.Property<int>("FamilyMemberId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("Kinship");

                    b.Property<double>("MonthlyIncome");

                    b.Property<string>("Name");

                    b.Property<int>("PatientId");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<int>("Sex");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("FamilyMemberId");

                    b.HasIndex("PatientId");

                    b.ToTable("FamilyMembers");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.FileAttachment", b =>
                {
                    b.Property<int>("FileAttachmentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<string>("FileExtension");

                    b.Property<string>("FileName");

                    b.Property<string>("FilePath");

                    b.Property<double>("FileSize");

                    b.Property<int>("PatientId");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("FileAttachmentId");

                    b.HasIndex("PatientId");

                    b.ToTable("FileAttachments");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.Medicine", b =>
                {
                    b.Property<int>("MedicineId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Name");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("MedicineId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.Naturality", b =>
                {
                    b.Property<int>("NaturalityId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("CreatedBy");

                    b.Property<int>("PatientId");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<string>("State");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("NaturalityId");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.ToTable("Naturalities");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.Patient", b =>
                {
                    b.Property<int>("PatientId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CPF");

                    b.Property<int?>("CivilState");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<bool>("FamiliarityGroup");

                    b.Property<string>("FirstName");

                    b.Property<double>("MonthlyIncome");

                    b.Property<string>("Profession");

                    b.Property<string>("RG");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<int>("Sex");

                    b.Property<string>("Surname");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("PatientId");

                    b.HasIndex("CPF")
                        .IsUnique()
                        .HasFilter("[CPF] IS NOT NULL");

                    b.HasIndex("RG")
                        .IsUnique()
                        .HasFilter("[RG] IS NOT NULL");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.PatientInformation", b =>
                {
                    b.Property<int>("PatientInformationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<int>("PatientId");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<DateTime>("TreatmentbeginDate");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("PatientInformationId");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.ToTable("PatientInformation");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.Phone", b =>
                {
                    b.Property<int>("PhoneId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Number");

                    b.Property<string>("ObservationNote");

                    b.Property<int>("PatientId");

                    b.Property<int?>("PhoneType");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("PhoneId");

                    b.HasIndex("PatientId");

                    b.ToTable("Phones");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.Presence", b =>
                {
                    b.Property<int>("PresenceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<string>("Name");

                    b.Property<int>("PatientId");

                    b.Property<DateTime>("PresenceDateTime");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("PresenceId");

                    b.ToTable("Presences");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.TreatmentPlace", b =>
                {
                    b.Property<int>("TreatmentPlaceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedTime");

                    b.HasKey("TreatmentPlaceId");

                    b.HasIndex("City")
                        .IsUnique()
                        .HasFilter("[City] IS NOT NULL");

                    b.ToTable("TreatmentPlaces");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.RelationModels.PatientInformationCancerType", b =>
                {
                    b.Property<int>("PatientInformationId");

                    b.Property<int>("CancerTypeId");

                    b.HasKey("PatientInformationId", "CancerTypeId");

                    b.HasIndex("CancerTypeId");

                    b.ToTable("PatientInformationCancerType");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.RelationModels.PatientInformationDoctor", b =>
                {
                    b.Property<int>("PatientInformationId");

                    b.Property<int>("DoctorId");

                    b.HasKey("PatientInformationId", "DoctorId");

                    b.HasIndex("DoctorId");

                    b.ToTable("PatientInformationDoctor");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.RelationModels.PatientInformationMedicine", b =>
                {
                    b.Property<int>("PatientInformationId");

                    b.Property<int>("MedicineId");

                    b.HasKey("PatientInformationId", "MedicineId");

                    b.HasIndex("MedicineId");

                    b.ToTable("PatientInformationMedicine");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.RelationModels.PatientInformationTreatmentPlace", b =>
                {
                    b.Property<int>("PatientInformationId");

                    b.Property<int>("TreatmentPlaceId");

                    b.HasKey("PatientInformationId", "TreatmentPlaceId");

                    b.HasIndex("TreatmentPlaceId");

                    b.ToTable("PatientInformationTreatmentPlace");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.ActivePatient", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.Patient", "Patient")
                        .WithOne("ActivePatient")
                        .HasForeignKey("LigaCancer.Data.Models.PatientModels.ActivePatient", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.Address", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.Patient", "Patient")
                        .WithMany("Addresses")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.FamilyMember", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.Patient", "Patient")
                        .WithMany("FamilyMembers")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.FileAttachment", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.Patient", "Patient")
                        .WithMany("FileAttachments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.Naturality", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.Patient", "Patient")
                        .WithOne("Naturality")
                        .HasForeignKey("LigaCancer.Data.Models.PatientModels.Naturality", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.PatientInformation", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.Patient", "Patient")
                        .WithOne("PatientInformation")
                        .HasForeignKey("LigaCancer.Data.Models.PatientModels.PatientInformation", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LigaCancer.Data.Models.PatientModels.Phone", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.Patient", "Patient")
                        .WithMany("Phones")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LigaCancer.Data.Models.RelationModels.PatientInformationCancerType", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.CancerType", "CancerType")
                        .WithMany("PatientInformationCancerTypes")
                        .HasForeignKey("CancerTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LigaCancer.Data.Models.PatientModels.PatientInformation", "PatientInformation")
                        .WithMany("PatientInformationCancerTypes")
                        .HasForeignKey("PatientInformationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LigaCancer.Data.Models.RelationModels.PatientInformationDoctor", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.Doctor", "Doctor")
                        .WithMany("PatientInformationDoctors")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LigaCancer.Data.Models.PatientModels.PatientInformation", "PatientInformation")
                        .WithMany("PatientInformationDoctors")
                        .HasForeignKey("PatientInformationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LigaCancer.Data.Models.RelationModels.PatientInformationMedicine", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.Medicine", "Medicine")
                        .WithMany("PatientInformationMedicines")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LigaCancer.Data.Models.PatientModels.PatientInformation", "PatientInformation")
                        .WithMany("PatientInformationMedicines")
                        .HasForeignKey("PatientInformationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("LigaCancer.Data.Models.RelationModels.PatientInformationTreatmentPlace", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.PatientModels.PatientInformation", "PatientInformation")
                        .WithMany("PatientInformationTreatmentPlaces")
                        .HasForeignKey("PatientInformationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LigaCancer.Data.Models.PatientModels.TreatmentPlace", "TreatmentPlace")
                        .WithMany("PatientInformationTreatmentPlaces")
                        .HasForeignKey("TreatmentPlaceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("LigaCancer.Data.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("LigaCancer.Data.Models.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
