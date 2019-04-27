using HedgehogTeam.EasyTouch;
using UnityEngine;

public class TwoDoubleTapMe : MonoBehaviour
{
    // Subscribe to events
    private void OnEnable()
    {
        EasyTouch.On_DoubleTap2Fingers += On_DoubleTap2Fingers;
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
        EasyTouch.On_DoubleTap2Fingers -= On_DoubleTap2Fingers;
    }

    // Double Tap
    private void On_DoubleTap2Fingers(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
    }
}