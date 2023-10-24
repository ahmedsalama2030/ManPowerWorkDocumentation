using Core.Interfaces.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
namespace Infrastructure.Services.Common;
public class JsonStringLocalizer: IStringLocalizerCustom
    {
        private readonly IDistributedCache _cache;
        private readonly Newtonsoft.Json.JsonSerializer _serializer = new();

        public JsonStringLocalizer(IDistributedCache cache)
        {
            _cache = cache;
        }

        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var actualValue = this[name];
                return !actualValue.ResourceNotFound
                    ? new LocalizedString(name, string.Format(actualValue.Value, arguments))
                    : actualValue;
            }
        }
        public LocalizedString getValue(string name, string culture, params object[] arguments)
        {

            var value = GetString(name, culture, true);
            var valueLocal = new LocalizedString(name, value);
            var actualValue = valueLocal;
            return !actualValue.ResourceNotFound
                ? new LocalizedString(name, string.Format(actualValue.Value, arguments))
                : actualValue;

        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var filePath = $"Resources/{Thread.CurrentThread.CurrentCulture.Name}.json";

            using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new(stream);
            using JsonTextReader reader = new(streamReader);

            while (reader.Read())
            {
                if (reader.TokenType != JsonToken.PropertyName)
                    continue;

                var key = reader.Value as string;
                reader.Read();
                var value = _serializer.Deserialize<string>(reader);
                yield return new LocalizedString(key, value);
            }
        }

        public string getValues(string key, string culture, params object[] arguments)
        {
            var filePath = $"Resources/{culture}.json";
            var fullFilePath = Path.GetFullPath(filePath);

            if (File.Exists(fullFilePath))
            {
                var cacheKey = $"locale_{culture}_{key}";
                var cacheValue = _cache.GetString(cacheKey);

                if (!string.IsNullOrEmpty(cacheValue))
                    return cacheValue;

                var result = GetValueFromJSON(key, fullFilePath);

                if (!string.IsNullOrEmpty(result))
                    _cache.SetString(cacheKey, result);

                return result;
            }

            return string.Empty;
        }

        private string GetString(string key, string culture = "", bool withCulture = false)
        {
            culture = withCulture ? culture : GetCultureFull();
            var filePath = $"Resources/{culture}.json";
            var fullFilePath = Path.GetFullPath(filePath);

            if (File.Exists(fullFilePath))
            {
                var cacheKey = $"locale_{culture}_{key}";
                var cacheValue = _cache.GetString(cacheKey);

                if (!string.IsNullOrEmpty(cacheValue))
                    return cacheValue;

                var result = GetValueFromJSON(key, fullFilePath);

                if (!string.IsNullOrEmpty(result))
                    _cache.SetString(cacheKey, result);

                return result;
            }

            return string.Empty;
        }



        private string GetValueFromJSON(string propertyName, string filePath)
        {
            if (string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(filePath))
                return string.Empty;

            using FileStream stream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new(stream);
            using JsonTextReader reader = new(streamReader);

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName && reader.Value as string == propertyName)
                {
                    reader.Read();
                    return _serializer.Deserialize<string>(reader);
                }
            }

            return string.Empty;
        }
        private string GetCultureFull(){
            var culture=Thread.CurrentThread.CurrentCulture.Name;
            string result=string.Empty;
            switch (culture)
            {
                case "en" :  result="en-US" ;break;
                case "ar" :  result="ar-EG" ;break;
                
                default: result=culture;break;
            }
            return result;
        }
    }
