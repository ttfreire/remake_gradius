using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Gradius
{
    class Volcano : Enemy
    {
      
      public enum EnemyState { NONE, ALIVE, EXPLODED }
      public EnemyState currentState = EnemyState.NONE;
      public AnimationController m_animator;
      public string currAnimation;
      float shootCooldown, shootCooldown2, shootCooldown3;

      public Volcano(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
                    MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;
          shootCooldown = rateoffire;
          shootCooldown2 = rateoffire;
          shootCooldown3 = rateoffire;
      }

    public override void Update(GameTime gameTime) {
       
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        shootCooldown -= dt;
        shootCooldown2 -= dt;
        shootCooldown3 -= dt;
        
        switch (currentState)
        {
            case EnemyState.NONE:
                {
                    m_dir = new Vector2(-1, -1);
                    currentState = EnemyState.ALIVE;
                }
                break;
            case EnemyState.ALIVE:
                {
                    if (shootCooldown <= 0)
                    {
                        shootCooldown = m_rateOfFire;
                        Random r = new Random();
                        int xVel = r.Next(0, 101);
                    }
                    if (shootCooldown2 <= 0.3f)
                    {
                        shootCooldown2 = m_rateOfFire;
                        Random r = new Random();
                        int xVel = r.Next(-101, 0);
                        Shoot(new Vector2(xVel, -500), new Vector2(m_pos.X - m_world.m_worldMap.screenWidth + 120, m_pos.Y), new Vector2(-1, -1), ProjectileType.VOLCANO);
                    }
                    if (shootCooldown3 <= 0.5f)
                    {
                        shootCooldown3 = m_rateOfFire;
                        Random r = new Random();
                        int xVel = r.Next(-50, 50);
                        Shoot(new Vector2(xVel, -500), new Vector2(m_pos.X - m_world.m_worldMap.screenWidth + 120, m_pos.Y), new Vector2(-1, -1), ProjectileType.VOLCANO);
                    }
                }
                break;

            case EnemyState.EXPLODED:
                {
                    m_dir = Vector2.Zero;
                    m_vel = Vector2.Zero;
                    currAnimation = "exploded";
                }
                break;
        }
      base.Update(gameTime);
    }
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {

        if (m_animator != null)
            spriteBatch.Draw(m_animator.m_spriteSheet, m_pos, m_animator.m_currentSpriteRect, Color.White, 0.0f,
            new Vector2(m_animator.m_currentSpriteRect.Width, m_animator.m_currentSpriteRect.Height) / 2, 2, SpriteEffects.None, m_depth);
    }
    }
}
