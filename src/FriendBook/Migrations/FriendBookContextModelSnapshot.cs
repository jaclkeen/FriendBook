﻿using System;
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

                    b.Property<int?>("PostId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<DateTime>("TimePosted");

                    b.Property<int>("UserId");

                    b.Property<int?>("YardSaleItemId");

                    b.HasKey("CommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.HasIndex("YardSaleItemId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("FriendBook.Models.Conversation", b =>
                {
                    b.Property<string>("ConversationRoomName");

                    b.Property<int>("ConversationRecieverId");

                    b.Property<bool>("ConversationRecieverIsActive");

                    b.Property<int>("ConversationStarterId");

                    b.Property<bool>("ConversationStarterIsActive");

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

            modelBuilder.Entity("FriendBook.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NotificationText")
                        .IsRequired();

                    b.Property<string>("NotificationType")
                        .IsRequired();

                    b.Property<DateTime>("NotificatonDate");

                    b.Property<int>("PostId");

                    b.Property<int>("RecievingUserId");

                    b.Property<bool>("Seen");

                    b.Property<int>("SenderUserId");

                    b.Property<int?>("SendingUserUserId");

                    b.Property<int>("YardSaleItemId");

                    b.HasKey("NotificationId");

                    b.HasIndex("RecievingUserId");

                    b.HasIndex("SendingUserUserId");

                    b.ToTable("Notification");
                });

            modelBuilder.Entity("FriendBook.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Dislikes");

                    b.Property<string>("ImgUrl");

                    b.Property<int>("Likes");

                    b.Property<string>("PostType")
                        .IsRequired();

                    b.Property<int?>("RecievingUserId");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasAnnotation("MaxLength", 200);

                    b.Property<DateTime>("TimePosted");

                    b.Property<int>("UserId");

                    b.HasKey("PostId");

                    b.HasIndex("RecievingUserId");

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

            modelBuilder.Entity("FriendBook.Models.Tag", b =>
                {
                    b.Property<int>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PersonBeingTaggedId");

                    b.Property<int>("PostId");

                    b.Property<int>("TaggerId");

                    b.HasKey("TagId");

                    b.HasIndex("PersonBeingTaggedId");

                    b.HasIndex("PostId");

                    b.HasIndex("TaggerId");

                    b.ToTable("Tag");
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

            modelBuilder.Entity("FriendBook.Models.YardSaleItem", b =>
                {
                    b.Property<int>("YardSaleItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<DateTime>("DatePosted");

                    b.Property<string>("ItemDescription")
                        .IsRequired();

                    b.Property<string>("ItemImage1")
                        .IsRequired();

                    b.Property<string>("ItemImage2");

                    b.Property<string>("ItemImage3");

                    b.Property<string>("ItemImage4");

                    b.Property<string>("ItemName")
                        .IsRequired();

                    b.Property<double>("ItemPrice");

                    b.Property<int>("PostingUserId");

                    b.HasKey("YardSaleItemId");

                    b.HasIndex("PostingUserId");

                    b.ToTable("YardSaleItem");
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
                        .HasForeignKey("PostId");

                    b.HasOne("FriendBook.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FriendBook.Models.YardSaleItem", "YardSaleItem")
                        .WithMany("ItemComments")
                        .HasForeignKey("YardSaleItemId");
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

            modelBuilder.Entity("FriendBook.Models.Notification", b =>
                {
                    b.HasOne("FriendBook.Models.User", "RecievingUser")
                        .WithMany()
                        .HasForeignKey("RecievingUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FriendBook.Models.User", "SendingUser")
                        .WithMany()
                        .HasForeignKey("SendingUserUserId");
                });

            modelBuilder.Entity("FriendBook.Models.Post", b =>
                {
                    b.HasOne("FriendBook.Models.User", "RecievingUser")
                        .WithMany()
                        .HasForeignKey("RecievingUserId");

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

            modelBuilder.Entity("FriendBook.Models.Tag", b =>
                {
                    b.HasOne("FriendBook.Models.User", "PersonBeingTagged")
                        .WithMany()
                        .HasForeignKey("PersonBeingTaggedId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FriendBook.Models.Post", "Post")
                        .WithMany("TaggedUsers")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FriendBook.Models.User", "Tagger")
                        .WithMany()
                        .HasForeignKey("TaggerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.YardSaleItem", b =>
                {
                    b.HasOne("FriendBook.Models.User", "PostingUser")
                        .WithMany()
                        .HasForeignKey("PostingUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
