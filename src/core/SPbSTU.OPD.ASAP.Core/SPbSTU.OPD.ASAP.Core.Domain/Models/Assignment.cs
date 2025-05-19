using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public class Assignment
{
    public long Id { get; init; }
    public string Title { get; init; } = null!;
    public string? Description { get; init; }
    public int? MaxPoints { get; init; }
    public DateTime? DueTo { get; init; }
    public string Link { get; init; } = null!;
    public string? SpreadSheetId { get; set; }
    public Position? Position { get; init; }

    public Assignment()
    {
    }

    public Assignment(long id, string title, string description, int maxPoints, DateTime dueTo, string link)
    {
        Id = id;
        Title = title;
        Description = description;
        MaxPoints = maxPoints;
        DueTo = dueTo;
        Link = link;
    }
}