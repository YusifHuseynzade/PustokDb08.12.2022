using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokDb2022.Areas.Manage.ViewModels;
using PustokDb2022.DAL;
using PustokDb2022.Helpers;
using PustokDb2022.Models;

namespace PustokDb2022.Areas.Manage.Controllers
{
    [Area("manage")]
    public class BookController : Controller
    {
        private readonly PustokDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(PustokDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index(int page = 1)
        {
            var query = _context.Books
                .Include(x => x.Genres)
                .Include(x => x.Authors)
                .Include(x => x.BookImages);


            var model = PaginationList<Book>.Create(query, page, 5);

            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Authors = _context.Authors.ToList();


            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (!_context.Authors.Any(x => x.Id == book.AuthorId))
                ModelState.AddModelError("AuthorId", "Author not found");

            if (!_context.Genres.Any(x => x.Id == book.GenreId))
                ModelState.AddModelError("GenreId", "Genre not found");

            _checkImageFiles(book.ImageFiles, book.PosterFile, book.HoverPosterFile);

            if (!ModelState.IsValid)
            {
                ViewBag.Genres = _context.Genres.ToList();
                ViewBag.Authors = _context.Authors.ToList();

                return View();
            }

            book.BookImages = new List<BookImage>();

            BookImage poster = new BookImage
            {
                Image = FileManager.Save(book.PosterFile, _env.WebRootPath, "uploads/books"),
                PosterStatus = true,
            };

            book.BookImages.Add(poster);

            BookImage hoverPoster = new BookImage
            {
                Image = FileManager.Save(book.HoverPosterFile, _env.WebRootPath, "uploads/books"),
                PosterStatus = false
            };
            book.BookImages.Add(hoverPoster);


            foreach (var imgFile in book.ImageFiles)
            {
                BookImage bookImage = new BookImage
                {
                    Image = FileManager.Save(imgFile, _env.WebRootPath, "uploads/books"),
                };
                book.BookImages.Add(bookImage);
            }
            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        private void _checkImageFiles(List<IFormFile> images, IFormFile posterFile, IFormFile hoverPosterFile)
        {
            if (posterFile == null)
                ModelState.AddModelError("PosterFile", "PosterFile is required");
            else if (posterFile.ContentType != "image/png" && posterFile.ContentType != "image/jpeg")
                ModelState.AddModelError("PosterFile", "Content type must be image/png or image/jpeg!");
            else if (posterFile != null && posterFile.Length > 2097152)
                ModelState.AddModelError("PosterFile", "File size must be less than 2MB!");

            if (hoverPosterFile == null)
                ModelState.AddModelError("hoverPosterFile", "hoverPosterFile is required");
            else if (hoverPosterFile!=null && hoverPosterFile.ContentType != "image/png" && hoverPosterFile.ContentType != "image/jpeg")
                ModelState.AddModelError("HoverPosterFile", "Content type must be image/png or image/jpeg!");
            else if (hoverPosterFile!=null && hoverPosterFile.Length > 2097152)
                ModelState.AddModelError("HoverPosterFile", "File size must be less than 2MB!");

            if (images != null)
            {
                foreach (var imgFile in images)
                {
                    if (imgFile.ContentType != "image/png" && imgFile.ContentType != "image/jpeg")
                        ModelState.AddModelError("ImageFiles", "Content type must be image/png or image/jpeg!");

                    if (imgFile.Length > 2097152)
                        ModelState.AddModelError("ImageFiles", "File size must be less than 2MB!");
                }
            }
        }

        public IActionResult Edit(int id)
        {
            Book book = _context.Books.Include(x => x.BookImages).FirstOrDefault(x => x.Id == id);

            if (book == null)
                return RedirectToAction("error", "dashboard");
            ViewBag.Genres = _context.Genres.ToList();
            ViewBag.Authors = _context.Authors.ToList();

            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            return Ok(book);
        }
    }
}
