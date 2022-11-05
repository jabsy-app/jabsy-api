using Npgsql;
using System.Security.Cryptography;

public class UserDatabase {
    string cs = "Host=localhost;Username=postgres;Password='pwd';Database=jabsyuserdb";

    public User? GetUser(int id) {
        using var con = new NpgsqlConnection(cs);
        con.Open();
        
        string sql = $"SELECT * FROM users WHERE id = {id};";
        using var cmd = new NpgsqlCommand(sql, con);

        using NpgsqlDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            return new User {
                ID = rdr.GetInt32(0), 
                Name = rdr.GetString(1), 
                Status = rdr.GetString(2), 
                ProfilePicture = rdr.GetString(3), 
                Hero = rdr.GetString(4), 
                Email = rdr.GetString(5), 
                Password = rdr.GetString(6)
            };
        }
        
        return null;
    }

    public string? GetSalt(string email) {
        using var con = new NpgsqlConnection(cs);
        con.Open();
        
        string sql = $"SELECT password FROM users WHERE email = @email;";
        using var cmd = new NpgsqlCommand(sql, con);

        cmd.Parameters.AddWithValue("email", email);

        cmd.Prepare();
        using NpgsqlDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read()) {
            return rdr.GetString(0).Split(":")[0];
        }
        
        return null;
    }

    public User? GetUser(string email) {
        using var con = new NpgsqlConnection(cs);
        con.Open();
        
        string sql = $"SELECT * FROM users WHERE email = @email;";
        using var cmd = new NpgsqlCommand(sql, con);

        cmd.Parameters.AddWithValue("email", email);

        cmd.Prepare();
        using NpgsqlDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            return new User {
                ID = rdr.GetInt32(0), 
                Name = rdr.GetString(1), 
                Status = rdr.GetString(2), 
                ProfilePicture = rdr.GetString(3), 
                Hero = rdr.GetString(4), 
                Email = rdr.GetString(5), 
                Password = rdr.GetString(6)
            };
        }
        
        return null;
    }

    public void CreateUser(User user) {
        using var con = new NpgsqlConnection(cs);
        con.Open();
        var sql = "INSERT INTO users(name, status, profilepicture, hero, email, password, privatekey, publickey) VALUES(@name, @status, @profilepicture, @hero, @email, @password, @privatekey, @publickey);";
        using (var cmd = new NpgsqlCommand(sql, con)) {
            cmd.Parameters.AddWithValue("name", user.Name);
            cmd.Parameters.AddWithValue("status", user.Status);
            cmd.Parameters.AddWithValue("profilepicture", user.ProfilePicture);
            cmd.Parameters.AddWithValue("hero", user.Hero);
            cmd.Parameters.AddWithValue("email", user.Email);
            cmd.Parameters.AddWithValue("password", System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(user.Password)));
            
            cmd.Parameters.AddWithValue("privatekey", "");
            cmd.Parameters.AddWithValue("publickey", "");
            cmd.Prepare();

            cmd.ExecuteNonQuery();
        }

        sql = "SELECT * FROM users;";
        using (var cmd = new NpgsqlCommand(sql, con)) {
            using NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("{0} {1}", rdr.GetInt32(0), rdr.GetString(1));
            }
        }

        Logger.Log("User created");
    }
}

public class User {
    public int ID { get; set; } = 0; 
    public string Name { get; set; } = "";
    public string Status { get; set; } = "";
    public string ProfilePicture { get; set; } = "";
    public string Hero { get; set; } = "";
    public string Email { get; set; } = "";
    public string PublicKey { get; set; } = "";
    public string PrivateKey { get; set; } = "";
    public string Password { get; set; } = "";
}