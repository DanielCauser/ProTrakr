using System.Collections.Generic;
using System.Threading.Tasks;
using ProTrakr.Models;

namespace ProTrakr.Services
{
    public class ClientDataService :BaseDataService<Client>, IDataService<Client>
    {
        public Task<List<Client>> GetClientList()
        {
            return GetData();
        }
    }
}
