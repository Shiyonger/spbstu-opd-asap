namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public record Assignment(
    long Id,
    string Title,
    string Description,
    int MaxPoints,
    DateTime DueTo,
    string Link);