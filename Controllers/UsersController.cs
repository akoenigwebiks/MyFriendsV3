using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyFriendsV3.Data;
using MyFriendsV3.Models;
using MyFriendsV3.Services;
using MyFriendsV3.ViewModels;

namespace MyFriendsV3.Controllers
{
    public class UsersController : Controller
    {
        private readonly MyFriendsV3Context _context;

        public UsersController(MyFriendsV3Context context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var myFriendsV3Context = _context.User.Include(u => u.ProfilePicture);
            return View(await myFriendsV3Context.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View(new AddUserViewModel());
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel formInputModel)
        {
            IFormFile picture = formInputModel.ProfileImage;
            User userInput = formInputModel.User;

            if (ModelState.IsValid && FileService.IsValidImageFile(picture))
            {
                _context.User.Add(userInput);
                await _context.SaveChangesAsync();

                var userProfilePic = new Picture
                {
                    PictureName = $"Profile_{userInput.FirstName}_{userInput.LastName}",
                    PictureFile = FileService.ConvertToByteArray(picture),
                    UserId = userInput.Id
                };

                _context.Picture.Add(userProfilePic);
                await _context.SaveChangesAsync();

                userInput.ProfilePictureId = userProfilePic.Id;
                _context.Update(userInput);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // Return the same view with the model if validation fails
            return View(formInputModel);
        }
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["ProfilePictureId"] = new SelectList(_context.Picture, "Id", "Id", user.ProfilePictureId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Phone,ProfilePictureId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            ViewData["ProfilePictureId"] = new SelectList(_context.Picture, "Id", "Id", user.ProfilePictureId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.ProfilePicture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
