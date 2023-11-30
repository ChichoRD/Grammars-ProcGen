using System;

namespace GrammarsProcGen.Graph.Edge
{
    internal readonly struct Edge<TVertex> : IEdge<TVertex>, IEquatable<IEdge<TVertex>>, IEquatable<Edge<TVertex>>
        where TVertex : IEquatable<TVertex>
    {
        public TVertex From { get; }
        public TVertex To { get; }

        public Edge(TVertex from, TVertex to)
        {
            From = from;
            To = to;
        }

        public bool Equals(IEdge<TVertex> other) => From.Equals(other.From) && To.Equals(other.To);
        public bool Equals(Edge<TVertex> other) => From.Equals(other.From) && To.Equals(other.To);
    }
}
