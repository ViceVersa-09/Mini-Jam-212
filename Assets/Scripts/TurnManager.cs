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
    [SerializeField] float timeBetweenTurns = 0.5f;

    bool turnHappening;
    public bool TurnHappening { get { return turnHappening; } set { turnHappening = value; } }
    int turnCount = 0;
    public int TurnCount { get { return turnCount; } set { turnCount = value; } }
    bool hasWaited = true;
    float timeWaited = 0;

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

        if (waitTurn.WasPerformedThisFrame())
        {
            TurnStarts();
        }

        if (resetLevel.WasPerformedThisFrame())
        {
            ResetLevel();
        }

        WaitBeforeNextTurn();
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
            if (player.playerPath == null)
            {
                Destroy(placedObject);
                return;
            }
            TurnStarts();
            stumpPlaceLimit--;
        }
        else if (obj == bearTrap && trapPlaceLimit > 0)
        {
            GameObject placedObject = Instantiate(obj, MousePosition(), quaternion.identity);
            if (pathfinding.GeneratePath(Pathfinding.Instance.player, player.finish) == null)
            {
                Destroy(placedObject);
                return;
            }
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

    public void SelectedObject(GameObject obj)
    {
        selectedObject = obj;
    }

    void DeselectObject()
    {
        selectedObject = null;
    }

    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
