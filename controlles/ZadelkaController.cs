using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.models;

namespace WebApplication1.Controllers // Исправлено: controlles -> Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZadelkaController : ControllerBase
    {
        private readonly ZadelkaContext _context;

        public ZadelkaController(ZadelkaContext context)
        {
            _context = context;
        }

        [HttpGet("status/{number}")]
        public async Task<ActionResult<StatusResponseDto>> GetStatus(string number)
        {
            if (string.IsNullOrEmpty(number) || number.Length != 10)
            {
                return BadRequest(new StatusResponseDto
                {
                    Message = "Неверный формат номера",
                    Status = "error",     


                    Color = "red",
                    CanScan = false
                });
            }

            var series1 = number.Substring(0, 2);
            var series2 = number.Substring(2, 2);
            var num = number.Substring(4);

            var record = await _context.ZadelkaRecords
                .FirstOrDefaultAsync(r => r.series1 == series1 &&
                                        r.series2 == series2 &&
                                        r.number == num);

            if (record != null)
            {
                if (!string.IsNullOrEmpty(record.checkedtabnom) && record.checkeddatetime.HasValue)
                {
                    var dateStr = record.checkeddatetime.Value.ToString("dd.MM.yyyy");
                    return Ok(new StatusResponseDto
                    {
                        Message = $"Заделан. Проверен ({dateStr})",
                        Status = "checked",
                        Color = "yellow",
                        CanScan = false
                    });
                }
                else
                {
                    return Ok(new StatusResponseDto
                    {
                        Message = "Направлен на заделку",
                        Status = "pending",
                        Color = "green",
                        CanScan = true
                    });
                }
            }
            else
            {
                return Ok(new StatusResponseDto
                {
                    Message = "На заделку не отправлялся",
                    Status = "not_sent",
                    Color = "red",
                    CanScan = false
                });
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult> UpdateStatus(UpdateStatusDto dto)
        {
            if (string.IsNullOrEmpty(dto.Number) || dto.Number.Length != 10 ||
                string.IsNullOrEmpty(dto.OperatorTabnom))
            {
                return BadRequest(new { error = "Неверные параметры" });
            }

            var series1 = dto.Number.Substring(0, 2);
            var series2 = dto.Number.Substring(2, 2);
            var num = dto.Number.Substring(4);

            var record = await _context.ZadelkaRecords
                .FirstOrDefaultAsync(r => r.series1 == series1 &&
                                        r.series2 == series2 &&
                                        r.number == num);

            if (record == null)
            {
                record = new ZadelkaRecords // Убедитесь, что имя класса правильное
                {
                    series1 = series1,
                    series2 = series2,
                    number = num,
                    checkedtabnom = dto.OperatorTabnom,
                    checkeddatetime = DateTime.Now
                };
                _context.ZadelkaRecords.Add(record);
            }
            else
            {
                record.checkedtabnom = dto.OperatorTabnom;
                record.checkeddatetime = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Статус обновлен" });
        }

        [HttpGet("test")]
        public async Task<ActionResult> TestConnection()
        {
            try
            {
                await _context.Database.CanConnectAsync();
                return Ok(new { success = true, message = "✅ Подключение успешно\nТаблица доступна" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"❌ Ошибка подключения к БД: {ex.Message}" });
            }
        }
    }
}