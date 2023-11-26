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

    internal readonly struct Edge<TVertex, TData> : IDataEdge<TVertex, TData>,
        IEquatable<IEdge<TVertex>>,
        IEquatable<IDataEdge<TVertex>>,
        IEquatable<IDataEdge<TVertex, TData>>,
        IEquatable<Edge<TVertex, TData>>
        where TVertex : IEquatable<TVertex>
        where TData : IEquatable<TData>
    {
        public TVertex From { get; }
        public TVertex To { get; }
        public TData Data { get; }

        public Edge(TVertex from, TVertex to, TData data)
        {
            From = from;
            To = to;
            Data = data;
        }

        public bool Equals(IEdge<TVertex> other) => From.Equals(other.From) && To.Equals(other.To);
        public bool Equals(IDataEdge<TVertex> other) => Data.Equals(other.Data);
        public bool Equals(IDataEdge<TVertex, TData> other) => From.Equals(other.From) && To.Equals(other.To) && Data.Equals(other.Data);
        public bool Equals(Edge<TVertex, TData> other) => From.Equals(other.From) && To.Equals(other.To) && Data.Equals(other.Data);
    }
}
