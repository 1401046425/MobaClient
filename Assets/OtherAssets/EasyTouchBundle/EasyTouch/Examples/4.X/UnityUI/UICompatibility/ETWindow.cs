using HedgehogTeam.EasyTouch;
using UnityEngine;

public class ETWindow : MonoBehaviour
{
    private bool drag = false;

    private void OnEnable()
    {
        EasyTouch.On_TouchDown += On_TouchDown;
        EasyTouch.On_TouchStart += On_TouchStart;
    }

    private void OnDestroy()
    {
        EasyTouch.On_TouchDown -= On_TouchDown;
        EasyTouch.On_TouchStart -= On_TouchStart;
    }

    private void On_TouchStart(Gesture gesture)
    {
        drag = false;
        if (gesture.isOverGui)
        {
            if (gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform))
            {
                transform.SetAsLastSibling();
                drag = true;
            }
        }
    }

    private void On_TouchDown(Gesture gesture)
    {
        if (gesture.isOverGui)
        {
            if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)) && drag)
            {
                transform.position += (Vector3)gesture.deltaPosition;
            }
        }
    }
}