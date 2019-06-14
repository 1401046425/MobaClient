using UnityEngine;

public class EntityModel : MonoBehaviour,IHurtObject
{
    public bool IsDead;

    public BattleFieldCamp Camp;
    public int Health;
    public int Index;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnDisable()
    {
        IsDead = true;
    }

    private void OnEnable()
    {
        IsDead = false;
    }

    private void OnDestroy()
    {
        IsDead = true;
    }

    public void SendHurtRequest(int HurtValue, int ObjectID)
    {
        if(BattleFieldManager.Instance.GetEntity(ObjectID)!=null&&BattleFieldManager.Instance.GetEntity(ObjectID).Camp==BattleFieldManager.Instance.MyCamp)
            BattleFieldRequest.Instance.HurtRequest(Index, HurtValue);
    }
}