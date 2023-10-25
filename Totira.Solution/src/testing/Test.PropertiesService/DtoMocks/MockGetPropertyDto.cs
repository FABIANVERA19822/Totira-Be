using Totira.Bussiness.PropertiesService.DTO;
using Totira.Bussiness.PropertiesService.Queries;

namespace Test.PropertiesService;
public class MockGetPropertyDto
{
    
    public static GetPropertyDto GetFiveAndSortByNewest()
    {
        var dto = new GetPropertyDto()
        {
            SortBy = EnumPropertySortBy.Newest,
            Count = 4,
            PageNumber = 1,
            PageSize = 10
        };

        dto.Properties.Add(new()
        {
            Id = "C5948209",
            Ml_num = "C5948209",
            Area = "Toronto",
            Address = "38 Salonica Rd",
            Location = "38 Salonica",
            Bedrooms = 3,
            Washrooms = 3,
            ApproxSquareFootage = string.Empty,
            ListPrice = 7000000,
            StreetName = "Salonica",
            Photo = new()
            {
                FileUrl = "https://mock-bucket-totira-propertiesfiles.test.com/C5948209",
                ContentType = "image/jpeg"
            }
        });

        dto.Properties.Add(new()
        {
            Id = "C5873325",
            Ml_num = "C5873325",
            Area = "Toronto",
            Address = "555 Yonge St",
            Location = "555 Yonge",
            Bedrooms = 1,
            Washrooms = 1,
            ApproxSquareFootage = "500-599",
            ListPrice = 589000,
            StreetName = "Yonge",
            Photo = new()
            {
                FileUrl = "https://mock-bucket-totira-propertiesfiles.test.com/C5873325",
                ContentType = "image/jpeg"
            }
        });

        dto.Properties.Add(new()
        {
            Id = "C5881180",
            Ml_num = "C5881180",
            Area = "Toronto",
            Address = "1 Bloor St W",
            Location = "1 Bloor",
            Bedrooms = 3,
            Washrooms = 4,
            ApproxSquareFootage = "2500-2749",
            ListPrice = 6565000,
            StreetName = "Bloor",
            Photo = new()
            {
                FileUrl = "https://mock-bucket-totira-propertiesfiles.test.com/C5881180",
                ContentType = "image/jpeg"
            }
        });

        dto.Properties.Add(new()
        {
            Id = "C5948201",
            Ml_num = "C5948201",
            Area = "Toronto",
            Address = "38 Salonica Rd",
            Location = "38 Salonica",
            Bedrooms = 5,
            Washrooms = 11,
            ApproxSquareFootage = string.Empty,
            ListPrice = 12880000,
            StreetName = "Salonica",
            Photo = new()
            {
                FileUrl = "https://mock-bucket-totira-propertiesfiles.test.com/C5948201",
                ContentType = "image/jpeg"
            }
        });

        dto.Properties.Add(new()
        {
            Id = "C5799045",
            Ml_num = "C5799045",
            Area = "Toronto",
            Address = "11 Dunvegan Rd",
            Location = "11 Dunvegan",
            Bedrooms = 3,
            Washrooms = 4,
            ApproxSquareFootage = string.Empty,
            ListPrice = 3895000,
            StreetName = "Dunvegan",
            Photo = new()
            {
                FileUrl = "https://mock-bucket-totira-propertiesfiles.test.com/C5799045",
                ContentType = "image/jpeg"
            }
        });

        return dto;
    }
}
