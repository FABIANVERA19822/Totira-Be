﻿using Totira.Services.RootService.DTO.Common;
using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Services.RootService.Commands;

[RoutingKey(nameof(CreateTenantStudentFinancialDetailCommand))]
public record CreateTenantStudentFinancialDetailCommand(
    Guid TenantId,
    string UniversityOrInstitute,
    string Degree,
    bool IsOverseasStudent,
    List<TenantFileInfoDto> EnrollmentProofs,
    List<TenantFileInfoDto>? StudyPermitsOrVisas,
    List<TenantFileInfoDto> IncomeProofs) : ICommand;
