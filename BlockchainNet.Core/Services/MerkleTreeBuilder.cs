namespace BlockchainNet.Core.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Security.Cryptography;
    using System.Collections.Concurrent;

    using BlockchainNet.Core.Models;
    using BlockchainNet.Core.Interfaces;

    public class MerkleTreeBuilder : IMerkleTreeBuilder
    {
        public MerkleNode BuildTree(IEnumerable<byte[]> nodes)
        {
            var sortedSet = new SortedSet<byte[]>(nodes, default(ByteArrayComparer));
            var next = new ConcurrentBag<MerkleNode>();

            using (var hashAlgorithm = SHA256.Create())
            {
                for (int i = 0; i < sortedSet.Count; i++)
                {
                    if ((i % 2) == 0)
                    {
                        var left = sortedSet.ElementAt(i);
                        var right = sortedSet.ElementAt(Math.Min(i + 1, sortedSet.Count - 1));
                        var combined = left.Concat(right);

                        var node = new MerkleNode()
                        {
                            Value = hashAlgorithm.ComputeHash(combined.ToArray()),
                            Left = new MerkleNode() { Value = hashAlgorithm.ComputeHash(left) },
                            Right = new MerkleNode() { Value = hashAlgorithm.ComputeHash(right) }
                        };
                        next.Add(node);
                    }
                }

                return BuildBranches(new HashSet<MerkleNode>(next), hashAlgorithm);
            }
        }

        private MerkleNode BuildBranches(HashSet<MerkleNode> nodes, HashAlgorithm hashAlgorithm)
        {
            var current = nodes;

            while (current.Count > 1)
            {
                var sortedSet = new SortedSet<MerkleNode>(current, default(MerkelNodeComparer));
                var next = new ConcurrentBag<MerkleNode>();

                for (int i = 0; i < sortedSet.Count; i++)
                {
                    if ((i % 2) == 0)
                    {
                        var left = sortedSet.ElementAt(i);
                        var right = sortedSet.ElementAt(Math.Min(i + 1, sortedSet.Count - 1));
                        var combined = left.Value.Concat(right.Value);

                        var node = new MerkleNode()
                        {
                            Value = hashAlgorithm.ComputeHash(combined.ToArray()),
                            Left = left,
                            Right = right
                        };
                        next.Add(node);
                    }
                };
                current = new HashSet<MerkleNode>(next);
            }

            return current.Single();
        }

        struct ByteArrayComparer : IComparer<byte[]>
        {
            public int Compare(byte[] x, byte[] y)
            {
                var len = Math.Min(x.Length, y.Length);
                for (var i = 0; i < len; i++)
                {
                    var c = x[i].CompareTo(y[i]);
                    if (c != 0)
                    {
                        return c;
                    }
                }

                return x.Length.CompareTo(y.Length);
            }
        }

        struct MerkelNodeComparer : IComparer<MerkleNode>
        {
            public int Compare(MerkleNode x, MerkleNode y)
            {
                return default(ByteArrayComparer).Compare(x.Value, y.Value);
            }
        }
    }
}
