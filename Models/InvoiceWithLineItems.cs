using PROG2230_AS4_BH6010.Models.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PROG2230_AS4_BH6010.Models
{
    public class InvoiceWithLineItems
    {
        public Vendor Vendor { get; set; }
        public List<Invoice> Invoices { get; set; }
        public List<InvoiceLineItem> InvoiceLineItems { get; set; }
        public InvoiceLineItem NewLineItem { get; set; }

        public InvoiceWithLineItems(Vendor vendor, List<Invoice> invoices, List<InvoiceLineItem> invoiceLineItems, InvoiceLineItem newLineItem)
        {
            this.Vendor = vendor;
            this.Invoices = invoices;
            this.InvoiceLineItems = invoiceLineItems;
            this.NewLineItem = newLineItem;
        }
    }
}
