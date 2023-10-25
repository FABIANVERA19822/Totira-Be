using System;
namespace Totira.Bussiness.UserService.DTO.PropertyService
{
	public class GetPropertyDetailstoApplyDto
	{
        public GetPropertyDetailstoApplyDto() { }

        public GetPropertyDetailstoApplyDto(PropertyMainImage propertyImageFile, string id, string area, string address, string approxSquareFootage, decimal bedrooms, decimal washrooms, decimal parkingSpaces, string petsPermitted, decimal listPrice)
        {
            PropertyImageFile = propertyImageFile;
            Id = id;
            Area = area;
            Address = address;
            ApproxSquareFootage = approxSquareFootage;
            Bedrooms = bedrooms;
            Washrooms = washrooms;
            ParkingSpaces = parkingSpaces;
            PetsPermitted = petsPermitted;
            ListPrice = listPrice;
        }

        public PropertyMainImage PropertyImageFile { get; set; } = default!;


        public class PropertyMainImage
        {
            public string FileName { get; set; } = string.Empty;
            public string ContentType { get; set; } = default!;
            public string FileUrl { get; set; } = default!;

            public PropertyMainImage(string fileName, string contentType, string fileUrl)
            {
                FileName = fileName;
                ContentType = contentType;
                FileUrl = fileUrl;
            }
        }
        public string Id { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal ListPrice { get; set; }
        public string ApproxSquareFootage { get; set; } = string.Empty;
        public decimal Bedrooms { get; set; }
        public decimal Washrooms { get; set; }
        public decimal ParkingSpaces { get; set; }
        public string PetsPermitted { get; set; } = string.Empty;

    }
}

