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

      public Ducker(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;
          m_dropsPowerUp = dropsPowerUp;

          int[] duckerAnimationFramesWalking = { 45, 46 };
          Animation duckerAnimationWalking = new Animation(PlayType.Loop, duckerAnimationFramesWalking, 16.0f);
          int[] duckerAnimationFramesHolding = { 47 };
          Animation duckerAnimationHolding = new Animation(PlayType.Loop, duckerAnimationFramesHolding, 3.0f);
          int[] duckerAnimationFramesExploded = { 80, 81, 82, 83 };
          Animation duckerAnimationExploded = new Animation(PlayType.Loop, duckerAnimationFramesExploded, 3.0f);
          Dictionary<string, Animation> duckerAnimations = new Dictionary<string, Animation>() { { "walking", duckerAnimationWalking } ,
                                                                                    { "shooting", duckerAnimationHolding },
                                                                                    { "waiting", duckerAnimationHolding }, 
                                                                                    { "exploded", duckerAnimationExploded}};
          m_animator = new AnimationController(m_world.m_spriteEnemies, duckerAnimations, 5, 18, this);
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
                        m_currentState = EnemyState.BACK;
                    }


                }
                break;

            case EnemyState.BACK:
            {
                currAnimation = "waiting";
                m_dir = -Vector2.UnitX;
                if (m_pos.X < 100)
                    m_currentState = EnemyState.FORWARD;
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
              spriteBatch.Draw(m_animator.m_spriteSheet, m_pos, m_animator.m_currentSpriteRect, Color.White, 0.0f,
              new Vector2(m_animator.m_currentSpriteRect.Width, m_animator.m_currentSpriteRect.Height) / 2, 2, SpriteEffects.None, m_depth);
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
