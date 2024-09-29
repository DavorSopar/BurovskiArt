using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArtistPortfolio.Data;
using ArtistPortfolio.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtistPortfolio.Controllers
{
    [Authorize]
    //[Authorize(Roles = SD.Role_Admin)]
    public class ImageDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageDashboardController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /ImageDashboard
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_context.Images == null)
            {
                return NotFound();
            }

            return View(await _context.Images!.OrderByDescending(p => p.Id).ToListAsync());
        }

        // GET: /ImageDashboard/ImageDetail/{id}
        [HttpGet]
        public async Task<IActionResult> ImageDetail(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images!.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: /ImageDashboard/AddImage
        [HttpGet]
        public IActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage([Bind("TitleMK,TitleEN,DescMK,DescEN,TechniqueMK,TechniqueEN,Format,ImageFile,IsForSale")] Image image)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(image);
                image.ImageUrl = uniqueFileName;
                _context.Attach(image);
                _context.Entry(image).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(image);
        }

        // GET: /ImageDashboard/UpdateImage/{id}
        [HttpGet]
        public async Task<IActionResult> UpdateImage(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images!.FindAsync(id);

            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateImage(long? id, [Bind("Id,TitleMK,TitleEN,DescMK,DescEN,TechniqueMK,TechniqueEN,Format,ImageFile,IsForSale")] Image image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(image);
                    image.ImageUrl = uniqueFileName;
                    _context.Attach(image);
                    _context.Entry(image).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var currentImage = await _context.Images!.FirstOrDefaultAsync(x => x.Id == image.Id);
                    if (currentImage != null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(long? id)
        {
            var image = await _context.Images!.FirstOrDefaultAsync(x => x.Id == id);
            if (image != null)
            {
                _context.Entry(image).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Method: Upload file to DB
        private string UploadedFile(Image image)
        {
            string uniqueFileName = null!;

            if (image.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = image.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}