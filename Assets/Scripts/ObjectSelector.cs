using TMPro;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    // this script is mostly for changing the text on the object selection buttons
    [SerializeField] TextMeshProUGUI stump;
    [SerializeField] TextMeshProUGUI trap;

    TurnManager turnManager;

    private void Start()
    {
        turnManager = FindAnyObjectByType<TurnManager>();
    }

    private void Update()
    {
        if (turnManager != null)
        {
            stump.text = "x" + turnManager.stumpPlaceLimit;
            trap.text = "x" + turnManager.trapPlaceLimit;
        }
    }
}
