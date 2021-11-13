using System;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace fna_starterkit
{
    //A grab bag of helper functions.
    public static class Helpers
    {
        public static readonly float PI2 = (float)Math.PI * 2;

        public static float CubicEaseIn(float lerpValue)
        {
            return lerpValue * lerpValue * lerpValue;
        }

        public static float CubicEaseOut(float lerpValue)
        {
            float f = (lerpValue - 1);
            return f * f * f + 1;
        }

        public static float CubicSmoothStep(float lerpValue)
        {
            if (lerpValue < .5f)
            {
                return 4 * lerpValue * lerpValue * lerpValue;
            }
            else
            {
                float f = ((2 * lerpValue) - 2);
                return .5f * f * f * f + 1;
            }
        }

        public static void FatalPopup(string bodyText, params string[] args)
        {
            ErrorPopup(bodyText + "\n\nEXITING NOW.", args);
            Environment.Exit(0);
        }

        public static void ErrorPopup(string bodyText, params string[] args)
        {
            SDL2.SDL.SDL_ShowSimpleMessageBox(
                SDL2.SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
                "ERROR!",
                string.Format(bodyText, args),
                IntPtr.Zero
            );
        }

        public static float PopLerp(float minSize, float maxSize, float normalSize, float transition)
        {
            if (transition >= 1)
                return normalSize;

            if (transition <= 0)
                return minSize;

            float popPoint = 0.6f;
            float popRemnant = 0.4f;

            if (transition < popPoint)
            {
                transition /= popPoint;
                return MathHelper.Lerp(minSize, maxSize, transition);
            }

            transition -= popPoint;
            transition /= popRemnant;

            return MathHelper.Lerp(maxSize, normalSize, transition);
        }


        public static float Pulse(GameTime gameTime, float amount, float pulseSpeed)
        {
            return (float)(amount * Math.Sin(gameTime.TotalGameTime.TotalSeconds * pulseSpeed));
        }

        public static string WordWrap(string text, float maxLineWidth, SpriteFont textFont)
        {
            string[] words = text.Split(' ');
            string workString = string.Empty;
            float currentLineWidth = 0;
            float spaceWidth = textFont.MeasureString(" ").X;

            foreach (string word in words)
            {
                Vector2 size = textFont.MeasureString(word);

                if (word.Contains("\n"))
                {
                    workString += (word + " ");
                    currentLineWidth = size.X + spaceWidth;
                }
                else if (currentLineWidth + size.X < maxLineWidth)
                {
                    workString += (word + " ");
                    currentLineWidth += size.X + spaceWidth;
                }
                else
                {
                    //do a line break. start a new line.
                    workString += "\n" + word + " ";
                    currentLineWidth = size.X + spaceWidth;
                }
            }

            return workString;
        }

        public static Vector2 GetAngledVector(float distance, float angle)
        {
            Vector2 basePos = new Vector2(0, 0);
            basePos.Y += (float)Math.Sin(angle) * distance;
            basePos.X += (float)Math.Cos(angle) * distance;

            return basePos;
        }

        public static float GetVectorAngle(Vector2 position, Vector2 targetPosition)
        {
            Vector2 direction = targetPosition - position;

            float adjacent = direction.X;
            float opposite = direction.Y;
            return (float)System.Math.Atan2(opposite, adjacent);
        }

        public static float GetAngleBetweenTwoV3(Vector3 v1, Vector3 v2)
        {
            v1.Normalize();
            v2.Normalize();
            double Angle = (float)Math.Acos(Vector3.Dot(v1, v2));
            return (float)Angle;
        }

        public static float GetPitchBetweenTwoV3(Vector3 v1, Vector3 v2)
        {
            Vector3 dir = v2 - v1;

            float adjusted = (float)Math.Sqrt(Math.Pow(dir.X, 2) + Math.Pow(dir.Z, 2));
            return (float)Math.Atan2(adjusted, dir.Y);

        }

        public static float GetDot(Vector3 origin, Vector3 target, Vector3 originFacing)
        {
            Vector3 dir = Vector3.Normalize(target - origin);
            float frontDot = Vector3.Dot(originFacing, dir);

            return frontDot;
        }

        public static float RadiansDistance(float a0, float a1)
        {
            float max = (float)Math.PI * 2f;
            float da = (a1 - a0) % max;
            return 2f * (da % max) - da;
        }

        public static float LerpRadians(float a0, float a1, float alpha)
        {
            var distance = RadiansDistance(a0, a1);
            return a0 + distance * alpha;
        }

        public static float RadiansBetween(float x1, float y1, float x2, float y2, bool wrap)
        {
            float deltaY = y2 - y1;
            float deltaX = x2 - x1;
            var ret = (float)Math.Atan2(deltaY, deltaX);
            if (wrap) { while (ret < 0) ret += PI2; }
            return ret;
        }

        //Find where a ray intersects with a flat plane.
        public static Vector3? GetRayPlaneIntersectionPoint(Ray ray, Plane plane)
        {
            float? distance = ray.Intersects(plane);

            if (distance.HasValue)
            {
                return ray.Position + ray.Direction * distance.Value;
            }

            return null;
        }

        public static bool WriteTextFile(string stringArray, string fullPath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    writer.Write(stringArray);
                }

                return true;
            }
            catch (Exception e)
            {
                Helpers.ErrorPopup("Error writing text file. ({0})", e.Message);
            }

            return false;
        }

        public static string GetFileContents(string filepath)
        {
            string output = string.Empty;

            try
            {
                using (FileStream stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {                        
                        output = reader.ReadToEnd(); //dump file contents into a string.
                    }
                }
            }
            catch (Exception e)
            {
                Helpers.ErrorPopup("Error reading text file. ({0})", e.Message);
                return string.Empty;
            }

            return output;
        }

        public static Vector3 GetForwardVector(Quaternion quaternion)
        {
            Quaternion temporaryQuaternion = quaternion;
            temporaryQuaternion.Conjugate();
            return Vector3.Transform(Vector3.Forward, temporaryQuaternion);
        }

        public static Matrix CreateLookMatrix(Vector3 startPoint, Vector3 targetPoint)
        {
            Matrix matrix = Matrix.CreateLookAt(startPoint, targetPoint, Vector3.Up);
            float yaw = (float)Math.Atan2(matrix.M13, matrix.M33);
            float pitch = (float)Math.Asin(-matrix.M23);
            float roll = (float)Math.Atan2(matrix.M21, matrix.M22);

            return Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
        }



        // --- end of file ---
    }
}