using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class ChangedFileAttahcments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentId",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentId1",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentId2",
                table: "FileAttachments");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_FileAttachments_AttachmentId",
                table: "FileAttachments");

            migrationBuilder.DropIndex(
                name: "IX_FileAttachments_AttachmentId1",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "AttachmentId",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "AttachmentId1",
                table: "FileAttachments");

            migrationBuilder.RenameColumn(
                name: "AttachmentId2",
                table: "FileAttachments",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_AttachmentId2",
                table: "FileAttachments",
                newName: "IX_FileAttachments_PatientId");

            migrationBuilder.AddColumn<int>(
                name: "ArchiveCategorie",
                table: "FileAttachments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Patients_PatientId",
                table: "FileAttachments",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Patients_PatientId",
                table: "FileAttachments");

            migrationBuilder.DropColumn(
                name: "ArchiveCategorie",
                table: "FileAttachments");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "FileAttachments",
                newName: "AttachmentId2");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_PatientId",
                table: "FileAttachments",
                newName: "IX_FileAttachments_AttachmentId2");

            migrationBuilder.AddColumn<int>(
                name: "AttachmentId",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttachmentId1",
                table: "FileAttachments",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeletedDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUserUpdateId = table.Column<string>(nullable: true),
                    PatientId = table.Column<int>(nullable: true),
                    RegisterDate = table.Column<DateTime>(nullable: false),
                    UserCreatedId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachmentId);
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

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_AttachmentId",
                table: "FileAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FileAttachments_AttachmentId1",
                table: "FileAttachments",
                column: "AttachmentId1");

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

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentId",
                table: "FileAttachments",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentId1",
                table: "FileAttachments",
                column: "AttachmentId1",
                principalTable: "Attachments",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentId2",
                table: "FileAttachments",
                column: "AttachmentId2",
                principalTable: "Attachments",
                principalColumn: "AttachmentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
