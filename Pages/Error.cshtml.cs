using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FootManager.Pages;

[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    public string? RequestId { get; set; }
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    
    public int StatusCode { get; set; }
    public string ErrorTitle { get; set; } = "Erreur Interne";
    public string ErrorMessage { get; set; } = "Une erreur imprévue est survenue sur le serveur de FootManager.";

    public void OnGet(int? statusCode)
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        
        if (statusCode.HasValue)
        {
            StatusCode = statusCode.Value;

            if (StatusCode == 404)
            {
                ErrorTitle = "Page ou Élément Introuvable (404)";
                ErrorMessage = "Le joueur recherché, le match ou l'URL saisie n'existe pas ou a été déplacé.";
            }
            else if (StatusCode == 403)
            {
                ErrorTitle = "Accès Refusé (403)";
                ErrorMessage = "Vous n'avez pas les privilèges administratifs requis pour accéder à cette zone.";
            }
        }
        else
        {
            
            StatusCode = 500;
        }
    }
}