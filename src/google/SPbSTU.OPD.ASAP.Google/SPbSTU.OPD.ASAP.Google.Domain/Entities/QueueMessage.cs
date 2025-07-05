namespace SPbSTU.OPD.ASAP.Google.Domain.Entities;

public record QueueMessage(
    long Id,
    string GithubLink,
    long StudentId, 
    string StudentName,
    long GroupId,
    string SpreadSheetId,
    DateTime SubmissionDate,
    string Action
    );