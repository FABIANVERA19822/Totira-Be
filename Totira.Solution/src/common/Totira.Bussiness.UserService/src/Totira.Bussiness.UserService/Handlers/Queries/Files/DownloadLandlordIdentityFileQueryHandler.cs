using Microsoft.Extensions.Logging;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.DTO.Files;
using Totira.Bussiness.UserService.Queries.Files;
using Totira.Support.Application.Queries;
using static Totira.Support.Persistance.IRepository;

namespace Totira.Bussiness.UserService.Handlers.Queries.Files
{
    public class DownloadLandlordIdentityFileQueryHandler : IQueryHandler<QueryDownloadLandlordIdentityFile, DownloadFileDto>
    {
        private readonly ILogger<DownloadLandlordIdentityFileQueryHandler> _logger;
        private readonly IRepository<LandlordIdentityInformation, Guid> _landlordIdentityRepository;
        private readonly ICommonFunctions _helper;

        public DownloadLandlordIdentityFileQueryHandler(
            ILogger<DownloadLandlordIdentityFileQueryHandler> logger,
            IRepository<LandlordIdentityInformation, Guid> landlordIdentityRepository,
            ICommonFunctions helper)
        {
            _logger = logger;
            _landlordIdentityRepository = landlordIdentityRepository;
            _helper = helper;
        }

        public async Task<DownloadFileDto> HandleAsync(QueryDownloadLandlordIdentityFile query)
        {
            var landlordIdentity = await _landlordIdentityRepository.GetByIdAsync(query.LandlordId);

            if (landlordIdentity is null)
                return new("application/octet-stream", "", Array.Empty<byte>());

            _logger.LogInformation("Landlord identity{landlordId}", query.LandlordId);

            Func<Domain.Common.File, bool> isRequestedFile = (file => file.FileName == query.FileName);
            DownloadFileDto response = new(string.Empty, string.Empty, Array.Empty<byte>());

            if (landlordIdentity.IdentityProofs is not null && landlordIdentity.IdentityProofs.Any())
            {
                var identityProof = landlordIdentity.IdentityProofs.FirstOrDefault(x => x.FileName == query.FileName);

                if (identityProof is not null)
                {
                    response = await _helper.DownloadFileAsync(identityProof);
                }
            }
            return response;
        }
    }
}
