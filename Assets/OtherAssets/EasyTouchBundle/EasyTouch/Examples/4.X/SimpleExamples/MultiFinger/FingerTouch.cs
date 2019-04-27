using HedgehogTeam.EasyTouch;
using UnityEngine;

public class FingerTouch : MonoBehaviour
{
    private TextMesh textMesh;
    public Vector3 deltaPosition = Vector2.zero;
    public int fingerId = -1;

    private void OnEnable()
    {
        EasyTouch.On_TouchStart += On_TouchStart;
        EasyTouch.On_TouchUp += On_TouchUp;
        EasyTouch.On_Swipe += On_Swipe;
        EasyTouch.On_Drag += On_Drag;
        EasyTouch.On_DoubleTap += On_DoubleTap;
        textMesh = (TextMesh)GetComponentInChildren<TextMesh>();
    }

    private void OnDestroy()
    {
        EasyTouch.On_TouchStart -= On_TouchStart;
        EasyTouch.On_TouchUp -= On_TouchUp;
        EasyTouch.On_Swipe -= On_Swipe;
        EasyTouch.On_Drag -= On_Drag;
        EasyTouch.On_DoubleTap -= On_DoubleTap;
    }

    private void On_Drag(Gesture gesture)
    {
        if (gesture.pickedObject.transform.IsChildOf(gameObject.transform) && fingerId == gesture.fingerIndex)
        {
            Vector3 position = gesture.GetTouchToWorldPoint(gesture.pickedObject.transform.position);
            transform.position = position - deltaPosition;
        }
    }

    private void On_Swipe(Gesture gesture)
    {
        if (fingerId == gesture.fingerIndex)
        {
            Vector3 position = gesture.GetTouchToWorldPoint(transform.position);
            transform.position = position - deltaPosition;
        }
    }

    private void On_TouchStart(Gesture gesture)
    {
        if (gesture.pickedObject != null && gesture.pickedObject.transform.IsChildOf(gameObject.transform))
        {
            fingerId = gesture.fingerIndex;
            textMesh.text = fingerId.ToString();

            Vector3 position = gesture.GetTouchToWorldPoint(gesture.pickedObject.transform.position);
            deltaPosition = position - transform.position;
        }
    }

    private void On_TouchUp(Gesture gesture)
    {
        if (gesture.fingerIndex == fingerId)
        {
            fingerId = -1;
            textMesh.text = "";
        }
    }

    public void InitTouch(int ind)
    {
        fingerId = ind;
        textMesh.text = fingerId.ToString();
    }

    private void On_DoubleTap(Gesture gesture)
    {
        if (gesture.pickedObject != null && gesture.pickedObject.transform.IsChildOf(gameObject.transform))
        {
            DestroyImmediate(transform.gameObject);
        }
    }
}