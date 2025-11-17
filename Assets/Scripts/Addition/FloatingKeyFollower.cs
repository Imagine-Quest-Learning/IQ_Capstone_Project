using UnityEngine;

/*
    Description: Makes the golden key follow a target (usually the Player)
                 with a small offset and floating (bobbing) animation.

    Written By: Jiayi(Jemma) [2025]
*/
public class FloatingKeyFollower : MonoBehaviour
{
    public Transform target;
    public Vector2 offset = new Vector2(0.6f, 0.5f);
    public float followLerp;
    public float bobAmplitude;
    public float bobFrequency;

    // Find the Player automatically if no target is set in the Inspector
    void Awake()
    {
        if (!target)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }
    }

    // Each frame, move the key toward the target with offset and bobbing
    void LateUpdate()
    {
        if (!target) return;

        float flip = target.lossyScale.x < 0f ? -1f : 1f;
        Vector3 desiredOffset = new Vector3(offset.x * flip, offset.y, 0f);

        float bob = Mathf.Sin(Time.time * Mathf.PI * 2f * bobFrequency) * bobAmplitude;

        Vector3 targetPos = target.position + desiredOffset + new Vector3(0f, bob, 0f);

        float t = 1f - Mathf.Exp(-followLerp * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, targetPos, t);
    }
}
