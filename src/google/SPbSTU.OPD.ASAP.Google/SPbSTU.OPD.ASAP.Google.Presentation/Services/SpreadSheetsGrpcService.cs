using Grpc.Core;
using SPbSTU.OPD.ASAP.Google.Presentation;

namespace SPbSTU.OPD.ASAP.Google.Services;

public class SpreadSheetsGrpcService : SpreadSheetsService.SpreadSheetsServiceBase
{
    // TODO: write grpc service
    public override Task<CreateSpreadSheetsResponse> CreateSpreadSheets(CreateSpreadSheetsRequest request, ServerCallContext context)
    {
        return base.CreateSpreadSheets(request, context);
    }
}