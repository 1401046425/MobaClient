using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform HBtransform = null;
    private Vector3 Offset;
    public static CameraFollow cameraFollow;
    public Transform LookPOS;
    public float DIRdistance;
    public float UpHead;

    private void Awake()
    {
        cameraFollow = this;
    }

    // Use this for initialization
    private void Start()
    {
        //transform.rotation = Quaternion.LookRotation(LookPOS.position-this.transform.position);
    }

    public void Init(Transform pltransform, Transform CameraTransForm)
    {
        transform.position = CameraTransForm.position;
        HBtransform = pltransform;
        transform.LookAt(HBtransform.position + new Vector3(0, UpHead, 0));

        Offset = HBtransform.position - transform.position;
    }

    // Update is called once per framei
    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if (BattleFieldManager.Instance.GetEntity(BattleFieldManager.Instance.MyPlayerIndex) != null && HBtransform.position != null && Offset.x != 0)
        {
            transform.position = HBtransform.position - Offset * DIRdistance;
        }
    }
}