using System;
using Xunit;
using AVLTreeLib;
using System.Collections.Generic;

namespace AVLTreeTests
{
    public class AVLTests
    {
        [Fact]
        public void CountIncreaseAfterAdding()
        {
            var tree = new AVLTree<int, int>();
            int n = 2;
            for (int i = 0; i < n; i++)
            {
                tree.Add(i, i);
            }
            Assert.Equal(n, tree.Count);
        }
        [Fact]
        public void ItemsExistsAfterAdding()
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 22, 30, 15, 5, 17, 24, 33, 10, 16, 26 };
            int n = a.Length;
            for (int i = 0; i < n; i++)
            {
                tree.Add(a[i], i);
            }
            Assert.Equal(n, tree.Count);
        }

        [Fact]
        public void CountIsZeroAfterTreeCreation()
        {
            var tree = new AVLTree<int, int>();
            Assert.Equal(0, tree.Count);
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void ContainsFindsElement(int keyToFind)
        {
            bool expected = (keyToFind % 2 == 0);
            var tree = new AVLTree<int, int>();
            for(int i = 0;i<10;i++)
            {
                tree.Add(i * 2, i * 2);
            }
            Assert.Equal(tree.ContainsKey(keyToFind), expected);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(7)]
        [InlineData(6)]
        [InlineData(18)]
        [InlineData(15)]
        public void ElementWasRemoved(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 3, 7, 8, 16, 25, 20, 26 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(keyToRemove);
            Assert.True(!tree.ContainsKey(keyToRemove));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(7)]
        [InlineData(6)]
        [InlineData(18)]
        [InlineData(15)]
        public void CountDecreasedAfterDeletion(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 3, 7, 8, 16, 25, 20, 26 };
            int expected = a.Length - 1;
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(keyToRemove);
            Assert.Equal(tree.Count, expected);
        }


        [Theory]
        [InlineData(3)]
        [InlineData(7)]
        [InlineData(6)]
        [InlineData(18)]
        [InlineData(15)]
        public void RemoveRemovesOnlyOneElement(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 3, 7, 8, 16, 25, 20, 26 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(keyToRemove);
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == keyToRemove)
                    continue;
                Assert.True(tree.ContainsKey(a[i]));
            }

        }

        [Theory]
        [InlineData(7)]
        public void AfterRemovalInsertedElementHasProperParentRootCase(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 3, 7, 8, 16, 25, 20, 26 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            var expectedInsertedParent = tree.Find(keyToRemove).Parent;
            tree.Remove(keyToRemove);
            Assert.Equal(expectedInsertedParent, tree.Find(8).Parent);
        }

        [Theory]
        [InlineData(15)]
        public void AfterRemovalChildElementsHaveProperParentRootCase(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 3, 9, 16, 20 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            var expectedInsertedParent = 16;
            var childLeft = tree.Find(keyToRemove).Left;
            var childRight = tree.Find(keyToRemove).Right;
            tree.Remove(keyToRemove);
            Assert.Equal(expectedInsertedParent, childLeft.Parent.Value);
            Assert.Equal(expectedInsertedParent, childRight.Parent.Value);
        }


        [Theory]
        [InlineData(6)]
        public void AfterRemovalChildElementsHaveProperParentOneChildCase(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 9 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            var expectedNewParent = 15;
            var childRight = tree.Find(keyToRemove).Right;
            tree.Remove(keyToRemove);
            Assert.Equal(expectedNewParent, childRight.Parent.Value);
        }

        [Theory]
        [InlineData(25)]
        public void AfterRemovalInsertedElementHasProperParentInstantSuccessorCase(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 3, 7, 8, 16, 25, 20, 26 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            var expectedInsertedParent = tree.Find(keyToRemove).Parent;
            tree.Remove(keyToRemove);
            Assert.Equal(expectedInsertedParent, tree.Find(26).Parent);
        }

        [Theory]
        [InlineData(25)]
        public void AfterRemovalChildElementsHaveProperParentInstantSuccessorCase(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 3, 7, 8, 16, 25, 20, 26 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            var expectedNewParent = 26;
            var childLeft = tree.Find(keyToRemove).Left;
            //var childRight = tree.Find(keyToRemove).Right;
            tree.Remove(keyToRemove);
            Assert.Equal(expectedNewParent, childLeft.Parent.Value);
            //Assert.Equal(expectedNewParent, childRight.Parent.Value);
        }

        [Theory]
        [InlineData(18)]
        public void AfterRemovalChildElementsHaveProperParentNotInstantSuccessorCase(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 3, 7, 8, 16, 25, 20, 26 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            var expectedNewParent = 20;
            var childLeft = tree.Find(keyToRemove).Left;
            var childRight = tree.Find(keyToRemove).Right;
            tree.Remove(keyToRemove);
            Assert.Equal(expectedNewParent, childLeft.Parent.Value);
            Assert.Equal(expectedNewParent, childRight.Parent.Value);
        }

        [Theory]
        [InlineData(18)]
        public void AfterRemovalInsertedElementHaveProperParentNotInstantSuccessorCase(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 3, 7, 8, 16, 25, 20, 26 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            var expectedNewParent = tree.Find(keyToRemove).Parent;
            tree.Remove(keyToRemove);
            Assert.Equal(expectedNewParent, tree.Find(20).Parent);
        }
        [Fact]
        public void CantAddDuplicateKey()
        {
            var tree = new AVLTree<int, int>();
            tree.Add(5, 5);
            Assert.Throws<ArgumentException>(() => tree.Add(5, 2));
        }

        [Fact]
        public void TreeProperlyRebalancedLLCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 15, 7, 18, 6, 8, 3 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            int[] expectedTraversedTree = new[] { 3, 6, 8, 18, 15, 7 };
            List<int> traversedTree = TraverseTree(tree.Root);
            Assert.Equal(expectedTraversedTree, traversedTree.ToArray());
        }

        [Fact]
        public void TreeHasProperHeightsAfterRebalanceLLCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 15, 7, 18, 6, 8, 3 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            Assert.Equal(1, tree.Find(3).Height);
            Assert.Equal(1, tree.Find(8).Height);
            Assert.Equal(1, tree.Find(18).Height);
            Assert.Equal(2, tree.Find(6).Height);
            Assert.Equal(2, tree.Find(15).Height);
            Assert.Equal(3, tree.Find(7).Height);
        }

        [Fact]
        public void TreeProperlyRebalancedRRCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 15, 7, 18, 16, 19, 20 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            int[] expectedTraversedTree = new[] { 7, 16, 15, 20, 19, 18 };
            List<int> traversedTree = TraverseTree(tree.Root);
            Assert.Equal(expectedTraversedTree, traversedTree.ToArray());
        }

        [Fact]
        public void TreeHasProperHeightsAfterRebalanceRRCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 15, 7, 18, 16, 19, 20 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            Assert.Equal(1, tree.Find(7).Height);
            Assert.Equal(1, tree.Find(16).Height);
            Assert.Equal(1, tree.Find(20).Height);
            Assert.Equal(2, tree.Find(15).Height);
            Assert.Equal(2, tree.Find(19).Height);
            Assert.Equal(3, tree.Find(18).Height);
        }

        [Fact]
        public void TreeProperlyRebalancedLRCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 17, 13, 20, 21, 10, 11 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            int[] expectedTraversedTree = new[] { 10, 13, 11, 21, 20, 17 };
            List<int> traversedTree = TraverseTree(tree.Root);
            Assert.Equal(expectedTraversedTree, traversedTree.ToArray());
        }

        [Fact]
        public void TreeHasProperHeightsAfterRebalanceLRCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 17, 13, 20, 21, 10, 11 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }            
            Assert.Equal(1, tree.Find(10).Height);
            Assert.Equal(1, tree.Find(13).Height);
            Assert.Equal(1, tree.Find(21).Height);
            Assert.Equal(2, tree.Find(20).Height);
            Assert.Equal(2, tree.Find(11).Height);
            Assert.Equal(3, tree.Find(17).Height);
        }

        [Fact]
        public void TreeProperlyRebalancedRLCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 50, 25, 75, 12, 35, 87, 80 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            int[] expectedTraversedTree = new[] { 12, 35, 25, 75, 87, 80, 50 }; 
            List<int> traversedTree = TraverseTree(tree.Root);
            Assert.Equal(expectedTraversedTree, traversedTree.ToArray());
        }

        [Fact]
        public void TreeHasProperHeightsAfterRebalanceRLCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 50, 25, 75, 12, 35, 87, 80 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            Assert.Equal(1, tree.Find(12).Height);
            Assert.Equal(1, tree.Find(35).Height);
            Assert.Equal(1, tree.Find(75).Height);
            Assert.Equal(1, tree.Find(87).Height);
            Assert.Equal(2, tree.Find(25).Height);
            Assert.Equal(2, tree.Find(80).Height);
            Assert.Equal(3, tree.Find(50).Height);
        }

        [Fact]
        public void RootHasProperLeftAndRightHeights()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 50, 25, 75, 12, 35, 87,36};
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            Node<int, int> root = tree.Find(50);
            Assert.Equal(3, root.Left.Height);
            Assert.Equal(2, root.Right.Height);
        }

        [Fact]
        public void InsideElementHasProperLeftAndRightHeights()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 50, 25, 75, 12, 35, 87,36};
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            Node<int, int> root = tree.Find(25);
            Assert.Equal(1, root.Left.Height);
            Assert.Equal(2, root.Right.Height);

        }

        [Fact]
        public void TreeIsRebalancedAfterRemovalLRCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 50, 25, 75, 12, 35, 100,40};
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(100);
            int[] expectedTraversedTree = new[] { 12, 25, 40, 75, 50, 35};
            List<int> traversedTree = TraverseTree(tree.Root);
            Assert.Equal(expectedTraversedTree, traversedTree.ToArray());
        }

        [Fact]
        public void TreeIsRebalancedAfterRemovalRLCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 50, 25, 75, 12, 35, 87,70, 80 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(70);
            int[] expectedTraversedTree = new[] { 12, 35, 25, 75, 87, 80, 50 }; 
            List<int> traversedTree = TraverseTree(tree.Root);
            Assert.Equal(expectedTraversedTree, traversedTree.ToArray());
        }

        [Fact]
        public void TreeIsRebalancedAfterRemovalLLCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 15, 7, 18, 6, 19,8, 3 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(19);
            int[] expectedTraversedTree = new[] { 3, 6, 8, 18, 15, 7 };
            List<int> traversedTree = TraverseTree(tree.Root);
            Assert.Equal(expectedTraversedTree, traversedTree.ToArray());
        }

        [Fact]
        public void TreeIsRebalancedAfterRemovalRRCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 15, 7, 18, 16, 19,5, 20 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(5);
            int[] expectedTraversedTree = new[] { 7, 16, 15, 20, 19, 18 };
            List<int> traversedTree = TraverseTree(tree.Root);
            Assert.Equal(expectedTraversedTree, traversedTree.ToArray());
        }
        
        [Fact]
        public void RemoveElementThatDoesNotExist()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 15, 7, 18, 16, 19, 20 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(5);
            int[] expectedTraversedTree = new[] { 7, 16, 15, 20, 19, 18 };
            List<int> traversedTree = TraverseTree(tree.Root);
            Assert.Equal(expectedTraversedTree, traversedTree.ToArray());
        }

        [Theory]
        [InlineData(7)]
        public void AfterRemovalTreeHasProperStructureRootCase(int keyToRemove)
        {
            var tree = new AVLTree<int, int>();
            var a = new[] { 15, 6, 18, 3, 7, 8, 16, 25, 20, 26 };
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            var expectedInsertedParent = tree.Find(keyToRemove).Parent;
            tree.Remove(keyToRemove);
            int[] expectedTraversedTree = new[] { 3, 6, 16, 15, 20, 26,25,18,8 };
            List<int> traversedTree = TraverseTree(tree.Root);
            Assert.Equal(expectedTraversedTree, traversedTree.ToArray());
        }

        [Fact]
        public void TreeHasProperHeightsAfterRemovalLeafCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 15, 7, 18, 6, 19, 8, 3};
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(3);
            Assert.Equal(1, tree.Find(6).Height);
            Assert.Equal(1, tree.Find(8).Height);
            Assert.Equal(2, tree.Find(7).Height);
            Assert.Equal(1, tree.Find(19).Height);
            Assert.Equal(2, tree.Find(18).Height);
            Assert.Equal(3, tree.Find(15).Height);
        }

        [Fact]
        public void TreeHasProperHeightsAfterRemovalOneChildCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 7,6,15,3,8,19};
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(6);
            Assert.Equal(1, tree.Find(3).Height);
            Assert.Equal(1, tree.Find(8).Height);
            Assert.Equal(1, tree.Find(19).Height);
            Assert.Equal(2, tree.Find(15).Height);
            Assert.Equal(3, tree.Find(7).Height);
        }

        [Fact]
        public void TreeHasProperHeightsAfterRemovalTwoChildsInstantSuccessorCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 7,6,15,3,8,19};
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(15);
            Assert.Equal(1, tree.Find(3).Height);
            Assert.Equal(2, tree.Find(6).Height);
            Assert.Equal(1, tree.Find(8).Height);
            Assert.Equal(2, tree.Find(19).Height);
            Assert.Equal(3, tree.Find(7).Height);
        }

        [Fact]
        public void TreeHasProperHeightsAfterRemovalTwoChildsNonInstantSuccessorCase()
        {
            var tree = new AVLTree<int, int>();
            int[] a = new[] { 50,6,65,3,20,62,69,10,25};
            for (int i = 0; i < a.Length; i++)
            {
                tree.Add(a[i], a[i]);
            }
            tree.Remove(50);
            Assert.Equal(1, tree.Find(10).Height);
            Assert.Equal(1, tree.Find(25).Height);
            Assert.Equal(2, tree.Find(20).Height);
            Assert.Equal(1, tree.Find(3).Height);
            Assert.Equal(3, tree.Find(6).Height);
            Assert.Equal(4, tree.Find(62).Height);
            Assert.Equal(1, tree.Find(69).Height);
            Assert.Equal(2, tree.Find(65).Height);
        }

        private List<int> TraverseTree(Node<int, int> root)
        {
            List<int> treeElements = new List<int>();
            var current = root;
            if (current.Left != null)
            {
                treeElements.AddRange(TraverseTree(current.Left));
            }
            if (current.Right != null)
            {
                treeElements.AddRange(TraverseTree(current.Right));
            }
            treeElements.Add(current.Key);
            return treeElements;
        }
    }
}
