using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StormingCompetition.Models;
using StormingCompetition.Validation;

namespace StormingCompetition.Pages;
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly DataContext _dataContext;
    private readonly IConfiguration _configuration;
    private void SetViewData(string? message = null, bool isLocked = false)
    {
        ViewData["Message"] = message;
        ViewData["IsLocked"] = isLocked;
    }

    public IndexModel(ILogger<IndexModel> logger, 
        DataContext dataContext, 
        IConfiguration configuration)
    {
        _logger = logger;
        _dataContext = dataContext;
        _configuration = configuration;
    }

    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            SetViewData(message: "Please enter username and password");
            ResetModel();
            return Page();
        }

        if (!UsernameValidator.IsValidUserName(Username))
        {
            SetViewData(message: "Username can only contains word characters (letters, digits, and underscores) and spaces");
            ResetModel(true);
            return Page();
        }

        var user = _dataContext.Users.FirstOrDefault(u => u.Username == Username);
        if (user is not null && user.IsLocked)
        {
            SetViewData("User is locked", true);
            ResetModel();
            return Page();
        }
        if (user == null)
        {
            user = new User
            {
                Username = Username,
            };

            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();

            var newUserLog = new UserLog
            {
                Log = $"User [{Username}] created [{DateTime.Now}]",
                LogTime = DateTime.Now,
                UserId = user.Id,
            };
            _dataContext.UsersLog.Add(newUserLog);
            _dataContext.SaveChanges(true);
        }

        AddLog(user.Id, Password);

        var isPasswordCorrect = Password.ToLowerInvariant() == _configuration["Password"];
        if (isPasswordCorrect)
        {
            user.Win = true;
            _dataContext.Users.Update(user);
            _dataContext.SaveChanges();
            return RedirectToPage("Congratulations");
        }
        else
        {
            user.WrongPasswordCount++;
            if (user.WrongPasswordCount >= 3)
                user.IsLocked = true;

            _dataContext.Users.Update(user);
            _dataContext.SaveChanges();
        }

        if (user.IsLocked)
            SetViewData("User locked", true);
        else
            SetViewData($"Password is incorrect, you used {user.WrongPasswordCount} of your 3 chances", false);
        ResetModel();
        return Page();
    }

    private void ResetModel(bool resetUsername = false)
    {
        if (resetUsername)
            Username = null;
        Password = null;
        ModelState.Clear();
    }

    private void AddLog(int userId, string password)
    {
       var log = new UserLog
       {
           Log = $"Entered Password {password}",
           LogTime = DateTime.Now,
           UserId = userId,
       };
       _dataContext.UsersLog.Add(log);
       _dataContext.SaveChanges();
    }
}
