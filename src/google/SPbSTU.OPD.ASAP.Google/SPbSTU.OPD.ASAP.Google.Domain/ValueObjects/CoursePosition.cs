using SPbSTU.OPD.ASAP.Google.Domain.Entities;
namespace SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;

public record CoursePosition(Course Course, string Cell, string SpreadSheetId);
