using GrammarsProcGen.Graph.Edge;
using System.Collections.Generic;

namespace GrammarsProcGen.Graph
{
    internal interface IRelatableGraph<out TVertex, out TEdge, in TGraph>
        where TEdge : IEdge<TVertex>
        where TGraph : IReadOnlyGraph<TVertex, TEdge>
    {
        bool IsGraphOf(TGraph other, IEqualityComparer<TVertex> vertexEqualityComparer, IEqualityComparer<TEdge> edgeEqualityComparer);

        bool IsSubgraphOf(TGraph superGraph, IEqualityComparer<TVertex> vertexEqualityComparer, IEqualityComparer<TEdge> edgeEqualityComparer);

        bool IsSupergraphOf(TGraph subGraph, IEqualityComparer<TVertex> vertexEqualityComparer, IEqualityComparer<TEdge> edgeEqualityComparer);

        public bool IsProperSubgraphOf(TGraph superGraph, IEqualityComparer<TEdge> edgeEqualityComparer, IEqualityComparer<TVertex> vertexEqualityComparer) =>
            IsSubgraphOf(superGraph, vertexEqualityComparer, edgeEqualityComparer)
            && !IsGraphOf(superGraph, vertexEqualityComparer, edgeEqualityComparer);
        public bool IsProperSupergraphOf(TGraph subGraph, IEqualityComparer<TEdge> edgeEqualityComparer, IEqualityComparer<TVertex> vertexEqualityComparer) =>
            IsSupergraphOf(subGraph, vertexEqualityComparer, edgeEqualityComparer)
            && !IsGraphOf(subGraph, vertexEqualityComparer, edgeEqualityComparer);
    }
}