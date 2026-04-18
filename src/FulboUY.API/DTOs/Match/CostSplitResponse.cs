namespace FulboUY.API.DTOs.Match;

public class CostSplitResponse
{
    public Guid MatchId { get; set; }
    public decimal TotalCost { get; set; }
    public int PlayerCount { get; set; }
    public decimal CostPerPlayer { get; set; }
    public int PaidCount { get; set; }
    public int PendingCount { get; set; }
}
