using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadContentAs<T>(this HttpResponseMessage httpresponse)
        {
            if (!httpresponse.IsSuccessStatusCode)
                throw new ApplicationException($"Something went wrong calling the API: {httpresponse.ReasonPhrase}");

            var dataAsString = await httpresponse.Content.ReadAsStringAsync().ConfigureAwait(false); //Read data As data as string

            return JsonSerializer.Deserialize<T>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });//covert json to object 
        }
    }
}
