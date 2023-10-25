using MongoDB.Driver;
using Moq;
using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;

namespace Test.UserService.Mocks.ObjectMocks;

public static class MockTenantEmployeeIncomes
{
    public static TenantEmployeeIncomes GetEmployeeIncomes(Guid tenantId)
    {
        var incomeId = Guid.NewGuid();
        var mock = TenantEmployeeIncomes.Create(tenantId);
        mock.Id = tenantId;
        mock.EmployeeIncomes = new()
        {
            TenantEmployeeIncome.Create(
                incomeId,
                tenantId,
                "Test Company",
                "Test Position",
                new DateTime(2020, 1, 10),
                true,
                null,
                1200,
                EmploymentContactReference.Create("John",
                        "Doe",
                        "Test Job",
                        "Friends",
                        "test@example.com",
                        EmploymentContactReferencePhoneNumber.Create("555-123-4567", "+1")),
                new List<TenantFile>()
                {
                    TenantFile.Create("Test.pdf", $"{tenantId}/{incomeId}/Test.pdf", "application.pdf", 1024)
                }),
            TenantEmployeeIncome.Create(
                incomeId,
                tenantId,
                "Test Company",
                "Test Position",
                new DateTime(2020, 1, 10),
                false,
                new DateTime(2021, 2, 10),
                null,
                default!,
                new List<TenantFile>()
                {
                    TenantFile.Create("Test2.pdf", $"{tenantId}/{incomeId}/Test2.pdf", "application/pdf", 1024)
                })
        };

        return mock;
    }

    public static TenantEmployeeIncomes GetStudentIncomes(Guid tenantId)
    {
        var mock = GetEmployeeIncomes(tenantId);
        mock.IsStudent = true;
        mock.StudentIncomes = new List<TenantStudentFinancialDetail>()
        { 
            TenantStudentFinancialDetail.Create(
            "Test university",
            "Test degree",
            true)
        };
        mock.StudentIncomes.FirstOrDefault().EnrollmentProofs.Add(TenantFile.Create(
            "EnrollmentProofs.pdf",
            $"{tenantId}/EnrollmentProofs.pdf",
            ".pdf",
            1024));

        mock.StudentIncomes.FirstOrDefault().StudyPermitsOrVisas.Add(TenantFile.Create(
            "Visa.pdf",
            $"{tenantId}/Visa.pdf",
            ".pdf",
            1024));

        mock.StudentIncomes.FirstOrDefault().IncomeProofs.Add(TenantFile.Create(
            "Income.pdf",
            $"{tenantId}/Income.pdf",
            ".pdf",
            1024));

        return mock;
    }



}
