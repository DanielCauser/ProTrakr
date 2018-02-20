using Realms;
using Realms.Sync;

namespace ProTrakr.Services
{
    public class RealmService : IRealmService
    {
        public User User { get; set; }
        public SyncConfiguration SyncConfiguration { get; set; }

        public Realm GetInstance()
        {
            if (SyncConfiguration == null) return Realm.GetInstance();
            return Realm.GetInstance(SyncConfiguration);
        }
    }
}
