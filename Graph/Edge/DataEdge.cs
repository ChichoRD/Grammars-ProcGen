using System;
using System.Collections.Generic;

namespace GrammarsProcGen.Graph.Edge
{
    internal readonly struct DataEdge<TVertex, TData> : IDataEdge<TVertex, TData>,
        IEquatable<IEdge<TVertex>>,
        IEquatable<IDataEdge<TData>>,
        IEquatable<IDataEdge<TVertex, TData>>,
        IEquatable<DataEdge<TVertex, TData>>
        where TData : IEquatable<TData>
    {
        public TVertex From { get; }
        public TVertex To { get; }
        public TData Data { get; }

        public DataEdge(TVertex from, TVertex to, TData data)
        {
            From = from;
            To = to;
            Data = data;
        }

        public bool Equals(IEdge<TVertex> other) =>
            EqualityComparer<TVertex>.Default.Equals(From, other.From)
            && EqualityComparer<TVertex>.Default.Equals(To, other.To);
        public bool Equals(IDataEdge<TData> other) => Data.Equals(other.Data);
        public bool Equals(IDataEdge<TVertex, TData> other) =>
            EqualityComparer<TVertex>.Default.Equals(From, other.From)
            && EqualityComparer<TVertex>.Default.Equals(To, other.To)
            && Data.Equals(other.Data);
        public bool Equals(DataEdge<TVertex, TData> other) =>
            EqualityComparer<TVertex>.Default.Equals(From, other.From)
            && EqualityComparer<TVertex>.Default.Equals(To, other.To)
            && Data.Equals(other.Data);
    }
}
