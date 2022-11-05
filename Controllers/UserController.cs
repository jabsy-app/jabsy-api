using Microsoft.AspNetCore.Mvc;
using System.Text.Json;


namespace jabsy_api.Controllers;

[ApiController]
[Route("User")]
public class UserController : ControllerBase
{
    public UserDatabase userDatabase = new UserDatabase();

    [HttpGet]
    public string? Get([FromHeader]int id)
    {
        var user = userDatabase.GetUser(id);

        if (user is not {}) {
            return null;
        }

        user.Email = "";
        user.Password = "";
        user.PrivateKey = "";

        var jsonString = JsonSerializer.Serialize(user);
        return jsonString;
    }

    [HttpGet("login")]
    public string? Login([FromHeader]string email, [FromHeader]string password) {
        var user = userDatabase.GetUser(email);

        if (user is not {} || (user is {} && user.Password != System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(password)))) {
            return null;
        }

        var jsonString = JsonSerializer.Serialize(user);
        return jsonString;
    }

    [HttpGet("exists")]
    public bool Exists([FromHeader]string email) {
        return userDatabase.GetUser(email) != null;
    }

    [HttpGet("salt")]
    public string? Salt([FromHeader]string email) {
        return userDatabase.GetSalt(email);
    }

    [HttpPost("new")]
    public void CreateUser([FromBody]User user) {
        userDatabase.CreateUser(user);
    }
}
