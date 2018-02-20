using Realms;
using Realms.Sync;

namespace ProTrakr.Services
{
    public interface IRealmService
    {
        User User { get; set; }
        SyncConfiguration SyncConfiguration { get; set; }
        Realm GetInstance();
    }
}
