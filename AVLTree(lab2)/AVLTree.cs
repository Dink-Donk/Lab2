using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AVLTreeLib
{
    public class Node<TKey, TValue>
    {
        public TKey Key { get; set; }

        public TValue Value { get; set; }

        private Node<TKey, TValue> left, right;

        public Node<TKey, TValue> Parent;

        public Node<TKey, TValue> Left
        {
            get
            {
                return left;
            }
            set
            {
                if (value != null)
                {
                    value.Parent = this;
                }
                left = value;
            }
        }

        public Node<TKey, TValue> Right
        {
            get
            {
                return right;
            }
            set
            {
                if (value != null)
                {
                    value.Parent = this;
                }
                right = value;
            }
        }

        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        internal int balanceFactor
        {
            get
            {
                return LeftHeight - RightHeight;
            }
        }

        private int height;

        public int Height
        {
            get {
                return height;
            }
            internal set {
                height = value;
            }
        }
        internal void RecalculateHeight()
        {
            var current = this;

            while (current != null)
            {
                if (current.Left == null && current.Right == null)
                {
                    current.Height = 1;
                }
                else if (current.Left == null)
                {
                    current.Height = current.Right.Height + 1;
                }
                else if (current.Right == null)
                {
                    current.Height = current.Left.Height + 1;
                }
                else
                {
                    int maxHeight = Math.Max(current.Right.Height, current.Left.Height) + 1;
                    current.Height = maxHeight;
                }

                current = current.Parent;
            }
        }



        public int LeftHeight
        {
            get
            {
                if (this.Left != null)
                    return this.Left.Height;
                else
                    return 0;
            }
        }

        public int RightHeight
        {
            get
            {
                if (this.Right != null)
                    return this.Right.Height;
                else
                    return 0;
            }
        }

        public Node(Node<TKey, TValue> parent)
        {
            this.Parent = parent;
        }

    }

    public class AVLTree<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        public int Count { get; private set; }

        private Node<TKey, TValue> _root;

        public Node<TKey, TValue> Root
        {
            get
            {
                return _root;
            }
        }

        public AVLTree()
        {
            Count = 0;
        }

        public void Add(TKey key, TValue val)
        {
            Count++;
            Node<TKey, TValue> newNode = new Node<TKey, TValue>(key, val);
            if (_root == null)
            {
                _root = newNode;
                _root.Height = 0;
                return;
            }
            var current = _root;
            while (true)
            {
                int comparisionResult = current.Key.CompareTo(key);
                if (comparisionResult > 0)
                {
                    if (current.Left == null)
                    {
                        current.Left = newNode;
                        current = current.Left;
                        break;
                    }
                    current = current.Left;
                }
                else if (comparisionResult < 0)
                {
                    if (current.Right == null)
                    {
                        current.Right = newNode;
                        current = current.Right;
                        break;
                    }
                    current = current.Right;
                }
                else if (comparisionResult == 0)
                {
                    throw new ArgumentException("Can't add duplicate key");
                }
            }
            current.RecalculateHeight();
            BalanceTree(current);
        }

        public void Remove(TKey key)
        {
            var current = Find(key);
            if (current != null)
                Count--;
            else
                return;
            var parent = current.Parent;
            if (isLeaf(current))
            {
                if (parent.Left == current)
                {
                    parent.Left = null;
                    parent.RecalculateHeight();
                }
                else if(parent.Right==current)
                {
                    parent.Right = null;
                    parent.RecalculateHeight();
                }
                current = null;
            }
            else if (!hasRightChild(current) || !hasLeftChild(current))
            {
                current = NodeWithOneChildRemove(current);
            }
            else if (hasRightChild(current) && hasLeftChild(current))
            {
                current = NodeWithTwoChildsRemove(current);
            }
            if (current != null)
            {
                current.RecalculateHeight();
                BalanceTree(current);
                return;
            }
            else
            {
                if (parent.Left != null)
                {
                    if (parent.Left.Key.CompareTo(key) == 0)
                    {
                        parent.Left = null;
                    }
                }
                if (parent.Right != null)
                {
                    if (parent.Right.Key.CompareTo(key) == 0)
                    {
                        parent.Right = null;
                    }
                }
                parent.RecalculateHeight();
                BalanceTree(parent);
                return;
            }

            //_root = InternalRemove(key);
        }

        //private Node<TKey,TValue> InternalRemove(TKey key)
        //{
        //    var current = this.Find(key);
        //    if (current != null)
        //        Count--;
        //    else
        //        return _root;
        //    var parent = current.Parent;
        //    if (isLeaf(current))
        //    {
        //        current = null;
        //    }
        //    else if (!hasRightChild(current) || !hasLeftChild(current))
        //    {
        //        current = NodeWithOneChildRemove(current);
        //    }
        //    else if (hasRightChild(current) && hasLeftChild(current))
        //    {
        //        current = NodeWithTwoChildsRemove(current);
        //    }
        //    if (current != null)
        //    {
        //        current.RecalculateHeight();
        //        BalanceTree(current);
        //        while (current.Parent != null)
        //        {
        //            current = current.Parent;
        //        }
        //        return current;
        //    }
        //    else
        //    {
        //        if (parent.Left != null)
        //        {
        //            if (parent.Left.Key.CompareTo(key) == 0)
        //            {
        //                parent.Left = current;
        //            }
        //        }
        //        if (parent.Right != null)
        //        {
        //            if (parent.Right.Key.CompareTo(key) == 0)
        //            {
        //                parent.Right = current;
        //            }
        //        }
        //        parent.RecalculateHeight();
        //        BalanceTree(parent);
        //        while(parent.Parent!=null)
        //        {
        //            parent = parent.Parent;
        //        }
        //        return parent;
        //    }
        //}

        private Node<TKey, TValue> NodeWithTwoChildsRemove(Node<TKey, TValue> current)
        {
            var successor = findSuccessor(current);
            var nodeToCheck = successor.Parent;
            //Instant successor
            if (current.Right == successor)
            {
                successor.Left = current.Left;
                successor.Parent = current.Parent;
                if (current.Parent != null)
                {
                    if (current.Parent.Left == current)
                    {
                        current.Parent.Left = successor;
                    }
                    else if (current.Parent.Right == current)
                    {
                        current.Parent.Right = successor;
                    }
                }
                else
                    _root = successor;
                current = successor;
            }
            //Non instant successor
            else
            {
                Node<TKey, TValue> temp = new Node<TKey, TValue>(successor.Key, successor.Value);
                var successorParent = successor.Parent;
                successorParent.Left = null;
                successor = successor.Right;
                temp.Left = current.Left;
                temp.Right = current.Right;
                temp.Parent = current.Parent;
                if (current.Parent != null)
                {
                    if (current.Parent.Left == current)
                    {
                        current.Parent.Left = temp;
                    }
                    else if(current.Parent.Right==current)
                    {
                        current.Parent.Right = temp;
                    }
                }
                else
                {
                    _root = temp;
                }
                current = temp;
                successorParent.RecalculateHeight();
            }
            return current;
        }

        private Node<TKey, TValue> NodeWithOneChildRemove(Node<TKey, TValue> nodeToRemove)
        {
            if (!hasRightChild(nodeToRemove))
            {
                nodeToRemove.Left.Parent = nodeToRemove.Parent;
                if (nodeToRemove.Parent != null)
                {
                    if (nodeToRemove.Parent.Left == nodeToRemove)
                    {
                        nodeToRemove.Parent.Left = nodeToRemove.Left;
                    }
                    else if (nodeToRemove.Parent.Right == nodeToRemove)
                    {
                        nodeToRemove.Parent.Right = nodeToRemove.Left;
                    }
                }
                nodeToRemove = nodeToRemove.Left;
            }
            else if (!hasLeftChild(nodeToRemove))
            {
                nodeToRemove.Right.Parent = nodeToRemove.Parent;
                if (nodeToRemove.Parent != null)
                {
                    if (nodeToRemove.Parent.Left == nodeToRemove)
                    {
                        nodeToRemove.Parent.Left = nodeToRemove.Right;
                    }
                    else if (nodeToRemove.Parent.Right == nodeToRemove)
                    {
                        nodeToRemove.Parent.Right = nodeToRemove.Right;
                    }
                }
                nodeToRemove = nodeToRemove.Right;
            }
            return nodeToRemove;
        }

        public bool ContainsKey(TKey key)
        {
            var current = _root;
            while (current != null)
            {
                int comparisionResult = current.Key.CompareTo(key);

                if (comparisionResult > 0)
                {
                    current = current.Left;
                }
                else if (comparisionResult < 0)
                {
                    current = current.Right;
                }
                else if (comparisionResult == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private Node<TKey, TValue> findSuccessor(Node<TKey, TValue> startingNode)
        {
            Node<TKey, TValue> successor = startingNode.Right;
            while (successor.Left != null)
            {
                successor = successor.Left;
            }
            return successor;
        }

        private bool isLeaf(Node<TKey, TValue> node)
        {
            if (node.Left == null && node.Right == null)
                return true;
            else
                return false;
        }

        private bool hasLeftChild(Node<TKey, TValue> node)
        {
            if (node.Left != null)
                return true;
            else
                return false;
        }

        private bool hasRightChild(Node<TKey, TValue> node)
        {
            if (node.Right != null)
                return true;
            else
                return false;
        }

        private void BalanceTree(Node<TKey, TValue> current)
        {
            while (current != null)
            {
                int balanceFactor = current.balanceFactor;
                if (balanceFactor > 1)
                {
                    if (current.Left.balanceFactor > 0)
                    {
                        current = RotateLL(current);
                    }
                    else
                    {
                        RotateLR(current);
                    }
                }
                else if (balanceFactor < -1)
                {
                    if (current.Right.balanceFactor > 0)
                    {
                        RotateRL(current);
                    }
                    else
                    {
                        RotateRR(current);
                    }
                }
                current = current.Parent;
            }
        }

        private void RotateRR(Node<TKey, TValue> parent)
        {
            Node<TKey, TValue> pivot = parent.Right;
            parent.Right = pivot.Left;

            pivot.Parent = parent.Parent;
            if (pivot.Parent == null)
                _root = pivot;
            else
            {
                if (parent.Parent.Left == parent)
                    pivot.Parent.Left = pivot;
                else
                    pivot.Parent.Right = pivot;
            }
            pivot.Left = parent;
            pivot.Left.RecalculateHeight();
        }

        private Node<TKey, TValue> RotateLL(Node<TKey, TValue> parent)
        {
            Node<TKey, TValue> pivot = parent.Left;
            parent.Left = pivot.Right;

            pivot.Parent = parent.Parent;
            if (pivot.Parent == null)
                _root = pivot;
            else
            {
                if (parent.Parent.Left == parent)
                    pivot.Parent.Left = pivot;
                else
                    pivot.Parent.Right = pivot;
            }

            pivot.Right = parent;
            pivot.Right.RecalculateHeight();
            return pivot;
        }
        private void RotateLR(Node<TKey, TValue> parent)
        {
            Node<TKey, TValue> pivot = parent.Left;
            RotateRR(pivot);
            RotateLL(parent);
        }
        private void RotateRL(Node<TKey, TValue> parent)
        {
            Node<TKey, TValue> pivot = parent.Right;
            parent.Right = RotateLL(pivot);
            RotateRR(parent);
        }

        public TValue this[TKey index]
        {
            get
            {
                var current = _root;
                var parent = _root;
                while (current != null)
                {
                    parent = current;
                    if (current.Key.CompareTo(index) == 0)
                    {
                        return current.Value;
                    }
                    if (current.Key.CompareTo(index) > 0)
                    {
                        current = current.Left;
                    }
                    else if (current.Key.CompareTo(index) < 0)
                    {
                        current = current.Right;
                    }
                }
                throw new IndexOutOfRangeException("No such key");
            }
        }

        public Node<TKey, TValue> Find(TKey keyToFind)
        {
            var current = _root;
            var parent = _root;
            while (current != null)
            {
                parent = current;
                if (current.Key.CompareTo(keyToFind) == 0)
                {
                    return current;
                }
                if (current.Key.CompareTo(keyToFind) > 0)
                {
                    current = current.Left;
                }
                else if (current.Key.CompareTo(keyToFind) < 0)
                {
                    current = current.Right;
                }
            }
            return null;
        }
    }
}

