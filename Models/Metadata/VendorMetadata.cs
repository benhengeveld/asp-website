using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PROG2230_AS4_BH6010.Models.Generated
{
    [ModelMetadataType(typeof(VendorMetadata))]
    public partial class Vendor { }

    public class VendorMetadata
    {
        [Required]
        public string VendorAddress1 { get; set; }

        [Remote(action: "IsStateValid", controller:"Vendors")]
        public string VendorState { get; set; }

        [RegularExpression(@"^\d{5}(?:[-\s]\d{4})?$", ErrorMessage = "Invalid Zip code")]
        public string VendorZipCode { get; set; }

        [Required]
        [Remote(action: "IsPhoneInUse", controller: "Vendors", AdditionalFields = "VendorId")]
        public string VendorPhone { get; set; }

        [Required]
        public int DefaultTermsId { get; set; }

        [Required]
        public int DefaultAccountNumber { get; set; }
    }
}
