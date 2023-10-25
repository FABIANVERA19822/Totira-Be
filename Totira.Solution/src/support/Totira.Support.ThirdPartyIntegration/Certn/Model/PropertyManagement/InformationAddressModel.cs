using Newtonsoft.Json;

namespace Totira.Support.ThirdPartyIntegration.Certn.Model.PropertyManagement;

[JsonObject]
public class InformationAddressModel
{
    [JsonProperty("address")]
    public string Address { get; set; } = default!;
    [JsonProperty("city")]
    public string City { get; set; } = default!;
    [JsonProperty("province_state")]
    public string State { get; set; } = default!;
    [JsonProperty("country")]
    public string Country { get; set; } = default!;
    [JsonProperty("current")]
    public bool Current { get; set; }
}
