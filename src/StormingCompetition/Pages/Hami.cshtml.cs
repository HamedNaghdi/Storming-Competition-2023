using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using StormingCompetition.Models;
using Microsoft.EntityFrameworkCore;

namespace StormingCompetition.Pages
{
    public class HamiModel : PageModel
    {
        #region Fields

        private readonly DataContext _dataContext;

        #endregion

        #region Ctor

        public HamiModel(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #endregion

        #region Properties

        [BindProperty]
        public string? Username { get; set; }
        [BindProperty]
        public string? Log { get; set; }

        public List<User> Users { get; set; }

        #endregion

        public void OnGet()
        {
            Users = _dataContext.Users.Include(u => u.UserLogs).ToList();
        }

        public IActionResult OnPost()
        {
            var user = _dataContext.Users
                .Include(u => u.UserLogs)
                .FirstOrDefault(u => u.Username == Username);
            if (user == null)
            {
                user = new User
                {
                    Username = Username ?? string.Empty,
                    WrongPasswordCount = 0,
                    IsLocked = false,
                };

                _dataContext.Users.Add(user);
                _dataContext.SaveChanges();
            }

            var log = new UserLog
            {
                Log = Log ?? string.Empty,
                UserId = user.Id,
                LogTime = DateTime.Now,
            };

            _dataContext.UsersLog.Add(log);
            _dataContext.SaveChanges(true);

            return RedirectToPage();
        }
    }
}
