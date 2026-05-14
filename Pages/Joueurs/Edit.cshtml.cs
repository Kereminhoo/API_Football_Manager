using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; 
using FootManager.Models;
using FootManager.Services;

namespace FootManager.Pages.Joueurs;

public class Edit : PageModel
{
    private readonly JoueurService _joueurService;
    private readonly EquipeService _equipeService;

    [BindProperty]
    public Joueur Joueur { get; set; } = new();
    
    public SelectList ListeEquipes { get; set; }

    public Edit(JoueurService js, EquipeService es)
    {
        _joueurService = js;
        _equipeService = es;
    }

    public IActionResult OnGet(int id)
    {
        var j = _joueurService.GetById(id);
        if (j == null) return RedirectToPage("/Index");

        Joueur = j;
        var equipes = _equipeService.GetAll();
        ListeEquipes = new SelectList(equipes, "Id", "Nom");
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) 
        {
            var equipes = _equipeService.GetAll();
            ListeEquipes = new SelectList(equipes, "Id", "Nom");
            return Page();
        }

        _joueurService.Update(Joueur);
        return RedirectToPage("/Index");
    }
}