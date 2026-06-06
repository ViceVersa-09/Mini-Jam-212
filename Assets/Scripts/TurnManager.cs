using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TurnManager : MonoBehaviour
{
    [SerializeField] GameObject stump;
    [SerializeField] GameObject bearTrap;
    [SerializeField] int stumpPlaceLimit = 5;
    [SerializeField] int trapPlaceLimit = 5;
    [SerializeField] int turnsRemaining = 5;

    bool turnHappening;
    public bool TurnHappening { get { return turnHappening; } set { turnHappening = value; } }
    public int TurnsRemaining { get { return turnsRemaining; } set { turnsRemaining = value; } }

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
        if (mouseClick.WasPerformedThisFrame() && turnsRemaining > 0)
        {
            PlaceObject(selectedObject);
        }
        else if (deselectObject.WasPerformedThisFrame())
        {
            DeselectObject();
        }

        if (waitTurn.WasPerformedThisFrame())
        {
            TurnStarts();
            turnsRemaining--;
        }

        if (resetLevel.WasPerformedThisFrame())
        {
            ResetLevel();
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
    }

    void PlaceObject(GameObject obj)
    {
        if (obj == stump && stumpPlaceLimit > 0)
        {
            Instantiate(obj, MousePosition(), quaternion.identity);
            TurnStarts();
            stumpPlaceLimit--;
            turnsRemaining--;
        }
        else if (obj == bearTrap && trapPlaceLimit > 0)
        {
            Instantiate(obj, MousePosition(), quaternion.identity);
            TurnStarts();
            trapPlaceLimit--;
            turnsRemaining--;
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
