using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using EHRsafe_Task.DTOs;
using System.Security.Claims;
using EHRsafe_Task.Models;


namespace EHRsafe_Task.Controllers
{
	[Authorize] 
	[Route("api/medications")]
	[ApiController]
	public class MedicationController : ControllerBase
	{
		private readonly AppDbContext _context;

		public MedicationController(AppDbContext context)
		{
			_context = context;
		}

		[HttpPost("add")]
		public async Task<IActionResult> AddMedication(MedicationDto request)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			var medication = new Medication
			{
				UserId = userId,
				Description = request.Description,
				Dosage = request.Dosage,
				Frequency = request.Frequency,
				Duration = request.Duration,
				Reason = request.Reason,
				DateOfIssue = request.DateOfIssue,
				Instructions = request.Instructions
			};

			_context.Medications.Add(medication);
			await _context.SaveChangesAsync();
			return Ok("Medication added successfully");
		}

		[HttpGet("list")]
		public async Task<IActionResult> GetMedications()
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			var medications = await _context.Medications
				.Where(m => m.UserId == userId)
				.ToListAsync();

			return Ok(medications);
		}

		[HttpPut("update/{id}")]
		public async Task<IActionResult> UpdateMedication(int id, MedicationDto request)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			var medication = await _context.Medications
				.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

			if (medication == null)
				return NotFound("Medication not found");

			medication.Description = request.Description;
			medication.Dosage = request.Dosage;
			medication.Frequency = request.Frequency;
			medication.Duration = request.Duration;
			medication.Reason = request.Reason;
			medication.DateOfIssue = request.DateOfIssue;
			medication.Instructions = request.Instructions;

			await _context.SaveChangesAsync();
			return Ok("Medication updated successfully");
		}

		[HttpDelete("delete/{id}")]
		public async Task<IActionResult> DeleteMedication(int id)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			var medication = await _context.Medications
				.FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

			if (medication == null)
				return NotFound("Medication not found");

			_context.Medications.Remove(medication);
			await _context.SaveChangesAsync();
			return Ok("Medication deleted successfully");
		}

		[HttpGet("filterByDate")]
		public async Task<IActionResult> FilterByDate([FromQuery] DateTime afterDate)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			var medications = await _context.Medications
				.Where(m => m.UserId == userId && m.DateOfIssue > afterDate)
				.ToListAsync();

			return Ok(medications);
		}

		[HttpGet("search")]
		public async Task<IActionResult> SearchByDescription([FromQuery] string query)
		{
			var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

			var medications = await _context.Medications
				.Where(m => m.UserId == userId && m.Description.Contains(query))
				.ToListAsync();

			return Ok(medications);
		}
	}
}
