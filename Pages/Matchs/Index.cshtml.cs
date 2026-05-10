using Microsoft.AspNetCore.Mvc.RazorPages; 
using FootManager.Models;                
using FootManager.Services;               

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