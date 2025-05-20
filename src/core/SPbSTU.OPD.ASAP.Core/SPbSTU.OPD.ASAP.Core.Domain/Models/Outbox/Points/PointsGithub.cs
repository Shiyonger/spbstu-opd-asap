namespace SPbSTU.OPD.ASAP.Core.Domain.Models.Outbox.Points;

public record PointsGithub(string AssignmentTitle, string CourseTitle, string Username, DateTime Date, int Points)
    : Github(Username, AssignmentTitle);