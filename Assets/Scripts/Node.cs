using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public Node cameFrom;
    public List<Node> connections;

    public float hScore;
    public float gScore;
    public bool nodeEnabled = true;
    public bool touchingEnemy = false;

    [SerializeField] Collider2D nodeCollider;
    public List<Collider2D> collidingWith = new List<Collider2D>(100);

    private void Start()
    {
        connections = Pathfinding.Instance.GetConnections(this);
    }

    private void Update()
    {
        if (nodeCollider != null)
        {
            if (Physics2D.OverlapCollider(nodeCollider, ContactFilter2D.noFilter, collidingWith) > 0)
            {
                foreach (Collider2D c in collidingWith)
                {
                    if (c != null)
                    {
                        if (c.gameObject.layer == LayerMask.NameToLayer("Stump"))
                        {
                            nodeEnabled = false;
                            return;
                        }
                        else
                        {
                            nodeEnabled = true;
                        }
                        if (c.gameObject.layer == LayerMask.NameToLayer("Player"))
                        {
                            Pathfinding.Instance.player = this;
                        }
                        else if (c.gameObject.layer == LayerMask.NameToLayer("Spider"))
                        {
                            Pathfinding.Instance.spider = this;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Node Has no Collider Attached To It");
        }
    }

    public float FScore()
    {
        return hScore + gScore;
    }
}
