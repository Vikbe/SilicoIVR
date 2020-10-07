﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SilicoIVR.Models;

namespace SilicoIVR.Migrations
{
    [DbContext(typeof(SilicoDBContext))]
    partial class SilicoDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SilicoIVR.Models.Agent", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Extension");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("ID");

                    b.ToTable("Agents");
                });

            modelBuilder.Entity("SilicoIVR.Models.Call", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AgentCalledID");

                    b.Property<string>("CallerType");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("From");

                    b.Property<string>("Name");

                    b.Property<string>("SID");

                    b.Property<string>("State");

                    b.Property<string>("To");

                    b.Property<string>("Zipcode");

                    b.HasKey("ID");

                    b.HasIndex("AgentCalledID");

                    b.ToTable("Calls");
                });

            modelBuilder.Entity("SilicoIVR.Models.DB.Recording", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CallID");

                    b.Property<string>("SID");

                    b.Property<string>("Transcription");

                    b.Property<double>("duration");

                    b.HasKey("ID");

                    b.HasIndex("CallID");

                    b.ToTable("Recordings");
                });

            modelBuilder.Entity("SilicoIVR.Models.IvrOption", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("ID");

                    b.ToTable("IvrOptions");
                });

            modelBuilder.Entity("SilicoIVR.Models.Call", b =>
                {
                    b.HasOne("SilicoIVR.Models.Agent", "AgentCalled")
                        .WithMany()
                        .HasForeignKey("AgentCalledID");
                });

            modelBuilder.Entity("SilicoIVR.Models.DB.Recording", b =>
                {
                    b.HasOne("SilicoIVR.Models.Call", "Call")
                        .WithMany()
                        .HasForeignKey("CallID");
                });
#pragma warning restore 612, 618
        }
    }
}
