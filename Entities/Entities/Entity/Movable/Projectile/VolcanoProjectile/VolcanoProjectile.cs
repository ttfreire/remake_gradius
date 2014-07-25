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

    public class VolcanoProjectile : Projectile
    {
        public VolcanoProjectile(Game1 world, Vector2 pos, Vector2 size, Texture2D sprite, Vector2 velocity, Vector2 direction, MovableType type, ProjectileType projectileType, Character shooter)
            : base(world, pos, size, sprite, velocity, direction, type, projectileType, shooter)
        {
            m_vel = velocity;
            m_pos = pos;
        }

        public override void Update(GameTime gameTime)
        {

            currAnimation = "enemy";
            m_animator.Update(gameTime, currAnimation);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            
            //update position:
            {
                m_vel.X += (float) (m_vel.X * Math.Cos(30));
                m_vel.Y += (float) (m_vel.Y * Math.Sin(0)) * 100;
                m_pos.X += m_vel.X * dt;
                m_pos.Y += (float) (m_vel.Y * dt + 0.5 * dt * dt);

                Character enemy = null;
                Projectile proj = null;
                bool isColliding = false;
                bool isCollidingWithProjectile = false;
                foreach (Entity e in m_world.m_entities)
                {
                    if (e != this && e is Character)
                    {
			Character character = (Character)e;
                        if (this.TestCollision(character) && character.m_type != MovableType.Option)
                        {
                            isColliding = true;
                            enemy = character;
                            break;
                        }
                        if (e is Projectile)
                        {
                            proj = (Projectile)e;
                            if (this.TestCollision(proj) && proj.m_shooter is Player)
                            {
                                isCollidingWithProjectile = true;
                                break;
                            }
                        }
                    }
                }

                if (isColliding)
                {
                    enemy.Die();
                    m_world.Remove(this);
                }
                if (isCollidingWithProjectile)
                {
                    m_world.Remove(proj);
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
