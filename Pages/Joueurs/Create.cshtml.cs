using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.AspNetCore.Authorization; 
using FootManager.Models;
using FootManager.Services;

namespace FootManager.Pages.Joueurs;

[Authorize(Roles = "Admin")] // verif acces admin 
public class CreateModel : PageModel
{
    private readonly JoueurService _joueurService;
    private readonly EquipeService _equipeService;

    [BindProperty]
    public Joueur NouveauJoueur { get; set; } = new();
    
    public SelectList ListeEquipes { get; set; } 

    // injection des dependances pour le service
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
            return Page(); // si erreur on insert pas, retour a la page
        }

        _joueurService.Add(NouveauJoueur);
        return RedirectToPage("/Joueurs/Index"); // si ok, on insert a la bd et on reviens a la pge d'effectif
    }
}