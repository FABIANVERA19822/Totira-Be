﻿using Totira.Bussiness.PropertiesService.DTO.Common;
using Totira.Bussiness.PropertiesService.Queries;

namespace Totira.Bussiness.PropertiesService.DTO;

public class GetPropertyDto
{
    public EnumPropertySortBy? SortBy { get; set; } = EnumPropertySortBy.Newest;
    public long Count { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public List<PropertyDto> Properties { get; set; } = new();
}

public class PropertyDto
{
    public string Id { get; set; } = string.Empty;
    public string Ml_num { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public decimal Bedrooms { get; set; }
    public decimal Washrooms { get; set; }
    public string ApproxSquareFootage { get; set; } = string.Empty;

    public decimal OriginalPrice { get; set; }
    public decimal ListPrice { get; set; }
    public string StreetName { get; set; } = string.Empty;
    public PropertyImageDto? Photo { get; set; }
}