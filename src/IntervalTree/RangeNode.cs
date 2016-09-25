using System;
using System.Collections.Generic;

namespace IntervalTree
{
    public class Node<TKey, T>
        where TKey : IComparable<TKey>
        where T : IRangeProvider<TKey>
    {
        private TKey _center;
        private Node<TKey, T> _leftNode;
        private Node<TKey, T> _rightNode;
        private List<T> _items;

        private readonly IComparer<T> _rangeComparer;

        /// <summary>
        /// Initializes a node with a list of items, builds the sub tree.
        /// </summary>
        /// <param name="rangeComparer">The comparer used to compare two items.</param>
        public Node(IEnumerable<T> items, IComparer<T> rangeComparer = null)
        {
            if (rangeComparer != null)
                _rangeComparer = rangeComparer;

            // first, find the median
            var endPoints = new List<TKey>();
            foreach (var o in items)
            {
                var range = o.Range;
                endPoints.Add(range.From);
                endPoints.Add(range.To);
            }
            endPoints.Sort();

            // the median is used as center value
            _center = endPoints[endPoints.Count / 2];
            _items = new List<T>();

            var left = new List<T>();
            var right = new List<T>();

            // iterate over all items
            // if the range of an item is completely left of the center, add it to the left items
            // if it is on the right of the center, add it to the right items
            // otherwise (range overlaps the center), add the item to this node's items
            foreach (var o in items)
            {
                var range = o.Range;

                if (range.To.CompareTo(_center) < 0)
                    left.Add(o);
                else if (range.From.CompareTo(_center) > 0)
                    right.Add(o);
                else
                    _items.Add(o);
            }

            // sort the items, this way the query is faster later on
            if (_items.Count > 0)
                _items.Sort(_rangeComparer);
            else
                _items = null;

            // create left and right nodes, if there are any items
            if (left.Count > 0)
                _leftNode = new Node<TKey, T>(left, _rangeComparer);
            if (right.Count > 0)
                _rightNode = new Node<TKey, T>(right, _rangeComparer);
        }

        public bool Exists(Range<TKey> range)
        {
            if (_items != null)
            {
                foreach (var o in _items)
                {
                    if (o.Range.From.CompareTo(range.To) > 0)
                        break;
                    else if (o.Range.Intersects(range))
                        return true;
                }
            }

            // go to the left or go to the right of the tree, depending
            // where the query value lies compared to the center
            if (range.From.CompareTo(_center) < 0 && _leftNode != null)
                _leftNode.Exists(range);
            if (range.To.CompareTo(_center) > 0 && _rightNode != null)
                _rightNode.Exists(range);

            return false;
        }

        public List<T> Query(Range<TKey> range)
        {
            var results = new List<T>();

            // If the node has items, check their ranges.
            if (_items != null)
            {
                foreach (var o in _items)
                {
                    if (o.Range.From.CompareTo(range.To) > 0)
                        break;
                    else if (o.Range.Intersects(range))
                        results.Add(o);
                }
            }

            // go to the left or go to the right of the tree, depending
            // where the query value lies compared to the center
            if (range.From.CompareTo(_center) < 0 && _leftNode != null)
                results.AddRange(_leftNode.Query(range));
            if (range.To.CompareTo(_center) > 0 && _rightNode != null)
                results.AddRange(_rightNode.Query(range));

            return results;
        }
    }
}
