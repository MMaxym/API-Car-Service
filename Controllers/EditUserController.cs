using API.Models;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
// [Authorize]
public class EditUserController : ControllerBase
{
    private readonly IUserService _userService;

    public EditUserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut("editUser/{id}")]
    public async Task<IActionResult> EditUser(string id, [FromBody] EditUserModel model)
    {
        string name = model.Name;
        string surname = model.Surname;
        string email = model.Email;
        string phoneNumber = model.PhoneNumber;

        var result = await _userService.UpdateUser(id, name, surname, email, phoneNumber);

        if (result.Succeeded)
        {
            return Ok(new { status = 200, message = "Інформацію про користувача успішно оновлено!"});
        }
        return BadRequest("Не вдалося оновити інформацію про користувача.");
    }
    
    [HttpGet("getAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Помилка сервера: {ex.Message}");
        }
    }
}