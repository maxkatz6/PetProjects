namespace Lain.Xaml.Helpers
{
    using System.Collections;
    using System.Collections.Generic;
    public class StorageDictionary : IDictionary<string, object>
    {
        public int Count { get; }
        public bool IsReadOnly { get; }
        public ICollection<string> Keys { get; }
        public ICollection<object> Values { get; }

        public object this[string key]
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            throw new System.NotImplementedException();
        }
        public void Add(string key, object value)
        {
            throw new System.NotImplementedException();
        }
        public void Clear()
        {
            throw new System.NotImplementedException();
        }
        public bool Contains(KeyValuePair<string, object> item)
        {
            throw new System.NotImplementedException();
        }
        public bool ContainsKey(string key)
        {
            throw new System.NotImplementedException();
        }
        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
        public bool Remove(KeyValuePair<string, object> item)
        {
            throw new System.NotImplementedException();
        }
        public bool Remove(string key)
        {
            throw new System.NotImplementedException();
        }
        public bool TryGetValue(string key, out object value)
        {
            throw new System.NotImplementedException();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
