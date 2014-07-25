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
    class Hatch : Enemy
    {
      bool m_dropsPowerUp; 
      public enum EnemyState {NONE, IDLE, EXPLODED }
      public EnemyState m_currentState = EnemyState.IDLE;
      public AnimationController m_animator;
      float m_timeToDie;
      float m_rotation = 3.2f;
      SpriteEffects m_spriteEffect = SpriteEffects.None;
      int timesShot = 0; 

      public Hatch(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;
          m_dropsPowerUp = dropsPowerUp;

          int[] HatchAnimationFrames = { 0, 1};
          Animation HatchAnimation = new Animation(PlayType.Loop, HatchAnimationFrames, 3.0f);
          int[] HatchAnimationFramesExploded = { 4, 5, 6, 7 };
          Animation HatchAnimationExploded = new Animation(PlayType.Loop, HatchAnimationFramesExploded, 3.0f);
          Dictionary<string, Animation> HatchAnimations = new Dictionary<string, Animation>() { { "idle", HatchAnimation }, 
                                                                                              { "exploded", HatchAnimationExploded}};
          m_animator = new AnimationController(m_world.m_spriteHatch, HatchAnimations, 4, 2);
          currAnimation = "idle";
      }

    public override void Update(GameTime gameTime) {

        Player player = (Player) m_world.m_entities.Find(s => s is Player);
        m_animator.Update(gameTime, currAnimation);
        currentAnimationState = (int)m_currentState;

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
        switch (m_currentState)
        {
            case EnemyState.IDLE:
                {
                    currAnimation = "idle";
                    m_dir = -Vector2.UnitX;

                   
                    
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

          if (m_animator != null)
              spriteBatch.Draw(m_animator.m_spriteSheet, m_pos, m_animator.m_currentSpriteRect, Color.White, m_rotation,
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
