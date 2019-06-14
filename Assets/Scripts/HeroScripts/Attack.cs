using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Attack : MonoBehaviour, IPointerClickHandler
{
    public static Attack GetAttack;
    private HeroAttack _HeroAtk = null;

    private void Awake()
    {
        GetAttack = this;
    }

    public void InitAttack()
    {
        if (BattleFieldManager.Instance.MyPlayerIndex != 0)
        {
            _HeroAtk = BattleFieldManager.Instance.GetEntity(BattleFieldManager.Instance.MyPlayerIndex).GetComponent<HeroAttack>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BattleFieldManager.Instance.OnAttack();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}