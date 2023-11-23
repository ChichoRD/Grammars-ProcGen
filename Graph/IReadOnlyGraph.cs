using System.Collections.Generic;

namespace GrammarsProcGen.Graph
{
    internal interface IReadOnlyGraph<TNodeData, out TEdgeData>
    {
        IReadOnlyCollection<INode<TNodeData>> Nodes { get; }
        IReadOnlyCollection<IEdge<TNodeData, TEdgeData>> Edges { get; }
    }
}