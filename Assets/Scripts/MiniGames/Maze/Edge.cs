namespace MazeMiniGame
{
    public enum EdgeState
    {
        Unvisited,
        Dot,
        Missing,
    }

    public class Edge
    {
        public readonly Node From;
        public readonly Node To;
        private EdgeState _type;

        public EdgeState Type
        {
            get => _type;
            set
            {
                _type = value;
                UpdateTexture();
            }
        }

        private bool _visited;

        public bool Visited
        {
            get => _visited;
            set
            {
                _visited = value;
                //animator.SetBool("visited", _visited);
            }
        }

        public Edge(Node from, Node to, EdgeState type)
        {
            // TODO
            From = from;
            To = to;
            Type = type;
        }

        public Node GetOtherNode(Node node)
        {
            if (From != node && To != node)
                return null;
            return From == node ? To : From;
        }

        private void UpdateTexture()
        {
            if (Visited)
                return;
            //animator.SetInteger("type", (int)Type);
        }
    }
}