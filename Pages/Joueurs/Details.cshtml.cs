using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FootManager.Models;
using FootManager.Services;

namespace FootManager.Pages.Joueurs;

public class Details : PageModel
{
    private readonly JoueurService _joueurService;

    public Joueur Joueur { get; set; } = default!;

    public Details(JoueurService joueurService)
    {
        _joueurService = joueurService;
    }

    public IActionResult OnGet(int id)
    {
        var j = _joueurService.GetById(id);
        if (j == null)
        {
            return RedirectToPage("/Index");
        }

        Joueur = j;
        return Page();
    }
}