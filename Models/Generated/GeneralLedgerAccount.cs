using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PROG2230_AS4_BH6010.Models.Generated
{
    [Table("general_ledger_accounts")]
    [Index(nameof(AccountDescription), Name = "UQ__general___EEFBAF027D6DCEDB", IsUnique = true)]
    public partial class GeneralLedgerAccount
    {
        public GeneralLedgerAccount()
        {
            InvoiceLineItems = new HashSet<InvoiceLineItem>();
            Vendors = new HashSet<Vendor>();
        }

        [Key]
        [Column("account_number")]
        public int AccountNumber { get; set; }
        [Column("account_description")]
        [StringLength(50)]
        public string AccountDescription { get; set; }

        [InverseProperty(nameof(InvoiceLineItem.AccountNumberNavigation))]
        public virtual ICollection<InvoiceLineItem> InvoiceLineItems { get; set; }
        [InverseProperty(nameof(Vendor.DefaultAccountNumberNavigation))]
        public virtual ICollection<Vendor> Vendors { get; set; }
    }
}
