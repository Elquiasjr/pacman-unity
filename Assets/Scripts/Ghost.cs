using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostsHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }

    public GhostFrightened frightened { get; private set; }

    public List<Vector2> path { get; private set; } = new List<Vector2>();
    public GhostBehavior initialBehavior;
    public Transform target;


    public NodeTree tree { get; private set; }

    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostsHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();
        tree = gameObject.AddComponent<NodeTree>();
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior)
        {
            home.Disable();
        }
        if (initialBehavior != null)
        {
            initialBehavior.Enable();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }

    public void BackHomeAStar(Node node)
    {
        Vector3 outsideTransform = home.outsideTransform.transform.position;
        Node current = node;
        current.hCost = Vector3.Distance(current.transform.position, home.outsideTransform.transform.position);
        current.gCost = 0;
        current.fCost = current.hCost + current.gCost;
        current.parent = null;
        current.directionTo = Vector2.zero;

        tree.setRoot(current);
        bool find = false;

        if (current != null && enabled)
        {
            while (!find)
            {
                List<Vector2> avaliableDirectionsToAStar = new(current.availableDirections);
                avaliableDirectionsToAStar.Remove(current.directionTo * -1);
                foreach (Vector2 avPosition in avaliableDirectionsToAStar)
                {
                    Node nextNode = current.GetNextNode(avPosition);
                    if (nextNode != null)
                    {
                        nextNode.directionTo = avPosition;
                        float hCost = Vector3.Distance(nextNode.transform.position, outsideTransform);
                        int gCost = 1 + current.gCost;

                        nextNode.gCost = gCost;
                        nextNode.hCost = hCost;
                        nextNode.fCost = hCost + gCost;

                        current.children.Add(nextNode);

                        nextNode.parent = current;


                        if (nextNode.hCost < 1)
                        {
                            find = true;
                            break;
                        }
                    }
                }
                current = tree.getLeafs().First();
            }

            current = tree.getLeafs().First();

            while (current.parent != null)
            {
                path.Add(current.directionTo);
                current = current.parent;
            }
            tree.resetNodes();

            path.Reverse();
        }
    }

}
