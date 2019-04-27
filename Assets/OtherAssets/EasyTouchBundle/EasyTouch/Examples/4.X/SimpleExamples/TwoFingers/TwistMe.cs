using HedgehogTeam.EasyTouch;
using UnityEngine;

public class TwistMe : MonoBehaviour
{
    private TextMesh textMesh;

    // Subscribe to events
    private void OnEnable()
    {
        EasyTouch.On_TouchStart2Fingers += On_TouchStart2Fingers;
        EasyTouch.On_Twist += On_Twist;
        EasyTouch.On_TwistEnd += On_TwistEnd;
        EasyTouch.On_Cancel2Fingers += On_Cancel2Fingers;
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }

    private void OnDestroy()
    {
        UnsubscribeEvent();
    }

    private void UnsubscribeEvent()
    {
        EasyTouch.On_TouchStart2Fingers -= On_TouchStart2Fingers;
        EasyTouch.On_Twist -= On_Twist;
        EasyTouch.On_TwistEnd -= On_TwistEnd;
        EasyTouch.On_Cancel2Fingers -= On_Cancel2Fingers;
    }

    private void Start()
    {
        textMesh = (TextMesh)GetComponentInChildren<TextMesh>();
    }

    private void On_TouchStart2Fingers(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            // disable twist gesture recognize for a real pinch end
            EasyTouch.SetEnableTwist(true);
            EasyTouch.SetEnablePinch(false);
        }
    }

    // during the txist
    private void On_Twist(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            transform.Rotate(new Vector3(0, 0, gesture.twistAngle));
            textMesh.text = "Delta angle : " + gesture.twistAngle.ToString();
        }
    }

    // at the twist end
    private void On_TwistEnd(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            EasyTouch.SetEnablePinch(true);
            transform.rotation = Quaternion.identity;
            textMesh.text = "Twist me";
        }
    }

    // If the two finger gesture is finished
    private void On_Cancel2Fingers(Gesture gesture)
    {
        EasyTouch.SetEnablePinch(true);
        transform.rotation = Quaternion.identity;
        textMesh.text = "Twist me";
    }
}