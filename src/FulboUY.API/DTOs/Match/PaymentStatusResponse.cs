namespace FulboUY.API.DTOs.Match;

public class PaymentStatusResponse
{
    public Guid ParticipantId { get; set; }
    public required string PlayerName { get; set; }
    public bool HasPaid { get; set; }
}
