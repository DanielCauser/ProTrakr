using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ProTrakr.Services
{
    public abstract class BaseDataService<T>
    {
        protected static readonly HttpClient HttpClient = new HttpClient();
        public virtual async Task<List<T>> GetData()
        {
            var result = await HttpClient.GetStringAsync($"http://demo7345493.mockable.io/api/{typeof(T).Name}");
            var clients = JsonConvert.DeserializeObject<List<T>>(result);
            return clients;
        }
    }
}