using Action = SPbSTU.OPD.ASAP.Core.Domain.ValueObjects.Action;

namespace SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Queue;

public record OutboxQueueCreateModel(string Link, long MentorId, long AssignmentId, long SubmissionId, Action Action);