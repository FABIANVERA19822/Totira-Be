﻿using Totira.Services.RootService.DTO.Common;

namespace Totira.Services.RootService.DTO.Landlord.GetDtos
{
    public class GetPendingLandlordClaimsDto
    {
        public Guid Id { get; set; }
        public string MlsID { get; set; }
        public string Address { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string ListingUrl { get; set; }
        public List<FileInfoDisplayDto> OwnershipProofs { get; set; }
    }
}