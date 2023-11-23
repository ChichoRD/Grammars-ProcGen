using System;

namespace GrammarsProcGen.Graph
{
    internal class Edge<TNodeData> : IEdge<TNodeData>, IEquatable<IEdge<TNodeData>>
    {
        private readonly INode<TNodeData> _from;
        private readonly INode<TNodeData> _to;

        public Edge(in INode<TNodeData> from, in INode<TNodeData> to)
        {
            _from = from;
            _to = to;
        }

        public ref readonly INode<TNodeData> From => ref _from;
        public ref readonly INode<TNodeData> To => ref _to;

        public bool Equals(IEdge<TNodeData> other) => _from.Equals(other.From) && _to.Equals(other.To);
        public override bool Equals(object obj) => obj is Edge<TNodeData> other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(_from, _to);
    }

    internal class Edge<TNodeData, TEdgeData> : IEdge<TNodeData, TEdgeData>, IEquatable<IEdge<TNodeData>>
    {
        private readonly INode<TNodeData> _from;
        private readonly INode<TNodeData> _to;
        private readonly TEdgeData _data;

        public Edge(in INode<TNodeData> from, in INode<TNodeData> to, in TEdgeData data)
        {
            _from = from;
            _to = to;
            _data = data;
        }

        public ref readonly INode<TNodeData> From => ref _from;
        public ref readonly INode<TNodeData> To => ref _to;
        public TEdgeData Data => _data;

        public bool Equals(IEdge<TNodeData> other) => _from.Equals(other.From) && _to.Equals(other.To);
        public override bool Equals(object obj) => obj is Edge<TNodeData> other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(_from, _to);
    }
}
