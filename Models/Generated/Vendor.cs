using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace PROG2230_AS4_BH6010.Models.Generated
{
    [Table("vendors")]
    [Index(nameof(VendorName), Name = "UQ__vendors__6107BA717CA343BA", IsUnique = true)]
    public partial class Vendor
    {
        public Vendor()
        {
            Invoices = new HashSet<Invoice>();
        }

        [Key]
        [Column("vendor_id")]
        public int VendorId { get; set; }
        [Required]
        [Column("vendor_name")]
        [StringLength(256)]
        public string VendorName { get; set; }
        [Column("vendor_address1")]
        [StringLength(128)]
        public string VendorAddress1 { get; set; }
        [Column("vendor_address2")]
        [StringLength(128)]
        public string VendorAddress2 { get; set; }
        [Required]
        [Column("vendor_city")]
        [StringLength(64)]
        public string VendorCity { get; set; }
        [Required]
        [Column("vendor_state")]
        [StringLength(2)]
        public string VendorState { get; set; }
        [Required]
        [Column("vendor_zip_code")]
        [StringLength(20)]
        public string VendorZipCode { get; set; }
        [Column("vendor_phone")]
        [StringLength(50)]
        public string VendorPhone { get; set; }
        [Column("vendor_contact_last_name")]
        [StringLength(128)]
        public string VendorContactLastName { get; set; }
        [Column("vendor_contact_first_name")]
        [StringLength(128)]
        public string VendorContactFirstName { get; set; }
        [Column("vendor_contact_email")]
        [StringLength(128)]
        public string VendorContactEmail { get; set; }
        [Column("default_terms_id")]
        public int DefaultTermsId { get; set; }
        [Column("default_account_number")]
        public int DefaultAccountNumber { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(DefaultAccountNumber))]
        [InverseProperty(nameof(GeneralLedgerAccount.Vendors))]
        public virtual GeneralLedgerAccount DefaultAccountNumberNavigation { get; set; }
        [ForeignKey(nameof(DefaultTermsId))]
        [InverseProperty(nameof(Term.Vendors))]
        public virtual Term DefaultTerms { get; set; }
        [InverseProperty(nameof(Invoice.Vendor))]
        public virtual ICollection<Invoice> Invoices { get; set; }
    }
}
