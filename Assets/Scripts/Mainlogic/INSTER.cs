using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

public class INSTER : MonoBehaviour
{
    public GameObject PhotonEngineINS;

    long The_Rest_Of_mylifetime = 3153600000;
    public bool Alive;
    private void Awake()
    {
        Alive = true;
        InvokeRepeating("MyLifeTime", 1, 1);
    }
    public void MyLifeTime()
    {
        The_Rest_Of_mylifetime--;
        if (The_Rest_Of_mylifetime == 0)
        {
            Alive = false;
            Die();
        }
    }




    // Start is called before the first frame update
    private void Start()
    {
        if (GameObject.Find("PhotonEngine(Clone)") == null)
        {
            Instantiate(PhotonEngineINS);
        }

        Destroy(this.gameObject);
    }

    // Update is called once per frame
    private void Update()
    {
    }


    private void Die()
    {
        throw new NotImplementedException();
    }
}