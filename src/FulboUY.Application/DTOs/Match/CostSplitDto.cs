namespace FulboUY.Application.DTOs.Match;

public class CostSplitDto
{
    public Guid MatchId { get; set; }
    public decimal TotalCost { get; set; }
    public int PlayerCount { get; set; }
    public decimal CostPerPlayer { get; set; }
    public int PaidCount { get; set; }
    public int PendingCount { get; set; }
}
