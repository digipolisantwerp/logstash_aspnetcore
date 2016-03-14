using System;
using System.Net;
using System.Threading.Tasks;

namespace Toolbox.Logstash.Client
{
    public interface IWebClient
    {
        Task<T> Post<T>(string data, WebHeaderCollection headers, Func<WebHeaderCollection, string, T> resultor);
        Task<T> Get<T>(string id, WebHeaderCollection headers, Func<WebHeaderCollection, string, T> resultor);
    }
}
