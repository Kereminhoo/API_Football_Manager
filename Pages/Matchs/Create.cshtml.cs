using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization; 
using FootManager.Models;
using FootManager.Services;

namespace FootManager.Pages.Matchs;

[Authorize(Roles = "Admin")] 
public class CreateModel : PageModel
{
    private readonly MatchService _matchService;
    private readonly EquipeService _equipeService;

    [BindProperty]
    public Match NouveauMatch { get; set; } = new() { DateMatch = DateTime.Now };

    public SelectList ListeEquipes { get; set; }

    public CreateModel(MatchService ms, EquipeService es)
    {
        _matchService = ms;
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

        _matchService.Add(NouveauMatch);
        return RedirectToPage("/Matchs/Index");
    }
}