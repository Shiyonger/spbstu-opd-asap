using SPbSTU.OPD.ASAP.Core.Domain.ValueObjects;

namespace SPbSTU.OPD.ASAP.Core.Domain.Contracts.Repositories;

public interface IGoogleRepository
{
    Task<Dictionary<string, Position>> GetStudentsPositions(List<string> githubUsernames, CancellationToken ct);
    
    Task<Dictionary<string, Position>> GetAssignmentsPositions(List<string> titles, CancellationToken ct);
}