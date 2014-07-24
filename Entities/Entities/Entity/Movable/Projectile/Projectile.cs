using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Gradius
{

    public enum ProjectileType { NONE, STANDARD, MISSILE, DOUBLE, LASER, ENEMY, VOLCANO }
    public class Projectile : Movable
    {
        
        public AnimationController m_animator;
        public string currAnimation;
        public ProjectileType m_projectileType;
        public Vector2 m_dir;
        public Vector2 m_vel;

        public Texture2D m_sprite;
        public Vector2 m_spriteSize;

        public float m_depth = 0.5f;

        public Character m_shooter = null;

        public Projectile(Game1 world, Vector2 pos, Vector2 size, Texture2D sprite, Vector2 velocity, Vector2 direction, MovableType type, ProjectileType projectileType, Character shooter)
            : base(world, pos, size, type)
        {
            m_sprite = sprite;
            m_spriteSize = new Vector2(m_sprite.Width, m_sprite.Height);
            m_vel = velocity;
            m_dir = direction;
            m_shooter = shooter;
            int[] playerProjectileAnimationFramesStandard = { 0 };
            Animation playerProjectileAnimationStandard = new Animation(PlayType.Once, playerProjectileAnimationFramesStandard, 3.0f);
            int[] playerProjectileAnimationFramesDouble = { 1 };
            Animation playerProjectileAnimationDouble = new Animation(PlayType.Once, playerProjectileAnimationFramesDouble, 3.0f);
            int[] playerProjectileAnimationFramesLaser = { 2 };
            Animation playerProjectileAnimationLaser = new Animation(PlayType.Once, playerProjectileAnimationFramesLaser, 3.0f);
            int[] playerAnimationFramesMissileForward = { 3 };
            Animation playerAnimationMissileForward = new Animation(PlayType.Once, playerAnimationFramesMissileForward, 3.0f);
            int[] playerAnimationFramesMissileDiagonal = { 4 };
            Animation playerAnimationMissileDiagonal = new Animation(PlayType.Once, playerAnimationFramesMissileDiagonal, 3.0f);
            int[] enemyProjectileAnimationFrames = { 5 };
            Animation enemyProjectileAnimation = new Animation(PlayType.Once, enemyProjectileAnimationFrames, 3.0f);
            Dictionary<string, Animation> projectileAnimations = new Dictionary<string, Animation>() { { "standard", playerProjectileAnimationStandard },
                                                                                        { "double", playerProjectileAnimationDouble },
                                                                                        { "laser", playerProjectileAnimationLaser},
                                                                                        { "missile forward", playerAnimationMissileForward },
                                                                                        { "missile diagonal", playerAnimationMissileDiagonal },
                                                                                        { "enemy" , enemyProjectileAnimation}};

            m_animator = new AnimationController(m_world.m_spriteProjectile, projectileAnimations, 8, 3, this);
            m_projectileType = projectileType;
        }

        public override bool TestCollision(Movable other)
        {
            if (this.m_shooter.m_type == MovableType.Option && other.m_type == MovableType.Player)
                return false;
            if (this.m_shooter.m_type == MovableType.Player && other.m_type == MovableType.Option)
                return false;
            if (other.m_type == m_shooter.m_type)
                return false;
            if (other.m_type == MovableType.Option)
                return false;
            return base.TestCollision(other);
        }

        public override void Update(GameTime gameTime)
        {

            switch (m_projectileType)
            {
                case ProjectileType.STANDARD:
                    {
                        currAnimation = "standard";
                    }
                    break;
                case ProjectileType.DOUBLE:
                    {
                        currAnimation = "double";
                    }
                    break;
                case ProjectileType.LASER:
                    {
                        currAnimation = "laser";
                    }
                    break;
                case ProjectileType.ENEMY:
                    {
                        currAnimation = "enemy";
                    }
                    break;
                case ProjectileType.MISSILE:
                    {
                        currAnimation = "missile diagonal";
                    }
                    break;
            }

            m_animator.Update(gameTime, currAnimation);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //normalize direction:
            float d = m_dir.Length();
            if (d > 0.0f)
                m_dir = m_dir / d;

            //update position:
            {
                for (int axis = 0; axis <= 1; axis++)
                {
                    Vector2 prevPos = m_pos;

                    if (axis == 0)
                        m_pos.X = m_pos.X + m_vel.X * dt;
                    else
                        m_pos.Y = m_pos.Y + m_vel.Y * dt;
                }
                Character enemy = null;
                bool isColliding = false;
                foreach (Entity e in m_world.m_entities)
                {
                    if (e != this && e is Character)
                    {
                        if (this.TestCollision((Character)e))
                        {
                            isColliding = true;
                            enemy = (Character)e;
                            break;
                        }
                    }
                }

                if (isColliding)
                {
                    enemy.Die();
                    m_world.Remove(this);
                }

                if (this.m_pos.X > m_world.m_worldMap.screenWidth ||
                    this.m_pos.X < 0 ||
                    this.m_pos.Y > m_world.m_worldMap.screenHeigth ||
                    this.m_pos.Y < 0)
                    m_world.Remove(this);
                

            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 recSize = new Vector2(m_animator.m_currentSpriteRect.Width, m_animator.m_currentSpriteRect.Height);
            Vector2 spriteSize = new Vector2(m_sprite.Width, m_sprite.Height);
            Vector2 scale = new Vector2(spriteSize.X / recSize.X / 3, spriteSize.Y / recSize.Y);
            spriteBatch.Draw(m_sprite, m_pos, m_animator.m_currentSpriteRect, Color.White, 0.0f,
                recSize / 2, scale, SpriteEffects.None, m_depth);
        }
    }
}
