using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FootManager.Services;
using Npgsql; 

namespace FootManager.Pages;

public class LoginModel : PageModel
{
    private readonly UserService _userService;

    
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    
    [BindProperty]
    public string Password { get; set; } = string.Empty;

    
    public string ErrorMessage { get; set; } = string.Empty;

    
    public LoginModel(UserService userService)
    {
        _userService = userService;
    }

    // login
    public void OnGet()
    {
        
        var connection = HttpContext.RequestServices.GetRequiredService<NpgsqlConnection>();
        if (connection.State != System.Data.ConnectionState.Open) connection.Open();

        
        using var cmdCheck = connection.CreateCommand();
        cmdCheck.CommandText = "SELECT COUNT(*) FROM users WHERE email = 'admin@galatasaray.com'";
        long count = Convert.ToInt64(cmdCheck.ExecuteScalar());

        
        if (count == 0)
        {
            
            string perfectHash = _userService.HashPassword("admin@galatasaray.com", "Cimbom2026!");

            
            using var cmdInsert = connection.CreateCommand();
            cmdInsert.CommandText = "INSERT INTO users (email, password_hash, role) VALUES ('admin@galatasaray.com', @hash, 'Admin')";
            cmdInsert.Parameters.AddWithValue("hash", perfectHash);
            cmdInsert.ExecuteNonQuery();
        }
    }

    // page connexion
    public async Task<IActionResult> OnPostAsync()
    {
        
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Veuillez remplir tous les champs.";
            return Page();
        }

        // verifier user
        var user = _userService.ValidateUser(Email, Password);

        
        if (user != null)
        {
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role) 
            };

            // creation de la carte id
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            // creation des cookies et envoie au navigateur
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            
            return RedirectToPage("/Index");
        }

        
        ErrorMessage = "Email ou mot de passe incorrect.";
        return Page();
    }

    // deco
    public async Task<IActionResult> OnPostLogoutAsync()
    {
        // Supp les cookies 
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
        return RedirectToPage("/Index");
    }
}