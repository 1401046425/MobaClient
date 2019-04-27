using HedgehogTeam.EasyTouch;
using UnityEngine;

public class UIPinch : MonoBehaviour
{
    public void OnEnable()
    {
        EasyTouch.On_Pinch += On_Pinch;
    }

    public void OnDestroy()
    {
        EasyTouch.On_Pinch -= On_Pinch;
    }

    private void On_Pinch(Gesture gesture)
    {
        if (gesture.isOverGui)
        {
            if (gesture.pickedUIElement == gameObject || gesture.pickedUIElement.transform.IsChildOf(transform))
            {
                transform.localScale = new Vector3(transform.localScale.x + gesture.deltaPinch * Time.deltaTime, transform.localScale.y + gesture.deltaPinch * Time.deltaTime, transform.localScale.z);
            }
        }
    }
}