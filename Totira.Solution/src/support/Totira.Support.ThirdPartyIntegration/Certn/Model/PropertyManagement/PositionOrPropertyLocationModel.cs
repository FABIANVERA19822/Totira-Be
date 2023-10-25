using Newtonsoft.Json;

namespace Totira.Support.ThirdPartyIntegration.Certn.Model.PropertyManagement;

[JsonObject]
public class PositionOrPropertyLocationModel
{
    [JsonProperty("address")]
    public string Address { get; set; } = default!;
    [JsonProperty("city")]
    public string City { get; set; } = default!;
    [JsonProperty("county")]
    public string County { get; set; } = default!;
    [JsonProperty("province_state")]
    public string ProvinceState { get; set; } = default!;
    [JsonProperty("country")]
    public string Country { get; set; } = default!;
    [JsonProperty("postal_code")]
    public string PostalCode { get; set; } = default!;
    [JsonProperty("location_type")]
    public string LocationType { get; set; } = default!;
}
