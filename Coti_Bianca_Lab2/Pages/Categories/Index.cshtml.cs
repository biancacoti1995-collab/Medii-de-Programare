using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coti_Bianca_Lab2.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly Coti_Bianca_Lab2.Data.Coti_Bianca_Lab2Context _context;

        public IndexModel(Coti_Bianca_Lab2.Data.Coti_Bianca_Lab2Context context)
        {
            _context = context;
        }

        public IList<Category> Category { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Category = await _context.Category.ToListAsync();
        }
    }
}
