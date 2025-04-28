namespace SPbSTU.OPD.ASAP.API.Domain.Models;

public record Assignment(
    long Id,
    string Title,
    string Description,
    int MaxPoints,
    DateTimeOffset DueTo,
    string Link);