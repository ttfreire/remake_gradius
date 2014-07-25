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
    class Rugal : Enemy
    {
      float center;
      float default_maxVel;
      public enum EnemyState {NONE, FORWARD, DIAGONAL, RETREAT, EXPLODED}
      public EnemyState m_currentState = EnemyState.DIAGONAL;
      public AnimationController m_animator;
      float m_timeToDie;

      public Rugal(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;
          center = m_pos.Y;
          default_maxVel = maxVel;
          int[] rugalAnimationFramesDiagonalUp = { 6 };
          Animation rugalAnimationDiagonalUp = new Animation(PlayType.Once, rugalAnimationFramesDiagonalUp, 1.0f);
          int[] rugalAnimationFramesDiagonalDown = { 7 };
          Animation rugalAnimationDiagonalDown = new Animation(PlayType.Once, rugalAnimationFramesDiagonalDown, 1.0f);
          int[] rugalAnimationFramesForward = { 8 };
          Animation rugalAnimationForward = new Animation(PlayType.Once, rugalAnimationFramesForward, 1.0f);
          int[] rugalAnimationFramesExploded = { 80, 81, 82, 83 };
          Animation rugalAnimationExploded = new Animation(PlayType.Once, rugalAnimationFramesExploded, 5.0f);

          Dictionary<string, Animation> rugalAnimations = new Dictionary<string, Animation>() { { "forward", rugalAnimationForward },
                                                                                                { "diagonal up", rugalAnimationDiagonalUp },
                                                                                                { "diagonal down", rugalAnimationDiagonalDown },
                                                                                                { "exploded", rugalAnimationExploded} };
          m_animator = new AnimationController(m_world.m_spriteEnemies, rugalAnimations, 5, 18);
          currAnimation = "forward";
      }

    public override void Update(GameTime gameTime) 
    {
        Player player = (Player) m_world.m_entities.Find(s => s is Player);
        m_animator.Update(gameTime, currAnimation);
        switch (m_currentState)
        {
            case EnemyState.DIAGONAL:
                {
                    m_maxVel = default_maxVel;
                    if (player != null)
                    {
                        if (m_pos.X > player.m_pos.X)
                            if (m_pos.Y < player.m_pos.Y)
                            {
                                m_dir.X -= 5.0f;
                                m_dir.Y += 2.0f;
                                currAnimation = "diagonal down";
                            }
                            else
                            {
                                m_dir.X -= 5.0f;
                                m_dir.Y -= 2.0f;
                                currAnimation = "diagonal up";
                            }
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
                    currAnimation = "forward";
                    m_dir = -Vector2.UnitX;
                    this.m_maxVel = 200;

                    if (m_pos.X < 0)
                        m_world.Remove(this);

                    if(player != null)
                        if (m_pos.Y < player.m_pos.Y - player.m_spriteSize.Y / 4 ||
                            m_pos.Y > player.m_pos.Y + player.m_spriteSize.Y / 4)
                        {
                            m_currentState = EnemyState.DIAGONAL;
                            m_vel = Vector2.Zero;
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
