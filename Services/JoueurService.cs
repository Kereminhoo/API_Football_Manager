using Npgsql;
using FootManager.Models;
using System.Data; 

namespace FootManager.Services;

public class JoueurService {
    private readonly NpgsqlConnection _connection;

    public JoueurService(NpgsqlConnection connection) {
        _connection = connection; 
    }

    public List<Joueur> GetAll() {
        if (_connection.State != ConnectionState.Open) _connection.Open();
        var joueurs = new List<Joueur>();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "SELECT id, nom, prenom, poste, numero, equipe_id FROM joueurs ORDER BY id DESC";
        using var reader = cmd.ExecuteReader(); 
        while (reader.Read()) {
            joueurs.Add(new Joueur {
                Id = reader.GetInt32(0), Nom = reader.GetString(1), Prenom = reader.GetString(2),
                Poste = reader.GetString(3), Numero = reader.GetInt32(4), EquipeId = reader.GetInt32(5)
            });
        }
        return joueurs;
    }

    public List<Joueur> GetFiltered(string? search, string? poste, int page, int pageSize, out int totalCount) {
        if (_connection.State != ConnectionState.Open) _connection.Open();

        string searchPattern = string.IsNullOrEmpty(search) ? "%" : $"%{search}%";
        string posteParam = string.IsNullOrEmpty(poste) ? "" : poste;
        
        using (var cmdCount = _connection.CreateCommand()) {
            cmdCount.CommandText = @"
                SELECT COUNT(*) 
                FROM joueurs 
                WHERE (nom ILIKE @s OR prenom ILIKE @s) 
                  AND (@poste = '' OR poste = @poste)"; 
            
            cmdCount.Parameters.AddWithValue("s", searchPattern);
            cmdCount.Parameters.AddWithValue("poste", posteParam);
            
            totalCount = Convert.ToInt32(cmdCount.ExecuteScalar());
        }

        var joueurs = new List<Joueur>();
        int offset = (page - 1) * pageSize;

        using (var cmdData = _connection.CreateCommand()) {
            cmdData.CommandText = @"
                SELECT id, nom, prenom, poste, numero, equipe_id 
                FROM joueurs 
                WHERE (nom ILIKE @s OR prenom ILIKE @s) 
                  AND (@poste = '' OR poste = @poste)
                ORDER BY id DESC 
                LIMIT @limit OFFSET @offset";

            cmdData.Parameters.AddWithValue("s", searchPattern);
            cmdData.Parameters.AddWithValue("poste", posteParam);
            cmdData.Parameters.AddWithValue("limit", pageSize);
            cmdData.Parameters.AddWithValue("offset", offset);

            using var reader = cmdData.ExecuteReader(); 
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
        }
        return joueurs;
    }

    
    public Joueur? GetById(int id) {
        if (_connection.State != ConnectionState.Open) _connection.Open();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = @"
            SELECT j.id, j.nom, j.prenom, j.poste, j.numero, j.equipe_id, e.nom as nom_equipe, e.ville
            FROM joueurs j
            JOIN equipes e ON j.equipe_id = e.id
            WHERE j.id = @id";
            
        cmd.Parameters.AddWithValue("id", id);
        using var reader = cmd.ExecuteReader();
        if (reader.Read()) {
            return new Joueur {
                Id = reader.GetInt32(0),
                Nom = reader.GetString(1),
                Prenom = reader.GetString(2),
                Poste = reader.GetString(3),
                Numero = reader.GetInt32(4),
                EquipeId = reader.GetInt32(5),
                NomEquipe = reader.GetString(6),  
                VilleEquipe = reader.GetString(7) 
            };
        }
        return null;
    }

    public void Add(Joueur j) {
        if (_connection.State != ConnectionState.Open) _connection.Open();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "INSERT INTO joueurs (nom, prenom, poste, numero, equipe_id) VALUES (@n, @p, @pst, @num, @eid)";
        cmd.Parameters.AddWithValue("n", j.Nom);
        cmd.Parameters.AddWithValue("p", j.Prenom);
        cmd.Parameters.AddWithValue("pst", j.Poste);
        cmd.Parameters.AddWithValue("num", j.Numero);
        cmd.Parameters.AddWithValue("eid", j.EquipeId);
        cmd.ExecuteNonQuery();
    }

    public void Update(Joueur j) {
        if (_connection.State != ConnectionState.Open) _connection.Open();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "UPDATE joueurs SET nom=@n, prenom=@p, poste=@pst, numero=@num, equipe_id=@eid WHERE id=@id";
        cmd.Parameters.AddWithValue("n", j.Nom);
        cmd.Parameters.AddWithValue("p", j.Prenom);
        cmd.Parameters.AddWithValue("pst", j.Poste);
        cmd.Parameters.AddWithValue("num", j.Numero);
        cmd.Parameters.AddWithValue("eid", j.EquipeId);
        cmd.Parameters.AddWithValue("id", j.Id);
        cmd.ExecuteNonQuery();
    }
    
    public void Delete(int id) {
        if (_connection.State != ConnectionState.Open) _connection.Open();
        using var cmd = _connection.CreateCommand();
        cmd.CommandText = "DELETE FROM joueurs WHERE id = @id";
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }
}