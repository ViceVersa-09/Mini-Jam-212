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
    Animator animator;
    Node finish;
    Vector2 moveVector;
    float distance;

    void Awake()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        FindFinish();
        GetPath();
        if (turnManager.TurnHappening && spiderPath != null)
        {
            distance = Vector2.Distance(transform.position, spiderPath[1].transform.position);
            StartCoroutine(Move());
        }
        UpdateAnimations();
        finish = null;
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
        List<Node> path;
        if (Pathfinding.Instance.spider != null)
        {
            path = Pathfinding.Instance.GeneratePath(Pathfinding.Instance.spider, finish);
        }
        else
        {
            return false;
        }
        if (path != null)
        {
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].transform.position.y != transform.position.y || path[^1].transform.position == transform.position)
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    void UpdateAnimations()
    {
        if (moveVector == Vector2.zero) // Idle
        {
            animator.SetInteger("State", 0);
        }
        else if (moveVector.x > 0) // Right
        {
            animator.SetInteger("State", 1);
        }
        else if (moveVector.x < 0) // Left
        {
            animator.SetInteger("State", 2);
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (spiderPath != null)
        {
            if (spiderPath.Count > 1)
            {
                spiderCollider.enabled = false;
                while (transform.position != spiderPath[1].transform.position)
                {
                    transform.position = Vector2.MoveTowards(transform.position, spiderPath[1].transform.position, distance / turnManager.timeBetweenTurns * Time.deltaTime + 0.01f);
                    if (spiderPath[1].transform.position.x - transform.position.x != 0)
                    {
                        moveVector.x = Mathf.Sign(spiderPath[1].transform.position.x - transform.position.x);
                    }
                    yield return new WaitForEndOfFrame();
                }
                spiderCollider.enabled = true;
                moveVector = Vector2.zero;
            }
        }
    }

    void GetPath()
    {
        if (finish != null && Pathfinding.Instance.spider != null && spiderCollider.isActiveAndEnabled)
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
