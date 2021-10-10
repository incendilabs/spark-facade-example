using System.Collections.Generic;
using System.Threading.Tasks;
using Spark.Core;
using Spark.Engine;
using Spark.Engine.Core;
using Spark.Engine.Store.Interfaces;

namespace Spark.Facade.Store
{
    public class SqlStore : IFhirStore
    {
        private readonly StoreSettings _settings;

        public SqlStore(StoreSettings settings)
        {
            _settings = settings;
        }
        
        public async Task AddAsync(Entry entry)
        {
            IFhirStore store = null;
            switch (entry?.Resource?.TypeName)
            {
                case  "Patient":
                    store = new PatientStore(_settings);
                    break;
            }

            if (store != null)
            {
                await store.AddAsync(entry);
            }
            else
            {
                throw Error.BadRequest($"Cannot process resource with Resource Type: '{entry?.Resource?.TypeName}'.");
            }
        }

        public async Task<Entry> GetAsync(IKey key)
        {
            IFhirStore store = null;
            switch (key?.TypeName)
            {
                case  "Patient":
                    store = new PatientStore(_settings);
                    break;
            }

            if (store != null)
            {
                return await store.GetAsync(key);
            }
            else
            {
                throw Error.BadRequest($"Cannot process resource with Resource Type: '{key?.TypeName}'.");
            }
        }

        public Task<IList<Entry>> GetAsync(IEnumerable<IKey> localIdentifiers)
        {
            throw new System.NotImplementedException();
        }
        
        public void Add(Entry entry) => throw new System.NotImplementedException();

        public Entry Get(IKey key) => throw new System.NotImplementedException();

        public IList<Entry> Get(IEnumerable<IKey> localIdentifiers) => throw new System.NotImplementedException();
    }
}
