namespace DLQService.API.DTOs
{
    public class DlqMessageDto
    {
        public ulong DeliveryTag { get; set; }

        public string Payload { get; set; }

        public DateTime RetrievedAt { get; set; }
    }
}
