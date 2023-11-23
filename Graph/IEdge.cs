using System;

namespace GrammarsProcGen.Graph
{
    internal interface IEdge<TNodeData> : IEquatable<IEdge<TNodeData>>
    {
        ref readonly INode<TNodeData> From { get; }
        ref readonly INode<TNodeData> To { get; }
    }

    internal interface IEdge<TNodeData, out TEdgeData> : IEdge<TNodeData>
    {
        TEdgeData Data { get; }
    }
}
