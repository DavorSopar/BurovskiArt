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
    public class BiographyDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BiographyDashboardController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /BiographyDashboard
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_context.Biography == null)
            {
                return NotFound();
            }

            return View(await _context.Biography!.ToListAsync());
        }

        // GET: /BiographyDashboard/AddBiography
        [HttpGet]
        public IActionResult AddBiography()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBiography([Bind("BiographyContentMK,BiographyContentEN,ImageFile")] Biography biography)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(biography);
                biography.ImageUrl = uniqueFileName;
                _context.Attach(biography);
                _context.Entry(biography).State = EntityState.Added;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(biography);
        }

        // GET: /BiographyDashboard/UpdateBiography/{id}
        [HttpGet]
        public async Task<IActionResult> UpdateBiography(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var biography = await _context.Biography!.FirstOrDefaultAsync(b => b.Id == id);

            if (biography == null)
            {
                return NotFound();
            }

            return View(biography);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBiography(long? id, [Bind("Id,BiographyContentMK,BiographyContentEN,ImageFile")] Biography biography)
        {
            if (id != biography.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadedFile(biography);
                    biography.ImageUrl = uniqueFileName;
                    _context.Attach(biography);
                    _context.Entry(biography).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    var currentImage = await _context.Images!.FirstOrDefaultAsync(x => x.Id == biography.Id);
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
            return View(biography);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBiography(long? id)
        {
            var biography = await _context.Biography!.FirstOrDefaultAsync(x => x.Id == id);
            if (biography != null)
            {
                _context.Entry(biography).State = EntityState.Deleted;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Method: Upload file to DB
        private string UploadedFile(Biography biography)
        {
            string uniqueFileName = null!;

            if (biography.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = biography.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    biography.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}