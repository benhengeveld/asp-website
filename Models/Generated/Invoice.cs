using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PROG2230_AS4_BH6010.Models.Generated
{
    [Table("invoices")]
    [Index(nameof(InvoiceDate), Name = "invoices_invoice_date_ix")]
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceLineItems = new HashSet<InvoiceLineItem>();
        }

        [Key]
        [Column("invoice_id")]
        public int InvoiceId { get; set; }
        [Column("vendor_id")]
        public int VendorId { get; set; }
        [Required]
        [Column("invoice_number")]
        [StringLength(50)]
        public string InvoiceNumber { get; set; }
        [Column("invoice_date", TypeName = "datetime")]
        public DateTime InvoiceDate { get; set; }
        [Column("invoice_total", TypeName = "decimal(9, 2)")]
        public decimal InvoiceTotal { get; set; }
        [Column("payment_total", TypeName = "decimal(9, 2)")]
        public decimal PaymentTotal { get; set; }
        [Column("credit_total", TypeName = "decimal(9, 2)")]
        public decimal CreditTotal { get; set; }
        [Column("terms_id")]
        public int TermsId { get; set; }
        [Column("invoice_due_date", TypeName = "datetime")]
        public DateTime InvoiceDueDate { get; set; }
        [Column("payment_date", TypeName = "datetime")]
        public DateTime? PaymentDate { get; set; }

        [ForeignKey(nameof(TermsId))]
        [InverseProperty(nameof(Term.Invoices))]
        public virtual Term Terms { get; set; }
        [ForeignKey(nameof(VendorId))]
        [InverseProperty("Invoices")]
        public virtual Vendor Vendor { get; set; }
        [InverseProperty(nameof(InvoiceLineItem.Invoice))]
        public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; }
    }
}
