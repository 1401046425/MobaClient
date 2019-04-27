using HedgehogTeam.EasyTouch;
using UnityEngine;

public class DoubleTapMe : MonoBehaviour
{
    // Subscribe to events
    private void OnEnable()
    {
        EasyTouch.On_DoubleTap += On_DoubleTap;
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
        EasyTouch.On_DoubleTap -= On_DoubleTap;
    }

    // Double tap
    private void On_DoubleTap(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        }
    }
}