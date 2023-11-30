using System;

namespace GrammarsProcGen.Graph.Edge
{
    internal interface IDataEdge<out TData>
    {
        TData Data { get; }
    }

    internal interface IDataEdge<out TVertex, out TData> : IEdge<TVertex>, IDataEdge<TData>
    {
    }
}