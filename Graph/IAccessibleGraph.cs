using GrammarsProcGen.Graph.Edge;
using System.Collections.Generic;

namespace GrammarsProcGen.Graph
{
    internal interface IAccessibleGraph<in Tvertex, out TEdge>
        where TEdge : IEdge<Tvertex>
    {
        IReadOnlyCollection<TEdge> GetEdges(Tvertex vertex);
    }
}
