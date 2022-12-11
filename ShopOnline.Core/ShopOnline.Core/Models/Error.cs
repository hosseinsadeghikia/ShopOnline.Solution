using Newtonsoft.Json;

namespace ShopOnline.Core.Models
{
    public class Error
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
