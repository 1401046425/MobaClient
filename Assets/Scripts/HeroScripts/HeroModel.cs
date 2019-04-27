using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public int HP = 100;
    public bool ISME = false;
    public int PlayerIndex;
    public HeroAttack Hero_Attack;
    public HeroMove Hero_Move;
    public Animator animator;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}