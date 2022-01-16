using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PROG2230_AS4_BH6010.Models.Generated;

namespace PROG2230_AS4_BH6010.Controllers
{
    public class VendorsController : Controller
    {
        private readonly apContext _context;

        private List<string> validStates = new List<string>{ "AL", "AK", "AS", "AZ", "AR", "CA", "CO", "CT", "DE", "DC", "FM", "FL", "GA", "GU", "HI", "ID", "IL", "IN", "IA", "KS", "KY", "LA", "ME", "MH", "MD", "MA", "MI", "MN", "MS", "MO", "MT", "NE", "NV", "NH", "NJ", "NM", "NY", "NC", "ND", "MP", "OH", "OK", "OR", "PW", "PA", "PR", "RI", "SC", "SD", "TN", "TX", "UT", "VT", "VI", "VA", "WA", "WV", "WI", "WY" };
        private const string PHONE_NUMBER_PATTERN = @"^[^\d]*(\d{3})[^\d]*(\d{3})[^\d]*(\d{4})[^\d]*$";
        private const string NAME_FILTER_KEY = "name-filter";
        public static void SaveCurrentNameFilter(HttpContext httpContext, string nameFilter)
        {
            httpContext.Response.Cookies.Append(NAME_FILTER_KEY, nameFilter);
        }
        public static string GetCurrentNameFilter(HttpContext httpContext)
        {
            if (httpContext.Request.Cookies.ContainsKey(NAME_FILTER_KEY))
            {
                return httpContext.Request.Cookies[NAME_FILTER_KEY];
            }
            return "";
        }

        public VendorsController(apContext context)
        {
            _context = context;
        }

        // GET: Vendors
        public async Task<IActionResult> Index(string nameFilter)
        {
            IQueryable<Vendor> apContext = _context.Vendors.Include(v => v.DefaultAccountNumberNavigation).Include(v => v.DefaultTerms).OrderBy(row => row.VendorName).Where(row => row.IsDeleted == false);

            if (nameFilter == null || nameFilter == "")
            {
                nameFilter = GetCurrentNameFilter(HttpContext);
                if (nameFilter == null || nameFilter == "")
                    nameFilter = "aToE";
                
                return RedirectToAction(nameof(VendorsController.Index), "Vendors", new { nameFilter = nameFilter });
            }

            if (nameFilter == "aToE")
            {
                apContext = apContext.Where(row => row.VendorName.ToUpper().StartsWith("A") ||
                    row.VendorName.ToUpper().StartsWith("B") ||
                    row.VendorName.ToUpper().StartsWith("C") ||
                    row.VendorName.ToUpper().StartsWith("D") ||
                    row.VendorName.ToUpper().StartsWith("E"));
            }
            else if (nameFilter == "fToK")
            {
                apContext = apContext.Where(row => row.VendorName.ToUpper().StartsWith("F") ||
                    row.VendorName.ToUpper().StartsWith("G") ||
                    row.VendorName.ToUpper().StartsWith("H") ||
                    row.VendorName.ToUpper().StartsWith("I") ||
                    row.VendorName.ToUpper().StartsWith("J") ||
                    row.VendorName.ToUpper().StartsWith("K"));
            }
            else if (nameFilter == "lToR")
            {
                apContext = apContext.Where(row => row.VendorName.ToUpper().StartsWith("L") ||
                    row.VendorName.ToUpper().StartsWith("M") ||
                    row.VendorName.ToUpper().StartsWith("N") ||
                    row.VendorName.ToUpper().StartsWith("O") ||
                    row.VendorName.ToUpper().StartsWith("P") ||
                    row.VendorName.ToUpper().StartsWith("Q") ||
                    row.VendorName.ToUpper().StartsWith("R"));
            }
            else if (nameFilter == "sToZ")
            {
                apContext = apContext.Where(row => row.VendorName.ToUpper().StartsWith("S") ||
                    row.VendorName.ToUpper().StartsWith("T") ||
                    row.VendorName.ToUpper().StartsWith("U") ||
                    row.VendorName.ToUpper().StartsWith("V") ||
                    row.VendorName.ToUpper().StartsWith("W") ||
                    row.VendorName.ToUpper().StartsWith("X") ||
                    row.VendorName.ToUpper().StartsWith("Y") ||
                    row.VendorName.ToUpper().StartsWith("Z"));
            }

            SaveCurrentNameFilter(HttpContext, nameFilter);
            ViewBag.nameFilter = nameFilter;
            return View(await apContext.ToListAsync());
        }

        public async Task<IActionResult> IsPhoneInUse(string VendorPhone, int VendorId = -1)
        {
            if (!Regex.IsMatch(VendorPhone, PHONE_NUMBER_PATTERN))
                return Json("Invalid phone number format!");

            VendorPhone = Regex.Replace(VendorPhone, PHONE_NUMBER_PATTERN, "($1) $2-$3");
            Vendor vendorMatch = await _context.Vendors.FirstOrDefaultAsync(m => m.VendorPhone == VendorPhone);

            if (vendorMatch == null)
            {
                return Json(true);
            }
            else
            {
                if(VendorId == vendorMatch.VendorId)
                    return Json(true);
                else
                    return Json($"The phone number '{VendorPhone}' is already in use!");
            }
                
            
        }

        public IActionResult IsStateValid(string VendorState)
        {
            VendorState = VendorState.ToUpper();
            if (validStates.Contains(VendorState))
                return Json(true);
            else
                return Json($"The state '{VendorState}' is invalid");
        }

        // GET: Vendors/Create
        public IActionResult Create()
        {
            ViewData["DefaultAccountNumber"] = new SelectList(_context.GeneralLedgerAccounts, "AccountNumber", "AccountNumber");
            ViewData["DefaultTermsId"] = new SelectList(_context.Terms, "TermsId", "TermsDescription");
            return View();
        }

        // POST: Vendors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendorId,VendorName,VendorAddress1,VendorAddress2,VendorCity,VendorState,VendorZipCode,VendorPhone,VendorContactLastName,VendorContactFirstName,VendorContactEmail,DefaultTermsId,DefaultAccountNumber,IsDeleted")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                TempData["message"] = $"<span class='alert alert-success'>{vendor.VendorName} was successfully added!</span>";
                return RedirectToAction(nameof(Index));
            }
            ViewData["DefaultAccountNumber"] = new SelectList(_context.GeneralLedgerAccounts, "AccountNumber", "AccountNumber", vendor.DefaultAccountNumber);
            ViewData["DefaultTermsId"] = new SelectList(_context.Terms, "TermsId", "TermsDescription", vendor.DefaultTermsId);
            ViewData["message"] = $"<span class='alert alert-warning'>Failed to add vendor!</span>";
            return View(vendor);
        }

        // GET: Vendors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null)
            {
                return NotFound();
            }
            ViewData["DefaultAccountNumber"] = new SelectList(_context.GeneralLedgerAccounts, "AccountNumber", "AccountNumber", vendor.DefaultAccountNumber);
            ViewData["DefaultTermsId"] = new SelectList(_context.Terms, "TermsId", "TermsDescription", vendor.DefaultTermsId);
            return View(vendor);
        }

        // POST: Vendors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendorId,VendorName,VendorAddress1,VendorAddress2,VendorCity,VendorState,VendorZipCode,VendorPhone,VendorContactLastName,VendorContactFirstName,VendorContactEmail,DefaultTermsId,DefaultAccountNumber,IsDeleted")] Vendor vendor)
        {
            if (id != vendor.VendorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendorExists(vendor.VendorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["message"] = $"<span class='alert alert-success'>{vendor.VendorName} was successfully edited!</span>";
                return RedirectToAction(nameof(Index));
            }
            ViewData["DefaultAccountNumber"] = new SelectList(_context.GeneralLedgerAccounts, "AccountNumber", "AccountNumber", vendor.DefaultAccountNumber);
            ViewData["DefaultTermsId"] = new SelectList(_context.Terms, "TermsId", "TermsDescription", vendor.DefaultTermsId);
            ViewData["message"] = $"<span class='alert alert-warning'>Failed to edit vendor!</span>";
            return View(vendor);
        }

        public async Task<IActionResult> Delete(int? id, string nameFilter)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var vendor = await _context.Vendors.FindAsync(id);
                vendor.IsDeleted = true;
                _context.Vendors.Update(vendor);
                await _context.SaveChangesAsync();

                TempData["lastDeletedId"] = vendor.VendorId;
                TempData["lastDeletedName"] = vendor.VendorName;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists((int)id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index), "Vendors", new { nameFilter = nameFilter });
        }

        public async Task<IActionResult> UndoDelete(int? id, string nameFilter)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var vendor = await _context.Vendors.FindAsync(id);
                vendor.IsDeleted = false;
                _context.Vendors.Update(vendor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VendorExists((int)id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["message"] = $"<span class='alert alert-success'>Successfully undid delete!</span>";
            return RedirectToAction(nameof(Index), "Vendors", new { nameFilter = nameFilter });
        }

        private bool VendorExists(int id)
        {
            return _context.Vendors.Any(e => e.VendorId == id);
        }
    }
}
