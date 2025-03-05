using UnityEngine;

public class enemywalk : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    private Rigidbody2D rigid;

    [Header("Ground Check")]
    public LayerMask groundLayer;         // 在 Inspector 指定为包含“Ground”这一层
    public Transform groundCheck;         // 在角色脚底的子物体
    public float checkRadius = 0.2f;      // 检测范围
    private bool isGrounded;              // 是否在地面上

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 每帧都检测地面
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
    }

    void FixedUpdate()
    {
        // 如果 isGrounded 为 false，就说明脚下没有地面，可以让角色下落或保持现状
        // 如果 true，就执行原本的左右移动

        // 你的原有移动逻辑：根据方向向左/向右移动
        Vector3 front = new Vector3(-1, 0, 0);

        if (transform.localScale.x > 0)
        {
            front = new Vector3(1, 0, 0);
        }

        // 只有站在地面上时才移动水平方向
        if (isGrounded)
        {
            rigid.linearVelocity = new Vector2(front.x * speed, rigid.linearVelocity.y);
        }
        // 否则，不做任何水平速度修改，让刚体的重力继续下落
    }

    // 你也可以在 OnCollisionEnter2D 中做翻转之类的逻辑，
    // 或者检测“墙壁”来掉头，这里不赘述。
}
