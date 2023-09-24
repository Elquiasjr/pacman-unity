using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeTree : MonoBehaviour
{
    public Node root { get; private set; }

    public void setRoot(Node root)
    {
        this.root = root;
    }
    public List<Node> getLeafs()
    {
        return getLeafsIntern(this.root);
    }
    public List<Node> getLeafsIntern(Node current)
    {
        List<Node> leafs = new();
        if (current.children.Count == 0)
        {

            leafs.Add(current);
        }
        else
        {
            foreach (Node node in current.children)
            {
                leafs.AddRange(getLeafsIntern(node));
            }
        }
        //compare f cost and sort
        leafs = leafs.OrderBy(o => o.fCost).ToList();
        return leafs;
    }

    public void resetNodes()
    {
        resetNodesIntern(this.root);
    }

    public void resetNodesIntern(Node current)
    {
        foreach (Node node in current.children)
        {
            resetNodesIntern(node);
        }
        current.ResetState();
    }

}
