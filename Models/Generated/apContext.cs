using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace PROG2230_AS4_BH6010.Models.Generated
{
    public partial class apContext : DbContext
    {
        public apContext()
        {
        }

        public apContext(DbContextOptions<apContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GeneralLedgerAccount> GeneralLedgerAccounts { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceLineItem> InvoiceLineItems { get; set; }
        public virtual DbSet<Term> Terms { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=ApDbContext");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<GeneralLedgerAccount>(entity =>
            {
                entity.HasKey(e => e.AccountNumber)
                    .HasName("PK__general___AF91A6AC9D69AA80");

                entity.Property(e => e.AccountNumber).ValueGeneratedNever();

                entity.Property(e => e.AccountDescription).IsUnicode(false);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.InvoiceNumber).IsUnicode(false);

                entity.HasOne(d => d.Terms)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.TermsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("invoices_fk_terms");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Invoices)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("invoices_fk_vendors");
            });

            modelBuilder.Entity<InvoiceLineItem>(entity =>
            {
                entity.HasKey(e => new { e.InvoiceId, e.InvoiceSequence })
                    .HasName("line_items_pk");

                entity.Property(e => e.LineItemDescription).IsUnicode(false);

                entity.HasOne(d => d.AccountNumberNavigation)
                    .WithMany(p => p.InvoiceLineItems)
                    .HasForeignKey(d => d.AccountNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("line_items_fk_acounts");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceLineItems)
                    .HasForeignKey(d => d.InvoiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("line_items_fk_invoices");
            });

            modelBuilder.Entity<Term>(entity =>
            {
                entity.HasKey(e => e.TermsId)
                    .HasName("PK__terms__10DEB556E549B079");

                entity.Property(e => e.TermsId).ValueGeneratedNever();

                entity.Property(e => e.TermsDescription).IsUnicode(false);
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.Property(e => e.VendorAddress1).IsUnicode(false);

                entity.Property(e => e.VendorAddress2).IsUnicode(false);

                entity.Property(e => e.VendorCity).IsUnicode(false);

                entity.Property(e => e.VendorContactEmail).IsUnicode(false);

                entity.Property(e => e.VendorContactFirstName).IsUnicode(false);

                entity.Property(e => e.VendorContactLastName).IsUnicode(false);

                entity.Property(e => e.VendorName).IsUnicode(false);

                entity.Property(e => e.VendorPhone).IsUnicode(false);

                entity.Property(e => e.VendorState)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.VendorZipCode).IsUnicode(false);

                entity.HasOne(d => d.DefaultAccountNumberNavigation)
                    .WithMany(p => p.Vendors)
                    .HasForeignKey(d => d.DefaultAccountNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vendors_fk_accounts");

                entity.HasOne(d => d.DefaultTerms)
                    .WithMany(p => p.Vendors)
                    .HasForeignKey(d => d.DefaultTermsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vendors_fk_terms");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
