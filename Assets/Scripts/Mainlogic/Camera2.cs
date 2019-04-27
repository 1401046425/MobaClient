using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{
    public static Camera2 instence;
    private GameObject MatchingPanel;
    public GameObject ChoosePanel;
    public GameObject ChooseScene;
    private Animator Cmera2Animator;

    // Start is called before the first frame update
    private void Awake()
    {
        instence = this;
    }

    private void Start()
    {
        Cmera2Animator = GetComponent<Animator>();
        MatchingPanel = GameObject.Find("MatchingPanel");
    }

    public void StartChooseHeroAnimation()
    {
        MatchingPanel.SetActive(false);
        ChooseScene.SetActive(true);
        Cmera2Animator.SetTrigger("Anim1");
        Invoke("InsChoosePanel", 4);
    }

    public void InsChoosePanel()
    {
        if (ChoosePanel != null)
            ChoosePanel.SetActive(true);
    }

    public void StartGame()
    {
        if (MatchingRequest.Instance.INSHero == null) return;//判断当前选中的英雄是否为空
        GameObject.Find("ChoosePanel").SetActive(false);
        Cmera2Animator.SetTrigger("Anim2");
        //销毁特效
        Destroy(MatchingRequest.Instance.SpawnFX);
        Destroy(MatchingRequest.Instance.SpawnFX2);
        var HeroChooseCtr = MatchingRequest.Instance.INSHero.GetComponent<ChooseHeroCtr>();
        HeroChooseCtr.PrepareJoin();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}