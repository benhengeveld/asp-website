using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PROG2230_AS4_BH6010.Models.Generated
{
    [Table("terms")]
    public partial class Term
    {
        public Term()
        {
            Invoices = new HashSet<Invoice>();
            Vendors = new HashSet<Vendor>();
        }

        [Key]
        [Column("terms_id")]
        public int TermsId { get; set; }
        [Required]
        [Column("terms_description")]
        [StringLength(50)]
        public string TermsDescription { get; set; }
        [Column("terms_due_days")]
        public int TermsDueDays { get; set; }

        [InverseProperty(nameof(Invoice.Terms))]
        public virtual ICollection<Invoice> Invoices { get; set; }
        [InverseProperty(nameof(Vendor.DefaultTerms))]
        public virtual ICollection<Vendor> Vendors { get; set; }
    }
}
