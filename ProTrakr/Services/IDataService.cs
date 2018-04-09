using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProTrakr.Services
{
    public interface IDataService<T>
    {
        Task<List<T>> GetData();
    }
}