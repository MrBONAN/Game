using UnityEngine.EventSystems;

namespace MazeMiniGame
{
    public class Maze
    {
        private Node[,] maze;
        public int X => maze.GetLength(1);
        public int Y => maze.GetLength(0);

        public Maze(int y, int x)
        {
            maze = new Node[y, x];
            for (var i = 0; i < y; i++)
            {
                for (var j = 0; j < x; j++)
                {
                    maze[i, j] = new Node(i, j);
                }
            }

            for (var i = 0; i < y; i++)
            {
                for (var j = 0; j < x; j++)
                {
                    if (j + 1 < x)
                        maze[i, j].Connect(maze[i, j + 1]);
                    if (i + 1 < y)
                        maze[i, j].Connect(maze[i + 1, j]);
                }
            }
        }

        private void MoveInDirection(ref Node curNode, MoveDirection direction)
        {
            var otherNode = curNode.GetNeighborNode(direction);
            if (otherNode is null ||
                otherNode.visited ||
                curNode.GetEdgeBetween(otherNode).Type == EdgeState.Missing)
                return;
            if (otherNode == curNode.prevNode)
            {
                curNode.visited = false;
                curNode.GetEdgeBetween(otherNode).Visited = false;
                curNode.prevNode = null;
                curNode = otherNode;
            }
            else
            {
                otherNode.visited = true;
                curNode.GetEdgeBetween(otherNode).Visited = true;
                otherNode.prevNode = curNode;
                curNode = otherNode;
            }
        }

        // TODO перенести это в класс MazeObject
        // private void OnDestroy()
        // {
        //     // Я хз, можно ли удалить объект дважды, поэтому лучше в HashSet записать их
        //     var edges = new HashSet<Edge>();
        //     foreach (var node in maze)
        //         edges.AddRange(node.edges);
        //     foreach (var edge in edges)
        //         Destroy(edge);
        // }
    }
}