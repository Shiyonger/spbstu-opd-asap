namespace SPbSTU.OPD.ASAP.Core.Domain.Models.Submission;

public record Submission(
    long Id,
    long StudentId,
    long MentorId,
    long AssignmentId,
    string RepositoryLink,
    DateTime CreatedAt,
    DateTime UpdatedAt);