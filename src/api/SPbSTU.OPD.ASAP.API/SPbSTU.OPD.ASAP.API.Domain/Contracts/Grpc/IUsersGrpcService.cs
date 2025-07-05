using SPbSTU.OPD.ASAP.API.Domain.Models;
using SPbSTU.OPD.ASAP.API.Domain.Results;

namespace SPbSTU.OPD.ASAP.API.Domain.Contracts.Grpc;

public interface IUsersGrpcService
{
    Task<long> CreateUser(User user, CancellationToken ct);
    
    Task<GrpcGetUserResult> GetUser(string login, CancellationToken ct);
}