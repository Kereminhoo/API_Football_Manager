using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization; 
using FootManager.Models;
using FootManager.Services;

namespace FootManager.Pages.Matchs;


[Authorize(Roles = "Admin")] 
public class Edit : PageModel
{
    private readonly MatchService _matchService;

    // lescture des donner du hmtl
    [BindProperty]
    public Match Match { get; set; } = new();

    
    public Edit(MatchService ms)
    {
        _matchService = ms;
    }

    
    public IActionResult OnGet(int id)
    {
        var m = _matchService.GetById(id);
        
        if (m == null) return RedirectToPage("/Matchs/Index");

        
        Match = m;
        return Page();
    }

    // enregistrer bouton
    public IActionResult OnPost()
    {
        // si pas ok, on affiche les erreurs
        if (!ModelState.IsValid) return Page();

        // ajout des donnees a la bd via ado.net
        _matchService.Update(Match);
        
        
        return RedirectToPage("/Matchs/Index");
    }
}