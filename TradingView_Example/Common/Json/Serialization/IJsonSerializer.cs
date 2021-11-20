using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace TradingView_Example.Common.Json.Serialization
{
    public interface IJsonSerializer : ISerializer, IDeserializer
    {

    }
}
