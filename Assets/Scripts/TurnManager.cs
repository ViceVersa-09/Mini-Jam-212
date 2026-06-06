using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnManager : MonoBehaviour
{
    [SerializeField] GameObject stump;
    [SerializeField] GameObject bearTrap;
    [SerializeField] int stumpPlaceLimit;
    [SerializeField] int trapPlaceLimit;

    bool turnHappening;
    public bool TurnHappening { get { return turnHappening; } set { turnHappening = value; } }

    GameObject selectedObject;
    InputAction mouseClick;
    InputAction deselectObject;

    void Awake()
    {
        mouseClick = InputSystem.actions.FindAction("MouseClick");
        deselectObject = InputSystem.actions.FindAction("DeselectObject");
    }

    void Update()
    {
        turnHappening = false;
        if (mouseClick.WasPerformedThisFrame())
        {
            PlaceObject(selectedObject);
        }
        else if (deselectObject.WasPerformedThisFrame())
        {
            DeselectObject();
        }
    }

    Vector2 MousePosition()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos = new Vector2(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));
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
        }
        else if (obj == bearTrap && trapPlaceLimit > 0)
        {
            Instantiate(obj, MousePosition(), quaternion.identity);
            TurnStarts();
            trapPlaceLimit--;
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
}
