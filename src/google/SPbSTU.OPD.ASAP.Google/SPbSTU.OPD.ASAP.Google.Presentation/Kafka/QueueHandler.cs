using Confluent.Kafka;
using SPbSTU.OPD.ASAP.Google.Application.Interfaces;
using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Google.Infrastructure.Kafka;

namespace SPbSTU.OPD.ASAP.Google.Kafka;

public class QueueHandler(IUpdateSheetService _updateSheetService) : IHandler<Ignore, QueueKafka>
{

    public async Task Handle(IReadOnlyCollection<ConsumeResult<Ignore, QueueKafka>> messages, CancellationToken token)
    {
        var queueMessages = messages
            .Select(m => MapToDomain(m.Message.Value))
            .ToList();

        await _updateSheetService.UpdateQueueAsync(queueMessages, token);
    }
    
    private static QueueMessage MapToDomain(QueueKafka kafka)
    {
        return new QueueMessage(
            kafka.Id,
            kafka.Link,
            kafka.StudentId,
            kafka.StudentName,
            kafka.GroupId,
            kafka.SpreadsheetId,
            kafka.SubmissionDate,
            kafka.Action.ToString() 
        );
    }
}