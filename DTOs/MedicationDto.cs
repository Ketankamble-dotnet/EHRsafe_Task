namespace EHRsafe_Task.DTOs
{
	public class MedicationDto
	{
		public string Description { get; set; }
		public string Dosage { get; set; }
		public string Frequency { get; set; }
		public int Duration { get; set; }
		public string Reason { get; set; }
		public DateTime DateOfIssue { get; set; }
		public string Instructions { get; set; }
	}
}
