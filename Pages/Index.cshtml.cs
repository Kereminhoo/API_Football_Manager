using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FootManager.Pages;

public class IndexModel : PageModel
{
    
    public void OnGet()
    {
        
    }

    // mode invitee
    public async Task<IActionResult> OnPostGuestLoginAsync()
    {
        // definir id et role 
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, "Invité"),
            new Claim(ClaimTypes.Role, "User") 
        };

        // la carte id du user, on regroupe les donne et les cookies.
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // cookies creer et envoiyer au navigateur 
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        
        return RedirectToPage("/Joueurs/Index");
    }
}