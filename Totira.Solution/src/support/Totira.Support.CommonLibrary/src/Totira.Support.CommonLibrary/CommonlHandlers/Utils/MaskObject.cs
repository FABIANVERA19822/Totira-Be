using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

using Totira.Support.CommonLibrary.Interfaces;

namespace Totira.Support.CommonLibrary.CommonlHandlers.Utils
{
    public class MaskObject : IMaskObject
    {
        private readonly ILogger<SecurityHandler> _logger;

        public MaskObject(ILogger<SecurityHandler> logger)
        {
            _logger = logger;
        }

        public List<T> ModifyPropertiesListObjectToAsterisk<T>(List<T> objectsToMask, Type maskType)
        {
            if (objectsToMask != null)
            {
                foreach (T objectToMask in objectsToMask)
                {
                    ModifyPropertiesToAsterisk(objectToMask, maskType);
                }
            }
            return objectsToMask;
        }

        public T ModifyPropertiesToAsterisk<T>(T objectToMask, Type maskType)
        {
            if (objectToMask != null)
            {
                var type = objectToMask.GetType();
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    var hasMaskAttribute = Attribute.IsDefined(property, maskType);
                    try
                    {
                        if (hasMaskAttribute)
                        {
                            var value = property.GetValue(objectToMask);
                            if (value != null && value is string stringValue)
                            {
                                string maskedValue = Regex.Replace(stringValue, "[a-zA-Z0-9]", "*");
                                property.SetValue(objectToMask, maskedValue);
                            }
                            if (value != null && value is int intValue)
                            {
                                int maskedValue = 0;
                                property.SetValue(objectToMask, maskedValue);
                            }
                        }
                        var propertyType = property.PropertyType;
                        if (!propertyType.IsPrimitive && propertyType != typeof(string) && propertyType != typeof(DateTime))
                        {
                            var subObj = property.GetValue(objectToMask);
                            if (subObj != null)
                            {
                                ModifyPropertiesToAsterisk(subObj, maskType);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        _logger.LogError($"Error while masking property {property.Name}, error: {ex.Message}-{ex.StackTrace}");
                    }
                }
            }
            return objectToMask;
        }

        public void ModifyPropertiesToAsteriskJT(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (var child in token.Children<JProperty>())
                    {
                        ModifyPropertiesToAsteriskJT(child.Value);
                    }
                    break;

                case JTokenType.Array:
                    foreach (var child in token.Children())
                    {
                        ModifyPropertiesToAsteriskJT(child);
                    }
                    break;

                case JTokenType.String:
                    if (!token.Path.ToLower().Contains("id"))
                    {
                        var stringValue = token.ToString();
                        string maskedValue = Regex.Replace(stringValue, "[a-zA-Z0-9]", "*");
                        token.Replace(maskedValue);
                    }
                    break;
                default:
                    break;

            }
        }
    }
}
