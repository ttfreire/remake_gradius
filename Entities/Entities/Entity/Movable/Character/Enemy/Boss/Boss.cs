﻿using System;
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
    class Boss : Enemy
    {
      
      public enum EnemyState { NONE, ALIVE, EXPLODED }
      public EnemyState m_currentState = EnemyState.NONE;
      public AnimationController m_animator;
      public string currAnimation;
      float shootCooldown;
      int shieldCount = 30;
      float m_timeToDie;

      public Boss(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
                    MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;

          int[] bossAnimationFramesMoving = { 0 };
          Animation bossAnimationMoving = new Animation(PlayType.Loop, bossAnimationFramesMoving, 11.0f);
          int[] bossAnimationFramesExploded = { 4, 5, 6, 7 };
          Animation bossAnimationExploded = new Animation(PlayType.Loop, bossAnimationFramesExploded, 3.0f);

          Dictionary<string, Animation> bossAnimations = new Dictionary<string, Animation>() { { "moving", bossAnimationMoving },
                                                                                                {"exploded", bossAnimationExploded }};
          m_animator = new AnimationController(m_world.m_spriteBoss, bossAnimations, 2, 1);
          currAnimation = "moving";
          shootCooldown = rateoffire;
          m_size = new Vector2(m_world.m_spriteBoss.Bounds.Width / 2, m_world.m_spriteBoss.Bounds.Height);
      }

    public override void Update(GameTime gameTime) {
        m_animator.Update(gameTime, currAnimation);
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        shootCooldown -= dt;

        currentAnimationState = (int)m_currentState;
        switch (m_currentState)
        {
            case EnemyState.NONE:
                {
                    m_dir = new Vector2(0, -1);
                    m_currentState = EnemyState.ALIVE;
                }
                break;
            case EnemyState.ALIVE:
                {
                    if (m_pos.Y - m_size.Y / 2 <= 80)
                    {
                        m_dir.Y = 1;
                        if (shootCooldown <= 0)
                        {
                            shootCooldown = m_rateOfFire;
                            Shoot(new Vector2(-500, 0), m_pos - new Vector2(0, m_size.Y / 2), new Vector2(-1, 0), ProjectileType.LASER);
                            Shoot(new Vector2(-500, 0), m_pos - new Vector2(0, m_size.Y / 6), new Vector2(-1, 0), ProjectileType.LASER);
                            Shoot(new Vector2(-500, 0), m_pos + new Vector2(0, m_size.Y / 6), new Vector2(-1, 0), ProjectileType.LASER);
                            Shoot(new Vector2(-500, 0), m_pos + new Vector2(0, m_size.Y / 2), new Vector2(-1, 0), ProjectileType.LASER);
                        }
                    }
                    if (m_pos.Y + m_size.Y / 2 >= m_world.m_worldMap.m_view.Height - 200)
                    {
                        m_dir.Y = -1;
                        if (shootCooldown <= 0)
                        {
                            shootCooldown = m_rateOfFire;
                            Shoot(new Vector2(-500, 0), m_pos - new Vector2(0, m_size.Y / 2), new Vector2(-1, 0), ProjectileType.LASER);
                            Shoot(new Vector2(-500, 0), m_pos - new Vector2(0, m_size.Y / 6), new Vector2(-1, 0), ProjectileType.LASER);
                            Shoot(new Vector2(-500, 0), m_pos + new Vector2(0, m_size.Y / 6), new Vector2(-1, 0), ProjectileType.LASER);
                            Shoot(new Vector2(-500, 0), m_pos + new Vector2(0, m_size.Y / 2), new Vector2(-1, 0), ProjectileType.LASER);
                        }
                    }
                   /**
                    if (shootCooldown <= 0)
                    {
                        shootCooldown = m_rateOfFire;
                        Shoot(new Vector2(-500, 0), m_pos - new Vector2(0, m_size.Y / 2), new Vector2(-1, 0), ProjectileType.LASER);
                        Shoot(new Vector2(-500, 0), m_pos - new Vector2(0, m_size.Y / 6), new Vector2(-1, 0), ProjectileType.LASER);
                        Shoot(new Vector2(-500, 0), m_pos + new Vector2(0, m_size.Y / 6), new Vector2(-1, 0), ProjectileType.LASER);
                        Shoot(new Vector2(-500, 0), m_pos + new Vector2(0, m_size.Y / 2), new Vector2(-1, 0), ProjectileType.LASER);
                    }
                    **/
                }
                break;

            case EnemyState.EXPLODED:
                {
                    m_dir = Vector2.Zero;
                    m_vel = Vector2.Zero;
                    currAnimation = "exploded";
                    m_timeToDie -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_timeToDie <= 0)
                        m_world.Remove(this);
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

    public override void Die()
    {
        if (shieldCount > 0)
            shieldCount--;
        else
        {
            base.Die();
            m_currentState = EnemyState.EXPLODED;
            isdead = true;
            m_timeToDie = 0.5f;
            m_world.m_hudController.subtractLife();
        }
    }
    }
}
