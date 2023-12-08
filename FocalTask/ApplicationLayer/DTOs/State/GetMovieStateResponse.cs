namespace FocalTask.ApplicationLayer.DTOs.State;

public class GetMovieStateResponse
{
    public int MovieId { get; set; }
    public string? Title { get; set; }
    public long? AverageWatchDurationS { get; set; }
    public int? Watches { get; set; }
    public int ReleaseYear { get; set; }
}