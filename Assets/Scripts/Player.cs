using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] Transform pathFinding;
    [SerializeField] Transform goalTransform;
    [SerializeField] float moveSpeed;
    [SerializeField] bool move;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] GameObject nextLevel;
    [HideInInspector] public List<Node> playerPath;

    bool traped = false;
    Vector2 goalPosition;

    TurnManager turnManager;
    public Node finish;

    void Awake()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
        goalPosition = goalTransform.position;
    }

    private void Update()
    {
        FindNode();
        GetPath();

        if (turnManager.TurnHappening && !traped && playerPath != null)
        {
            StartCoroutine(Move());
        }
        else if (turnManager.TurnHappening)
        {
            traped = false;
        }
        PlayerWon();
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
            if (playerPath != null)
            {
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

    void PlayerWon()
    {
        if (playerPath != null)
        {
            if (transform.position == playerPath[^1].transform.position)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        LayerMask trap = LayerMask.NameToLayer("Trap");
        LayerMask spider = LayerMask.NameToLayer("Spider");
        if (other.gameObject.layer == trap)
        {
            traped = true;
        }
        else if (other.gameObject.layer == spider)
        {
            nextLevel.SetActive(true);
            turnManager.LevelComplete = true;
        }
    }
}
