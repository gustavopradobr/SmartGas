using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SceneScript : MonoBehaviour
{
    [SerializeField] private PlayableDirector timelineGasPump;
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.appUiManager.EnablePumpButton(true);
            GameManager.Instance.EnableVehicleInput(true);
            GameManager.Instance.inputManager.EnableInput(true);
            GameManager.Instance.cameraSelector.SetCamera(1);
            NotificationManager.Instance.EndNotification();
        }
    }
#endif
    void Start()
    {
        GameManager.Instance.EnableVehicleInput(false);
        GameManager.Instance.inputManager.EnableInput(false);

        DelayedHelper.InvokeDelayed(Notification1, 3);
    }

    private void Notification1()
    {
        NotificationManager.Instance.ShowNotification("Vamos começar!",
            "Você acaba de perceber que seu tanque está quase vazio, mas lembra que é cliente SmartGas!",
            NotificationObject.NotificationButtonType.ConfirmButton,
            true, 0, Notification2);

        GameManager.Instance.audioManager.PlayLowFuel();
    }

    private void Notification2()
    {
        NotificationManager.Instance.ShowNotification(null,
            "Aperte M para pegar seu celular e abrir o aplicativo.",
            NotificationObject.NotificationButtonType.None,
            true, 0, null);

        InputManager.M_KeyDown += FirstOpenPhone;
        GameManager.Instance.inputManager.EnableInput(true);
    }

    private void FirstOpenPhone()
    {
        GameManager.Instance.inputManager.EnableInput(false);
        NotificationManager.Instance.EndNotification();
        InputManager.M_KeyDown -= FirstOpenPhone;
    }

    private bool mapButtonClickedCompleted = false;
    public void MapButtonClicked()
    {
        if (mapButtonClickedCompleted) return; mapButtonClickedCompleted = true;

        GameManager.Instance.appUiManager.SetTouchActive(false);
    }

    private bool mapFoundCompleted = false;
    public void MapFound()
    {
        if (mapFoundCompleted) return; mapFoundCompleted = true;

        GameManager.Instance.inputManager.EnableInput(true);

        NotificationManager.Instance.ShowNotification("Posto encontrado!",
            "Aperte M para guardar o celular e siga a rota até o posto mais próximo.",
            NotificationObject.NotificationButtonType.None,
            false, 0, null);

        InputManager.M_KeyDown += ClosePhone;
    }

    private void ClosePhone()
    {
        InputManager.M_KeyDown -= ClosePhone;

        GameManager.Instance.inputManager.EnableInput(false);

        NotificationManager.Instance.ShowNotification("Vamos aprender a dirigir!",
            "Use WASD para controlar o veículo.",
            NotificationObject.NotificationButtonType.ConfirmButton,
            true, 0, delegate { GameManager.Instance.inputManager.EnableInput(false); GameManager.Instance.EnableVehicleInput(true); });
    }

    private bool pumpTriggerCompleted = false;
    public void PumpTrigger()
    {
        if (pumpTriggerCompleted) return; pumpTriggerCompleted = true;

        GameManager.Instance.appUiManager.ShowGPSArrivedScreen();
        GameManager.Instance.appUiManager.EnableMapButton(false);
        GameManager.Instance.appUiManager.EnablePumpButton(true);
        GameManager.Instance.EnableVehicleInput(false);
        GameManager.Instance.inputManager.EnableInput(true);
        GameManager.Instance.carHelper.ActivateHandbrake();

        NotificationManager.Instance.ShowNotification("Você chegou ao posto!",
            "Aperte M para pegar o celular e liberar a bomba de combustível.",
            NotificationObject.NotificationButtonType.None,
            true, 0, null);

        InputManager.M_KeyDown += OpenPhoneOnPump;
    }

    private void OpenPhoneOnPump()
    {
        InputManager.M_KeyDown -= OpenPhoneOnPump;
        NotificationManager.Instance.EndNotification();

        AppUI.onEnableGasPump += EnableGasPump;
    }

    private void EnableGasPump()
    {
        AppUI.onEnableGasPump -= EnableGasPump;

        timelineGasPump.Play();
        timelineGasPump.stopped += EndGasPumpTimeline;
    }

    private void EndGasPumpTimeline(PlayableDirector director)
    {
        timelineGasPump.stopped -= EndGasPumpTimeline;

        GameManager.Instance.appUiManager.EnablePumpButton(false);
        GameManager.Instance.cameraSelector.SetCamera(0);

        DelayedHelper.InvokeDelayed(FuelComplete1, 1.5f);
    }

    private void FuelComplete1()
    {
        NotificationManager.Instance.ShowNotification("Placa confirmada!",
            "Você foi encontrado no banco de dados e foi feita liberação da bomba de combustível.",
            NotificationObject.NotificationButtonType.ConfirmButton,
            true, 0, FuelComplete2);
    }

    private void FuelComplete2()
    {
        NotificationManager.Instance.ShowNotification("Abastecimento realizado!",
        "Seu tanque está decente novamente, obrigado pela preferência e aproveite o seu possante.",
        NotificationObject.NotificationButtonType.ConfirmButton,
        true, 0, FuelComplete3);
    }    
    
    private void FuelComplete3()
    {
        GameManager.Instance.EnableVehicleInput(true);
    }
}
