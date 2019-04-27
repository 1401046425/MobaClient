using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDBtn : MonoBehaviour
{
    Image cdimage;
    private ETCButton ETCbtn;
    public float CDTime;
    // Start is called before the first frame update
    void Start()
    {
        cdimage = transform.Find("CDImage").GetComponent<Image>();
        ETCbtn = GetComponent<ETCButton>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButton()
    {
        cdimage.fillAmount = 1;
        StartCoroutine(StartCD());
    }
    IEnumerator StartCD()
    {
        var time = Time.time + CDTime;
        ETCbtn.activated = false;
        while (true)
        {
            cdimage.fillAmount = (time - Time.time) / CDTime;
            if (time <= Time.time)
            {
                ETCbtn.activated = true;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
