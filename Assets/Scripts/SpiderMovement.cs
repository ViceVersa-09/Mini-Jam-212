using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpiderMovement : MonoBehaviour
{
    [SerializeField] Transform pathFinding;
    [SerializeField] float moveSpeed;
    [SerializeField] bool move;
    [SerializeField] Collider2D spiderCollider;
    [HideInInspector] public List<Node> spiderPath;

    TurnManager turnManager;
    Node finish;

    void Awake()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
    }

    private void Update()
    {
        FindFinish();
        GetPath();
        if (turnManager.TurnHappening)
        {
            StartCoroutine(Move());
        }
    }

    void FindFinish()
    {
        if (finish == null)
        {
            for (int i = 0; i < pathFinding.childCount; i++)
            {
                if (PathIsStraight(pathFinding.GetChild(i).GetComponent<Node>()))
                {
                    finish = pathFinding.GetChild(i).GetComponent<Node>();
                    return;
                }
            }
        }
    }

    bool PathIsStraight(Node finish)
    {
        List<Node> path = new();
        if (Pathfinding.Instance.spider != null)
        {
            path = Pathfinding.Instance.GeneratePath(Pathfinding.Instance.spider, finish);
        }
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].transform.position.y != transform.position.y || path[i].cameFrom.transform.position.x == transform.position.x)
                {
                    return false;
                }
            }
            for (int i = 0; i < path.Count; i++)
            {
                Debug.Log(path[i]);
            }
            return true;
        }
        return false;
    }

    IEnumerator Move()
    {
        if (spiderPath.Count > 1)
        {
            spiderCollider.enabled = false;
            while (transform.position != spiderPath[1].transform.position)
            {
                float speed = Vector2.Distance(transform.position, spiderPath[1].transform.position) * moveSpeed;
                transform.position = Vector2.MoveTowards(transform.position, spiderPath[1].transform.position, Mathf.Clamp(speed, moveSpeed / 50, 1));
                yield return new WaitForEndOfFrame();
            }
            spiderCollider.enabled = true;
        }
    }

    void GetPath()
    {
        if (finish != null && Pathfinding.Instance.spider != null)
        {
            spiderPath = Pathfinding.Instance.GeneratePath(Pathfinding.Instance.spider, finish);
            if (spiderPath.Count > 1)
            {
                for (int i = 0; i < spiderPath.Count; i++)
                {
                    if (i + 1 < spiderPath.Count)
                    {
                        Debug.DrawLine(spiderPath[i].transform.position, spiderPath[i + 1].transform.position, Color.green);
                    }
                }
            }
        }
    }
}
