using GrammarsProcGen.Graph.Edge;
using System.Collections.Generic;

namespace GrammarsProcGen.Graph
{
    internal interface IReadOnlyGraph<out TVertex, out TEdge>
        where TEdge : IEdge<TVertex>
    {
        IReadOnlyCollection<TVertex> Vertices { get; }
        IReadOnlyCollection<TEdge> Edges { get; }
    }
}
