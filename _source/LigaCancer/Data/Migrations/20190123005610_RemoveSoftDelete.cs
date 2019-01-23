using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class RemoveSoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivePatients_AspNetUsers_LastUserUpdateId",
                table: "ActivePatients");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_LastUserUpdateId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CancerTypes_AspNetUsers_LastUserUpdateId",
                table: "CancerTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AspNetUsers_LastUserUpdateId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_AspNetUsers_LastUserUpdateId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMembers_AspNetUsers_LastUserUpdateId",
                table: "FamilyMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_AspNetUsers_LastUserUpdateId",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_AspNetUsers_LastUserUpdateId",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Naturalities_AspNetUsers_LastUserUpdateId",
                table: "Naturalities");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientInformation_AspNetUsers_LastUserUpdateId",
                table: "PatientInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_LastUserUpdateId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_AspNetUsers_LastUserUpdateId",
                table: "Phones");

            migrationBuilder.DropForeignKey(
                name: "FK_Presences_AspNetUsers_LastUserUpdateId",
                table: "Presences");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentPlaces_AspNetUsers_LastUserUpdateId",
                table: "TreatmentPlaces");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "TreatmentPlaces");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TreatmentPlaces");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Presences");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Presences");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Phones");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "PatientInformation");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PatientInformation");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Naturalities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Naturalities");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Medicines");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FamilyMembers");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Families");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "CancerTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CancerTypes");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "ActivePatients");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ActivePatients");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "TreatmentPlaces",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "TreatmentPlaces",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_TreatmentPlaces_LastUserUpdateId",
                table: "TreatmentPlaces",
                newName: "IX_TreatmentPlaces_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "Presences",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Presences",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Presences_LastUserUpdateId",
                table: "Presences",
                newName: "IX_Presences_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "Phones",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Phones",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Phones_LastUserUpdateId",
                table: "Phones",
                newName: "IX_Phones_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "Patients",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Patients",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_LastUserUpdateId",
                table: "Patients",
                newName: "IX_Patients_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "PatientInformation",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "PatientInformation",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_PatientInformation_LastUserUpdateId",
                table: "PatientInformation",
                newName: "IX_PatientInformation_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "Naturalities",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Naturalities",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Naturalities_LastUserUpdateId",
                table: "Naturalities",
                newName: "IX_Naturalities_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "Medicines",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Medicines",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Medicines_LastUserUpdateId",
                table: "Medicines",
                newName: "IX_Medicines_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "FileAttachments",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "FileAttachments",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_LastUserUpdateId",
                table: "FileAttachments",
                newName: "IX_FileAttachments_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "FamilyMembers",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "FamilyMembers",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyMembers_LastUserUpdateId",
                table: "FamilyMembers",
                newName: "IX_FamilyMembers_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "Families",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Families",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Families_LastUserUpdateId",
                table: "Families",
                newName: "IX_Families_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "Doctors",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Doctors",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_LastUserUpdateId",
                table: "Doctors",
                newName: "IX_Doctors_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "CancerTypes",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "CancerTypes",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_CancerTypes_LastUserUpdateId",
                table: "CancerTypes",
                newName: "IX_CancerTypes_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "Addresses",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "Addresses",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_LastUserUpdateId",
                table: "Addresses",
                newName: "IX_Addresses_UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUserUpdateId",
                table: "ActivePatients",
                newName: "UserUpdatedId");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedDate",
                table: "ActivePatients",
                newName: "UpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_ActivePatients_LastUserUpdateId",
                table: "ActivePatients",
                newName: "IX_ActivePatients_UserUpdatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivePatients_AspNetUsers_UserUpdatedId",
                table: "ActivePatients",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_UserUpdatedId",
                table: "Addresses",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CancerTypes_AspNetUsers_UserUpdatedId",
                table: "CancerTypes",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AspNetUsers_UserUpdatedId",
                table: "Doctors",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_AspNetUsers_UserUpdatedId",
                table: "Families",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMembers_AspNetUsers_UserUpdatedId",
                table: "FamilyMembers",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_AspNetUsers_UserUpdatedId",
                table: "FileAttachments",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_AspNetUsers_UserUpdatedId",
                table: "Medicines",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Naturalities_AspNetUsers_UserUpdatedId",
                table: "Naturalities",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientInformation_AspNetUsers_UserUpdatedId",
                table: "PatientInformation",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_UserUpdatedId",
                table: "Patients",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_AspNetUsers_UserUpdatedId",
                table: "Phones",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Presences_AspNetUsers_UserUpdatedId",
                table: "Presences",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentPlaces_AspNetUsers_UserUpdatedId",
                table: "TreatmentPlaces",
                column: "UserUpdatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivePatients_AspNetUsers_UserUpdatedId",
                table: "ActivePatients");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_UserUpdatedId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_CancerTypes_AspNetUsers_UserUpdatedId",
                table: "CancerTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AspNetUsers_UserUpdatedId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Families_AspNetUsers_UserUpdatedId",
                table: "Families");

            migrationBuilder.DropForeignKey(
                name: "FK_FamilyMembers_AspNetUsers_UserUpdatedId",
                table: "FamilyMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_AspNetUsers_UserUpdatedId",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Medicines_AspNetUsers_UserUpdatedId",
                table: "Medicines");

            migrationBuilder.DropForeignKey(
                name: "FK_Naturalities_AspNetUsers_UserUpdatedId",
                table: "Naturalities");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientInformation_AspNetUsers_UserUpdatedId",
                table: "PatientInformation");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_AspNetUsers_UserUpdatedId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Phones_AspNetUsers_UserUpdatedId",
                table: "Phones");

            migrationBuilder.DropForeignKey(
                name: "FK_Presences_AspNetUsers_UserUpdatedId",
                table: "Presences");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentPlaces_AspNetUsers_UserUpdatedId",
                table: "TreatmentPlaces");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "TreatmentPlaces",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "TreatmentPlaces",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_TreatmentPlaces_UserUpdatedId",
                table: "TreatmentPlaces",
                newName: "IX_TreatmentPlaces_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "Presences",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Presences",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Presences_UserUpdatedId",
                table: "Presences",
                newName: "IX_Presences_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "Phones",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Phones",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Phones_UserUpdatedId",
                table: "Phones",
                newName: "IX_Phones_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "Patients",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Patients",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_UserUpdatedId",
                table: "Patients",
                newName: "IX_Patients_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "PatientInformation",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "PatientInformation",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_PatientInformation_UserUpdatedId",
                table: "PatientInformation",
                newName: "IX_PatientInformation_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "Naturalities",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Naturalities",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Naturalities_UserUpdatedId",
                table: "Naturalities",
                newName: "IX_Naturalities_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "Medicines",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Medicines",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Medicines_UserUpdatedId",
                table: "Medicines",
                newName: "IX_Medicines_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "FileAttachments",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "FileAttachments",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_UserUpdatedId",
                table: "FileAttachments",
                newName: "IX_FileAttachments_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "FamilyMembers",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "FamilyMembers",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_FamilyMembers_UserUpdatedId",
                table: "FamilyMembers",
                newName: "IX_FamilyMembers_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "Families",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Families",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Families_UserUpdatedId",
                table: "Families",
                newName: "IX_Families_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "Doctors",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Doctors",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Doctors_UserUpdatedId",
                table: "Doctors",
                newName: "IX_Doctors_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "CancerTypes",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "CancerTypes",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_CancerTypes_UserUpdatedId",
                table: "CancerTypes",
                newName: "IX_CancerTypes_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "Addresses",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "Addresses",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_UserUpdatedId",
                table: "Addresses",
                newName: "IX_Addresses_LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UserUpdatedId",
                table: "ActivePatients",
                newName: "LastUserUpdateId");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "ActivePatients",
                newName: "LastUpdatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_ActivePatients_UserUpdatedId",
                table: "ActivePatients",
                newName: "IX_ActivePatients_LastUserUpdateId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "TreatmentPlaces",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TreatmentPlaces",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Presences",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Presences",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Phones",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Phones",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Patients",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Patients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "PatientInformation",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PatientInformation",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Naturalities",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Naturalities",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Medicines",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Medicines",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "FileAttachments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FileAttachments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "FamilyMembers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FamilyMembers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Families",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Families",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Doctors",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Doctors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "CancerTypes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CancerTypes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Addresses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Addresses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "ActivePatients",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ActivePatients",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ActivePatients_AspNetUsers_LastUserUpdateId",
                table: "ActivePatients",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_LastUserUpdateId",
                table: "Addresses",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CancerTypes_AspNetUsers_LastUserUpdateId",
                table: "CancerTypes",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AspNetUsers_LastUserUpdateId",
                table: "Doctors",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Families_AspNetUsers_LastUserUpdateId",
                table: "Families",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FamilyMembers_AspNetUsers_LastUserUpdateId",
                table: "FamilyMembers",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_AspNetUsers_LastUserUpdateId",
                table: "FileAttachments",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medicines_AspNetUsers_LastUserUpdateId",
                table: "Medicines",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Naturalities_AspNetUsers_LastUserUpdateId",
                table: "Naturalities",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientInformation_AspNetUsers_LastUserUpdateId",
                table: "PatientInformation",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_AspNetUsers_LastUserUpdateId",
                table: "Patients",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Phones_AspNetUsers_LastUserUpdateId",
                table: "Phones",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Presences_AspNetUsers_LastUserUpdateId",
                table: "Presences",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentPlaces_AspNetUsers_LastUserUpdateId",
                table: "TreatmentPlaces",
                column: "LastUserUpdateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
