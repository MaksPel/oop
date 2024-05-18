using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTreeCollection
{
    // Класс для узла дерева
    public class BinaryTreeNode<T>
    {
        public T Data { get; set; }
        public BinaryTreeNode<T> Left { get; set; }
        public BinaryTreeNode<T> Right { get; set; }

        public BinaryTreeNode(T data)
        {
            Data = data;
            Left = null;
            Right = null;
        }
    }

    // Класс для бинарного дерева
    public class BinaryTree<T> : ICollection<T>
    {
        private BinaryTreeNode<T> root;

        public int Count { get; private set; }

        public bool IsReadOnly => false;

        public BinaryTree()
        {
            root = null;
            Count = 0;
        }

        // Добавление элемента в дерево
        public void Add(T item)
        {
            if (root == null)
            {
                root = new BinaryTreeNode<T>(item);
            }
            else
            {
                AddRecursive(root, item);
            }
            Count++;
        }

        private void AddRecursive(BinaryTreeNode<T> node, T item)
        {
            IComparable<T> comparable = item as IComparable<T>;
            if (comparable.CompareTo(node.Data) < 0)
            {
                if (node.Left == null)
                {
                    node.Left = new BinaryTreeNode<T>(item);
                }
                else
                {
                    AddRecursive(node.Left, item);
                }
            }
            else
            {
                if (node.Right == null)
                {
                    node.Right = new BinaryTreeNode<T>(item);
                }
                else
                {
                    AddRecursive(node.Right, item);
                }
            }
        }

        // Итератор для обхода элементов дерева
        public IEnumerator<T> GetEnumerator()
        {
            return InOrderTraversal(root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IEnumerable<T> InOrderTraversal(BinaryTreeNode<T> node)
        {
            if (node != null)
            {
                foreach (var leftNodeData in InOrderTraversal(node.Left))
                {
                    yield return leftNodeData;
                }
                yield return node.Data;
                foreach (var rightNodeData in InOrderTraversal(node.Right))
                {
                    yield return rightNodeData;
                }
            }
        }

        // Остальные методы ICollection можно реализовать по аналогии

        public void Clear()
        {
            root = null;
            Count = 0;
        }

        public bool Contains(T item)
        {
            return ContainsRecursive(root, item);
        }

        private bool ContainsRecursive(BinaryTreeNode<T> node, T item)
        {
            if (node == null)
            {
                return false;
            }

            IComparable<T> comparable = item as IComparable<T>;

            if (comparable.CompareTo(node.Data) < 0)
            {
                return ContainsRecursive(node.Left, item);
            }
            else if (comparable.CompareTo(node.Data) > 0)
            {
                return ContainsRecursive(node.Right, item);
            }
            else
            {
                return true;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            if (!Contains(item))
            {
                return false;
            }

            root = RemoveRecursive(root, item);
            Count--;

            return true;
        }

        private BinaryTreeNode<T> RemoveRecursive(BinaryTreeNode<T> node, T item)
        {
            if (node == null)
            {
                return null;
            }

            IComparable<T> comparable = item as IComparable<T>;

            if (comparable.CompareTo(node.Data) < 0)
            {
                node.Left = RemoveRecursive(node.Left, item);
            }
            else if (comparable.CompareTo(node.Data) > 0)
            {
                node.Right = RemoveRecursive(node.Right, item);
            }
            else
            {
                if (node.Left == null && node.Right == null)
                {
                    return null;
                }
                else if (node.Left == null)
                {
                    return node.Right;
                }
                else if (node.Right == null)
                {
                    return node.Left;
                }
                else
                {
                    BinaryTreeNode<T> minRightNode = FindMinNode(node.Right);
                    node.Data = minRightNode.Data;
                    node.Right = RemoveRecursive(node.Right, minRightNode.Data);
                }
            }

            return node;
        }

        private BinaryTreeNode<T> FindMinNode(BinaryTreeNode<T> node)
        {
            while (node.Left != null)
            {
                node = node.Left;
            }
            return node;
        }
    }
}
