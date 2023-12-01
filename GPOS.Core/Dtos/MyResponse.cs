using Newtonsoft.Json;

namespace GPOS.Core.Dtos
{
    public class MyResponse<T> where T : class
    {
        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
        [JsonProperty("data")]
        public T? Data { get; set; }
    }
}
