using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Totira.Support.Persistance.IRepository;
using Totira.Bussiness.UserService.Common;
using Totira.Bussiness.UserService.Domain.Landlords;
using Totira.Bussiness.UserService.Queries.Files;
using Totira.Support.Application.Queries;
using Totira.Bussiness.UserService.DTO.Files;

namespace Totira.Bussiness.UserService.Handlers.Queries.Files
{
    public class DownloadLandlordClaimFileQueryHandler : IQueryHandler<QueryDownloadLandlordClaimFile, DownloadFileDto>
    {
        private readonly ILogger<DownloadLandlordIdentityFileQueryHandler> _logger;
        private readonly IRepository<LandlordPropertyClaim, Guid> _landlordClaimRepository;
        private readonly ICommonFunctions _helper;

        public DownloadLandlordClaimFileQueryHandler(
            ILogger<DownloadLandlordIdentityFileQueryHandler> logger,
            IRepository<LandlordPropertyClaim, Guid> landlordClaimRepository,
            ICommonFunctions helper)
        {
            _logger = logger;
            _landlordClaimRepository = landlordClaimRepository;
            _helper = helper;
        }

        public async Task<DownloadFileDto> HandleAsync(QueryDownloadLandlordClaimFile query)
        {

            var landlordClaim = (await _landlordClaimRepository.Get(x=>x.LandlordId == query.LandlordId)).Where(x=>x.OwnershipProofs.Any(x=>x.FileName == query.FileName)).FirstOrDefault();

            if (landlordClaim is null)
                return new("application/octet-stream", "", Array.Empty<byte>());

            _logger.LogInformation("Landlord identity{landlordId}", query.LandlordId);

            Func<Domain.Common.File, bool> isRequestedFile = (file => file.FileName == query.FileName);
            DownloadFileDto response = new(string.Empty, string.Empty, Array.Empty<byte>());

            if (landlordClaim.OwnershipProofs is not null && landlordClaim.OwnershipProofs.Any())
            {
                var identityProof = landlordClaim.OwnershipProofs.FirstOrDefault(x => x.FileName == query.FileName);
                if (identityProof is not null)
                {
                    response = await _helper.DownloadFileAsync(identityProof);
                }
            }
            return response;
        }
    }
}
