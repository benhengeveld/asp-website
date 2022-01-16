using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG2230_AS4_BH6010.Models;
using PROG2230_AS4_BH6010.Models.Generated;

namespace PROG2230_AS4_BH6010.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly apContext _context;

        public InvoicesController(apContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index(int id, int? invoiceId)
        {
            //Get the Invoices from the db that match the vendor id given
            IQueryable<Invoice> apContext = _context.Invoices.Include(i => i.Terms).Include(i => i.Terms).OrderBy(row => row.InvoiceNumber).Where(row => row.VendorId == id);
            IQueryable<InvoiceLineItem> apContextLineItems = null;
            InvoiceWithLineItems invoiceLineItem = null;

            //Get the return vendor name filter from the cookies
            string vendorNameFilter = VendorsController.GetCurrentNameFilter(HttpContext);
            if (vendorNameFilter == "aToE")
                ViewBag.vendorNameFilter = "A - E";
            else if (vendorNameFilter == "fToK")
                ViewBag.vendorNameFilter = "F - K";
            else if (vendorNameFilter == "lToR")
                ViewBag.vendorNameFilter = "L - R";
            else if (vendorNameFilter == "sToZ")
                ViewBag.vendorNameFilter = "S - Z";
            else
                ViewBag.vendorNameFilter = "A - E";


            //Get the selected vendor
            Vendor vendor = await _context.Vendors.FirstOrDefaultAsync(m => m.VendorId == id);
            //Get the default term of the vender
            Term term = await _context.Terms.FirstOrDefaultAsync(m => m.TermsId == vendor.DefaultTermsId);
            ViewBag.termsDays = term.TermsDueDays;

            //Check if a invoice is selected
            if (invoiceId != null)
            {
                //Get the line items that match the invoice id given
                apContextLineItems = _context.InvoiceLineItems.Include(i => i.Invoice).OrderBy(row => row.InvoiceSequence).Where(row => row.InvoiceId == invoiceId);
                ViewBag.selectedInvoiceId = invoiceId;

                //Turn the line items into a list
                var lineItemsList = await apContextLineItems.ToListAsync();

                //Get the total of all the line items
                decimal total = 0;
                foreach (InvoiceLineItem lineItem in lineItemsList)
                {
                    total += lineItem.LineItemAmount;
                }
                ViewBag.total = total;

                //Make a template that is used if the user adds another line item
                InvoiceLineItem newLineItem = new InvoiceLineItem
                {
                    InvoiceId = (int)invoiceId,
                    InvoiceSequence = lineItemsList[lineItemsList.Count - 1].InvoiceSequence + 1,
                    AccountNumber = vendor.DefaultAccountNumber
                };
                //Make a InvoiceWithLineItems. it has a vendor a list of invoices, a list of line items, and a new line item template
                invoiceLineItem = new InvoiceWithLineItems(vendor, await apContext.ToListAsync(), lineItemsList, newLineItem);
            }
            else
            {
                //If there is no invoice selected then just give the list of invoices and the vendor selected
                invoiceLineItem = new InvoiceWithLineItems(vendor, await apContext.ToListAsync(), null, null);
            }

            return View(invoiceLineItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLineItem([Bind("InvoiceId,InvoiceSequence,AccountNumber,LineItemAmount,LineItemDescription")] InvoiceLineItem invoiceLineItem, int vendorId)
        {
            if (ModelState.IsValid)
            {
                _context.Add(invoiceLineItem);
                await _context.SaveChangesAsync();
                TempData["message"] = $"<span class='alert alert-success'>Successfully added a new line item!</span>";
                return RedirectToAction(nameof(InvoicesController.Index), "Invoices", new { id = vendorId, invoiceId = invoiceLineItem.InvoiceId});
            }

            TempData["message"] = $"<span class='alert alert-warning'>Failed to add line item!</span>";
            return RedirectToAction(nameof(InvoicesController.Index), "Invoices", new { id = vendorId, invoiceId = invoiceLineItem.InvoiceId });
        }
    }
}
