using System.Text.Json.Serialization;

namespace Myvas.AspNetCore.Authentication.QQConnect.Internal
{
    internal class QQConnectErrorJson
    {
        public int? ret { get; set; }
        public string msg { get; set; }

        [JsonIgnore]
        public bool Success { get { return ret.GetValueOrDefault(0) == 0; } }
    }
}
