using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField] Transform pathFinding;
    [SerializeField] Vector2 goalPosition;
    [SerializeField] float moveSpeed;
    [SerializeField] bool move;
    [SerializeField] Collider2D playerCollider;
    [HideInInspector] public List<Node> playerPath;

    bool traped = false;

    TurnManager turnManager;
    Node finish;

    void Awake()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
    }

    private void Update()
    {
        FindNode();
        GetPath();

        if (turnManager.TurnHappening && !traped)
        {
            StartCoroutine(Move());
        }
        else if (turnManager.TurnHappening)
        {
            traped = false;
        }
    }

    void FindNode()
    {
        if (finish == null)
        {
            for (int i = 0; i < pathFinding.childCount; i++)
            {
                if ((Vector2)pathFinding.GetChild(i).transform.position == goalPosition)
                {
                    finish = pathFinding.GetChild(i).GetComponent<Node>();
                    Debug.Log(pathFinding.GetChild(i).GetComponent<Node>());
                }
            }
        }
    }

    IEnumerator Move()
    {
        if (playerPath.Count > 1)
        {
            playerCollider.enabled = false;
            while (transform.position != playerPath[1].transform.position)
            {
                float speed = Vector2.Distance(transform.position, playerPath[1].transform.position) * moveSpeed;
                transform.position = Vector2.MoveTowards(transform.position, playerPath[1].transform.position, Mathf.Clamp(speed, moveSpeed / 50, 1));
                yield return new WaitForEndOfFrame();
            }
            playerCollider.enabled = true;
        }
    }

    void GetPath()
    {
        if (finish != null && Pathfinding.Instance.player != null)
        {
            playerPath = Pathfinding.Instance.GeneratePath(Pathfinding.Instance.player, finish);
            if (playerPath.Count > 1)
            {
                for (int i = 0; i < playerPath.Count; i++)
                {
                    if (i + 1 < playerPath.Count)
                    {
                        Debug.DrawLine(playerPath[i].transform.position, playerPath[i + 1].transform.position, Color.green);
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        LayerMask trap = LayerMask.NameToLayer("Trap");
        if (other.gameObject.layer == trap)
        {
            traped = true;
        }
    }
}
