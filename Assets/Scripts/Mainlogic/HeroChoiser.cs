using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeroChoiser : MonoBehaviour,IPointerClickHandler
{
    public HeroType HeroType;

    public void OnPointerClick(PointerEventData eventData)
    {
        MatchingRequest.Instance.InsChoiseHero(HeroType);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
