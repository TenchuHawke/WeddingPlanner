using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WeddingPlanner.Models;

namespace Weddings.Migrations
{
    [DbContext(typeof(BaseContext))]
    [Migration("20170323220622_NewRemoteMigration")]
    partial class NewRemoteMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("WeddingPlanner.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<bool>("Registered");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WeddingPlanner.Models.Wedding", b =>
                {
                    b.Property<int>("WeddingId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("EventDate");

                    b.Property<string>("EventName");

                    b.Property<int>("OwnerId");

                    b.Property<string>("SideA");

                    b.Property<string>("SideATitle");

                    b.Property<string>("SideB");

                    b.Property<string>("SideBTitle");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("WeddingId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Weddings");
                });

            modelBuilder.Entity("WeddingPlanner.Models.WeddingGuest", b =>
                {
                    b.Property<int>("WeddingGuestId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("EventId");

                    b.Property<int>("GuestId");

                    b.Property<bool>("GuestOfSideA");

                    b.Property<bool>("Pending");

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int?>("Wedding");

                    b.HasKey("WeddingGuestId");

                    b.HasIndex("GuestId");

                    b.HasIndex("Wedding");

                    b.ToTable("WeddingGuests");
                });

            modelBuilder.Entity("WeddingPlanner.Models.Wedding", b =>
                {
                    b.HasOne("WeddingPlanner.Models.User", "Owner")
                        .WithMany("WeddingsPlanned")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WeddingPlanner.Models.WeddingGuest", b =>
                {
                    b.HasOne("WeddingPlanner.Models.User", "Guest")
                        .WithMany("GuestAtWeddings")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WeddingPlanner.Models.Wedding", "Event")
                        .WithMany("GuestsAttending")
                        .HasForeignKey("Wedding");
                });
        }
    }
}
