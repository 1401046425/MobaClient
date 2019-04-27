using UnityEngine;

public class CharacterAnimationDungeon : MonoBehaviour
{
    private CharacterController cc;
    private Animation anim;

    // Use this for initialization
    private void Start()
    {
        cc = GetComponentInChildren<CharacterController>();
        anim = GetComponentInChildren<Animation>();
    }

    // Wait end of frame to manage charactercontroller, because gravity is managed by virtual controller
    private void LateUpdate()
    {
        if (cc.isGrounded && (ETCInput.GetAxis("Vertical") != 0 || ETCInput.GetAxis("Horizontal") != 0))
        {
            anim.CrossFade("soldierRun");
        }

        if (cc.isGrounded && ETCInput.GetAxis("Vertical") == 0 && ETCInput.GetAxis("Horizontal") == 0)
        {
            anim.CrossFade("soldierIdleRelaxed");
        }

        if (!cc.isGrounded)
        {
            anim.CrossFade("soldierFalling");
        }
    }
}