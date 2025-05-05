using System.Text.Json;
using System.Text.Json.Serialization;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Core.TestGenerator;

const string bootstrapServers = "localhost:9092";
const string topicName = "orderevents";
const int eventsCount = 100000;
const int timeoutMs = 5 * 60 * 1000;

using var cts = new CancellationTokenSource(timeoutMs);
var publisher1 = new KafkaPublisher<(string, string), ActionKafka>(
    bootstrapServers,
    topicName,
    keySerializer: null,
    new SystemTextJsonSerializer<ActionKafka>(new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } }));


var messages1 = ActionKafkaGenerator
    .Generate(eventsCount).ToList()
    .Select(e => ((e.Username, e.AssignmentTitle), e));

await publisher1.Publish(messages1, cts.Token);
var publisher2 = new KafkaPublisher<(string, string), PointsGithubKafka>(
    bootstrapServers,
    topicName,
    keySerializer: null,
    new SystemTextJsonSerializer<PointsGithubKafka>(new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } }));


var messages2 = PointsGithubKafkaGenerator
    .Generate(eventsCount).ToList()
    .Select(e => ((e.Username, e.AssignmentTitle), e));

await publisher2.Publish(messages2, cts.Token);

public static class ActionKafkaGenerator
{
    private static readonly Random _random = new();

    private static readonly string[] _usernames = { "HasanovTimur9", "Ilya", "zkmmhn", "MalyAnya" };
    private static readonly string[] _titles = { "Наследование_в_C++", "Лямбда-функции", "Сплайн-интерполяция", "Линейная_алгебра", "Методы_оптимизации" };

    public static IEnumerable<ActionKafka> Generate(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new ActionKafka
            {
                Username = _usernames[_random.Next(_usernames.Length)],
                Date = RandomDate(),
                AssignmentTitle = _titles[_random.Next(_titles.Length)],
                Action = (ActionKafka.ActionType)_random.Next(3)
            };
        }
    }

    private static DateTime RandomDate()
    {
        var start = DateTime.UtcNow.AddDays(-30);
        var range = (DateTime.UtcNow - start).Days;
        return start.AddDays(_random.Next(range)).AddHours(_random.Next(24)).AddMinutes(_random.Next(60));
    }
}

public static class PointsGithubKafkaGenerator
{
    private static readonly Random _random = new();

    private static readonly string[] _usernames = { "HasanovTimur9", "Ilya", "zkmmhn", "MalyAnya" };
    private static readonly string[] _assignmentTitles = { "Наследование_в_C++", "Лямбда-функции" };
    private static readonly string[] _courseTitles = { "ТООП-2024-осень" };

    public static IEnumerable<PointsGithubKafka> Generate(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new PointsGithubKafka
            {
                Username = _usernames[_random.Next(_usernames.Length)],
                AssignmentTitle = _assignmentTitles[_random.Next(_assignmentTitles.Length)],
                CourseTitle = _courseTitles[_random.Next(_courseTitles.Length)],
                Date = RandomDate(),
                Points = _random.Next(0, 101) // 0 to 100 points
            };
        }
    }

    private static DateTime RandomDate()
    {
        var start = DateTime.UtcNow.AddDays(-30);
        var range = (DateTime.UtcNow - start).Days;
        return start.AddDays(_random.Next(range)).AddHours(_random.Next(24)).AddMinutes(_random.Next(60));
    }
}