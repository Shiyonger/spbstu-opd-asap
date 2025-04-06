using SPbSTU.OPD.ASAP.Core.Domain.Contracts;
using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;

namespace SPbSTU.OPD.ASAP.Core.Kafka;

public class OutboxBackgroundService(
    IServiceScopeFactory scopeFactory,
    KafkaPublisher<long, PointsKafka> pointsPublisher,
    KafkaPublisher<long, QueueKafka> queuePublisher)
    : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly KafkaPublisher<long, PointsKafka> _pointsPublisher = pointsPublisher;
    private readonly KafkaPublisher<long, QueueKafka> _queuePublisher = queuePublisher;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        using var scope = _scopeFactory.CreateScope();
        var outboxService = scope.ServiceProvider.GetRequiredService<IOutboxService>();
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        var points = outboxService.GetNotSentPoints(token);
        var queue = outboxService.GetNotSentQueue(token);
        await Task.WhenAll(points, queue);

        var pointsPublish = _pointsPublisher.Publish(points.Result.Select(
            p => (p.Id, MapPointsToKafka(p))), token);
        var queuePublish = _queuePublisher.Publish(queue.Result.Select(
            q => (q.Id, MapQueueToKafka(q))), token);
        await Task.WhenAll(pointsPublish, queuePublish);

        var pointsUpdate = outboxService.UpdateSentPoints(points.Result.Select(
            p => p.Id).ToList(), token);
        var queueUpdate = outboxService.UpdateSentPoints(queue.Result.Select(
            q => q.Id).ToList(), token);
        await Task.WhenAll(pointsUpdate, queueUpdate);
    }

    private static PointsKafka MapPointsToKafka(OutboxPointsGetModel points)
    {
        return new PointsKafka
        {
            Id = points.Id, Points = points.Points, Date = points.Date,
            StudentPosition =
                new PointsKafka.Position
                {
                    Cell = points.StudentPosition.Cell,
                    SpreadSheetId = points.StudentPosition.SpreadSheetId
                },
            AssignmentPosition = new PointsKafka.Position
            {
                Cell = points.AssignmentPosition.Cell,
                SpreadSheetId = points.AssignmentPosition.SpreadSheetId
            }
        };
    }

    private static QueueKafka MapQueueToKafka(OutboxQueueGetModel queue)
    {
        return new QueueKafka
        {
            Id = queue.Id, Link = queue.Link, MentorId = queue.MentorId, MentorName = queue.MentorName,
            AssignmentId = queue.AssignmentId, AssignmentTitle = queue.AssignmentTitle,
            SubmissionDate = queue.SubmissionDate, Action = (QueueKafka.ActionType)queue.Action
        };
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}