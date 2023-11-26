using System;

namespace GrammarsProcGen.Graph.Vertex
{
    internal interface IVertex<TData> : IEquatable<IVertex<TData>>
    {
        TData Data { get; }
    }
}
