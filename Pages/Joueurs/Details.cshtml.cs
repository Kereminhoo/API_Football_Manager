using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FootManager.Models;
using FootManager.Services;

namespace FootManager.Pages.Joueurs;

public class Details : PageModel
{
    private readonly JoueurService _joueurService;

    public Joueur Joueur { get; set; } = default!; //valeur joueur choisi ( pour le voir en detail)

    public Details(JoueurService joueurService) // pour service
    {
        _joueurService = joueurService;
    }

    public IActionResult OnGet(int id) // recup id
    {
        var j = _joueurService.GetById(id); //recup info via id
        if (j == null)
        {
            return RedirectToPage("/Joueurs/Index");
        }

        Joueur = j;
        return Page();
    }
}