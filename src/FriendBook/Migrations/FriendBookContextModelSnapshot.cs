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
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

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

            modelBuilder.Entity("FriendBook.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImgUrl");

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

                    b.Property<int>("Status");

                    b.Property<int?>("UserId");

                    b.Property<int>("UserId1");

                    b.Property<int>("UserId2");

                    b.HasKey("RelationshipId");

                    b.HasIndex("UserId");

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

                    b.Property<int>("UserId");

                    b.HasKey("StyleId");

                    b.HasIndex("UserId");

                    b.ToTable("Style");
                });

            modelBuilder.Entity("FriendBook.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int?>("PostId");

                    b.Property<int?>("PostId1");

                    b.Property<string>("ProfileImg");

                    b.HasKey("UserId");

                    b.HasIndex("PostId");

                    b.HasIndex("PostId1");

                    b.ToTable("User");
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

            modelBuilder.Entity("FriendBook.Models.Post", b =>
                {
                    b.HasOne("FriendBook.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.Relationship", b =>
                {
                    b.HasOne("FriendBook.Models.User")
                        .WithMany("Relationships")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("FriendBook.Models.Style", b =>
                {
                    b.HasOne("FriendBook.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FriendBook.Models.User", b =>
                {
                    b.HasOne("FriendBook.Models.Post")
                        .WithMany("Dislikes")
                        .HasForeignKey("PostId");

                    b.HasOne("FriendBook.Models.Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId1");
                });
        }
    }
}
