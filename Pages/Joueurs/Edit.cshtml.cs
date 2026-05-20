using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; 
using Microsoft.AspNetCore.Authorization; 
using FootManager.Models;
using FootManager.Services;

namespace FootManager.Pages.Joueurs;


[Authorize(Roles = "Admin")] 
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
        
        if (j == null) return RedirectToPage("/Joueurs/Index");

        
        Joueur = j;
        
        // liste des equipes pour le choix
        var equipes = _equipeService.GetAll();
        ListeEquipes = new SelectList(equipes, "Id", "Nom");
        return Page();
    }

    
    public IActionResult OnPost()
    {
        
        if (!ModelState.IsValid) 
        {
            // si pas ok on recharge la page avec les erreurs
            var equipes = _equipeService.GetAll();
            ListeEquipes = new SelectList(equipes, "Id", "Nom");
            return Page();
        }

        // si tout ok, maj de la bd 
        _joueurService.Update(Joueur);
        return RedirectToPage("/Joueurs/Index");
    }
}