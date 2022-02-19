using System.Collections;
using System.Collections.Generic;

using GiantScape.Server.Data.Models;

namespace GiantScape.Server.Data
{
    internal class DbCollection<T> : IEnumerable<T> where T : BaseModel
    {
        private IEnumerable<T> _collection;
        private Dictionary<string, T> idMap;

        public DbCollection(IEnumerable<T> collection)
        {
            _collection = collection;
            idMap = new Dictionary<string, T>();

            foreach (T model in _collection)
            {
                idMap.Add(model.ID, model);
            }
        }

        public T Get(string id)
        {
            if (idMap.ContainsKey(id)) return idMap[id];
            else return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_collection).GetEnumerator();
        }
    }
}
