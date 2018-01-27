using UnityEngine;


public class PlayerController : RaycastController
{
    float maxClimbAngle = 80f;
    float maxDescendAngle = 75f;

    public CollisionInfo collisionInfo;
    [HideInInspector]
    public Vector2 playerInput;

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector2 deltaMovementOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset ()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0f;
        }
    }

    public override void Start ()
    { 
        base.Start ();
        collisionInfo.faceDir = 1;
    }

    public void Move ( Vector2 deltaMovement, bool standingOnPlatform = false )
    {
        Move ( deltaMovement, Vector2.zero, standingOnPlatform );
    }

    public void Move ( Vector2 deltaMovement, Vector2 input, bool standingOnPlatform = false )
    {
        UpdateRaycastOrigins ();
        collisionInfo.Reset ();
        collisionInfo.deltaMovementOld = deltaMovement;
        playerInput = input;

        if ( deltaMovement.x != 0 )
        {
            collisionInfo.faceDir = (int)Mathf.Sign ( deltaMovement.x );
        }

        if ( deltaMovement.y < 0 )
        {
            DescendSlope ( ref deltaMovement );
        }

        HorizontalCollisions ( ref deltaMovement );

        if ( deltaMovement.y != 0 )
        {
            VerticalCollisions ( ref deltaMovement );
        }

        transform.Translate ( deltaMovement );

        if ( standingOnPlatform )
        {
            collisionInfo.below = true;
        }
    }

    void HorizontalCollisions ( ref Vector2 deltaMovement )
    {
        float directionX = collisionInfo.faceDir;
        float rayLength = Mathf.Abs ( deltaMovement.x ) + skinWidth;

        if ( Mathf.Abs(deltaMovement.x) < skinWidth )
        {
            rayLength = 2 * skinWidth;
        }

        for ( int i = 0; i < horizontalRayCount; i++ )
        {
            // Check if we are moving left or right
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;

            rayOrigin += Vector2.up * ( horizontalRaySpacing * i );
            RaycastHit2D hit = Physics2D.Raycast ( rayOrigin, Vector2.right * directionX, rayLength, collisionMask );

            //Debug.DrawRay ( rayOrigin, Vector2.right * directionX, Color.red );

            if ( hit )
            {
                Debug.DrawRay ( rayOrigin, hit.point, Color.red );
                // If the ray is casted inside an object we skip to the next ray
                if ( hit.distance == 0 )
                {
                    Debug.Log ( "Hit distance is 0" );
                    continue;                
                }
                float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
                if ( i == 0 && slopeAngle <= maxClimbAngle )
                {
                    if ( collisionInfo.descendingSlope )
                    {
                        collisionInfo.descendingSlope = false;
                        deltaMovement = collisionInfo.deltaMovementOld;
                    }
                    float distanceToSlopeStart = 0;
                    if ( slopeAngle != collisionInfo.slopeAngleOld )
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        deltaMovement.x -= distanceToSlopeStart * directionX;
                    }

                    ClimbSlope ( ref deltaMovement, slopeAngle );
                    deltaMovement.x += distanceToSlopeStart * directionX;
                }

                if ( !collisionInfo.climbingSlope || slopeAngle > maxClimbAngle )
                {

                    deltaMovement.x = ( hit.distance - skinWidth ) * directionX;
                    rayLength = hit.distance;

                    if ( collisionInfo.climbingSlope )
                    {
                        deltaMovement.y = Mathf.Tan ( collisionInfo.slopeAngle * Mathf.Deg2Rad ) * Mathf.Abs ( deltaMovement.x );
                    }

                    collisionInfo.left = directionX == -1;
                    collisionInfo.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions ( ref Vector2 deltaMovement )
    {
        float directionY = Mathf.Sign (deltaMovement.y);
        float rayLength = Mathf.Abs( deltaMovement.y ) + skinWidth;

        for ( int i = 0; i < verticalRayCount; i++ )
        {
            // Check if we are moving up or down
            Vector2 rayOrigin = ( directionY == -1 ) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * ( verticalRaySpacing * i + deltaMovement.x );

            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            //Debug.DrawRay ( rayOrigin, Vector2.up * directionY, Color.red );

            if ( hit )
            {
                Debug.DrawRay ( rayOrigin, hit.point, Color.green );
                if ( ( hit.collider.tag ) == "Through" )
                {
                    if ( directionY == 1 || hit.distance == 0)
                    {
                        continue;
                    }
                    if ( collisionInfo.fallingThroughPlatform )
                    {
                        continue;
                    }
                    if ( playerInput.y == -1 )
                    {
                        collisionInfo.fallingThroughPlatform = true;
                        Invoke ( "ResetFallingThroughPlatform", .5f );
                        continue;
                    }
                }
        
                deltaMovement.y = ( hit.distance - skinWidth ) * directionY;
                rayLength = hit.distance;

                if ( collisionInfo.climbingSlope )
                {
                    deltaMovement.x = deltaMovement.y / Mathf.Tan ( collisionInfo.slopeAngle * Mathf.Deg2Rad ) * Mathf.Sign ( deltaMovement.x );
                }

                collisionInfo.below = directionY == -1;
                collisionInfo.above = directionY == 1;
            }
        }

        if ( collisionInfo.climbingSlope )
        {
            float directionX = Mathf.Sign (deltaMovement.x);
            rayLength = Mathf.Abs ( deltaMovement.x ) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * deltaMovement.y;
            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if ( hit )
            {
                float slopeAngle = Vector2.Angle ( hit.normal, Vector2.up );
                if ( slopeAngle != collisionInfo.slopeAngle )
                {
                    deltaMovement.x = ( hit.distance - skinWidth ) * directionX;
                    collisionInfo.slopeAngle = slopeAngle;
                }

            }
        }
    }

    void ClimbSlope ( ref Vector2 deltaMovement, float slopeAngle )
    {
        float moveDistance = Mathf.Abs ( deltaMovement.x);
        float climbdeltaMovementY = Mathf.Sin ( slopeAngle * Mathf.Deg2Rad ) * moveDistance;

        if ( deltaMovement.y <= climbdeltaMovementY )
        {
            deltaMovement.y = climbdeltaMovementY;
            deltaMovement.x = Mathf.Cos ( slopeAngle * Mathf.Deg2Rad ) * moveDistance * Mathf.Sign ( deltaMovement.x );

            collisionInfo.below = true;
            collisionInfo.climbingSlope = true;
            collisionInfo.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope ( ref Vector2 deltaMovement )
    {
        float directionX = Mathf.Sign (deltaMovement.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;

        RaycastHit2D hit = Physics2D.Raycast( rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask );

        if ( hit )
        {
            float slopeAngle = Vector2.Angle ( hit.normal, Vector2.up );
            if ( slopeAngle != 0 && slopeAngle <= maxDescendAngle )
            {
                if ( Mathf.Sign ( hit.normal.x ) == directionX )
                {
                    if ( hit.distance - skinWidth <=  Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (deltaMovement.x))
                    {
                        float moveDistance = Mathf.Abs( deltaMovement.x) ;
                        float descenddeltaMovementY = Mathf.Sin ( slopeAngle * Mathf.Deg2Rad ) * moveDistance;
                        deltaMovement.x = Mathf.Cos ( slopeAngle * Mathf.Deg2Rad ) * moveDistance * Mathf.Sign ( deltaMovement.x );
                        deltaMovement.y -= descenddeltaMovementY;

                        collisionInfo.slopeAngle = slopeAngle;
                        collisionInfo.descendingSlope = true;
                        collisionInfo.below = true;
                    }
                }
            }
        }
    }

    void ResetFallingThroughPlatform ()
    {
        collisionInfo.fallingThroughPlatform = false;
    }
}
