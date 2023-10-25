using Totira.Bussiness.UserService.Domain;
using Totira.Bussiness.UserService.Domain.Common;

namespace Test.UserService.Mocks.ObjectMocks;

public static class MockTenantEmployeeIncomes
{
    public static TenantEmployeeIncomes GetEmployeeIncomes(Guid tenantId)
    {
        var incomeId = Guid.NewGuid();
        var mock = TenantEmployeeIncomes.Create(tenantId);
        mock.EmployeeIncomes!.Add(
            TenantEmployeeIncome.Create(
                incomeId,
                tenantId,
                companyOrganizationName: "NordicTech Solutions",
                position: "Developer",
                startDate: new DateTime(2020, 1, 10),
                isCurrentlyWorking: true,
                endDate: null,
                monthlyIncome: 2500,
                EmploymentContactReference.Create(
                    firstName: "John",
                    lastName: "Doe",
                    jobTitle: "Director",
                    relationship: "Friends",
                    email: "john.doe@test.com",
                    EmploymentContactReferencePhoneNumber.Create("613-555-0110", "+1")),
                new List<TenantFile>()
                {
                    TenantFile.Create("Test.pdf", $"{tenantId}/{incomeId}/Test.pdf", "application.pdf", 1024)
                }));

        var studyDetail = TenantStudentFinancialDetail.Create(
            "Test university",
            "Test degree",
            true);

        studyDetail.EnrollmentProofs.Add(TenantFile.Create(
            "EnrollmentProof.pdf",
            $"{tenantId}/EnrollmentProof.pdf",
            ".pdf",
            1024));

        studyDetail.StudyPermitsOrVisas.Add(TenantFile.Create(
            "Visa.pdf",
            $"{tenantId}/Visa.pdf",
            ".pdf",
            1024));

        studyDetail.IncomeProofs.Add(TenantFile.Create(
            "Income.pdf",
            $"{tenantId}/Income.pdf",
            ".pdf",
            1024));

        mock.StudentIncomes!.Add(studyDetail);

        return mock;
    }

    public static TenantEmployeeIncomes GetStudentIncomes(Guid tenantId)
    {
        var mock = GetEmployeeIncomes(tenantId);
        mock.IsStudent = true;
        

        return mock;
    }



}
