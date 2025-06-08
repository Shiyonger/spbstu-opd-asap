using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Queue;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;
using Action = SPbSTU.OPD.ASAP.Core.Domain.ValueObjects.Action;

namespace SPbSTU.OPD.ASAP.Core.Kafka.Handlers;

public class ActionHandler(IActionService actionService) : IHandler<Ignore, ActionKafka>
{
    private readonly IActionService _actionService = actionService;
    
    public async Task Handle(IReadOnlyCollection<ConsumeResult<Ignore, ActionKafka>> messages, CancellationToken token)
    {
        var list = messages.Select(cr => MapToDomain(cr.Message.Value)).ToList();
        await _actionService.CreateQueueAndSubmissions(list, token);
    }

    private static ActionGithub MapToDomain(ActionKafka action)
    {
        return new ActionGithub(action.Username, DateTime.Now, action.AssignmentTitle, (Action)(int)action.Action);
    }
}
