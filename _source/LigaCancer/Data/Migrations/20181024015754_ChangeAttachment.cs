using Microsoft.EntityFrameworkCore.Migrations;

namespace LigaCancer.Migrations
{
    public partial class ChangeAttachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentsId",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentsId1",
                table: "FileAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentsId2",
                table: "FileAttachments");

            migrationBuilder.RenameColumn(
                name: "AttachmentsId2",
                table: "FileAttachments",
                newName: "AttachmentId2");

            migrationBuilder.RenameColumn(
                name: "AttachmentsId1",
                table: "FileAttachments",
                newName: "AttachmentId1");

            migrationBuilder.RenameColumn(
                name: "AttachmentsId",
                table: "FileAttachments",
                newName: "AttachmentId");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_AttachmentsId2",
                table: "FileAttachments",
                newName: "IX_FileAttachments_AttachmentId2");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_AttachmentsId1",
                table: "FileAttachments",
                newName: "IX_FileAttachments_AttachmentId1");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_AttachmentsId",
                table: "FileAttachments",
                newName: "IX_FileAttachments_AttachmentId");

            migrationBuilder.RenameColumn(
                name: "AttachmentsId",
                table: "Attachments",
                newName: "AttachmentId");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "FileAttachments",
                nullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "FileAttachments");

            migrationBuilder.RenameColumn(
                name: "AttachmentId2",
                table: "FileAttachments",
                newName: "AttachmentsId2");

            migrationBuilder.RenameColumn(
                name: "AttachmentId1",
                table: "FileAttachments",
                newName: "AttachmentsId1");

            migrationBuilder.RenameColumn(
                name: "AttachmentId",
                table: "FileAttachments",
                newName: "AttachmentsId");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_AttachmentId2",
                table: "FileAttachments",
                newName: "IX_FileAttachments_AttachmentsId2");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_AttachmentId1",
                table: "FileAttachments",
                newName: "IX_FileAttachments_AttachmentsId1");

            migrationBuilder.RenameIndex(
                name: "IX_FileAttachments_AttachmentId",
                table: "FileAttachments",
                newName: "IX_FileAttachments_AttachmentsId");

            migrationBuilder.RenameColumn(
                name: "AttachmentId",
                table: "Attachments",
                newName: "AttachmentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentsId",
                table: "FileAttachments",
                column: "AttachmentsId",
                principalTable: "Attachments",
                principalColumn: "AttachmentsId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentsId1",
                table: "FileAttachments",
                column: "AttachmentsId1",
                principalTable: "Attachments",
                principalColumn: "AttachmentsId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FileAttachments_Attachments_AttachmentsId2",
                table: "FileAttachments",
                column: "AttachmentsId2",
                principalTable: "Attachments",
                principalColumn: "AttachmentsId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
