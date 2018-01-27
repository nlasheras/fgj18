using UnityEngine;

[RequireComponent ( typeof ( BoxCollider2D ) )]
public class RaycastController : MonoBehaviour {

    public LayerMask collisionMask;

    protected int horizontalRayCount;
    protected int verticalRayCount;

    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    [HideInInspector]
    public BoxCollider2D boxCollider;
    protected RaycastOrigins raycastOrigins;

    protected const float skinWidth = 0.01f;
    protected const float dstBetweenRays = .125f;

    protected struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    // Use this for initialization
    public virtual void Awake ()
    {
        boxCollider = GetComponent<BoxCollider2D> ();
    }

    // Use this for initialization
    public virtual void Start ()
    {
        CalculateRaySpacing ();
    }

    protected void CalculateRaySpacing ()
    {
        // Shrink the bounds by skinwidth
        Bounds bounds = boxCollider.bounds;
        bounds.Expand ( skinWidth * -2f );

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt ( boundsHeight / dstBetweenRays );
        verticalRayCount = Mathf.RoundToInt ( boundsWidth / dstBetweenRays );

        horizontalRaySpacing = bounds.size.y / ( horizontalRayCount - 1 );
        verticalRaySpacing = bounds.size.x / ( verticalRayCount - 1 );
    }

    protected void UpdateRaycastOrigins ()
    {
        // Shrink the bounds by skinwidth
        Bounds bounds = boxCollider.bounds;
        bounds.Expand ( skinWidth * -2f );

        raycastOrigins.bottomLeft = new Vector2 ( bounds.min.x, bounds.min.y );
        raycastOrigins.bottomRight = new Vector2 ( bounds.max.x, bounds.min.y );
        raycastOrigins.topLeft = new Vector2 ( bounds.min.x, bounds.max.y );
        raycastOrigins.topRight = new Vector2 ( bounds.max.x, bounds.max.y );
    }
}
