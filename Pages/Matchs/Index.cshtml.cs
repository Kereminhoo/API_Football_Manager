using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.RazorPages; 
using Microsoft.AspNetCore.Authorization;
using FootManager.Models;                
using FootManager.Services;               

namespace FootManager.Pages.Matchs;

public class IndexModel : PageModel
{
    private readonly MatchService _matchService;
    
    
    public List<Match> MatchsAvenir { get; set; } = new();
    public List<Match> MatchsJoues { get; set; } = new();

    
    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public int TotalPages { get; set; }
    public const int PageSize = 9; 
    
    public IndexModel(MatchService matchService)
    {
        _matchService = matchService;
    }

    // onglet match
    public void OnGet()
    {
        if (CurrentPage < 1) CurrentPage = 1;
        
        // recup depuis bd via ado.net
        var tousLesMatchs = _matchService.GetFiltered(SearchTerm, CurrentPage, PageSize, out int totalCount);
        
        // compare date avec maintenant
        DateTime maintenant = DateTime.Now;
        
        // trie match a venir du plus proche vers le plus loin 
        MatchsAvenir = tousLesMatchs.Where(m => m.DateMatch > maintenant).OrderBy(m => m.DateMatch).ToList();
        
        // tire match passee du plus proche au plus ancien
        MatchsJoues = tousLesMatchs.Where(m => m.DateMatch <= maintenant).OrderByDescending(m => m.DateMatch).ToList();

        
        TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);
        if (TotalPages < 1) TotalPages = 1;
    }
    
    
    [Authorize(Roles = "Admin")]
    public IActionResult OnPostDelete(int id)
    {
        _matchService.Delete(id);
        return RedirectToPage(); 
    }
}