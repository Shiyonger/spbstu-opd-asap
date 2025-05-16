using SPbSTU.OPD.ASAP.Google.Domain.Entities;
using SPbSTU.OPD.ASAP.Google.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Google.Domain.Interfaces;

public interface ISpreadSheetBuilder
{
    Task<SpreadSheetResult> BuildAsync(
        IReadOnlyList<Student> students,
        IReadOnlyList<Assignment> assignments,
        CancellationToken cancellationToken);
}