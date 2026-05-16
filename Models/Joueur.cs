using System.ComponentModel.DataAnnotations;

namespace FootManager.Models;

public class Joueur {
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères.")]
    public string Nom { get; set; } = "";

    [Required(ErrorMessage = "Le prénom est obligatoire.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le prénom doit contenir entre 2 et 100 caractères.")]
    public string Prenom { get; set; } = "";

    [Required(ErrorMessage = "Le poste sur le terrain est obligatoire.")]
    public string Poste { get; set; } = "";

    [Required(ErrorMessage = "Le numéro de maillot est obligatoire.")]
    [Range(1, 99, ErrorMessage = "Le numéro de maillot doit être compris entre 1 et 99.")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "L'affiliation à une équipe est obligatoire.")]
    public int EquipeId { get; set; }
    
    public string? NomEquipe { get; set; }
    public string? VilleEquipe { get; set; }
}