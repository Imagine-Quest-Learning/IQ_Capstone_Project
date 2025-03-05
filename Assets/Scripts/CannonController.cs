using UnityEngine;

public class CannonController : MonoBehaviour
{
    private Vector2 direction;

    [Header("References")]
    public Transform FirePoint;           // 炮口
    public GameObject CannonBall;         // 炮弹 Prefab
    public GameObject pointPrefab;        // 轨迹点 Prefab

    [Header("Trajectory Settings")]
    public int NumberOfPoints = 40;       // 轨迹点数量
    public float SpaceBetweenPoints = 0.1f;// 每个点之间的时间间隔

    [Header("Fire Settings")]
    public float FireForce = 30f;         // 炮弹初速度越大，打得越远

    [Header("Gravity Settings")]
    public bool OverrideGlobalGravity = false;
    public float CustomGravityY = -5f;    // 若 override, 脚本会设置物理重力为 (0, -5)

    private GameObject[] points;

    void Start()
    {
        // 可选：脚本里更改全局重力
        if (OverrideGlobalGravity)
        {
            Physics2D.gravity = new Vector2(0f, CustomGravityY);
        }

        // 初始化轨迹点
        points = new GameObject[NumberOfPoints];
        for (int i = 0; i < NumberOfPoints; i++)
        {
            points[i] = Instantiate(pointPrefab, FirePoint.position, Quaternion.identity);
        }
    }

    void Update()
    {
        // 让炮管朝向鼠标
        RotateCannonToMouse();

        // 点击鼠标就发射
        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }

        // 更新抛物线点的位置
        for (int i = 0; i < NumberOfPoints; i++)
        {
            float t = i * SpaceBetweenPoints;
            points[i].transform.position = CalculateTrajectoryPoint(t);
        }
    }

    void RotateCannonToMouse()
    {
        Vector2 cannonPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mousePos - cannonPos;
        transform.right = direction; // 炮管对准鼠标
    }

    void Fire()
    {
        GameObject ball = Instantiate(CannonBall, FirePoint.position, FirePoint.rotation);
        Rigidbody2D rb = ball.GetComponent<Rigidbody2D>();
        if (rb)
        {
            // 给炮弹初速度
            rb.linearVelocity = direction.normalized * FireForce;

            // (可选)单独调整炮弹重力
            // rb.gravityScale = 0.5f; 
            // (可选)减小Drag
            // rb.drag = 0f; 
        }
    }

    Vector2 CalculateTrajectoryPoint(float t)
    {
        // 标准抛体运动公式: pos(t) = startPos + v0 * t + 1/2 * g * t^2
        Vector2 startPos = FirePoint.position;
        Vector2 initialVelocity = direction.normalized * FireForce;
        return startPos + initialVelocity * t + 0.5f * Physics2D.gravity * (t * t);
    }
}
