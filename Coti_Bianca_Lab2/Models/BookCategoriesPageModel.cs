using Microsoft.AspNetCore.Mvc.RazorPages;
using Coti_Bianca_Lab2.Data;

namespace Coti_Bianca_Lab2.Models
{
    public class BookCategoriesPageModel : PageModel
    {
        public List<AssignedCategoryData> AssignedCategoryDataList { get; set; } = new();

        public void PopulateAssignedCategoryData(Coti_Bianca_Lab2Context context, Book book)
        {
            var allCategories = context.Category;
            var bookCategories = new HashSet<int>(book.BookCategories.Select(c => c.CategoryID));

            AssignedCategoryDataList = new List<AssignedCategoryData>();

            foreach (var cat in allCategories)
            {
                AssignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = cat.ID,
                    Name = cat.CategoryName,
                    Assigned = bookCategories.Contains(cat.ID)
                });
            }
        }

        public void UpdateBookCategories(Coti_Bianca_Lab2Context context, string[] selectedCategories, Book bookToUpdate)
        {
            if (selectedCategories == null)
            {
                bookToUpdate.BookCategories = new List<BookCategory>();
                return;
            }

            var selectedHS = new HashSet<string>(selectedCategories);
            var bookHS = new HashSet<int>(bookToUpdate.BookCategories.Select(c => c.Category.ID));

            foreach (var cat in context.Category)
            {
                if (selectedHS.Contains(cat.ID.ToString()))
                {
                    if (!bookHS.Contains(cat.ID))
                    {
                        bookToUpdate.BookCategories.Add(new BookCategory
                        {
                            BookID = bookToUpdate.ID,
                            CategoryID = cat.ID
                        });
                    }
                }
                else
                {
                    if (bookHS.Contains(cat.ID))
                    {
                        var bookCategoryToRemove = bookToUpdate.BookCategories
                            .SingleOrDefault(i => i.CategoryID == cat.ID);
                        if (bookCategoryToRemove != null)
                        {
                            context.Remove(bookCategoryToRemove);
                        }
                    }
                }
            }
        }
    }
}

