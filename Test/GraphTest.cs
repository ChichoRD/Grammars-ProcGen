using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GrammarsProcGen.Graph
{
    internal class GraphTest : MonoBehaviour
    {
        [SerializeField]
        private Bounds _bounds;

        [SerializeField]
        private int _nodeCount;

        [SerializeField]
        private int _connectionsCount;

        private IGraph<NodeData, EdgeData> _graph;

        private void Awake()
        {
            Generate();
            DebugGraph();
        }

        [ContextMenu(nameof(Generate))]
        private void Generate()
        {
            if (_graph != null)
                foreach (INode<NodeData> node in _graph.Nodes)
                    Destroy(node.Data.GameObject);

            _graph = new Graph<NodeData, EdgeData>();
            Transform transform = this.transform;
            for (int i = 0; i < _connectionsCount; i++)
            {
                Vector3 position = new Vector3(Random.Range(_bounds.min.x, _bounds.max.x),
                                               Random.Range(_bounds.min.y, _bounds.max.y),
                                               Random.Range(_bounds.min.z, _bounds.max.z));

                Transform cubeTransform = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                cubeTransform.position = position;
                cubeTransform.SetParent(transform);

                _graph = _graph.WithNode(new Node<NodeData>(new NodeData(cubeTransform.gameObject)));
            }

            int connectionsMade = 0;
            while (connectionsMade < _connectionsCount)
            {
                INode<NodeData> nodeA = _graph.Nodes.ElementAt(Random.Range(0, _graph.Nodes.Count()));
                INode<NodeData> nodeB = _graph.Nodes.ElementAt(Random.Range(0, _graph.Nodes.Count()));

                if (nodeA.Equals(nodeB))
                    continue;

                if (nodeA.Edges.Any(edge => nodeB.Edges.Contains(edge)))
                    continue;

                float weight = Vector3.Distance(nodeA.Data.GameObject.transform.position,
                                                nodeB.Data.GameObject.transform.position);

                _graph = _graph.WithConnection(nodeA, nodeB, new EdgeData(weight));
                connectionsMade++;
            }
        }

        [ContextMenu(nameof(DebugGraph))]
        private void DebugGraph()
        {
            const float DURATION = 10.0f;
            IEnumerable<IEdge<NodeData, EdgeData>> edges = _graph.Edges.Cast<IEdge<NodeData, EdgeData>>();
            foreach (INode<NodeData> node in _graph.Nodes)
            {
                DrawCross3D(node.Data.GameObject.transform.position, 0.5f, Color.blue, DURATION);
            }

            foreach (IEdge<NodeData, EdgeData> edge in edges)
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

        private readonly struct NodeData
        {
            public readonly GameObject GameObject { get; }

            public NodeData(GameObject gameObject)
            {
                GameObject = gameObject;
            }
        }

        private readonly struct EdgeData
        {
            public readonly float Weight { get; }

            public EdgeData(float weight)
            {
                Weight = weight;
            }
        }
    }
}
