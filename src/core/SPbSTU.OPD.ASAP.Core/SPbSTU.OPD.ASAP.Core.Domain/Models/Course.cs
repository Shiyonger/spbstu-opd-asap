namespace SPbSTU.OPD.ASAP.Core.Domain.Models;

public class Course
{
    public long Id { get; init; }
    public string Title { get; init; } = null!;
    public string? SubjectTitle { get; init; }
    public string? GithubOrganizationLink { get; init; }
    public string? GoogleSpreadSheetLink { get; init; }
    public List<Student>? Students { get; set; }
    public List<Assignment>? Assignments { get; set; }

    public Course()
    {
    }

    public Course(long id, string title, string subjectTitle, string githubOrganizationLink)
    {
        Id = id;
        Title = title;
        SubjectTitle = subjectTitle;
        GithubOrganizationLink = githubOrganizationLink;
    }
}