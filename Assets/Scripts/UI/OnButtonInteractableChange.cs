using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OnButtonInteractableChange : MonoBehaviour
{
    [SerializeField] private Button button;

    [SerializeField] private UnityEvent interactableOn;
    [SerializeField] private UnityEvent interactableOff;
    [SerializeField] private UnityEvent interactableSwitch;

    private bool previousState;

    void Start()
    {
        previousState = button.interactable;

        if (previousState == true)
            interactableOn?.Invoke();
        else
            interactableOff?.Invoke();
    }

    void Update()
    {
        if(button.interactable != previousState)
        {
            previousState = button.interactable;
            interactableSwitch?.Invoke();

            if(previousState == true)
                interactableOn?.Invoke();
            else
                interactableOff?.Invoke();
        }
    }
}
