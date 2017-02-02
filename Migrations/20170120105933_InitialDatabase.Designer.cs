using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TCM.Models;

namespace tms.Migrations
{
    [DbContext(typeof(TCMContext))]
    [Migration("20170120105933_InitialDatabase")]
    partial class InitialDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TCM.Models.Step", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.Property<string>("Result");

                    b.Property<string>("Status");

                    b.Property<int?>("TestCaseId");

                    b.HasKey("Id");

                    b.HasIndex("TestCaseId");

                    b.ToTable("Steps");
                });

            modelBuilder.Entity("TCM.Models.TestCase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Name");

                    b.Property<string>("Preconditions");

                    b.Property<string>("Status");

                    b.HasKey("Id");

                    b.ToTable("TestCases");
                });

            modelBuilder.Entity("TCM.Models.Step", b =>
                {
                    b.HasOne("TCM.Models.TestCase")
                        .WithMany("Steps")
                        .HasForeignKey("TestCaseId");
                });
        }
    }
}
