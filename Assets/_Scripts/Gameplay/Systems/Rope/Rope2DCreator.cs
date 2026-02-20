using UnityEngine;

public class Rope2DCreator : MonoBehaviour
{
    [SerializeField, Range(2, 50)] int segmentsCount = 5;

    public Transform pointA;
    public Transform pointB;

    public HingeJoint2D hingePrefab;

    [HideInInspector] public Transform[] segments;

    Vector2 GetSegmentPosition(int segmentIndex)
    {
        Vector2 posA = pointA.position;
        Vector2 posB = pointB.position;

        float fraction = 1f / segmentsCount;
        return Vector2.Lerp(posA, posB, fraction * segmentIndex);
    }

    [ContextMenu("Generate Rope")]
    void GenerateRope()
    {
        DeleteSegments();
        segments = new Transform[segmentsCount];

        float normalized = Mathf.InverseLerp(2, 50, segmentsCount);

        float massPerSegment = Mathf.Lerp(0.6f, 0.2f, normalized);
        float linearDamping = Mathf.Lerp(0.05f, 0.1f, normalized);
        float angularDamping = Mathf.Lerp(0.1f, 0.3f, normalized);
        float gravityScale = Mathf.Lerp(1.2f, 0.8f, normalized);
        float hingeLimitDegrees = Mathf.Lerp(40f, 30f, normalized);


        for (int i = 0; i < segmentsCount; i++)
        {
            var joint = Instantiate(
                hingePrefab,
                GetSegmentPosition(i),
                Quaternion.identity,
                transform
            );

            segments[i] = joint.transform;

            Rigidbody2D rb = joint.GetComponent<Rigidbody2D>();

            rb.mass = massPerSegment;
            rb.linearDamping = linearDamping;
            rb.angularDamping = angularDamping;
            rb.gravityScale = gravityScale;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;

            if (i == 0)
            {
                rb.mass = massPerSegment * 4f;
                rb.angularDamping *= 1.6f;
            }

            joint.autoConfigureConnectedAnchor = true;
            joint.enableCollision = false;
            joint.useLimits = true;

            JointAngleLimits2D limits = joint.limits;
            limits.min = -hingeLimitDegrees;
            limits.max = hingeLimitDegrees;
            joint.limits = limits;

            // Connect chain
            if (i > 0)
            {
                joint.connectedBody =
                    segments[i - 1].GetComponent<Rigidbody2D>();
            }
        }
    }

    [ContextMenu("Delete Rope")]
    void DeleteSegments()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }

        segments = null;
    }
}
