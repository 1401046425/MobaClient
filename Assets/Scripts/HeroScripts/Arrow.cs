using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int OwnerIndex;
    public IHurtObject target;
    public float speed;

    public BattleFieldCamp ArrowCamp;
    public GameObject HurtFX;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("实列");
    }

    // Update is called once per frame
    private void Update()
    {
        if (target == null&&gameObject!=null)
        {
            Destroy(gameObject);
        }
        MonoBehaviour obj = target as MonoBehaviour;
        if (obj)
        {
            transform.position = Vector3.Lerp(transform.position, obj.transform.position + Vector3.up, speed * Time.deltaTime);
            transform.LookAt(obj.transform.position + Vector3.up);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("触发！！");
        if (collision.transform.CompareTag("Hero") && collision.transform.GetComponent<HeroModel>().Index != OwnerIndex)
        {
            //Instantiate(HurtFX1, new Vector3(collision.transform.position.x, collision.transform.position.y + 2.5f, collision.transform.position.z), Quaternion.identity);
            if (((HeroModel)BattleFieldManager.Instance.GetEntity(OwnerIndex)).ISME)
                target.SendHurtRequest(10, OwnerIndex);
            //BattleFieldRequest.Instance.HurtRequest(collision.transform.GetComponent<HeroMove>().Index, BattleFieldManager.Instance.GetEntity(Owner).GetComponent<HeroAttack>().Damage);
            Debug.Log("人物攻击");
            Instantiate(HurtFX, new Vector3(collision.transform.position.x, collision.transform.position.y + 1, collision.transform.position.z), Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (collision.transform.CompareTag("Tower"))
        {
            if (((HeroModel)BattleFieldManager.Instance.GetEntity(OwnerIndex)).ISME)
                target.SendHurtRequest(10, OwnerIndex);

            Instantiate(HurtFX, new Vector3(collision.transform.position.x, collision.transform.position.y + 5.6f, collision.transform.position.z), Quaternion.identity);

        }
        else if (collision.transform.CompareTag("Soldier"))
        {
            if (((HeroModel)BattleFieldManager.Instance.GetEntity(OwnerIndex)).ISME)
                target.SendHurtRequest(10, OwnerIndex);
            Instantiate(HurtFX, new Vector3(collision.transform.position.x, collision.transform.position.y + 1, collision.transform.position.z), Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}