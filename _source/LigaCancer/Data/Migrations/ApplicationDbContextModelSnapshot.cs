﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RVCC.Data;

namespace RVCC.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
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
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RVCC.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
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

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.ActivePatient", b =>
                {
                    b.Property<int>("ActivePatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Death")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DeathDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Discharge")
                        .HasColumnType("bit");

                    b.Property<DateTime>("DischargeDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("ActivePatientId");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.ToTable("ActivePatients");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Complement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HouseNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MonthlyAmmountResidence")
                        .HasColumnType("float");

                    b.Property<string>("Neighborhood")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObservationAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ResidenceType")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("AddressId");

                    b.HasIndex("PatientId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.CancerType", b =>
                {
                    b.Property<int>("CancerTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("CancerTypeId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("CancerTypes");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.Doctor", b =>
                {
                    b.Property<int>("DoctorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CRM")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("DoctorId");

                    b.HasIndex("CRM")
                        .IsUnique()
                        .HasFilter("[CRM] IS NOT NULL");

                    b.ToTable("Doctors");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.FamilyMember", b =>
                {
                    b.Property<int>("FamilyMemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Kinship")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("MonthlyIncome")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("FamilyMemberId");

                    b.HasIndex("PatientId");

                    b.ToTable("FamilyMembers");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.FileAttachment", b =>
                {
                    b.Property<int>("FileAttachmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileExtension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FilePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("FileSize")
                        .HasColumnType("float");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("FileAttachmentId");

                    b.HasIndex("PatientId");

                    b.ToTable("FileAttachments");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.Medicine", b =>
                {
                    b.Property<int>("MedicineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("MedicineId");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Medicines");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.Naturality", b =>
                {
                    b.Property<int>("NaturalityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("NaturalityId");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.ToTable("Naturalities");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.Patient", b =>
                {
                    b.Property<int>("PatientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CPF")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("CivilState")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<bool>("FamiliarityGroup")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("MonthlyIncome")
                        .HasColumnType("float");

                    b.Property<string>("Profession")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RG")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Sex")
                        .HasColumnType("int");

                    b.Property<string>("SocialObservation")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("PatientId");

                    b.HasIndex("CPF")
                        .IsUnique()
                        .HasFilter("[CPF] IS NOT NULL");

                    b.HasIndex("RG")
                        .IsUnique()
                        .HasFilter("[RG] IS NOT NULL");

                    b.ToTable("Patients");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.PatientInformation", b =>
                {
                    b.Property<int>("PatientInformationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TreatmentBeginDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("PatientInformationId");

                    b.HasIndex("PatientId")
                        .IsUnique();

                    b.ToTable("PatientInformation");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.Phone", b =>
                {
                    b.Property<int>("PhoneId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ObservationNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<int?>("PhoneType")
                        .HasColumnType("int");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("PhoneId");

                    b.HasIndex("PatientId");

                    b.ToTable("Phones");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.Presence", b =>
                {
                    b.Property<int>("PresenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PatientId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PresenceDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("PresenceId");

                    b.ToTable("Presences");
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.TreatmentPlace", b =>
                {
                    b.Property<int>("TreatmentPlaceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("TreatmentPlaceId");

                    b.HasIndex("City")
                        .IsUnique()
                        .HasFilter("[City] IS NOT NULL");

                    b.ToTable("TreatmentPlaces");
                });

            modelBuilder.Entity("RVCC.Data.Models.RelationModels.PatientInformationCancerType", b =>
                {
                    b.Property<int>("PatientInformationId")
                        .HasColumnType("int");

                    b.Property<int>("CancerTypeId")
                        .HasColumnType("int");

                    b.HasKey("PatientInformationId", "CancerTypeId");

                    b.HasIndex("CancerTypeId");

                    b.ToTable("PatientInformationCancerType");
                });

            modelBuilder.Entity("RVCC.Data.Models.RelationModels.PatientInformationDoctor", b =>
                {
                    b.Property<int>("PatientInformationId")
                        .HasColumnType("int");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int");

                    b.HasKey("PatientInformationId", "DoctorId");

                    b.HasIndex("DoctorId");

                    b.ToTable("PatientInformationDoctor");
                });

            modelBuilder.Entity("RVCC.Data.Models.RelationModels.PatientInformationMedicine", b =>
                {
                    b.Property<int>("PatientInformationId")
                        .HasColumnType("int");

                    b.Property<int>("MedicineId")
                        .HasColumnType("int");

                    b.HasKey("PatientInformationId", "MedicineId");

                    b.HasIndex("MedicineId");

                    b.ToTable("PatientInformationMedicine");
                });

            modelBuilder.Entity("RVCC.Data.Models.RelationModels.PatientInformationTreatmentPlace", b =>
                {
                    b.Property<int>("PatientInformationId")
                        .HasColumnType("int");

                    b.Property<int>("TreatmentPlaceId")
                        .HasColumnType("int");

                    b.HasKey("PatientInformationId", "TreatmentPlaceId");

                    b.HasIndex("TreatmentPlaceId");

                    b.ToTable("PatientInformationTreatmentPlace");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RVCC.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RVCC.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RVCC.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("RVCC.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.ActivePatient", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.Patient", "Patient")
                        .WithOne("ActivePatient")
                        .HasForeignKey("RVCC.Data.Models.PatientModels.ActivePatient", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.Address", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.Patient", "Patient")
                        .WithMany("Addresses")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.FamilyMember", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.Patient", "Patient")
                        .WithMany("FamilyMembers")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.FileAttachment", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.Patient", "Patient")
                        .WithMany("FileAttachments")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.Naturality", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.Patient", "Patient")
                        .WithOne("Naturality")
                        .HasForeignKey("RVCC.Data.Models.PatientModels.Naturality", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.PatientInformation", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.Patient", "Patient")
                        .WithOne("PatientInformation")
                        .HasForeignKey("RVCC.Data.Models.PatientModels.PatientInformation", "PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.PatientModels.Phone", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.Patient", "Patient")
                        .WithMany("Phones")
                        .HasForeignKey("PatientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.RelationModels.PatientInformationCancerType", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.CancerType", "CancerType")
                        .WithMany("PatientInformationCancerTypes")
                        .HasForeignKey("CancerTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RVCC.Data.Models.PatientModels.PatientInformation", "PatientInformation")
                        .WithMany("PatientInformationCancerTypes")
                        .HasForeignKey("PatientInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.RelationModels.PatientInformationDoctor", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.Doctor", "Doctor")
                        .WithMany("PatientInformationDoctors")
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RVCC.Data.Models.PatientModels.PatientInformation", "PatientInformation")
                        .WithMany("PatientInformationDoctors")
                        .HasForeignKey("PatientInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.RelationModels.PatientInformationMedicine", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.Medicine", "Medicine")
                        .WithMany("PatientInformationMedicines")
                        .HasForeignKey("MedicineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RVCC.Data.Models.PatientModels.PatientInformation", "PatientInformation")
                        .WithMany("PatientInformationMedicines")
                        .HasForeignKey("PatientInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RVCC.Data.Models.RelationModels.PatientInformationTreatmentPlace", b =>
                {
                    b.HasOne("RVCC.Data.Models.PatientModels.PatientInformation", "PatientInformation")
                        .WithMany("PatientInformationTreatmentPlaces")
                        .HasForeignKey("PatientInformationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RVCC.Data.Models.PatientModels.TreatmentPlace", "TreatmentPlace")
                        .WithMany("PatientInformationTreatmentPlaces")
                        .HasForeignKey("TreatmentPlaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
