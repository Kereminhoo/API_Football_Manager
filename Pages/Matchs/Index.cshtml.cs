using Microsoft.AspNetCore.Mvc.RazorPages; // Pour PageModel
using FootManager.Models;                 // Pour trouver la classe Match
using FootManager.Services;               // Pour trouver MatchService

namespace FootManager.Pages.Matchs;

public class IndexModel : PageModel
{
    private readonly MatchService _matchService;
    
    public List<Match> Matchs { get; set; } = new();

    public IndexModel(MatchService matchService)
    {
        _matchService = matchService;
    }

    public void OnGet()
    {
        Matchs = _matchService.GetAll();
    }
}