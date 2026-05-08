using System.ComponentModel.DataAnnotations;

namespace FootManager.Models;

public class Match
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La date est obligatoire")]
    public DateTime DateMatch { get; set; }

    public int EquipeDomicileId { get; set; }
    public int EquipeExterieurId { get; set; }

    [Display(Name = "Score Domicile")]
    public int ScoreDomicile { get; set; }

    [Display(Name = "Score Extérieur")]
    public int ScoreExterieur { get; set; }
    
    public string NomEquipeDomicile { get; set; } = "";
    public string NomEquipeExterieur { get; set; } = "";
}