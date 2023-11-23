using System;
using System.Collections.Generic;

namespace GrammarsProcGen.Graph
{
    internal readonly struct Node<T> : INode<T>, IEquatable<INode<T>>
    {
        private readonly IReadOnlyCollection<IEdge<T>> _edges;
        private readonly T _data;

        public Node(IReadOnlyCollection<IEdge<T>> edges, in T data)
        {
            _edges = edges;
            _data = data;
        }

        public Node(in T data)
        {
            _edges = Array.Empty<IEdge<T>>();
            _data = data;
        }

        public readonly IReadOnlyCollection<IEdge<T>> Edges => _edges;
        public readonly T Data => _data;

        public bool Equals(INode<T> other) => _data.Equals(other.Data);
        public override bool Equals(object obj) => obj is Node<T> other && Equals(other);
        public override int GetHashCode() => _data.GetHashCode();
    }
}
