using System;
using System.Collections.Generic;
using System.Linq;

namespace IntervalTree
{
    public class IntervalTree<TKey, T> where TKey : IComparable<TKey> where T : IRangeProvider<TKey>
    {
        private Node<TKey, T> _root;
        private List<T> _items;
        private IComparer<T> _rangeComparer;
        public IEnumerable<T> Items => _items; 
        public int Count => _items.Count; 

        public IntervalTree(IEnumerable<T> items, IComparer<T> rangeComparer)
        {
            _rangeComparer = rangeComparer;
            _root = new Node<TKey, T>(items, rangeComparer);
            _items = items.ToList();
        }

        public List<T> Query(Range<TKey> range) => _root.Query(range);
        public bool Exists(Range<TKey> range) => _root.Exists(range);
        public void Rebuild() => _root = new Node<TKey, T>(_items, _rangeComparer);
    }
}
