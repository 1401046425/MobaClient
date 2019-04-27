using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChooseHeroCtr : MonoBehaviour
{
    NavMeshAgent NavAi;
    Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        NavAi = GetComponent<NavMeshAgent>();
        Anim = GetComponent<Animator>();
    }
    public void PrepareJoin()
    {
        NavAi.SetDestination(GameObject.Find("PreparePos").transform.position);
        Anim.SetBool("IsWalk",true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (NavAi.hasPath)
        {
            Anim.SetBool("IsWalk", false);
        }
    }
}
