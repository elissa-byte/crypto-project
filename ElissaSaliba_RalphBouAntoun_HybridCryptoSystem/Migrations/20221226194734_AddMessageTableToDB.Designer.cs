﻿// <auto-generated />
using ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ElissaSaliba_RalphBouAntoun_HybridCryptoSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221226194734_AddMessageTableToDB")]
    partial class AddMessageTableToDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);
#pragma warning restore 612, 618
        }
    }
}
