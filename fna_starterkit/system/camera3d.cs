using System;

using Microsoft.Xna.Framework;

namespace fna_starterkit
{
    //Very basic 3D camera.
    public class Camera3D
    {
        const float NEAR_PLANE = 1;
        const float FAR_PLANE = 500;

        public Matrix ViewMatrix { get; private set; }
        public Matrix ProjectionMatrix { get; private set; }
        private float AspectRatio { get; set; } //Update this in case we resize window.
        private float FieldOfView { get; set; }
        public Vector3 cameraPosition;

        public Camera3D()
        {
            AspectRatio = Globals.screenManager.GraphicsDevice.Viewport.AspectRatio;
            FieldOfView = MathHelper.ToRadians(90); //fov
        }

        public void Update(GameTime gameTime)
        {
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(FieldOfView, AspectRatio, NEAR_PLANE, FAR_PLANE);
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraPosition + new Vector3(0, 0, -100), Vector3.Up);
        }

    }
}
