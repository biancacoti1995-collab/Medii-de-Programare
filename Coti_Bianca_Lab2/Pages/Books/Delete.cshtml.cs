using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Coti_Bianca_Lab2.Data;
using Coti_Bianca_Lab2.Models;

namespace Coti_Bianca_Lab2.Pages.Books
{
    public class DeleteModel : PageModel
    {
        private readonly Coti_Bianca_Lab2.Data.Coti_Bianca_Lab2Context _context;

        public DeleteModel(Coti_Bianca_Lab2.Data.Coti_Bianca_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FirstOrDefaultAsync(m => m.ID == id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                Book = book;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book != null)
            {
                Book = book;
                _context.Book.Remove(Book);
                await _context.SaveChangesAsync();
            }
#pragma warning disable CS8601 // Possible null reference assignment.
            Book = await _context.Book
               .Include(b => b.Publisher)
               .Include(b => b.Author)
               .FirstOrDefaultAsync(m => m.ID == id);
#pragma warning restore CS8601 // Possible null reference assignment.


            return RedirectToPage("./Index");
        }
    }
}
