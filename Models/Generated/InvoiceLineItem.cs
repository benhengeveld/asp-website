using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PROG2230_AS4_BH6010.Models.Generated
{
    [Table("invoice_line_items")]
    public partial class InvoiceLineItem
    {
        [Key]
        [Column("invoice_id")]
        public int InvoiceId { get; set; }
        [Key]
        [Column("invoice_sequence")]
        public int InvoiceSequence { get; set; }
        [Column("account_number")]
        public int AccountNumber { get; set; }
        [Column("line_item_amount", TypeName = "decimal(9, 2)")]
        public decimal LineItemAmount { get; set; }
        [Required]
        [Column("line_item_description")]
        [StringLength(100)]
        public string LineItemDescription { get; set; }

        [ForeignKey(nameof(AccountNumber))]
        [InverseProperty(nameof(GeneralLedgerAccount.InvoiceLineItems))]
        public virtual GeneralLedgerAccount AccountNumberNavigation { get; set; }
        [ForeignKey(nameof(InvoiceId))]
        [InverseProperty("InvoiceLineItems")]
        public virtual Invoice Invoice { get; set; }
    }
}
