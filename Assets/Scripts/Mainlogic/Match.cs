using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Matching()
    {
        MatchingRequest.Instance.MatchingStartRequest();
    }
}