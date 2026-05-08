namespace FootManager.Models;

using System.ComponentModel.DataAnnotations;

public class Joueur {
    public int Id { get; set; }
    [Required(ErrorMessage = "Le nom est requis")]
    [StringLength(100, MinimumLength = 2)]
    public string Nom { get; set; } = "";
    [Required]
    public string Prenom { get; set; } = "";
    [Required]
    public string Poste { get; set; } = "";
    [Range(1, 99)]
    public int Numero { get; set; }
    public int EquipeId { get; set; }
}