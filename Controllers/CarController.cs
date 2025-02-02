using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class CarController : ControllerBase
    {
        private readonly CarService _carService;

        public CarController(CarService carService)
        {
            _carService = carService;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Car>>> GetAllCars()
        {
            var cars = await _carService.GetAllCars();
            return Ok(cars);
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCarById(Guid id)
        {
            var car = await _carService.GetCarById(id);
            if (car == null)
                return NotFound(); 

            return Ok(car); 
        }

        [HttpPost]
        public async Task<ActionResult> CreateCar([FromBody] CreateCarDTO carDto) 
        {
            var result = await _carService.CreateCar(carDto);
            
            if (!result)
            {
                return BadRequest("Користувач з таким ID не існує."); 
            }
            
            return CreatedAtAction(
                nameof(GetCarById),
                new { id = carDto.UserId }, 
                new { message = "Автомобіль успішно створений!", car = carDto });
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCar(Guid id, [FromBody] CreateCarDTO carDto)
        {
            var result = await _carService.UpdateCar(id, carDto);

            if (!result)
                return NotFound("Автомобіль з таким ID не існує !!!"); 

            return Ok(new { message = "Автомобіль успішно оновлено !" });
        }
         
        [HttpPatch("{id}/changeOwner")]
        public async Task<ActionResult> ChangeCarOwner(Guid id, [FromForm] ChangeOwnerDTO changeOwnerDto)
        {
            var result = await _carService.ChangeCarOwner(id, changeOwnerDto.NewUserId);
    
            if (!result)
                return BadRequest("Не вдалося змінити власника автомобіля. Перевірте правильність ID автомобіля або користувача."); 

            return Ok(new { message = "Власник автомобіля успішно змінений!" });
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCar(Guid id)
        {
            var result = await _carService.DeleteCar(id);
            if (!result)
                return NotFound("Автомобіль з таким ID не існує !!!"); 

            return Ok(new { message = "Автомобіль успішно видалено !" });
        }
    }
}
