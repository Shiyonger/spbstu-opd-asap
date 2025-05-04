using Action = SPbSTU.OPD.ASAP.Core.Domain.ValueObjects.Action;

namespace SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Queue;

public record ActionGithub(string Username, DateTime Date, string AssignmentTitle, Action Action) : Github(Username, AssignmentTitle);