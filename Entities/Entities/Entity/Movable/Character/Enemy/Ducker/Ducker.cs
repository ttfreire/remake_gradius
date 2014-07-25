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
    class Ducker : Enemy
    {
      bool m_dropsPowerUp; 
      public enum EnemyState {NONE, FORWARD, SHOOT, BACK, EXPLODED }
      public EnemyState m_currentState = EnemyState.FORWARD;
      public AnimationController m_animator;
      float m_timeToDie;
      float m_rotation = 3.2f;
      SpriteEffects m_spriteEffect = SpriteEffects.None;
      int timesShot = 0; 

      public Ducker(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;
          m_dropsPowerUp = dropsPowerUp;

          int[] DuckerAnimationFramesWalking = { 45, 46 };
          Animation DuckerAnimationWalking = new Animation(PlayType.Loop, DuckerAnimationFramesWalking, 16.0f);
          int[] DuckerAnimationFramesHolding = { 47 };
          Animation DuckerAnimationHolding = new Animation(PlayType.Loop, DuckerAnimationFramesHolding, 3.0f);
          int[] DuckerAnimationFramesExploded = { 80, 81, 82, 83 };
          Animation DuckerAnimationExploded = new Animation(PlayType.Loop, DuckerAnimationFramesExploded, 3.0f);
          Dictionary<string, Animation> DuckerAnimations = new Dictionary<string, Animation>() { { "walking", DuckerAnimationWalking } ,
                                                                                    { "shooting", DuckerAnimationHolding },
                                                                                    { "waiting", DuckerAnimationHolding }, 
                                                                                    { "exploded", DuckerAnimationExploded}};
          m_animator = new AnimationController(m_world.m_spriteEnemies, DuckerAnimations, 5, 18);
          currAnimation = "walking";
      }

    public override void Update(GameTime gameTime) {

        Player player = (Player) m_world.m_entities.Find(s => s is Player);
        m_animator.Update(gameTime, currAnimation);
        currentAnimationState = (int)m_currentState;
        switch (m_currentState)
        {
            case EnemyState.FORWARD:
                {
                    currAnimation = "walking";
                    m_dir = Vector2.UnitX;

                    if (player != null)
                        if (m_pos.X > player.m_pos.X + 300)
                        {
                            m_currentState = EnemyState.SHOOT;
                            m_vel = Vector2.Zero;
                        }
                    if(m_pos.Y < m_world.m_worldMap.m_screenMiddle.Y)
                    {
                        m_rotation = 3.14f;
                        m_spriteEffect = SpriteEffects.None;
                    }
                    else
                    {
                        m_rotation = 0.0f;
                        m_spriteEffect = SpriteEffects.FlipHorizontally;
                    }
                }
                break;

            case EnemyState.SHOOT:
                {
                    currAnimation = "shooting";
                    m_dir = Vector2.Zero;
                    if (player != null)
                    {
                        Vector2 dir = (player.m_pos - m_pos) + new Vector2(player.m_size.Length(), 0);
                        Shoot(dir, new Vector2(this.m_pos.X, this.m_pos.Y), dir, ProjectileType.ENEMY);
                        timesShot++;
                        m_currentState = EnemyState.BACK;
                    }
                    if (m_pos.Y < m_world.m_worldMap.m_screenMiddle.Y)
                    {
                        m_rotation = 3.14f;
                        m_spriteEffect = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        m_rotation = 0.0f;
                        m_spriteEffect = SpriteEffects.None;
                    }


                }
                break;

            case EnemyState.BACK:
            {
                if (timesShot < 2)
                {
                    currAnimation = "waiting";
                    m_dir = -Vector2.UnitX;
                    if (m_pos.X < 100)
                        m_currentState = EnemyState.FORWARD;
                }
                else
                {
                    currAnimation = "walking";
                    m_dir = -Vector2.UnitX;
                    m_vel = new Vector2(-80, 0);
                }
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

      public void dropPowerUp()
      {
          PowerUp powerup = new PowerUp(m_world, this.m_pos);
          m_world.Add(powerup);
          m_world.powerUpCounter++;
      }


      public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
      {
          Color color = Color.White;
          if (m_dropsPowerUp)
              color = Color.Red;
          if (m_animator != null)
              spriteBatch.Draw(m_animator.m_spriteSheet, m_pos, m_animator.m_currentSpriteRect, color, m_rotation,
              new Vector2(m_animator.m_currentSpriteRect.Width, m_animator.m_currentSpriteRect.Height) / 2, 2, m_spriteEffect, 0);
      }

      public override void Die()
      {
          base.Die();
          m_currentState = EnemyState.EXPLODED;
          isdead = true;
          m_timeToDie = 0.5f;
      }
    }
}
