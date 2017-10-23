using UnityEngine;
using VRTK;

public class MenuControl : MonoBehaviour {

    public VRTK_ControllerEvents controllerEvents;
    public GameObject menu;

    public GameObject moveCont;
    private Move3dView moveScript;

    bool menuState = false;

    private void Start()
    {
        moveScript = moveCont.GetComponent<Move3dView>();

    }

    void OnEnable()
    {
        controllerEvents.ButtonTwoPressed += ControllerEvents_ButtonTwoPressed;
        controllerEvents.ButtonTwoReleased += ControllerEvents_ButtonTwoReleased;

        controllerEvents.TriggerAxisChanged += DoPosReset;
        controllerEvents.TouchpadAxisChanged += DoTouchpadAxisChanged;

        controllerEvents.TouchpadTouchEnd += DoTouchpadTouchEnd;
    }

    void OnDisable()
    {
        controllerEvents.ButtonTwoPressed -= ControllerEvents_ButtonTwoPressed;
        controllerEvents.ButtonTwoReleased -= ControllerEvents_ButtonTwoReleased;

        controllerEvents.TriggerAxisChanged -= DoPosReset;
        controllerEvents.TouchpadAxisChanged -= DoTouchpadAxisChanged;

        controllerEvents.TouchpadTouchEnd -= DoTouchpadTouchEnd;
    }

    private void ControllerEvents_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        menuState = !menuState;
        menu.SetActive(menuState);
    }

    private void ControllerEvents_ButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        


    }

    private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        moveScript.SetTouchAxis(e.touchpadAxis);
    }

    //private void DoTriggerAxisChanged(object sender, ControllerInteractionEventArgs e)
    //{
    //    moveScript.SetTriggerAxis(e.buttonPressure);
    //}

    private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        moveScript.SetTouchAxis(Vector2.zero);
    }

    //private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    //{
    //    moveScript.SetTriggerAxis(0f);
    //}

    private void DoPosReset(object sender, ControllerInteractionEventArgs e)
    {
        moveScript.ResetPos();
    }

}
