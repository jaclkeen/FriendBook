using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using FriendBook.Data;

namespace FriendBook.Migrations
{
    [DbContext(typeof(FriendBookContext))]
    partial class FriendBookContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1");

            modelBuilder.Entity("FriendBook.Models.Album", b =>
                {
                    b.Property<int>("AlbumId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AlbumDescription");

                    b.Property<string>("AlbumName");

                    b.Property<int>("UserId");

                    b.HasKey("AlbumId");

                    b.HasIndex("UserId");

                    b.ToTable("Album");
                });

            modelBuilder.Entity("FriendBook.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PostId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<DateTime>("TimePosted");

                    b.Property<int>("UserId");

                    b.HasKey("CommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("FriendBook.Models.Conversation", b =>
                {
                    b.Property<string>("ConversationRoomName");

                    b.Property<int>("ConversationRecieverId");

                    b.Property<int>("ConversationStarterId");

                    b.HasKey("ConversationRoomName");

                    b.HasIndex("ConversationRecieverId");

                    b.HasIndex("ConversationStarterId");

                    b.ToTable("Conversation");
                });

            modelBuilder.Entity("FriendBook.Models.Image", b =>
                {
                    b.Property<int>("ImageId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AlbumId");

                    b.Property<string>("ImageDescription");

                    b.Property<string>("ImagePath")
                        .IsRequired();

                    b.Property<int>("UserId");

                    b.HasKey("ImageId");

                    b.HasIndex("AlbumId");

                    b.HasIndex("UserId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("FriendBook.Models.Message", b =>
                {
                    b.Property<int>("MessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConversationRoomName")
                        .IsRequired();

                    b.Property<DateTime>("MessageSentDate");

                    b.Property<string>("MessageText")
                        .IsRequired();

                    b.Property<int>("SendingUserId");

                    b.HasKey("MessageId");

                    b.HasIndex("ConversationRoomName");

                    b.HasIndex("SendingUserId");

                    b.ToTable("Message");
                });

            modelBuilder.Entity("FriendBook.Models.MessageNotification", b =>
                {
                    b.Property<int>("MessageNotificationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("RecievingUserId");

                    b.Property<bool>("Seen");

                    b.Property<int>("SendingUserId");

                    b.HasKey("MessageNotificationId");

                    b.HasIndex("RecievingUserId");

                    b.HasIndex("SendingUserId");

                    b.ToTable("MessageNotification");
                });

            modelBuilder.Entity("FriendBook.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Dislikes");

                    b.Property<string>("ImgUrl");

                    b.Property<int>("Likes");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<DateTime>("TimePosted");

                    b.Property<int>("UserId");

                    b.HasKey("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Post");
                });

            modelBuilder.Entity("FriendBook.Models.Relationship", b =>
                {
                    b.Property<int>("RelationshipId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ReceivingUserUserId");

                    b.Property<int>("ReciverUserId");

                    b.Property<int>("SenderUserId");

                    b.Property<int>("Status");

                    b.HasKey("RelationshipId");

                    b.HasIndex("ReceivingUserUserId");

                    b.HasIndex("SenderUserId");

                    b.ToTable("Relationship");
                });

            modelBuilder.Entity("FriendBook.Models.Style", b =>
                {
                    b.Property<int>("StyleId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BackgroundColor");

                    b.Property<string>("DetailColor");

                    b.Property<string>("FontColor");

                    b.Property<string>("FontFamily");

                    b.Property<int>("FontSize");

                    b.Property<string>("NavColor");

                    b.Property<string>("PostBackgroundColor");

                    b.Property<string>("PostHeaderColor");

                    b.Property<int>("UserId");

                    b.Property<string>("WallBackgroundColor");

                    b.HasKey("StyleId");

                    b.HasIndex("UserId");

                    b.ToTable("Style");
                });

            modelBuilder.Entity("FriendBook.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CoverImg");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("ProfileImg");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("FriendBook.Models.Album", b =>
                {
                    b.HasOne("FriendBook.Models.User", "AlbumUser")
                        .WithMany("UserAlbums")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.Comment", b =>
                {
                    b.HasOne("FriendBook.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FriendBook.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.Conversation", b =>
                {
                    b.HasOne("FriendBook.Models.User", "ConversationReciever")
                        .WithMany()
                        .HasForeignKey("ConversationRecieverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FriendBook.Models.User", "ConversationStarter")
                        .WithMany()
                        .HasForeignKey("ConversationStarterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.Image", b =>
                {
                    b.HasOne("FriendBook.Models.Album", "album")
                        .WithMany("AlbumImages")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FriendBook.Models.User", "ImageUser")
                        .WithMany("UserImages")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.Message", b =>
                {
                    b.HasOne("FriendBook.Models.Conversation", "Conversation")
                        .WithMany("ConversationMessages")
                        .HasForeignKey("ConversationRoomName")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FriendBook.Models.User", "SendingUser")
                        .WithMany()
                        .HasForeignKey("SendingUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.MessageNotification", b =>
                {
                    b.HasOne("FriendBook.Models.User", "RecievingUser")
                        .WithMany()
                        .HasForeignKey("RecievingUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FriendBook.Models.User", "SendingUser")
                        .WithMany()
                        .HasForeignKey("SendingUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.Post", b =>
                {
                    b.HasOne("FriendBook.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.Relationship", b =>
                {
                    b.HasOne("FriendBook.Models.User", "ReceivingUser")
                        .WithMany()
                        .HasForeignKey("ReceivingUserUserId");

                    b.HasOne("FriendBook.Models.User", "SenderUser")
                        .WithMany()
                        .HasForeignKey("SenderUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.Style", b =>
                {
                    b.HasOne("FriendBook.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
