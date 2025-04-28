namespace SPbSTU.OPD.ASAP.Core.Domain.Models.Assignment;

public record Assignment(
    long Id,
    string Title,
    string Description,
    int MaxPoints,
    DateTime DueTo,
    string Link);