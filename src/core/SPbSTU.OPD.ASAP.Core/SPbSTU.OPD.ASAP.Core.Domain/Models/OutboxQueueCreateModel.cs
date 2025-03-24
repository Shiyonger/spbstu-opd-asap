using Action = SPbSTU.OPD.ASAP.Core.Domain.ValueObjects.Action;

namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public record OutboxQueueCreateModel(string Link, long MentorId, long AssignmentId, long SubmissionId, Action Action);