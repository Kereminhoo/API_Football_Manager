namespace FootManager.Services;

using Npgsql;
using FootManager.Models;
using System.Data; 

public class JoueurService {
    private readonly NpgsqlConnection _connection;

    public JoueurService(NpgsqlConnection connection) {
        _connection = connection; 
    }

    public List<Joueur> GetAll() {
        
        if (_connection.State != ConnectionState.Open) {
            _connection.Open();
        }

        var joueurs = new List<Joueur>();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT id, nom, prenom, poste, numero, equipe_id FROM joueurs";
        
        using var reader = cmd.ExecuteReader(); 
        while (reader.Read()) {
            joueurs.Add(new Joueur {
                Id = reader.GetInt32(0),
                Nom = reader.GetString(1),
                Prenom = reader.GetString(2),
                Poste = reader.GetString(3),
                Numero = reader.GetInt32(4),
                EquipeId = reader.GetInt32(5)
            });
        }
        return joueurs;
    }
}