using SPbSTU.OPD.ASAP.Core.Domain.Contracts.Services;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Points;
using SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Queue;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Kafka;

namespace SPbSTU.OPD.ASAP.Core.Kafka;

public class OutboxBackgroundService(
    IServiceScopeFactory scopeFactory,
    KafkaPublisher<long, PointsGoogleKafka> pointsPublisher,
    KafkaPublisher<long, QueueKafka> queuePublisher,
    ILogger<OutboxBackgroundService> logger)
    : IHostedService, IDisposable
{
    private Timer? _timer;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly KafkaPublisher<long, PointsGoogleKafka> _pointsPublisher = pointsPublisher;
    private readonly KafkaPublisher<long, QueueKafka> _queuePublisher = queuePublisher;
    private readonly ILogger<OutboxBackgroundService> _logger = logger;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var pointsService = scope.ServiceProvider.GetRequiredService<IPointsService>();
            var actionService = scope.ServiceProvider.GetRequiredService<IActionService>();
            using var cts = new CancellationTokenSource();
            var token = cts.Token;

            var points = pointsService.GetNotSentPoints(token);
            var queue = actionService.GetNotSentQueue(token);
            await Task.WhenAll(points, queue);

            var pointsPublish = _pointsPublisher.Publish(points.Result.Select(p => (p.Id, MapPointsToKafka(p))), token);
            var queuePublish = _queuePublisher.Publish(queue.Result.Select(q => (q.Id, MapQueueToKafka(q))), token);
            await Task.WhenAll(pointsPublish, queuePublish);

            var pointsUpdate = pointsService.UpdateSentPoints(points.Result.Select(p => p.Id).ToList(), token);
            var queueUpdate = actionService.UpdateSentQueue(queue.Result.Select(q => q.Id).ToList(), token);
            await Task.WhenAll(pointsUpdate, queueUpdate);
        }
        catch (Exception e)
        {
            _logger.LogCritical(e.Message);
        }
    }

    private static PointsGoogleKafka MapPointsToKafka(OutboxPointsGetModel points)
    {
        return new PointsGoogleKafka
        {
            Id = points.Id, Points = points.Points, Date = points.Date,
            StudentPosition =
                new PointsGoogleKafka.Position
                {
                    Cell = points.StudentPosition.Cell,
                    SpreadSheetId = points.StudentPosition.SpreadSheetId
                },
            AssignmentPosition = new PointsGoogleKafka.Position
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
            Id = queue.Id, Link = queue.Link, StudentId = queue.StudentId, StudentName = queue.StudentName,
            GroupId = queue.GroupId, MentorId = queue.MentorId, MentorName = queue.MentorName,
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