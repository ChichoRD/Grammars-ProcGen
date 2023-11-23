using System;
using System.Collections.Generic;

namespace GrammarsProcGen.Graph
{
    internal interface INode<T> : IEquatable<INode<T>>
    {
        IReadOnlyCollection<IEdge<T>> Edges { get; }
        T Data { get; }
    }
}
