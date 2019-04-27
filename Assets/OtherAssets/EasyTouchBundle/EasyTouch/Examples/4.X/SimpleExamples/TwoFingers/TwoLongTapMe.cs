using HedgehogTeam.EasyTouch;
using UnityEngine;

public class TwoLongTapMe : MonoBehaviour
{
    private TextMesh textMesh;
    private Color startColor;

    // Subscribe to events
    private void OnEnable()
    {
        EasyTouch.On_LongTapStart2Fingers += On_LongTapStart2Fingers;
        EasyTouch.On_LongTap2Fingers += On_LongTap2Fingers;
        EasyTouch.On_LongTapEnd2Fingers += On_LongTapEnd2Fingers;
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
        EasyTouch.On_LongTapStart2Fingers -= On_LongTapStart2Fingers;
        EasyTouch.On_LongTap2Fingers -= On_LongTap2Fingers;
        EasyTouch.On_LongTapEnd2Fingers -= On_LongTapEnd2Fingers;
        EasyTouch.On_Cancel2Fingers -= On_Cancel2Fingers;
    }

    private void Start()
    {
        textMesh = (TextMesh)GetComponentInChildren<TextMesh>();
        startColor = gameObject.GetComponent<Renderer>().material.color;
    }

    // At the long tap beginning
    private void On_LongTapStart2Fingers(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            RandomColor();
        }
    }

    // During the long tap
    private void On_LongTap2Fingers(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            textMesh.text = gesture.actionTime.ToString("f2");
        }
    }

    // At the long tap end
    private void On_LongTapEnd2Fingers(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            gameObject.GetComponent<Renderer>().material.color = startColor;
            textMesh.text = "Long tap me";
        }
    }

    // If the two finger gesture is finished
    private void On_Cancel2Fingers(Gesture gesture)
    {
        On_LongTapEnd2Fingers(gesture);
    }

    private void RandomColor()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}