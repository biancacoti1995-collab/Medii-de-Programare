using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Coti_Bianca_Lab2.Data;
using Coti_Bianca_Lab2.Models;

namespace Coti_Bianca_Lab2.Pages
{
    public class EditModel : PageModel
    {
        private readonly Coti_Bianca_Lab2Context _context;

        public EditModel(Coti_Bianca_Lab2Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Author Author { get; set; } // Add this property

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedCategories)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookToUpdate = await _context.Book
                .Include(b => b.Publisher)
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .FirstOrDefaultAsync(b => b.ID == id);

            if (bookToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Book>(
                bookToUpdate,
                "Book",
                i => i.Title, i => i.Author,
                i => i.Price, i => i.PublishingDate, i => i.PublisherID))
            {
                UpdateBookCategories(_context, selectedCategories, bookToUpdate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            // Apelăm UpdateBookCategories pentru a aplica informațiile din checkboxuri
            // la entitatea Books care este editată
            UpdateBookCategories(_context, selectedCategories, bookToUpdate);
            PopulateAssignedCategoryData(_context, bookToUpdate);
            return Page();
        }
        private void UpdateBookCategories(Coti_Bianca_Lab2Context context, string[] selectedCategories, Book bookToUpdate)
        {
            if (selectedCategories == null)
            {
                bookToUpdate.BookCategories = new List<BookCategory>();
                return;
            }

            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var bookCategories = new HashSet<int>(
                bookToUpdate.BookCategories.Select(c => c.CategoryID));

            foreach (var category in context.Category)
            {
                if (selectedCategoriesHS.Contains(category.ID.ToString()))
                {
                    // dacă categoria e bifată și nu e deja asociată, o adăugăm
                    if (!bookCategories.Contains(category.ID))
                    {
                        bookToUpdate.BookCategories.Add(new BookCategory
                        {
                            BookID = bookToUpdate.ID,
                            CategoryID = category.ID
                        });
                    }
                }
                else
                {
                    // dacă categoria nu mai e bifată, o eliminăm
                    if (bookCategories.Contains(category.ID))
                    {
                        var categoryToRemove = bookToUpdate.BookCategories
                            .FirstOrDefault(i => i.CategoryID == category.ID);
                        context.Remove(categoryToRemove);
                    }
                }
            }
        }
        // Actualizează categoriile unei cărți în funcție de checkbox-urile bifate
        

        // Populează ViewData cu informații despre categoriile asignate cărții
        private void PopulateAssignedCategoryData(Coti_Bianca_Lab2Context context, Book book)
        {
            var allCategories = context.Category;
            var bookCategories = new HashSet<int>(book.BookCategories.Select(c => c.CategoryID));
            var viewModel = new List<AssignedCategoryData>();

            foreach (var category in allCategories)
            {
                viewModel.Add(new AssignedCategoryData
                {
                    CategoryID = category.ID,
                    CategoryName = category.CategoryName,
                    Assigned = bookCategories.Contains(category.ID)
                });
            }

            ViewData["Categories"] = viewModel;
        }

        public void OnGet(int id)
        {
            // TODO: Load Author from data source by id
            // Author = ...;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // TODO: Save changes to Author
            return RedirectToPage("./Index");
        }
    }
}
