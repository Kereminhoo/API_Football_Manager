using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FootManager.Models;
using FootManager.Services;

namespace FootManager.Pages.Matchs;

public class Edit : PageModel
{
    private readonly MatchService _matchService;

    [BindProperty]
    public Match Match { get; set; } = new();

    public Edit(MatchService ms)
    {
        _matchService = ms;
    }

    public IActionResult OnGet(int id)
    {
        var m = _matchService.GetById(id);
        if (m == null) return RedirectToPage("/Matchs/Index");

        Match = m;
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        _matchService.Update(Match);
        return RedirectToPage("/Matchs/Index");
    }
}