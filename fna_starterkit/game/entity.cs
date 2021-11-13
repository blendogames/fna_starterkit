using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace fna_starterkit
{
    //Basic game object. Renders a 3D model.
    public class Entity
    {
        protected Model myModel;
        protected Matrix[] transforms;

        protected Vector3 modelPosition;
        public Vector3 GetPosition { get { return modelPosition; } }

        protected Matrix modelRotation;
        public Matrix GetRotation { get { return modelRotation; } }

        protected Vector3 color;
        public Vector3 GetColor { get { return color; } }

        protected bool shaded;

        protected float scale;

        public Entity(string modelPath)
        {
            modelPosition = Vector3.Zero;

            try
            {
                //Attempt to load model.
                myModel = Globals.screenManager.Content.Load<Model>(modelPath.Substring(0, modelPath.LastIndexOf(".xnb")));
            }
            catch (Exception e)
            {
                Helpers.FatalPopup("Failed to load model:\n'{0}'\n\nError: {1}", modelPath, e.Message);
            }

            transforms = new Matrix[myModel.Bones.Count];
            modelRotation = Matrix.Identity;

            color = new Vector3(.64f, .64f, .64f);
            shaded = true;
            scale = 1.0f;
        }

        public virtual void SetScale(float value)
        {
            scale = value;
        }

        public virtual void SetShaded(bool value)
        {
            shaded = value;
        }

        public virtual void SetRotation(Matrix _matrix)
        {
            modelRotation = _matrix;
        }

        public virtual void SetPosition(Vector3 _position)
        {
            modelPosition = _position;
        }

        public virtual void SetColor(Vector3 _color)
        {
            color = _color;
        }

        public void SetModel(string path)
        {
            try
            {
                myModel = Globals.screenManager.Content.Load<Model>(path.Substring(0, path.LastIndexOf(".xnb")));
            }
            catch (Exception e)
            {
                Helpers.ErrorPopup(string.Format("Failed to load model:\n{0}\n\n{1}", path, e.Message));
            }

            transforms = new Matrix[myModel.Bones.Count];
        }

        public virtual void Draw3D(GameTime gameTime, Camera3D camera)
        {
            myModel.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.LightingEnabled = shaded;

                    if (shaded)
                    {
                        effect.DiffuseColor = color;
                        effect.DirectionalLight0.DiffuseColor = new Vector3(.7f, .7f, .7f);
                        Vector3 lightAngle = new Vector3(20,-60, -60);
                        lightAngle.Normalize();
                        effect.DirectionalLight0.Direction = lightAngle;
                        effect.AmbientLightColor = new Vector3(.3f, .3f, .3f);
                    }
                    else
                    {
                        effect.DiffuseColor = color;
                        effect.AmbientLightColor = new Vector3(1,1,1);
                    }
                    

                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                    effect.World = transforms[mesh.ParentBone.Index] * modelRotation * Matrix.CreateScale(scale) * Matrix.CreateTranslation(modelPosition) ;
                }

                mesh.Draw();
            }

            
            
            
        }

        // --- end of file
    }
}