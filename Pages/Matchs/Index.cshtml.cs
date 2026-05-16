using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.RazorPages; 
using Microsoft.AspNetCore.Authorization;
using FootManager.Models;                
using FootManager.Services;               

namespace FootManager.Pages.Matchs;

public class IndexModel : PageModel
{
    private readonly MatchService _matchService;
    
    public List<Match> Matchs { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public int TotalPages { get; set; }
    public const int PageSize = 10;

    public IndexModel(MatchService matchService)
    {
        _matchService = matchService;
    }

    public void OnGet()
    {
        if (CurrentPage < 1) CurrentPage = 1;

        Matchs = _matchService.GetFiltered(SearchTerm, CurrentPage, PageSize, out int totalCount);

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