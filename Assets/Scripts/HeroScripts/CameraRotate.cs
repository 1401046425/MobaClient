using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform target;//旋转目标

    public float distance = 1.8f;//摄像机和目标之间的距离

    private float speedX = 240f;//x轴转速
    private float speedY = 120f;//y轴转速

    private float mX = 0.0f;//摄像机的旋转角度x
    private float mY = 0.0f;//摄像机的旋转角度y

    private float maxDistance = 5f; //摄像机最远
    private float minDistance = 1.2f;//摄像机最近

    private float zoomSpeed = 0.2f;//摄像机的前进速度（放大）

    public bool isNeedDamping = true;//是否平滑过渡

    public float damping = 1f;//平滑过渡速度

    public Joystick joystick;

    // Use this for initialization
    private void Start()
    {
        //开始的时候获得角度
        mX = transform.eulerAngles.x;
        //mY = transform.eulerAngles.y;
    }

    // Update is called once per frame
    private void Update()
    {
        if (target != null && Input.GetMouseButton(0))
        {
            //获得鼠标右键拖拽
            mX += joystick.Horizontal * speedX * Time.deltaTime;
            //mY -= Input.GetAxis("Mouse Y") * speedY * Time.deltaTime;

            //射线机的旋转(有万向锁问题)
            Quaternion mRotation = Quaternion.Euler(mY, mX, 0);
            Vector3 mPosition = mRotation * new Vector3(0, 0.5f, -1f) + target.position;
            transform.rotation = mRotation;
            transform.position = mPosition;
        }

        ////是否平滑过渡
        //if (isNeedDamping)
        //{
        //    //在from和to之间旋转-插值法
        //    transform.rotation = Quaternion.Lerp(transform.rotation, mRotation, Time.deltaTime * damping);
        //    transform.position = Vector3.Lerp(transform.position, mPosition, Time.deltaTime * damping);
        //}
        //else
        //{
        //    transform.rotation = mRotation;
        //    transform.position = mPosition;
        //}

        //不平滑移动
    }

    //限制角度
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}