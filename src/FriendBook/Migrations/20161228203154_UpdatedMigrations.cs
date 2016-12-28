using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FriendBook.Migrations
{
    public partial class UpdatedMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    CoverImg = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    ProfileImg = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Album",
                columns: table => new
                {
                    AlbumId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    AlbumDescription = table.Column<string>(nullable: true),
                    AlbumName = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Album", x => x.AlbumId);
                    table.ForeignKey(
                        name: "FK_Album_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversation",
                columns: table => new
                {
                    ConversationRoomName = table.Column<string>(nullable: false),
                    ConversationRecieverId = table.Column<int>(nullable: false),
                    ConversationRecieverIsActive = table.Column<bool>(nullable: false),
                    ConversationStarterId = table.Column<int>(nullable: false),
                    ConversationStarterIsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversation", x => x.ConversationRoomName);
                    table.ForeignKey(
                        name: "FK_Conversation_User_ConversationRecieverId",
                        column: x => x.ConversationRecieverId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversation_User_ConversationStarterId",
                        column: x => x.ConversationStarterId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageNotification",
                columns: table => new
                {
                    MessageNotificationId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    RecievingUserId = table.Column<int>(nullable: false),
                    Seen = table.Column<bool>(nullable: false),
                    SendingUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageNotification", x => x.MessageNotificationId);
                    table.ForeignKey(
                        name: "FK_MessageNotification_User_RecievingUserId",
                        column: x => x.RecievingUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageNotification_User_SendingUserId",
                        column: x => x.SendingUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Dislikes = table.Column<int>(nullable: false),
                    ImgUrl = table.Column<string>(nullable: true),
                    Likes = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 200, nullable: false),
                    TimePosted = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Post_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relationship",
                columns: table => new
                {
                    RelationshipId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    ReceivingUserUserId = table.Column<int>(nullable: true),
                    ReciverUserId = table.Column<int>(nullable: false),
                    SenderUserId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationship", x => x.RelationshipId);
                    table.ForeignKey(
                        name: "FK_Relationship_User_ReceivingUserUserId",
                        column: x => x.ReceivingUserUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Relationship_User_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Style",
                columns: table => new
                {
                    StyleId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    BackgroundColor = table.Column<string>(nullable: true),
                    DetailColor = table.Column<string>(nullable: true),
                    FontColor = table.Column<string>(nullable: true),
                    FontFamily = table.Column<string>(nullable: true),
                    FontSize = table.Column<int>(nullable: false),
                    NavColor = table.Column<string>(nullable: true),
                    PostBackgroundColor = table.Column<string>(nullable: true),
                    PostHeaderColor = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    WallBackgroundColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Style", x => x.StyleId);
                    table.ForeignKey(
                        name: "FK_Style_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ImageId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    AlbumId = table.Column<int>(nullable: false),
                    ImageDescription = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_Image_Album_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Album",
                        principalColumn: "AlbumId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Image_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    MessageId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    ConversationRoomName = table.Column<string>(nullable: false),
                    MessageSentDate = table.Column<DateTime>(nullable: false),
                    MessageText = table.Column<string>(nullable: false),
                    SendingUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Message_Conversation_ConversationRoomName",
                        column: x => x.ConversationRoomName,
                        principalTable: "Conversation",
                        principalColumn: "ConversationRoomName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Message_User_SendingUserId",
                        column: x => x.SendingUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    CommentId = table.Column<int>(nullable: false)
                        .Annotation("Autoincrement", true),
                    PostId = table.Column<int>(nullable: false),
                    Text = table.Column<string>(maxLength: 200, nullable: false),
                    TimePosted = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comment_Post_PostId",
                        column: x => x.PostId,
                        principalTable: "Post",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Album_UserId",
                table: "Album",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                table: "Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_ConversationRecieverId",
                table: "Conversation",
                column: "ConversationRecieverId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversation_ConversationStarterId",
                table: "Conversation",
                column: "ConversationStarterId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_AlbumId",
                table: "Image",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Image_UserId",
                table: "Image",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ConversationRoomName",
                table: "Message",
                column: "ConversationRoomName");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SendingUserId",
                table: "Message",
                column: "SendingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageNotification_RecievingUserId",
                table: "MessageNotification",
                column: "RecievingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageNotification_SendingUserId",
                table: "MessageNotification",
                column: "SendingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationship_ReceivingUserUserId",
                table: "Relationship",
                column: "ReceivingUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Relationship_SenderUserId",
                table: "Relationship",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Style_UserId",
                table: "Style",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "MessageNotification");

            migrationBuilder.DropTable(
                name: "Relationship");

            migrationBuilder.DropTable(
                name: "Style");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Album");

            migrationBuilder.DropTable(
                name: "Conversation");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
