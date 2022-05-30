﻿// <auto-generated />
using System;
using AwoBotDemo.Modules.MessageDeletionModule.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AwoBotDemo.Migrations
{
    [DbContext(typeof(MessageDeletionContext))]
    [Migration("20220530220657_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("AwoBotDemo.Modules.MessageDeletionModule.Data.ChannelFilterModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("MessageDeletion.ChannelFilters");
                });

            modelBuilder.Entity("AwoBotDemo.Modules.MessageDeletionModule.Data.MessageDeletionCandidate", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("MessageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("MessageDeletion.Messages");
                });

            modelBuilder.Entity("AwoBotDemo.Modules.MessageDeletionModule.Data.RoleFilterModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildRoleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("MessageDeletion.RoleFilters");
                });

            modelBuilder.Entity("AwoBotDemo.Modules.MessageDeletionModule.Data.UserFilterModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Enabled")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("GuildUserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("MessageDeletion.UserFilters");
                });
#pragma warning restore 612, 618
        }
    }
}