using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    [SerializeField] GameObject stump;
    [SerializeField] GameObject bearTrap;
    [SerializeField] Player player;
    [SerializeField] Pathfinding pathfinding;
    [SerializeField] int stumpPlaceLimit = 5;
    [SerializeField] int trapPlaceLimit = 5;
    public float timeBetweenTurns = 0.5f;

    bool turnHappening;
    public bool TurnHappening { get { return turnHappening; } set { turnHappening = value; } }
    bool levelComplete = false;
    public bool LevelComplete { get { return levelComplete; } set { levelComplete = value; } }
    int turnCount = 0;
    public int TurnCount { get { return turnCount; } set { turnCount = value; } }
    bool hasWaited = true;
    bool hasWaited2 = true;
    float timeWaited = 0;
    float timeWaited2 = 0;

    GameObject selectedObject;
    InputAction mouseClick;
    InputAction deselectObject;
    InputAction resetLevel;
    InputAction waitTurn;

    void Awake()
    {
        mouseClick = InputSystem.actions.FindAction("MouseClick");
        deselectObject = InputSystem.actions.FindAction("DeselectObject");
        resetLevel = InputSystem.actions.FindAction("ResetLevel");
        waitTurn = InputSystem.actions.FindAction("WaitTurn");
    }

    void Update()
    {
        turnHappening = false;
        if (!levelComplete)
        {
            if (mouseClick.WasPerformedThisFrame())
            {
                PlaceObject(selectedObject);
            }
            else if (stumpPlaceLimit <= 0 && trapPlaceLimit <= 0 && hasWaited)
            {
                TurnStarts();
            }
            else if (deselectObject.WasPerformedThisFrame())
            {
                DeselectObject();
            }
            else if (waitTurn.WasPerformedThisFrame() && hasWaited2)
            {
                TurnStarts();
                hasWaited2 = false;
            }

            if (resetLevel.WasPerformedThisFrame())
            {
                ResetLevel();
            }

            WaitBeforeNextTurn();
            WaitBeforeNextTurn2();
        }

    }

    Vector2 MousePosition()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos = new Vector2(Mathf.RoundToInt(mousePos.x - 0.5f), Mathf.RoundToInt(mousePos.y - 0.5f)) + new Vector2(0.5f, 0.5f);
        return mousePos;
    }

    void TurnStarts()
    {
        turnHappening = true;
        turnCount++;
    }

    void PlaceObject(GameObject obj)
    {
        if (obj == stump && stumpPlaceLimit > 0)
        {
            GameObject placedObject = Instantiate(obj, MousePosition(), quaternion.identity);
            if (Pathfinding.Instance.GeneratePath(Pathfinding.Instance.player, player.finish) == null)
            {
                Destroy(placedObject);
                return;
            }
            TurnStarts();
            stumpPlaceLimit--;
        }
        else if (obj == bearTrap && trapPlaceLimit > 0)
        {
            Instantiate(obj, MousePosition(), quaternion.identity);
            TurnStarts();
            trapPlaceLimit--;
        }
    }

    void WaitBeforeNextTurn()
    {
        timeWaited += Time.deltaTime;
        if (timeWaited >= timeBetweenTurns)
        {
            hasWaited = true;
            timeWaited = 0;
        }
        else
        {
            hasWaited = false;
        }
    }

    void WaitBeforeNextTurn2()
    {
        timeWaited2 += Time.deltaTime;
        if (timeWaited2 >= timeBetweenTurns)
        {
            hasWaited2 = true;
            timeWaited2 = 0;
        }
    }

    public void SelectedObject(GameObject obj)
    {
        selectedObject = obj;
    }

    void DeselectObject()
    {
        selectedObject = null;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
