using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public static MiniMap Instance;
    public GameObject Icon;
    private Dictionary<MiniMapObj,RectTransform> IconDic=new Dictionary<MiniMapObj, RectTransform>();
    WaitForSeconds Wait_time= new WaitForSeconds(0.1f);
    private void Awake()
    {
        Instance = this;
    }

    IEnumerator UpdateIcon()
    {
        while (true)
        {
            foreach (var item in IconDic)
            {
                print("更新坐标"+item.Value.name+":"+item.Value.localPosition);
                item.Value.localPosition = item.Key.transform.position *1.9f;
            }

            yield return Wait_time;
        }

    }
    public void AddIcon(MiniMapObj OBJ)
    {
        IconDic.Add(OBJ,Instantiate(Icon,Vector3.zero,Quaternion.identity).GetComponent<RectTransform>());
        //设置为minimap子对象
        IconDic[OBJ].parent = this.transform;
        IconDic[OBJ].localPosition = OBJ.transform.position * 1.9f;
        print("生成一个ICON"+OBJ.name+":"+IconDic[OBJ].localPosition);
    }

    public void RemoveIcon(MiniMapObj OBJ)
    {
        Destroy(IconDic[OBJ].gameObject);
        IconDic.Remove(OBJ);

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateIcon());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
