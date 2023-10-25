using MongoDB.Driver;
using Totira.Bussiness.UserService.Domain;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    public static class IncomeCompletionExtension
    {
        public static bool EmploymentComplete(this TenantEmployeeIncomes incomes)
        {
            bool incomeValidation = false;
            bool studentValidation = false;
            if (incomes != null)
            {
                incomeValidation = incomes.EmployeeIncomes != null &&
                                   incomes.EmployeeIncomes.Where(inc => inc.IsCurrentlyWorking &&
                                                                        !string.IsNullOrEmpty(inc.CompanyOrganizationName) &&
                                                                        !string.IsNullOrEmpty(inc.Position) &&
                                                                        !(inc.MonthlyIncome == 0)).Any();
                studentValidation = incomes.StudentIncomes != null &&
                                    incomes.StudentIncomes.Where(inc => inc.IncomeProofs.Any() &&
                                                                        inc.EnrollmentProofs.Any() &&
                                                                        !string.IsNullOrEmpty(inc.Degree) &&
                                                                        !string.IsNullOrEmpty(inc.UniversityOrInstitute)
                                                                 ).Any();
            }
            return incomeValidation || studentValidation;
        }
    }
}
