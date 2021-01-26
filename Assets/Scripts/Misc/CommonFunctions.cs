using UnityEngine;

public static class CommonFunctions
{
    /// <summary>
    /// Clamps the ship in the screen
    /// </summary>
    /// <param name="shipPosition">Current position of the ship</param>
    /// <param name="colliderHalfWidth">Half of the ships width</param>
    /// <param name="colliderHalfHeight">Half of the ships height</param>
    /// <returns></returns>
    public static Vector2 ClampShipInScreen(Vector2 shipPosition, float colliderHalfWidth, float colliderHalfHeight)
    {
        //Horizontal Clamp
        shipPosition = ClampShipHorizontallyInScreen(shipPosition, colliderHalfWidth, colliderHalfHeight);

        //Vertical Clamp
        shipPosition = ClampShipVerticallyInScreen(shipPosition, colliderHalfWidth, colliderHalfHeight);

        return shipPosition;
    }

    /// <summary>
    /// Clamps the ship horizontally in the screen
    /// </summary>
    /// <param name="shipPosition">Current position of the ship</param>
    /// <param name="colliderHalfWidth">Half of the ships width</param>
    /// <param name="colliderHalfHeight">Half of the ships height</param>
    /// <returns></returns>
    public static Vector2 ClampShipHorizontallyInScreen(Vector2 shipPosition, float colliderHalfWidth, float colliderHalfHeight)
    {
        // clamp position as necessary
        Vector3 position = shipPosition;

        //Horizontal Clamp
        if (position.x + colliderHalfWidth > ScreenUtils.ScreenRight)
        {
            position.x = ScreenUtils.ScreenRight - colliderHalfWidth;
        }
        else if (position.x - colliderHalfWidth < ScreenUtils.ScreenLeft)
        {
            position.x = ScreenUtils.ScreenLeft + colliderHalfWidth;
        }

        return position;
    }

    /// <summary>
    /// Clamps the ship vertically in the screen
    /// </summary>
    /// <param name="shipPosition">Current position of the ship</param>
    /// <param name="colliderHalfWidth">Half of the ships width</param>
    /// <param name="colliderHalfHeight">Half of the ships height</param>
    /// <returns></returns>
    public static Vector2 ClampShipVerticallyInScreen(Vector2 shipPosition, float colliderHalfWidth, float colliderHalfHeight)
    {
        // clamp position as necessary
        Vector3 position = shipPosition;

        //Vertical Clamp
        if (position.y + colliderHalfHeight > ScreenUtils.ScreenTop)
        {
            position.y = ScreenUtils.ScreenTop - colliderHalfHeight;
        }
        else if (position.y - colliderHalfHeight < ScreenUtils.ScreenBottom)
        {
            position.y = ScreenUtils.ScreenBottom + colliderHalfHeight;
        }

        return position;
    }
}
