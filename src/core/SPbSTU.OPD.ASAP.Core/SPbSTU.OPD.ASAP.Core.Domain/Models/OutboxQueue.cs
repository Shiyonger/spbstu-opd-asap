namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public record OutboxQueue(string Link, long MentorId, long AssignmentId, long SubmissionId);