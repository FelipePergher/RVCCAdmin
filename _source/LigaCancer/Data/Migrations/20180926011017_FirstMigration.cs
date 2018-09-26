using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    RegisterDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivePatients",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    ActivePatientId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Active = table.Column<bool>(nullable: false),
                    DeathDate = table.Column<DateTime>(nullable: false),
                    DischargeDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivePatients", x => x.ActivePatientId);
                    table.ForeignKey(
                        name: "FK_ActivePatients_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivePatients_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Naturalities",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    NaturalityId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Naturalities", x => x.NaturalityId);
                    table.ForeignKey(
                        name: "FK_Naturalities_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Naturalities_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Professions",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    ProfessionId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professions", x => x.ProfessionId);
                    table.ForeignKey(
                        name: "FK_Professions_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Professions_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ResidenceTypes",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    ResidenceTypeId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResidenceTypes", x => x.ResidenceTypeId);
                    table.ForeignKey(
                        name: "FK_ResidenceTypes_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResidenceTypes_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientInformation",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    PatientInformationId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TreatmentbeginDate = table.Column<DateTime>(nullable: false),
                    ActivePatientId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientInformation", x => x.PatientInformationId);
                    table.ForeignKey(
                        name: "FK_PatientInformation_ActivePatients_ActivePatientId",
                        column: x => x.ActivePatientId,
                        principalTable: "ActivePatients",
                        principalColumn: "ActivePatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientInformation_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientInformation_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Residences",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    ResidenceId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ResidenceTypeId = table.Column<int>(nullable: true),
                    ResidenceObservation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Residences", x => x.ResidenceId);
                    table.ForeignKey(
                        name: "FK_Residences_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Residences_ResidenceTypes_ResidenceTypeId",
                        column: x => x.ResidenceTypeId,
                        principalTable: "ResidenceTypes",
                        principalColumn: "ResidenceTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Residences_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CancerTypes",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    CancerTypeId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PatientInformationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CancerTypes", x => x.CancerTypeId);
                    table.ForeignKey(
                        name: "FK_CancerTypes_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CancerTypes_PatientInformation_PatientInformationId",
                        column: x => x.PatientInformationId,
                        principalTable: "PatientInformation",
                        principalColumn: "PatientInformationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CancerTypes_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    DoctorId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    CRM = table.Column<string>(nullable: true),
                    PatientInformationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.DoctorId);
                    table.ForeignKey(
                        name: "FK_Doctors_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Doctors_PatientInformation_PatientInformationId",
                        column: x => x.PatientInformationId,
                        principalTable: "PatientInformation",
                        principalColumn: "PatientInformationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Doctors_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    MedicineId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    PatientInformationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicines", x => x.MedicineId);
                    table.ForeignKey(
                        name: "FK_Medicines_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Medicines_PatientInformation_PatientInformationId",
                        column: x => x.PatientInformationId,
                        principalTable: "PatientInformation",
                        principalColumn: "PatientInformationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Medicines_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentPlaces",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    TreatmentPlaceId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    City = table.Column<string>(nullable: true),
                    PatientInformationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentPlaces", x => x.TreatmentPlaceId);
                    table.ForeignKey(
                        name: "FK_TreatmentPlaces_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreatmentPlaces_PatientInformation_PatientInformationId",
                        column: x => x.PatientInformationId,
                        principalTable: "PatientInformation",
                        principalColumn: "PatientInformationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TreatmentPlaces_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Families",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    FamilyId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MonthlyIncome = table.Column<double>(nullable: false),
                    FamilyIncome = table.Column<double>(nullable: false),
                    PerCapitaIncome = table.Column<double>(nullable: false),
                    ResidenceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Families", x => x.FamilyId);
                    table.ForeignKey(
                        name: "FK_Families_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Families_Residences_ResidenceId",
                        column: x => x.ResidenceId,
                        principalTable: "Residences",
                        principalColumn: "ResidenceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Families_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FamilyMembers",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    FamilyMemberId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Kinship = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    MonthlyIncome = table.Column<double>(nullable: false),
                    FamilyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FamilyMembers", x => x.FamilyMemberId);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "FamilyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FamilyMembers_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    PatientId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    RG = table.Column<string>(nullable: true),
                    CPF = table.Column<string>(nullable: true),
                    FamiliarityGroup = table.Column<bool>(nullable: false),
                    Sex = table.Column<int>(nullable: false),
                    CivilState = table.Column<int>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    NaturalityId = table.Column<int>(nullable: true),
                    ProfessionId = table.Column<int>(nullable: true),
                    FamilyId = table.Column<int>(nullable: true),
                    PatientInformationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patients_Families_FamilyId",
                        column: x => x.FamilyId,
                        principalTable: "Families",
                        principalColumn: "FamilyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_Naturalities_NaturalityId",
                        column: x => x.NaturalityId,
                        principalTable: "Naturalities",
                        principalColumn: "NaturalityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_PatientInformation_PatientInformationId",
                        column: x => x.PatientInformationId,
                        principalTable: "PatientInformation",
                        principalColumn: "PatientInformationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_Professions_ProfessionId",
                        column: x => x.ProfessionId,
                        principalTable: "Professions",
                        principalColumn: "ProfessionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    AddressId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Street = table.Column<string>(nullable: true),
                    Neighborhood = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    HouseNumber = table.Column<string>(nullable: true),
                    Complement = table.Column<string>(nullable: true),
                    ObservationAddress = table.Column<string>(nullable: true),
                    PatientId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    AttachmentsId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachmentsId);
                    table.ForeignKey(
                        name: "FK_Attachments_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attachments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attachments_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    PhoneId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(nullable: true),
                    PhoneType = table.Column<int>(nullable: false),
                    ObservationNote = table.Column<string>(nullable: true),
                    PatientId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phones", x => x.PhoneId);
                    table.ForeignKey(
                        name: "FK_Phones_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Phones_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Phones_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FileAttachments",
                columns: table => new
                {
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true),
                    IPAddressCreated = table.Column<string>(nullable: true),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    LastIPAddressUpdated = table.Column<string>(nullable: true),
                    FileAttachmentId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FilePath = table.Column<string>(nullable: true),
                    AttachmentsId = table.Column<int>(nullable: true),
                    AttachmentsId1 = table.Column<int>(nullable: true),
                    AttachmentsId2 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAttachments", x => x.FileAttachmentId);
                    table.ForeignKey(
                        name: "FK_FileAttachments_Attachments_AttachmentsId",
                        column: x => x.AttachmentsId,
                        principalTable: "Attachments",
                        principalColumn: "AttachmentsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileAttachments_Attachments_AttachmentsId1",
                        column: x => x.AttachmentsId1,
                        principalTable: "Attachments",
                        principalColumn: "AttachmentsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileAttachments_Attachments_AttachmentsId2",
                        column: x => x.AttachmentsId2,
                        principalTable: "Attachments",
                        principalColumn: "AttachmentsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileAttachments_AspNetUsers_LastUserUpdateId",
                        column: x => x.LastUserUpdateId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FileAttachments_AspNetUsers_UserCreatedId",
                        column: x => x.UserCreatedId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivePatients_LastUserUpdateId",
                table: "ActivePatients",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivePatients_UserCreatedId",
                table: "ActivePatients",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_LastUserUpdateId",
                table: "Addresses",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PatientId",
                table: "Addresses",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserCreatedId",
                table: "Addresses",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_LastUserUpdateId",
                table: "Attachments",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_PatientId",
                table: "Attachments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_UserCreatedId",
                table: "Attachments",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_CancerTypes_LastUserUpdateId",
                table: "CancerTypes",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_CancerTypes_PatientInformationId",
                table: "CancerTypes",
                column: "PatientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_CancerTypes_UserCreatedId",
                table: "CancerTypes",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_LastUserUpdateId",
                table: "Doctors",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_PatientInformationId",
                table: "Doctors",
                column: "PatientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_UserCreatedId",
                table: "Doctors",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_LastUserUpdateId",
                table: "Families",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_ResidenceId",
                table: "Families",
                column: "ResidenceId");

            migrationBuilder.CreateIndex(
                name: "IX_Families_UserCreatedId",
                table: "Families",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_FamilyId",
                table: "FamilyMembers",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_LastUserUpdateId",
                table: "FamilyMembers",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_FamilyMembers_UserCreatedId",
                table: "FamilyMembers",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_AttachmentsId",
                table: "FileAttachments",
                column: "AttachmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_AttachmentsId1",
                table: "FileAttachments",
                column: "AttachmentsId1");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_AttachmentsId2",
                table: "FileAttachments",
                column: "AttachmentsId2");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_LastUserUpdateId",
                table: "FileAttachments",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_UserCreatedId",
                table: "FileAttachments",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_LastUserUpdateId",
                table: "Medicines",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_PatientInformationId",
                table: "Medicines",
                column: "PatientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_UserCreatedId",
                table: "Medicines",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Naturalities_LastUserUpdateId",
                table: "Naturalities",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Naturalities_UserCreatedId",
                table: "Naturalities",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInformation_ActivePatientId",
                table: "PatientInformation",
                column: "ActivePatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInformation_LastUserUpdateId",
                table: "PatientInformation",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientInformation_UserCreatedId",
                table: "PatientInformation",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_FamilyId",
                table: "Patients",
                column: "FamilyId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_LastUserUpdateId",
                table: "Patients",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_NaturalityId",
                table: "Patients",
                column: "NaturalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PatientInformationId",
                table: "Patients",
                column: "PatientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ProfessionId",
                table: "Patients",
                column: "ProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_UserCreatedId",
                table: "Patients",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Phones_LastUserUpdateId",
                table: "Phones",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Phones_PatientId",
                table: "Phones",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Phones_UserCreatedId",
                table: "Phones",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Professions_LastUserUpdateId",
                table: "Professions",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Professions_UserCreatedId",
                table: "Professions",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_Residences_LastUserUpdateId",
                table: "Residences",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_Residences_ResidenceTypeId",
                table: "Residences",
                column: "ResidenceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Residences_UserCreatedId",
                table: "Residences",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceTypes_LastUserUpdateId",
                table: "ResidenceTypes",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_ResidenceTypes_UserCreatedId",
                table: "ResidenceTypes",
                column: "UserCreatedId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlaces_LastUserUpdateId",
                table: "TreatmentPlaces",
                column: "LastUserUpdateId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlaces_PatientInformationId",
                table: "TreatmentPlaces",
                column: "PatientInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentPlaces_UserCreatedId",
                table: "TreatmentPlaces",
                column: "UserCreatedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CancerTypes");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "FamilyMembers");

            migrationBuilder.DropTable(
                name: "FileAttachments");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropTable(
                name: "TreatmentPlaces");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Families");

            migrationBuilder.DropTable(
                name: "Naturalities");

            migrationBuilder.DropTable(
                name: "PatientInformation");

            migrationBuilder.DropTable(
                name: "Professions");

            migrationBuilder.DropTable(
                name: "Residences");

            migrationBuilder.DropTable(
                name: "ActivePatients");

            migrationBuilder.DropTable(
                name: "ResidenceTypes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
