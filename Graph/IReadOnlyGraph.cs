using GrammarsProcGen.Graph.Edge;
using System.Collections.Generic;

namespace GrammarsProcGen.Graph
{
    internal interface IReadOnlyGraph<out Tvertex, out TEdge>
        where TEdge : IEdge<Tvertex>
    {
        IReadOnlyCollection<Tvertex> Vertices { get; }
        IReadOnlyCollection<TEdge> Edges { get; }
    }
}
