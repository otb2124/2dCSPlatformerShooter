using Platformer.Entities;
using Platformer.Entities.group;
using Platformer.Entities.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using static Platformer.Entities.Types.FlatEntity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Platformer.Physics
{
    public enum ShapeType
    {
        Circle = 0,
        Box = 1,
    }

    public sealed class FlatBody
    {
        private FlatVector position;
        private FlatVector linearVelocity;
        private float angle;
        private float angularVelocity;
        public FlatVector force;

        public readonly ShapeType ShapeType;
        public float Density;
        public float Mass;
        public readonly float InvMass;
        public float Restitution;
        public readonly float Area;
        public float Inertia;
        public float InvInertia;
        public bool IsStatic;
        public float Radius;
        public float Width;
        public float Height;
        public float StaticFriction;
        public float DynamicFriction;

        private readonly FlatVector[] vertices;
        private FlatVector[] transformedVertices;
        private FlatAABB aabb;

        private bool transformUpdateRequired;
        public bool aabbUpdateRequired;


        public Object owner = null;


        public FlatVector Position
        {
            get { return position; }
        }

        public FlatVector LinearVelocity
        {
            get { return linearVelocity; }
            internal set { linearVelocity = value; }
        }

        public float Angle
        {
            get { return angle; }
        }

        public float AngularVelocity
        {
            get { return angularVelocity; }
            internal set { angularVelocity = value; }
        }

        private FlatBody(float density, float mass, float inertia, float restitution, float area,
            bool isStatic, float radius, float width, float height, FlatVector[] vertices, ShapeType shapeType)
        {
            position = FlatVector.Zero;
            linearVelocity = FlatVector.Zero;
            angle = 0f;
            angularVelocity = 0f;
            force = FlatVector.Zero;

            ShapeType = shapeType;
            Density = density;
            Mass = mass;
            InvMass = mass > 0f ? 1f / mass : 0f;
            Inertia = inertia;
            InvInertia = inertia > 0f ? 1f / inertia : 0f;
            Restitution = restitution;
            Area = area;
            IsStatic = isStatic;
            Radius = radius;
            Width = width;
            Height = height;
            StaticFriction = 0.6f;
            DynamicFriction = 0.4f;

            if (ShapeType is ShapeType.Box)
            {
                this.vertices = vertices;
                transformedVertices = new FlatVector[this.vertices.Length];
            }
            else
            {
                this.vertices = null;
                transformedVertices = null;
            }

            transformUpdateRequired = true;
            aabbUpdateRequired = true;
        }

        private static FlatVector[] CreateBoxVertices(float width, float height)
        {
            float left = -width / 2f;
            float right = left + width;
            float bottom = -height / 2f;
            float top = bottom + height;

            FlatVector[] vertices = new FlatVector[4];
            vertices[0] = new FlatVector(left, top);
            vertices[1] = new FlatVector(right, top);
            vertices[2] = new FlatVector(right, bottom);
            vertices[3] = new FlatVector(left, bottom);

            return vertices;
        }

        private static int[] CreateBoxTriangles()
        {
            int[] triangles = new int[6];
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
            return triangles;
        }

        public FlatVector[] GetTransformedVertices()
        {
            if (transformUpdateRequired)
            {
                FlatTransform transform = new FlatTransform(position, angle);

                for (int i = 0; i < vertices.Length; i++)
                {
                    FlatVector v = vertices[i];
                    transformedVertices[i] = FlatVector.Transform(v, transform);
                }

                FlatWorld.TransformCount++;
            }
            else
            {
                FlatWorld.NoTransformCount++;
            }

            transformUpdateRequired = false;
            return transformedVertices;
        }

        public FlatAABB GetAABB()
        {
            if (aabbUpdateRequired)
            {
                float minX = float.MaxValue;
                float minY = float.MaxValue;
                float maxX = float.MinValue;
                float maxY = float.MinValue;

                if (ShapeType is ShapeType.Box)
                {
                    FlatVector[] vertices = GetTransformedVertices();

                    for (int i = 0; i < vertices.Length; i++)
                    {
                        FlatVector v = vertices[i];

                        if (v.X < minX) { minX = v.X; }
                        if (v.X > maxX) { maxX = v.X; }
                        if (v.Y < minY) { minY = v.Y; }
                        if (v.Y > maxY) { maxY = v.Y; }
                    }
                }
                else if (ShapeType is ShapeType.Circle)
                {
                    minX = position.X - Radius;
                    minY = position.Y - Radius;
                    maxX = position.X + Radius;
                    maxY = position.Y + Radius;
                }
                else
                {
                    throw new Exception("Unknown ShapeType.");
                }

                aabb = new FlatAABB(minX, minY, maxX, maxY);
            }

            aabbUpdateRequired = false;
            return aabb;
        }

        internal void Step(float time, FlatVector gravity, int iterations)
        {
            if (IsStatic)
            {
                return;
            }

            time /= iterations;

            // force = mass * acc
            // acc = force / mass;

            //FlatVector acceleration = this.force / this.Mass;
            //this.linearVelocity += acceleration * time;


            linearVelocity += gravity * time;
            position += linearVelocity * time;

            angle += angularVelocity * time;

            force = FlatVector.Zero;
            transformUpdateRequired = true;
            aabbUpdateRequired = true;
        }

        public void Move(FlatVector amount)
        {
            position += amount;
            transformUpdateRequired = true;
            aabbUpdateRequired = true;
        }

        public void MoveTo(FlatVector position)
        {
            this.position = position;
            transformUpdateRequired = true;
            aabbUpdateRequired = true;
        }

        public void Rotate(float amount)
        {
            angle += amount;
            transformUpdateRequired = true;
            aabbUpdateRequired = true;
        }

        public void RotateTo(float angle)
        {
            this.angle = angle;
            transformUpdateRequired = true;
            aabbUpdateRequired = true;
        }

        public void AddForce(FlatVector amount)
        {
            force = amount;
        }

        public static bool CreateCircleBody(float radius, float density, bool isStatic, float restitution, out FlatBody body, out string errorMessage)
        {
            body = null;
            errorMessage = string.Empty;

            float area = radius * radius * MathF.PI;

            if (area < FlatWorld.MinBodySize)
            {
                errorMessage = $"Circle radius is too small. Min circle area is {FlatWorld.MinBodySize}.";
                return false;
            }

            if (area > FlatWorld.MaxBodySize)
            {
                errorMessage = $"Circle radius is too large. Max circle area is {FlatWorld.MaxBodySize}.";
                return false;
            }

            if (density < FlatWorld.MinDensity)
            {
                errorMessage = $"Density is too small. Min density is {FlatWorld.MinDensity}";
                return false;
            }

            if (density > FlatWorld.MaxDensity)
            {
                errorMessage = $"Density is too large. Max density is {FlatWorld.MaxDensity}";
                return false;
            }

            restitution = FlatMath.Clamp(restitution, 0f, 1f);

            float mass = 0f;
            float inertia = 0f;

            if (!isStatic)
            {
                // mass = area * depth * density
                mass = area * density;
                inertia = 1f / 2f * mass * radius * radius;

            }
            


            body = new FlatBody(density, mass, inertia, restitution, area, isStatic, radius, 0f, 0f, null, ShapeType.Circle);
            return true;
        }

        public static bool CreateBoxBody(float width, float height, float density, bool isStatic, float restitution, out FlatBody body, out string errorMessage)
        {
            body = null;
            errorMessage = string.Empty;

            float area = width * height;

            if (area < FlatWorld.MinBodySize)
            {
                errorMessage = $"Area is too small. Min area is {FlatWorld.MinBodySize}.";
                return false;
            }

            if (area > FlatWorld.MaxBodySize)
            {
                errorMessage = $"Area is too large. Max area is {FlatWorld.MaxBodySize}.";
                return false;
            }

            if (density < FlatWorld.MinDensity)
            {
                errorMessage = $"Density is too small. Min density is {FlatWorld.MinDensity}";
                return false;
            }

            if (density > FlatWorld.MaxDensity)
            {
                errorMessage = $"Density is too large. Max density is {FlatWorld.MaxDensity}";
                return false;
            }

            restitution = FlatMath.Clamp(restitution, 0f, 1f);

            float mass = 0f;
            float inertia = 0f;

            if (!isStatic)
            {
                // mass = area * depth * density
                mass = area * density;
                inertia = 1f / 12 * mass * (width * width + height * height);


                
            }

            FlatVector[] vertices = CreateBoxVertices(width, height);

            body = new FlatBody(density, mass, inertia, restitution, area, isStatic, 0f, width, height, vertices, ShapeType.Box);
            return true;
        }



        public void Jump(float jumpForce)
        {
            

            if (!(LinearVelocity.Y > 0.2f || LinearVelocity.Y < -0.2f))
            {

                if (!IsStatic)
                {
                    ApplyForce(new FlatVector(0f, jumpForce * 50));
                }


                if(owner is GroupMember)
                {
                    Restitution = 0.1f;
                    InvInertia = 0f;
                    StaticFriction = 0f;
                    DynamicFriction = 0f;
                    
                }
            }
        }




        public void ApplyForce(FlatVector force)
        {
            if (!IsStatic)
            {
                LinearVelocity += force * InvMass;
            }
        }



        public FlatBody(FlatBody existingBody, float newHeight, float newWidth)
        {
            // Copy properties from the existing body
            position = existingBody.Position;
            linearVelocity = existingBody.LinearVelocity;
            angle = existingBody.Angle;
            angularVelocity = existingBody.AngularVelocity;
            force = existingBody.force;

            ShapeType = existingBody.ShapeType;
            Density = existingBody.Density;
            Mass = existingBody.Mass;
            InvMass = existingBody.InvMass;
            Restitution = existingBody.Restitution;
            Area = existingBody.Area;
            Inertia = existingBody.Inertia;
            InvInertia = existingBody.InvInertia;
            IsStatic = existingBody.IsStatic;
            Radius = existingBody.Radius;
            Width = newWidth;
            Height = newHeight; // Set new height
            StaticFriction = existingBody.StaticFriction;
            DynamicFriction = existingBody.DynamicFriction;

            if (existingBody.ShapeType == ShapeType.Box)
            {
                // Recreate vertices with new height
                vertices = CreateBoxVertices(existingBody.Width, newHeight);
                transformedVertices = new FlatVector[vertices.Length];
            }
            else
            {
                vertices = null;
                transformedVertices = null;
            }

            transformUpdateRequired = true;
            aabbUpdateRequired = true;
        }



    }
}
