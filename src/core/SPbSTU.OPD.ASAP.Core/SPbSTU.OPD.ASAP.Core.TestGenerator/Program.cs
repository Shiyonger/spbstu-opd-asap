using System.Text.Json;
using System.Text.Json.Serialization;
using SPbSTU.OPD.ASAP.Core.Infrastructure.Contracts;
using SPbSTU.OPD.ASAP.Core.TestGenerator;

const string bootstrapServers = "localhost:9092";
const string topicName1 = "points-github";
const string topicName2 = "action";
const int eventsCount = 100000;
const int timeoutMs = 5 * 60 * 1000;

using var cts = new CancellationTokenSource(timeoutMs);
var publisher1 = new KafkaPublisher<string, ActionKafka>(
    bootstrapServers,
    topicName2,
    new SystemTextJsonSerializer<string>(),
    new SystemTextJsonSerializer<ActionKafka>(new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } }));


var messages1 = ActionKafkaGenerator
    .Generate(eventsCount).ToList()
    .Select(e => (e.Username + e.AssignmentTitle, e));

await publisher1.Publish(messages1, cts.Token);
var publisher2 = new KafkaPublisher<string, PointsGithubKafka>(
    bootstrapServers,
    topicName1,
    keySerializer: null,
    new SystemTextJsonSerializer<PointsGithubKafka>(new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } }));


var messages2 = PointsGithubKafkaGenerator
    .Generate(eventsCount).ToList()
    .Select(e => (e.Username + e.AssignmentTitle, e));

await publisher2.Publish(messages2, cts.Token);

public static class ActionKafkaGenerator
{
    private static readonly Random _random = new();

    private static readonly string[] _usernames = { "HasanovTimur9", "Ilya", "zkmmhn", "MalyAnya" };
    private static readonly string[] _titles = { "Наследование_в_C++", "Лямбда-функции" };

    public static IEnumerable<ActionKafka> Generate(int count)
    {
        foreach (var username in _usernames)
        {
            foreach (var title in _titles)
            {
                yield return new ActionKafka
                {
                    Username = username,
                    AssignmentTitle = title,
                    Action = (ActionKafka.ActionType.Create)
                };
            }
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
        foreach (var username in _usernames)
        {
            foreach (var assignmentTitle in _assignmentTitles)
            {
                yield return new PointsGithubKafka
                {
                    Username = username,
                    AssignmentTitle = assignmentTitle,
                    CourseTitle = _courseTitles.First(),
                    Points = _random.Next(0, 101) // 0 to 100 points
                };
            }
        }
    }

    private static DateTime RandomDate()
    {
        var start = DateTime.UtcNow.AddDays(-30);
        var range = (DateTime.UtcNow - start).Days;
        return start.AddDays(_random.Next(range)).AddHours(_random.Next(24)).AddMinutes(_random.Next(60));
    }
}