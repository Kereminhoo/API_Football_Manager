using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using FootManager.Models;   
using FootManager.Services; 
using System.Text; 

namespace FootManager.Pages;

public class IndexModel : PageModel {
    private readonly JoueurService _joueurService;
    
    public List<Joueur> Joueurs { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? PosteFilter { get; set; }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    
    public int TotalPages { get; set; }
    public const int PageSize = 10; 

    public IndexModel(JoueurService joueurService) {
        _joueurService = joueurService; 
    }
    
    public void OnGet() {
        if (CurrentPage < 1) CurrentPage = 1;

        Joueurs = _joueurService.GetFiltered(SearchTerm, PosteFilter, CurrentPage, PageSize, out int totalCount); 

        TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);
        if (TotalPages < 1) TotalPages = 1;
    }
    
    [Authorize(Roles = "Admin")]
    public IActionResult OnPostDelete(int id) {
        _joueurService.Delete(id);
        return RedirectToPage(); 
    }

    
    public IActionResult OnPostExportCsv()
    {
        
        var tousLesJoueurs = _joueurService.GetAll();

        var csvBuilder = new StringBuilder();
        
        
        csvBuilder.AppendLine("ID;Prénom;Nom;Poste;Numéro de Maillot");

        foreach (var j in tousLesJoueurs)
        {
            csvBuilder.AppendLine($"{j.Id};{j.Prenom};{j.Nom};{j.Poste};{j.Numero}");
        }

        
        var preamble = Encoding.UTF8.GetPreamble();
        var data = Encoding.UTF8.GetBytes(csvBuilder.ToString());
        var fileBytes = preamble.Concat(data).ToArray();

        string nomFichier = $"effectif_galatasaray_{DateTime.Now:yyyyMMdd}.csv";

        
        return File(fileBytes, "text/csv", nomFichier);
    }
}