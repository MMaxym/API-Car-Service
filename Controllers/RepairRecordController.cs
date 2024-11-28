using API.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize]
    public class RepairRecordController : ControllerBase
    {
        private readonly RepairRecordService _repairRecordService;

        public RepairRecordController(RepairRecordService repairRecordService)
        {
            _repairRecordService = repairRecordService;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<RepairRecordDTO>>> GetAllRepairRecords()
        {
            var repairRecords = await _repairRecordService.GetAllRepairRecords();
            return Ok(repairRecords);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<RepairRecordDTO>> GetRepairRecordById(Guid id)
        {
            var repairRecord = await _repairRecordService.GetRepairRecordById(id);
            if (repairRecord == null)
                return NotFound("Запис про ремонт не знайдений.");

            return Ok(repairRecord);
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateRepairRecord([FromBody] CreateRepairRecordDTO repairRecordDto)
        {
            var result = await _repairRecordService.CreateRepairRecord(repairRecordDto);

            if (!result)
            {
                return BadRequest("Не вдалося створити запис про ремонт.");
            }

            return CreatedAtAction(nameof(GetRepairRecordById), new { id = repairRecordDto.CarId }, repairRecordDto);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRepairRecord(Guid id, [FromForm] CreateRepairRecordDTO repairRecordDto)
        {
            var result = await _repairRecordService.UpdateRepairRecord(id, repairRecordDto);

            if (!result)
                return NotFound("Запис про ремонт не знайдений.");

            return Ok(new { message = "Запис про ремонт успішно оновлено!" });
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRepairRecord(Guid id)
        {
            var result = await _repairRecordService.DeleteRepairRecord(id);
            if (!result)
                return NotFound("Запис про ремонт не знайдений.");

            return Ok(new { message = "Запис про ремонт успішно видалено!" });
        }
        
        [HttpPatch("{id}/change-status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromForm] ChangeStatusDTO changeStatusDto)
        {
            var result = await _repairRecordService.ChangeStatus(id, changeStatusDto);
            if (!result)
            {
                return NotFound(new { message = "Запис ремонту не знайдено" });
            }
            return Ok(new { message = "Статус успішно оновлено !" });
        }
        
        [HttpPatch("{id}/change-master")]
        public async Task<IActionResult> ChangeMaster(Guid id, [FromForm] ChangeMasterDTO changeMasterDto)
        {
            var result = await _repairRecordService.ChangeMaster(id, changeMasterDto);
            if (!result)
            {
                return NotFound(new { message = "Запис ремонту не знайдено" });
            }
            return Ok(new { message = "Майстра успішно оновлено !" });
        }
        
        [HttpPatch("{id}/edit-scheduled-date")]
        public async Task<IActionResult> EditScheduledDate(Guid id, [FromForm] EditScheduledDateDTO editScheduledDateDto)
        {
            var result = await _repairRecordService.EditScheduledDate(id, editScheduledDateDto);
            if (!result)
            {
                return NotFound(new { message = "Запис ремонту не знайдено" });
            }
            return Ok(new { message = "Заплановану дату ремонту успішно оновлено !" });
        }

    }
}
