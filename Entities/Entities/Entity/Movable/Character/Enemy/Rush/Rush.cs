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
    class Rush : Enemy
    {
      float center;
      float default_maxVel;
      public enum EnemyState {NONE, UP, FORWARD, EXPLODED}
      public EnemyState m_currentState = EnemyState.UP;
      public AnimationController m_animator;
      float m_timeToDie;
      bool isSpawned = false;

      public Rush(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;
          center = m_pos.Y;
          default_maxVel = maxVel;
          int[] RushAnimationFrames = { 10, 11, 12, 13, 14, 15, 16, 17, 18 };
          Animation RushAnimation = new Animation(PlayType.Once, RushAnimationFrames, 1.0f);
          int[] RushAnimationFramesExploded = { 80, 81, 82, 83 };
          Animation RushAnimationExploded = new Animation(PlayType.Once, RushAnimationFramesExploded, 5.0f);

          Dictionary<string, Animation> RushAnimations = new Dictionary<string, Animation>() { { "moving", RushAnimation },
                                                                                                { "exploded", RushAnimationExploded} };
          m_animator = new AnimationController(m_world.m_spriteEnemies, RushAnimations, 5, 18, this);
          currAnimation = "moving";
      }

    public override void Update(GameTime gameTime) 
    {
        Player player = (Player) m_world.m_entities.Find(s => s is Player);
        m_animator.Update(gameTime, currAnimation);
        switch (m_currentState)
        {
            case EnemyState.UP:
                {
                    if (m_pos.Y > m_world.m_worldMap.m_screenMiddle.Y && !isSpawned)
                    {
                        m_dir = -Vector2.UnitY;
                        isSpawned = true;
                    }
                    else if (m_pos.Y < m_world.m_worldMap.m_screenMiddle.Y && !isSpawned)
                    {
                        m_dir = Vector2.UnitY;
                        isSpawned = true;
                    }
                    m_maxVel = default_maxVel;
                    if (player != null)
                    {
                        if (m_pos.Y >= player.m_pos.Y - player.m_spriteSize.Y / 4 &&
                            m_pos.Y <= player.m_pos.Y + player.m_spriteSize.Y / 4)
                        {
                            m_currentState = EnemyState.FORWARD;
                            m_vel = Vector2.Zero;
                        }
                    }

                }
                break;
            case EnemyState.FORWARD:
                {
                    currAnimation = "moving";
                    m_dir = -Vector2.UnitX;
                    this.m_maxVel = 500;

                    if (m_pos.X < 0)
                        m_world.Remove(this);

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
