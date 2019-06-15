using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       MiniMap.Instance.AddIcon(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        MiniMap.Instance.RemoveIcon(this);
    }

}
