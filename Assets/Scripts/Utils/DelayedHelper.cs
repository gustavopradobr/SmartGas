using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class DelayedHelper
{
    public static void CooldownButton(Button button, float secondsDelay)
    {
        button.interactable = false;
        CooldownButtonTask(button, Mathf.RoundToInt(secondsDelay * 1000));
        
    }
    private static async void CooldownButtonTask(Button button, int delay)
    {
        await Task.Delay(delay);
        button.interactable = true;
    }

    public static void CooldownNavigation(float secondsDelay)
    {
        bool originalState = EventSystem.current.sendNavigationEvents;
        EventSystem.current.sendNavigationEvents = false;
        CooldownNavigationTask(originalState, Mathf.RoundToInt(secondsDelay * 1000));
    }
    private static async void CooldownNavigationTask(bool originalState, int delay)
    {
        await Task.Delay(delay);
        EventSystem.current.sendNavigationEvents = originalState;
    }

    public static void SetActiveDelayed(GameObject m_gameObject, bool setActive, float secondsDelay)
    {
        SetActiveDelayedTask(m_gameObject, setActive, Mathf.RoundToInt(secondsDelay * 1000));
    }
    private static async void SetActiveDelayedTask(GameObject m_gameObject, bool setActive, int delay)
    {
        await Task.Delay(delay);
        m_gameObject.SetActive(setActive);
    }

    public static void InvokeDelayed(UnityAction action, float secondsDelay)
    {
        InvokeDelayedTask(action, Mathf.RoundToInt(secondsDelay * 1000));
    } 
    private static async void InvokeDelayedTask(UnityAction action, int delay)
    {
        await Task.Delay(delay);
        action.Invoke();
    }
}
