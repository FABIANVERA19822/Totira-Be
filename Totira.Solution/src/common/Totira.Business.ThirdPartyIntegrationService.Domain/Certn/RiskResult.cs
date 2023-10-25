using Totira.Support.Persistance;
using Totira.Support.Persistance.Document;

namespace Totira.Business.ThirdPartyIntegrationService.Domain.Certn
{
    public class RiskResult : ExternalDocument, IAuditable, IEquatable<RiskResult>
    {
        public string Status { get; set; } = string.Empty;
        public string StatusLabel { get; set; } = string.Empty;
        public List<string> RedFlags { get; set; } = new List<string>();
        public List<string> GreenFlags { get; set; } = new List<string>();
        public string Result { get; set; } = string.Empty;
        public string ResultLabel { get; set; } = string.Empty;
        public string JsonResponse { get; set; } = string.Empty;
        public List<RiskEvaluation> RiskEvaluations { get; set; } = new List<RiskEvaluation> { };

        public Guid CreatedBy => throw new NotImplementedException();

        public DateTimeOffset CreatedOn => throw new NotImplementedException();

        public Guid? UpdatedBy => throw new NotImplementedException();

        public DateTimeOffset? UpdatedOn => throw new NotImplementedException();

        public bool Equals(RiskResult? other)
        {
            throw new NotImplementedException();
        }
    }
}