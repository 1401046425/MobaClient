using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawn : MonoBehaviour
{
    private Coroutine Coroutine;
    [HideInInspector] public int MaxSoldierSpawn = 3;
    public GameObject SoldierGameObj;

    public List<GameObject> FinalTargetList;
    public int Index=0;
    public BattleFieldCamp Camp;
    // Start is called before the first frame update
    private void Start()
    {
        Coroutine = StartCoroutine(SpawnSoldier());
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {
        StopCoroutine(Coroutine);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private IEnumerator SpawnSoldier()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            for (int i = 0; i < MaxSoldierSpawn; i++)
            {
                var SoldierIns = Instantiate(SoldierGameObj, this.transform.position, Quaternion.identity);

                SoldierIns.GetComponent<Soldier>().init(FinalTargetList,Camp,6000+Index+(byte)Camp*100, Camp != BattleFieldManager.Instance.MyCamp);

                BattleFieldManager.Instance.SoldierList.Add(SoldierIns.GetComponent<Soldier>());
                Index++;
                if (Index == 99)
                {
                    Index = 0;
                }

                yield return new WaitForSeconds(3f);
            }
            yield return new WaitForSeconds(9f);
        }
    }


}