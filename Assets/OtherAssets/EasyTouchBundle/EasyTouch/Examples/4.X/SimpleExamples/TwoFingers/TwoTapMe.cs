using HedgehogTeam.EasyTouch;
using UnityEngine;

public class TwoTapMe : MonoBehaviour
{
    // Subscribe to events
    private void OnEnable()
    {
        EasyTouch.On_SimpleTap2Fingers += On_SimpleTap2Fingers;
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
        EasyTouch.On_SimpleTap2Fingers -= On_SimpleTap2Fingers;
    }

    // Simple 2 fingers tap
    private void On_SimpleTap2Fingers(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            RandomColor();
        }
    }

    private void RandomColor()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}