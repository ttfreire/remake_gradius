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
                m_pos.X += (float) (m_vel.X * Math.Cos(45) * dt);
                m_pos.Y += (float) (m_vel.Y * Math.Sin(45) * dt + 0.5 * 100 * dt * dt);

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
