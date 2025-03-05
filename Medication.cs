namespace EHRsafe_Task.Models
{
	public class Medication
	{
		public int Id { get; set; }
		public int UserId { get; set; }  
		public string Description { get; set; }
		public string Dosage { get; set; }  
		public string Frequency { get; set; } 
		public int Duration { get; set; }  
		public string Reason { get; set; }  
		public DateTime DateOfIssue { get; set; }
		public string Instructions { get; set; }

		public User User { get; set; }  
	}
}
