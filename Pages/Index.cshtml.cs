using Microsoft.AspNetCore.Mvc.RazorPages;
using FootManager.Models;   
using FootManager.Services; 

namespace FootManager.Pages;

public class IndexModel : PageModel {
    private readonly JoueurService _joueurService;
    
    
    public List<Joueur> Joueurs { get; set; } = new();

    
    public IndexModel(JoueurService joueurService) {
        _joueurService = joueurService; 
    }
    
    public void OnGet() {
        Joueurs = _joueurService.GetAll(); 
    }
}