
using Newtonsoft.Json;

namespace Totira.Support.ThirdPartyIntegration.Certn.Model.PropertyManagement
{
    [JsonObject]
    public class RiskResultResponseModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("risk_evaluations")]
        public List<RiskEvaluationModel> RiskEvaluation { get; set; }

        [JsonObject]
        public class RiskEvaluationModel
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("risk_information_result")]
            public RiskInformationResultModel RiskInformationResult { get; set; }

            [JsonObject]
            public class RiskInformationResultModel
            {
                [JsonProperty("id")]
                public string Id { get; set; }
                [JsonProperty("confidence")]
                public string Confidence { get; set; }
            }
        }
    }
}
