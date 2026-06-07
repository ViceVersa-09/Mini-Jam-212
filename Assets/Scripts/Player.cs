using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] Sprite trapActive;
    [SerializeField] Transform pathFinding;
    [SerializeField] Transform goalTransform;
    [SerializeField] float moveSpeed;
    [SerializeField] bool move;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] GameObject nextLevel;
    [HideInInspector] public List<Node> playerPath;

    bool traped = false;
    Vector2 goalPosition;
    float distance;

    GameObject trapObject;
    TurnManager turnManager;
    Animator animator;
    public Node finish;

    public Vector2 moveVector;

    void Awake()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        goalPosition = goalTransform.position;
    }

    private void Update()
    {
        FindNode();
        GetPath();

        if (turnManager.TurnHappening && !traped && playerPath != null)
        {
            distance = Vector2.Distance(transform.position, playerPath[1].transform.position);
            StartCoroutine(Move());
        }
        else if (turnManager.TurnHappening)
        {
            traped = false;
            Destroy(trapObject);
        }
        UpdateAnimations();
        if (Time.timeSinceLevelLoad > 0.1f)
        {
            PlayerWon();
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
                }
            }
        }
    }

    IEnumerator Move()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        if (playerPath != null)
        {
            if (playerPath.Count > 1)
            {
                playerCollider.enabled = false;
                while (transform.position != playerPath[1].transform.position)
                {
                    transform.position = Vector2.MoveTowards(transform.position, playerPath[1].transform.position, distance / turnManager.timeBetweenTurns * Time.deltaTime + 0.01f);
                    if (playerPath[1].transform.position.x - transform.position.x != 0)
                    {
                        moveVector.x = Mathf.Sign(playerPath[1].transform.position.x - transform.position.x);
                    }
                    if (playerPath[1].transform.position.y - transform.position.y != 0)
                    {
                        moveVector.y = Mathf.Sign(playerPath[1].transform.position.y - transform.position.y);
                    }
                    yield return new WaitForEndOfFrame();
                }
                playerCollider.enabled = true;
                moveVector = Vector2.zero;
            }
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

    void UpdateAnimations()
    {
        if (moveVector == Vector2.zero) // Idle
        {
            animator.SetInteger("State", 0);
        }
        else if (moveVector.x > 0) // Right
        {
            animator.SetInteger("State", 3);
        }
        else if (moveVector.x < 0) // Left
        {
            animator.SetInteger("State", 4);
        }
        else if (moveVector.y > 0) // Up
        {
            animator.SetInteger("State", 2);
        }
        else if (moveVector.y < 0) // Down
        {
            animator.SetInteger("State", 1);
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
            trapObject = other.gameObject;
            trapObject.GetComponentInChildren<SpriteRenderer>().sprite = trapActive;
        }
        else if (other.gameObject.layer == spider)
        {
            nextLevel.SetActive(true);
            turnManager.LevelComplete = true;
        }
    }
}
