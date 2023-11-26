using GrammarsProcGen.Graph.Edge;
using GrammarsProcGen.Graph.Vertex;
using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GrammarsProcGen.Graph
{
    internal class GraphTest : MonoBehaviour
    {
        [SerializeField]
        private Bounds _bounds;

        [SerializeField]
        private int _verticesCount;

        [SerializeField]
        private int _connectionsCount;

        private IGraph<
            IVertex<VertexData>,
            IDataEdge<IVertex<VertexData>, EdgeData>> _graph;

        private IReadOnlyGraph<
            IVertex<VertexData>,
            IDataEdge<IVertex<VertexData>, EdgeData>> _readOnlyGraph;

        private IAccessibleGraph<
            IVertex<VertexData>,
            IDataEdge<IVertex<VertexData>, EdgeData>> _accessibleGraph;

        private void Awake()
        {
            Generate();
            DebugGraph();
        }

        private T SetGraphs<T>(T graph)
            where T : IGraph<IVertex<VertexData>, IDataEdge<IVertex<VertexData>, EdgeData>>,
                      IReadOnlyGraph<IVertex<VertexData>, IDataEdge<IVertex<VertexData>, EdgeData>>,
                      IAccessibleGraph<IVertex<VertexData>, IDataEdge<IVertex<VertexData>, EdgeData>>
        {
            _graph = graph;
            _readOnlyGraph = graph;
            _accessibleGraph = graph;
            return graph;
        }

        [ContextMenu(nameof(Generate))]
        private void Generate()
        {
            if (_graph != null)
                foreach (var vertex in _readOnlyGraph.Vertices)
                    Destroy(vertex.Data.GameObject);

            var graph = SetGraphs(new BidirectionalGraph<IVertex<VertexData>, IDataEdge<IVertex<VertexData>, EdgeData>>());

            Transform transform = this.transform;
            for (int i = 0; i < _verticesCount; i++)
            {
                Vector3 position = new Vector3(Random.Range(_bounds.min.x, _bounds.max.x),
                                               Random.Range(_bounds.min.y, _bounds.max.y),
                                               Random.Range(_bounds.min.z, _bounds.max.z));

                Transform cubeTransform = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                cubeTransform.position = position;
                cubeTransform.SetParent(transform);

                graph = SetGraphs(graph.WithVertex(new Vertex<VertexData>(new VertexData(cubeTransform.gameObject))));
            }

            int connectionsMade = 0;
            while (connectionsMade < _connectionsCount)
            {
                IVertex<VertexData> from = _readOnlyGraph.Vertices.ElementAt(Random.Range(0, _readOnlyGraph.Vertices.Count));
                IVertex<VertexData> to = _readOnlyGraph.Vertices.ElementAt(Random.Range(0, _readOnlyGraph.Vertices.Count));

                if (from.Equals(to))
                    continue;

                //float weight = Vector3.Distance(from.Data.GameObject.transform.position, to.Data.GameObject.transform.position);

                graph = SetGraphs(graph.WithEdge(new Edge<IVertex<VertexData>, EdgeData>(from, to, new EdgeData(from.Data.GameObject.transform, to.Data.GameObject.transform))));
                ++connectionsMade;
            }
        }

        [ContextMenu(nameof(DebugGraph))]
        private void DebugGraph()
        {
            const float DURATION = 10.0f;
            foreach (IVertex<VertexData> vertex in _readOnlyGraph.Vertices)
                DrawCross3D(vertex.Data.GameObject.transform.position, 1.0f, Color.blue, DURATION);

            foreach (IDataEdge<IVertex<VertexData>, EdgeData> edge in _readOnlyGraph.Edges)
            {
                Color closeColor = Color.green;
                Color farColor = Color.red;
                float maxWeight = _bounds.size.magnitude;

                float lerpFactor = edge.Data.Weight / maxWeight;
                Color color = Contrast(Color.Lerp(closeColor, farColor, lerpFactor), 4.0f);

                Debug.DrawLine(edge.From.Data.GameObject.transform.position,
                               edge.To.Data.GameObject.transform.position,
                               color,
                               DURATION);

                static Color Contrast(Color color, float slope) => new Color(
                    (color.r - 0.5f) * slope + 0.5f,
                    (color.g - 0.5f) * slope + 0.5f,
                    (color.b - 0.5f) * slope + 0.5f,
                    (color.a - 0.5f) * slope + 0.5f);
            }

            static void DrawCross3D(Vector3 position, float size, Color color = default, float duration = 0.0f)
            {
                Debug.DrawLine(position + Vector3.up * size, position + Vector3.down * size, color, duration);
                Debug.DrawLine(position + Vector3.left * size, position + Vector3.right * size, color, duration);
                Debug.DrawLine(position + Vector3.forward * size, position + Vector3.back * size, color, duration);
            }
        }

        private readonly struct VertexData : IEquatable<VertexData>
        {
            public readonly GameObject GameObject { get; }

            public VertexData(GameObject gameObject)
            {
                GameObject = gameObject;
            }

            public bool Equals(VertexData other) => GameObject.Equals(other.GameObject);
        }

        private readonly struct EdgeData : IEquatable<EdgeData>
        {
            private readonly float _weight;
            public readonly float Weight { get => HasFromAndTo() ? Vector3.Distance(_from.position, _to.position) : _weight; }

            private readonly Transform _from;
            private readonly Transform _to;

            public EdgeData(Transform from, Transform to)
            {
                _weight = Vector3.Distance(from.position, to.position);
                _from = from;
                _to = to;
            }

            public EdgeData(float weight)
            {
                _weight = weight;
                _from = null;
                _to = null;
            }

            public bool Equals(EdgeData other) => Weight.Equals(other.Weight);

            private bool HasFromAndTo() => _from != null && _to != null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }
    }
}