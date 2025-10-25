using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Coti_Bianca_Lab2.Data;
using Coti_Bianca_Lab2.Models;

namespace Coti_Bianca_Lab2.Pages.Books
{
    public class CreateModel : BookCategoriesPageModel
    {
        private readonly Coti_Bianca_Lab2.Data.Coti_Bianca_Lab2Context _context;

        public CreateModel(Coti_Bianca_Lab2.Data.Coti_Bianca_Lab2Context context)
        {
            _context = context;
        }

        // 🔹 Proprietate legată de formular
        [BindProperty]
        public Book Book { get; set; } = default!;

        public IActionResult OnGet()
        {
            // Populează dropdown-uri
            ViewData["PublisherID"] = new SelectList(_context.Set<Publisher>(), "ID", "PublisherName");
            ViewData["AuthorID"] = new SelectList(_context.Author, "ID", "LastName");

            // 🔹 Creează instanță de Book cu lista de categorii goală
            var book = new Book
            {
                BookCategories = new List<BookCategory>()
            };

            // 🔹 Populează AssignedCategoryDataList pentru checkbox-uri
            PopulateAssignedCategoryData(_context, book);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string[] selectedCategories)
        {
            var newBook = new Book();

            // 🔹 Asociază categoriile bifate
            if (selectedCategories != null)
            {
                newBook.BookCategories = new List<BookCategory>();
                foreach (var cat in selectedCategories)
                {
                    newBook.BookCategories.Add(new BookCategory
                    {
                        CategoryID = int.Parse(cat)
                    });
                }
            }

            Book.BookCategories = newBook.BookCategories;

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

