using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Drawing;
using Platformer.Entities.group;
using Platformer.Entities.Types;
using Platformer.Entities.Weapons;
using SharpDX.MediaFoundation;

namespace Platformer.Physics
{
    public sealed class FlatWorld
    {
        public static int TransformCount = 0;
        public static int NoTransformCount = 0;

        public static readonly float MinBodySize = 0.01f * 0.01f;
        public static readonly float MaxBodySize = 64f * 64f;

        public static readonly float MinDensity = 0.5f;     // g/cm^3
        public static readonly float MaxDensity = 21.4f;

        public static readonly int MinIterations = 1;
        public static readonly int MaxIterations = 128;

        private FlatVector gravity;
        public List<FlatBody> bodyList;
        private List<(int, int)> contactPairs;

        private FlatVector[] contactList;
        private FlatVector[] impulseList;
        private FlatVector[] raList;
        private FlatVector[] rbList;
        private FlatVector[] frictionImpulseList;
        private float[] jList;

        public int BodyCount
        {
            get { return bodyList.Count; }
        }

        public FlatWorld()
        {
            gravity = new FlatVector(0f, -9.81f * 2);
            bodyList = new List<FlatBody>();
            contactPairs = new List<(int, int)>();

            contactList = new FlatVector[2];
            impulseList = new FlatVector[2];
            raList = new FlatVector[2];
            rbList = new FlatVector[2];
            frictionImpulseList = new FlatVector[2];
            jList = new float[2];
        }

        public void AddBody(FlatBody body)
        {
            bodyList.Add(body);

            if (body.owner is GroupMember)
            {
                // Disable inertia and friction
                body.InvInertia = 0f;
                body.StaticFriction = 0f;
                body.DynamicFriction = 0f;
            }
        }

        public bool RemoveBody(FlatBody body)
        {
            return bodyList.Remove(body);
        }

        public bool GetBody(int index, out FlatBody body)
        {
            body = null;

            if (index < 0 || index >= bodyList.Count)
            {
                return false;
            }

            body = bodyList[index];
            return true;
        }

        public void Step(float time, int totalIterations)
        {
            totalIterations = FlatMath.Clamp(totalIterations, MinIterations, MaxIterations);

            for (int currentIteration = 0; currentIteration < totalIterations; currentIteration++)
            {
                contactPairs.Clear();
                StepBodies(time, totalIterations);
                BroadPhase();
                NarrowPhase();
            }
        }

        private void BroadPhase()
        {
            for (int i = 0; i < bodyList.Count - 1; i++)
            {
                FlatBody bodyA = bodyList[i];
                FlatAABB bodyA_aabb = bodyA.GetAABB();

                for (int j = i + 1; j < bodyList.Count; j++)
                {
                    FlatBody bodyB = bodyList[j];
                    FlatAABB bodyB_aabb = bodyB.GetAABB();

                    if (bodyA.IsStatic && bodyB.IsStatic)
                    {
                        continue;
                    }

                    if (!Collisions.IntersectAABBs(bodyA_aabb, bodyB_aabb))
                    {
                        continue;
                    }

                    contactPairs.Add((i, j));
                }
            }
        }

        private void NarrowPhase()
        {
            for (int i = 0; i < contactPairs.Count; i++)
            {
                (int, int) pair = contactPairs[i];
                FlatBody bodyA = bodyList[pair.Item1];
                FlatBody bodyB = bodyList[pair.Item2];

                if (Collisions.Collide(bodyA, bodyB, out FlatVector normal, out float depth))
                {

                    {
                        if (

                            (bodyA.owner is Projectile projectile && projectile.projectileType != Projectile.ProjectileType.granade) || (bodyB.owner is Projectile projectile1 && projectile1.projectileType != Projectile.ProjectileType.granade) ||

                            (bodyA.owner is DecorationEntity) || (bodyB.owner is DecorationEntity) ||





                            (bodyA.owner is GroupMember && bodyB.owner is GroupMember) ||

                            (bodyA.owner is PlatformEntity platform && bodyB.owner is GroupMember && !platform.isCollidable) ||
                            (bodyA.owner is GroupMember && bodyB.owner is PlatformEntity platform1 && !platform1.isCollidable) ||






                            (bodyA.owner is InterractiveItem && bodyB.owner is DynamicEntity) ||
                            (bodyA.owner is DynamicEntity && bodyB.owner is InterractiveItem) ||

                            (bodyA.owner is LadderEntity && bodyB.owner is DynamicEntity) ||
                            (bodyA.owner is DynamicEntity && bodyB.owner is LadderEntity) ||


                            (bodyA.owner is FlatEntity flent && flent.isBossPart && bodyB.owner is FlatEntity flent1 && flent1.isBossPart) ||



                            (bodyA.owner is NPC && bodyB.owner is NPC) ||

                            (bodyA.owner is NPC && bodyB.owner is DynamicEntity) ||
                            (bodyA.owner is DynamicEntity && bodyB.owner is NPC))



                            
                        {
                            continue;
                        }

                        SeparateBodies(bodyA, bodyB, normal * depth);
                        Collisions.FindContactPoints(bodyA, bodyB, out FlatVector contact1, out FlatVector contact2, out int contactCount);
                        FlatManifold contact = new FlatManifold(bodyA, bodyB, normal, depth, contact1, contact2, contactCount);
                        ResolveCollisionWithRotationAndFriction(in contact);



                        if(bodyA.owner is GroupMember || bodyB.owner is GroupMember)
                        {
                            bodyA.LinearVelocity = new FlatVector(0, bodyA.LinearVelocity.Y);
                            bodyB.LinearVelocity = new FlatVector(0, bodyB.LinearVelocity.Y);
                        }
                    }

                }
            }
        }


        public void StepBodies(float time, int totalIterations)
        {
            for (int i = 0; i < bodyList.Count; i++)
            {
                bodyList[i].Step(time, gravity, totalIterations);
            }
        }

        private void SeparateBodies(FlatBody bodyA, FlatBody bodyB, FlatVector mtv)
        {
            if (bodyA.IsStatic)
            {
                bodyB.Move(mtv);
            }
            else if (bodyB.IsStatic)
            {
                bodyA.Move(-mtv);
            }
            else
            {
                bodyA.Move(-mtv / 2f);
                bodyB.Move(mtv / 2f);
            }
        }

        public void ResolveCollisionBasic(in FlatManifold contact)
        {
            FlatBody bodyA = contact.BodyA;
            FlatBody bodyB = contact.BodyB;
            FlatVector normal = contact.Normal;
            float depth = contact.Depth;

            FlatVector relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;

            if (FlatMath.Dot(relativeVelocity, normal) > 0f)
            {
                return;
            }

            float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

            float j = -(1f + e) * FlatMath.Dot(relativeVelocity, normal);
            j /= bodyA.InvMass + bodyB.InvMass;

            FlatVector impulse = j * normal;

            bodyA.LinearVelocity -= impulse * bodyA.InvMass;
            bodyB.LinearVelocity += impulse * bodyB.InvMass;
        }

        public void ResolveCollisionWithRotation(in FlatManifold contact)
        {
            FlatBody bodyA = contact.BodyA;
            FlatBody bodyB = contact.BodyB;

            
            FlatVector normal = contact.Normal;
            FlatVector contact1 = contact.Contact1;
            FlatVector contact2 = contact.Contact2;
            int contactCount = contact.ContactCount;

            float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

            contactList[0] = contact1;
            contactList[1] = contact2;

            for (int i = 0; i < contactCount; i++)
            {
                impulseList[i] = FlatVector.Zero;
                raList[i] = FlatVector.Zero;
                rbList[i] = FlatVector.Zero;
            }

            for (int i = 0; i < contactCount; i++)
            {
                FlatVector ra = contactList[i] - bodyA.Position;
                FlatVector rb = contactList[i] - bodyB.Position;

                raList[i] = ra;
                rbList[i] = rb;

                FlatVector raPerp = new FlatVector(-ra.Y, ra.X);
                FlatVector rbPerp = new FlatVector(-rb.Y, rb.X);

                FlatVector angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
                FlatVector angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

                FlatVector relativeVelocity =
                    bodyB.LinearVelocity + angularLinearVelocityB -
                    (bodyA.LinearVelocity + angularLinearVelocityA);

                float contactVelocityMag = FlatMath.Dot(relativeVelocity, normal);

                if (contactVelocityMag > 0f)
                {
                    continue;
                }

                float raPerpDotN = FlatMath.Dot(raPerp, normal);
                float rbPerpDotN = FlatMath.Dot(rbPerp, normal);

                float denom = bodyA.InvMass + bodyB.InvMass +
                    raPerpDotN * raPerpDotN * bodyA.InvInertia +
                    rbPerpDotN * rbPerpDotN * bodyB.InvInertia;

                float j = -(1f + e) * contactVelocityMag;
                j /= denom;
                j /= contactCount;

                FlatVector impulse = j * normal;
                impulseList[i] = impulse;
            }

            for (int i = 0; i < contactCount; i++)
            {
                FlatVector impulse = impulseList[i];
                FlatVector ra = raList[i];
                FlatVector rb = rbList[i];

                bodyA.LinearVelocity += -impulse * bodyA.InvMass;
                bodyA.AngularVelocity += -FlatMath.Cross(ra, impulse) * bodyA.InvInertia;
                bodyB.LinearVelocity += impulse * bodyB.InvMass;
                bodyB.AngularVelocity += FlatMath.Cross(rb, impulse) * bodyB.InvInertia;
            }
        }

        public void ResolveCollisionWithRotationAndFriction(in FlatManifold contact)
        {
            FlatBody bodyA = contact.BodyA;
            FlatBody bodyB = contact.BodyB;

            bool isBodyAGroupMember = bodyA.owner is GroupMember;
            bool isBodyBGroupMember = bodyB.owner is GroupMember;

            bool disableInertia = isBodyAGroupMember || isBodyBGroupMember;




            if (disableInertia)
            {
                ResolveCollisionBasic(contact);
                return;
            }


            FlatVector normal = contact.Normal;
            FlatVector contact1 = contact.Contact1;
            FlatVector contact2 = contact.Contact2;
            int contactCount = contact.ContactCount;

            float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);

            float sf = (bodyA.StaticFriction + bodyB.StaticFriction) * 0.5f;
            float df = (bodyA.DynamicFriction + bodyB.DynamicFriction) * 0.5f;

            contactList[0] = contact1;
            contactList[1] = contact2;

            for (int i = 0; i < contactCount; i++)
            {
                impulseList[i] = FlatVector.Zero;
                raList[i] = FlatVector.Zero;
                rbList[i] = FlatVector.Zero;
                frictionImpulseList[i] = FlatVector.Zero;
                jList[i] = 0f;
            }

            for (int i = 0; i < contactCount; i++)
            {
                FlatVector ra = contactList[i] - bodyA.Position;
                FlatVector rb = contactList[i] - bodyB.Position;

                raList[i] = ra;
                rbList[i] = rb;

                FlatVector raPerp = new FlatVector(-ra.Y, ra.X);
                FlatVector rbPerp = new FlatVector(-rb.Y, rb.X);

                FlatVector angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
                FlatVector angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

                FlatVector relativeVelocity =
                    bodyB.LinearVelocity + angularLinearVelocityB -
                    (bodyA.LinearVelocity + angularLinearVelocityA);

                float contactVelocityMag = FlatMath.Dot(relativeVelocity, normal);

                if (contactVelocityMag > 0f)
                {
                    continue;
                }

                float raPerpDotN = FlatMath.Dot(raPerp, normal);
                float rbPerpDotN = FlatMath.Dot(rbPerp, normal);

                float denom = bodyA.InvMass + bodyB.InvMass +
                    raPerpDotN * raPerpDotN * bodyA.InvInertia +
                    rbPerpDotN * rbPerpDotN * bodyB.InvInertia;

                float j = -(1f + e) * contactVelocityMag;
                j /= denom;
                j /= contactCount;

                jList[i] = j;

                FlatVector impulse = j * normal;
                impulseList[i] = impulse;
            }

            for (int i = 0; i < contactCount; i++)
            {
                FlatVector impulse = impulseList[i];
                FlatVector ra = raList[i];
                FlatVector rb = rbList[i];

                bodyA.LinearVelocity += -impulse * bodyA.InvMass;
                bodyA.AngularVelocity += -FlatMath.Cross(ra, impulse) * bodyA.InvInertia;
                bodyB.LinearVelocity += impulse * bodyB.InvMass;
                bodyB.AngularVelocity += FlatMath.Cross(rb, impulse) * bodyB.InvInertia;
            }

            for (int i = 0; i < contactCount; i++)
            {
                FlatVector ra = contactList[i] - bodyA.Position;
                FlatVector rb = contactList[i] - bodyB.Position;

                raList[i] = ra;
                rbList[i] = rb;

                FlatVector raPerp = new FlatVector(-ra.Y, ra.X);
                FlatVector rbPerp = new FlatVector(-rb.Y, rb.X);

                FlatVector angularLinearVelocityA = raPerp * bodyA.AngularVelocity;
                FlatVector angularLinearVelocityB = rbPerp * bodyB.AngularVelocity;

                FlatVector relativeVelocity =
                    bodyB.LinearVelocity + angularLinearVelocityB -
                    (bodyA.LinearVelocity + angularLinearVelocityA);

                FlatVector tangent = relativeVelocity - FlatMath.Dot(relativeVelocity, normal) * normal;

                if (FlatMath.NearlyEqual(tangent, FlatVector.Zero))
                {
                    continue;
                }
                else
                {
                    tangent = FlatMath.Normalize(tangent);
                }

                float raPerpDotT = FlatMath.Dot(raPerp, tangent);
                float rbPerpDotT = FlatMath.Dot(rbPerp, tangent);

                float denom = bodyA.InvMass + bodyB.InvMass +
                    raPerpDotT * raPerpDotT * bodyA.InvInertia +
                    rbPerpDotT * rbPerpDotT * bodyB.InvInertia;

                float jt = -FlatMath.Dot(relativeVelocity, tangent);
                jt /= denom;
                jt /= contactCount;

                FlatVector frictionImpulse;
                float j = jList[i];

                if (MathF.Abs(jt) <= j * sf)
                {
                    frictionImpulse = jt * tangent;
                }
                else
                {
                    frictionImpulse = -j * tangent * df;
                }

                frictionImpulseList[i] = frictionImpulse;
            }

            for (int i = 0; i < contactCount; i++)
            {

                FlatVector frictionImpulse = frictionImpulseList[i];
                FlatVector ra = raList[i];
                FlatVector rb = rbList[i];

                bodyA.LinearVelocity += -frictionImpulse * bodyA.InvMass;
                bodyA.AngularVelocity += -FlatMath.Cross(ra, frictionImpulse) * bodyA.InvInertia;
                bodyB.LinearVelocity += frictionImpulse * bodyB.InvMass;
                bodyB.AngularVelocity += FlatMath.Cross(rb, frictionImpulse) * bodyB.InvInertia;
            }

        }
    }
}
