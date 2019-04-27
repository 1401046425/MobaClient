using HedgehogTeam.EasyTouch;
using UnityEngine;

public class UIDrag : MonoBehaviour
{
    private int fingerId = -1;
    private bool drag = true;

    private void OnEnable()
    {
        EasyTouch.On_TouchDown += On_TouchDown;
        EasyTouch.On_TouchStart += On_TouchStart;
        EasyTouch.On_TouchUp += On_TouchUp;
        EasyTouch.On_TouchStart2Fingers += On_TouchStart2Fingers;
        EasyTouch.On_TouchDown2Fingers += On_TouchDown2Fingers;
        EasyTouch.On_TouchUp2Fingers += On_TouchUp2Fingers;
    }

    private void OnDestroy()
    {
        EasyTouch.On_TouchDown -= On_TouchDown;
        EasyTouch.On_TouchStart -= On_TouchStart;
        EasyTouch.On_TouchUp -= On_TouchUp;
        EasyTouch.On_TouchStart2Fingers -= On_TouchStart2Fingers;
        EasyTouch.On_TouchDown2Fingers -= On_TouchDown2Fingers;
        EasyTouch.On_TouchUp2Fingers -= On_TouchUp2Fingers;
    }

    private void On_TouchStart(Gesture gesture)
    {
        if (gesture.isOverGui && drag)
        {
            if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)) && fingerId == -1)
            {
                fingerId = gesture.fingerIndex;
                transform.SetAsLastSibling();
            }
        }
    }

    private void On_TouchDown(Gesture gesture)
    {
        if (fingerId == gesture.fingerIndex && drag)
        {
            if (gesture.isOverGui)
            {
                if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)))
                {
                    transform.position += (Vector3)gesture.deltaPosition;
                }
            }
        }
    }

    private void On_TouchUp(Gesture gesture)
    {
        if (fingerId == gesture.fingerIndex)
        {
            fingerId = -1;
        }
    }

    private void On_TouchStart2Fingers(Gesture gesture)
    {
        if (gesture.isOverGui && drag)
        {
            if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)) && fingerId == -1)
            {
                transform.SetAsLastSibling();
            }
        }
    }

    private void On_TouchDown2Fingers(Gesture gesture)
    {
        if (gesture.isOverGui)
        {
            if (gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform))
            {
                if ((gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform)))
                {
                    transform.position += (Vector3)gesture.deltaPosition;
                }
                drag = false;
            }
        }
    }

    private void On_TouchUp2Fingers(Gesture gesture)
    {
        if (gesture.isOverGui)
        {
            if (gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform))
            {
                drag = true;
            }
        }
    }
}