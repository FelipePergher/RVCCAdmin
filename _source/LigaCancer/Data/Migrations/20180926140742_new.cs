using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "TreatmentPlaces");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "TreatmentPlaces");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "ResidenceTypes");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "ResidenceTypes");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "Residences");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "Professions");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "Professions");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "PatientInformation");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "PatientInformation");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "Naturalities");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "Naturalities");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "CancerTypes");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "CancerTypes");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "IPAddressCreated",
                table: "ActivePatients");

            migrationBuilder.DropColumn(
                name: "LastIPAddressUpdated",
                table: "ActivePatients");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "TreatmentPlaces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "TreatmentPlaces",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "ResidenceTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "ResidenceTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "Residences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "Residences",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "Professions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "Professions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "Phones",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "Phones",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "PatientInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "PatientInformation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "Naturalities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "Naturalities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "Medicines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "Medicines",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "FamilyMembers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "FamilyMembers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "Families",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "Families",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "Doctors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "Doctors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "CancerTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "CancerTypes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "Attachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "Attachments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "Addresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "Addresses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IPAddressCreated",
                table: "ActivePatients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastIPAddressUpdated",
                table: "ActivePatients",
                nullable: true);
        }
    }
}
