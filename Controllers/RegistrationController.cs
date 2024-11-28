using API.Models;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegistrationController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly HttpClient _httpClient;

    public RegistrationController(IUserService userService, IHttpClientFactory httpClientFactory)
    {
        _userService = userService;
        _httpClient = httpClientFactory.CreateClient("Api");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        string username = model.Username;
        string password = model.Password;
        string role = "User";
        string name = model.Name;
        string surname = model.Surname;
        string email = model.Email;
        string phoneNumber = model.PhoneNumber;

        var (result, userId) = await _userService.RegisterUser(username, password, role, name, surname, email, phoneNumber);
        var content = new MultipartFormDataContent();

        if (result.Succeeded)
        {
            return Ok(new { status = 200, message = "Користувач успішно зареєстрований!"});
        }
        return BadRequest("Користувач уже існує.");
    }
}