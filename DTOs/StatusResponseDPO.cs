namespace WebApplication1.DTOs
{
    public class StatusResponseDto
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public string Color { get; set; }
        public bool CanScan { get; set; }
    }

    public class UpdateStatusDto
    {
        public string Number { get; set; }
        public string OperatorTabnom { get; set; }
    }
}
//