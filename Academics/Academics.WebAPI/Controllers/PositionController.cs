using Academics.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Academics.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PositionController : ControllerBase
	{
		private readonly AppDbContext _db;

		public PositionController(AppDbContext db)
		{
			_db = db;
		}

		[HttpGet("getAllPosition")]
		[ProducesResponseType(typeof(List<Position>), 200)]
		public async Task<ActionResult<List<Position>>> getAllPosition()
		{
			return await _db.Positions.ToListAsync();
		}

		[HttpGet("getPostionById")]
		public async Task<ActionResult<Position>> GetPositionById([FromQuery] int id)
		{
			var position = await _db.Positions
									.FirstOrDefaultAsync(f => f.Id == id);

			if (position == null)
			{
				return NotFound(new { message = $"Должность с Id {id} не найдена" });
			}
			return Ok(position);
		}

		[HttpPost("createPosition")]
		public async Task<IActionResult> createPosition([FromBody] Position position)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			position.CreatedAt = DateTime.Now;
			position.CreatedBy = position.CreatedBy ?? "api";
			position.Teachers = null;

			try
			{
				_db.Positions.Add(position);
				await _db.SaveChangesAsync();
				return Ok(new { message = "Должность успешно добавлена", positionId = position.Id });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при добавлении должности: " + ex.Message });
			}
		}

		[HttpPut("updatePosition/{id:int}")]
		public async Task<IActionResult> updatePosition(int id, [FromBody] Position position)
		{
			if (id != position.Id)
			{
				return BadRequest(new { message = "Id из URL и Id из тела запроса не совпадают" });
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var existingPosition = await _db.Positions.FindAsync(id);

			if (existingPosition == null)
			{
				return NotFound(new { message = "Должность не найдена" });
			}

			existingPosition.Name = position.Name;
			existingPosition.Description = position.Description;

			try
			{
				await _db.SaveChangesAsync();
				return Ok(new { message = "Должность успешно обновлена" });
			}
			catch (DbUpdateConcurrencyException)
			{
				return Conflict(new { message = "Конфликт параллельного доступа при обновлении." });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при обновлении должности: " + ex.Message });
			}
		}

		[HttpPatch("updatePosition/{id:int}")]
		public async Task<IActionResult> PatchPosition(int id, [FromBody] PositionPatchDto patchData)
		{
			if (patchData == null)
			{
				return BadRequest(new { message = "Данные для обновления не предоставлены." });
			}

			try
			{
				var position = await _db.Positions.FirstOrDefaultAsync(p => p.Id == id);
				if (position == null)
				{
					return NotFound(new { message = "Должность не найдена" });
				}

				bool changed = false;

				if (patchData.Name != null)
				{
					position.Name = patchData.Name;
					changed = true;
				}
				if (patchData.Description != null)
				{
					position.Description = patchData.Description;
					changed = true;
				}

				if (!changed)
				{
					return Ok(new { message = "Изменений не было применено" });
				}

				await _db.SaveChangesAsync();
				return Ok(new { message = "Должность частично обновлена" });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при частичном обновлении: " + ex.Message });
			}
		}

		[HttpDelete("deletePosition")]
		public async Task<IActionResult> Delete([FromQuery] int Id)
		{
			try
			{
				var data = await _db.Positions
								   .Include(p => p.Teachers)
								   .FirstOrDefaultAsync(f => f.Id == Id);

				if (data == null)
				{
					return NotFound(new { message = "Должность не найдена" });
				}

				if (data.Teachers != null && data.Teachers.Any())
				{
					return BadRequest(new { message = "Нельзя удалить должность, так как она назначена одному или нескольким преподавателям." });
				}


				_db.Positions.Remove(data);
				await _db.SaveChangesAsync();
				return Ok(new { message = "Должность успешно удалена" });
			}
			catch (DbUpdateException dbEx)
			{
				return BadRequest(new { message = "Ошибка при удалении: Возможно, существуют связанные данные. " + (dbEx.InnerException?.Message ?? dbEx.Message) });
			}
			catch (Exception ex)
			{
				return BadRequest(new { message = "Ошибка при удалении должности: " + ex.Message });
			}
		}
	}
}