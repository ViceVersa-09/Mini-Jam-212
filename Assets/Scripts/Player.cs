using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    [SerializeField] Node finish;
    [SerializeField] float moveSpeed;
    [SerializeField] bool move;
    [SerializeField] Collider2D playerCollider;
    [HideInInspector] public List<Node> playerPath;

    private void Update()
    {
        GetPath();

        if (move)
        {
            move = false;
            StartCoroutine(Move());
        }
    }

    IEnumerator Move()
    {
        if (1 < playerPath.Count)
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
}
