namespace SPbSTU.OPD.ASAP.Google.Domain.Entities;

public record Course(long Id, string Title, IReadOnlyList<Student> Students, IReadOnlyList<Assignment> Assignments);