using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokDb2022.DAL;
using PustokDb2022.Models;

namespace PustokDb2022.Controllers
{
    public class BookController : Controller
    {
        private readonly PustokDbContext _context;

        public BookController(PustokDbContext context)
        {
            _context = context;
        }

        public IActionResult GetBook(int id)
        {
            Book book = _context.Books.Include(x => x.Genres).Include(x => x.BookImages).FirstOrDefault(x => x.Id == id);

            return PartialView("_BookModalPartial", book);
        }
        public IActionResult Detail(int id)
        {
            Book book = _context.Books.Include(x => x.Genres).Include(x => x.BookImages).FirstOrDefault(x => x.Id == id);

            return View(book);
        }
    }
}
