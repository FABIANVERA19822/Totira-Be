﻿using Totira.Services.RootService.DTO.Common;

namespace Totira.Services.RootService.DTO.Landlord.GetDtos;
public class PropertyApplicationRequestDetailDto
{
    public PropertyDetailsDto PropertyDetails { get; set; } = new();
    public Guid TenantId { get; set; }
    public Guid PropertyApplicationId { get; set; }
    
}

public class PropertyDetailsDto
{
    public string? Area { get; set; }
    public string? Address { get; set; }
    public string? AmountFt2 { get; set; }
    public int AmountBeds { get; set; }
    public int AmountBaths { get; set; }
    public int AmountParkingSpaces { get; set; }
    public string? PropertyFronting { get; set; }
    public bool Pets { get; set; }
    public PropertyImageDto? Image { get; set; }
}