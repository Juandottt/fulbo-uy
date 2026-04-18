namespace FulboUY.Application.DTOs.Match;

public class PaymentStatusDto
{
    public Guid ParticipantId { get; set; }
    public Guid PlayerProfileId { get; set; }
    public required string PlayerName { get; set; }
    public bool HasPaid { get; set; }
}
