using Newtonsoft.Json;
using static Totira.Support.ThirdPartyIntegration.Certn.Model.PropertyManagement.SoftCheckRequestModel.InformationModel;

namespace Totira.Support.ThirdPartyIntegration.Certn.Model.PropertyManagement;

[JsonObject]
public class SoftCheckRequestModel
{
    [JsonProperty("request_softcheck")]
    public bool RequestSoftCheck { get; set; }
    [JsonProperty("request_equifax")]
    public bool RequestEquifax { get; set; }
    [JsonProperty("information")]
    public InformationModel Information { get; set; } = new();
    [JsonProperty("position_or_property_location")]
    public PositionOrPropertyLocationModel PropertyLocation { get; set; } = new();

    [JsonObject]
    public class InformationModel
    {
        [JsonProperty("first_name")]
        public string FirstName { get; set; } = default!;
        [JsonProperty("last_name")]
        public string LastName { get; set; } = default!;
        [JsonProperty("addresses")]
        public List<InformationAddressModel> Addresses { get; set; } = new();
        [JsonProperty("date_of_birth")]
        public string Birthday { get; set; }
        [JsonProperty("sin_ssn")]
        public string SinOrSsn { get; set; }
    }
}
