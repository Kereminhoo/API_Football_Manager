using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; 
using FootManager.Models;
using FootManager.Services;

namespace FootManager.Pages.Joueurs;

public class CreateModel : PageModel
{
    private readonly JoueurService _joueurService;
    private readonly EquipeService _equipeService;

    [BindProperty]
    public Joueur NouveauJoueur { get; set; } = new();
    
    public SelectList ListeEquipes { get; set; }

    public CreateModel(JoueurService js, EquipeService es)
    {
        _joueurService = js;
        _equipeService = es;
    }

    public void OnGet()
    {
        
        var equipes = _equipeService.GetAll();
        ListeEquipes = new SelectList(equipes, "Id", "Nom");
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) 
        {
            OnGet();
            return Page();
        }

        _joueurService.Add(NouveauJoueur);
        return RedirectToPage("/Index");
    }
}