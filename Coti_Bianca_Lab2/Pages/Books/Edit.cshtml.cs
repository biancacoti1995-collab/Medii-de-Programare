using Coti_Bianca_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Coti_Bianca_Lab2.Pages.Books
{
    public class EditModel : BookCategoriesPageModel
    {
        private readonly Coti_Bianca_Lab2.Data.Coti_Bianca_Lab2Context _context;

        public EditModel(Coti_Bianca_Lab2.Data.Coti_Bianca_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            Book = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.Author)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (Book == null)
                return NotFound();

           
            PopulateAssignedCategoryData(_context, Book);

            ViewData["PublisherID"] = new SelectList(_context.Publisher, "ID", "PublisherName");
            ViewData["AuthorID"] = new SelectList(_context.Author, "ID", "LastName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
                return NotFound();

            var bookToUpdate = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                    .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(s => s.ID == id);

            if (bookToUpdate == null)
                return NotFound();

            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "Book",
                b => b.Title, b => b.AuthorID,
                b => b.Price, b => b.PublishingDate, b => b.PublisherID))
            {
                UpdateBookCategories(_context, selectedCategories, bookToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            PopulateAssignedCategoryData(_context, bookToUpdate);
            return Page();
        }
    }
}

