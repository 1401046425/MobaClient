using HedgehogTeam.EasyTouch;
using UnityEngine;

public class TwoTouchMe : MonoBehaviour
{
    private TextMesh textMesh;
    private Color startColor;

    // Subscribe to events
    private void OnEnable()
    {
        EasyTouch.On_TouchStart2Fingers += On_TouchStart2Fingers;
        EasyTouch.On_TouchDown2Fingers += On_TouchDown2Fingers;
        EasyTouch.On_TouchUp2Fingers += On_TouchUp2Fingers;
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
        EasyTouch.On_TouchDown2Fingers -= On_TouchDown2Fingers;
        EasyTouch.On_TouchUp2Fingers -= On_TouchUp2Fingers;
        EasyTouch.On_Cancel2Fingers -= On_Cancel2Fingers;
    }

    private void Start()
    {
        textMesh = (TextMesh)GetComponentInChildren<TextMesh>();
        startColor = gameObject.GetComponent<Renderer>().material.color;
    }

    private void On_TouchStart2Fingers(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            RandomColor();
        }
    }

    private void On_TouchDown2Fingers(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            textMesh.text = "Down since :" + gesture.actionTime.ToString("f2");
        }
    }

    private void On_TouchUp2Fingers(Gesture gesture)
    {
        // Verification that the action on the object
        if (gesture.pickedObject == gameObject)
        {
            gameObject.GetComponent<Renderer>().material.color = startColor;
            textMesh.text = "Touch me";
        }
    }

    private void On_Cancel2Fingers(Gesture gesture)
    {
        On_TouchUp2Fingers(gesture);
    }

    private void RandomColor()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
    }
}