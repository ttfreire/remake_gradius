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
      float offset; //The offset to add to your Y
      float radius; //Whatever you want your radius to be
      float default_maxVel;
      public enum EnemyState { FORWARD, DIAGONAL, RETREAT }
      public EnemyState currentState = EnemyState.DIAGONAL;
      public Rugal(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;
          center = m_pos.Y;
          offset = 0; //The offset to add to your Y
          radius = 50; //Whatever you want your radius to be
          default_maxVel = maxVel; 
      }

    public override void Update(GameTime gameTime) 
    {
        Player player = (Player) m_world.m_entities.Find(s => s is Player);
        //if (this.m_pos.X > player.m_pos.X)
        //    m_dir = player.m_pos - this.m_pos;

        switch (currentState)
        {
            case EnemyState.DIAGONAL:
                {
                    m_maxVel = default_maxVel;
                    if (m_pos.X > player.m_pos.X)
                        if (m_pos.Y < player.m_pos.Y)
                        {
                            m_dir.X -= 5.0f;
                            m_dir.Y += 2.0f;
                        }
                        else
                        {
                            m_dir.X -= 5.0f;
                            m_dir.Y -= 2.0f;
                        }
                        if (m_pos.Y >= player.m_pos.Y - player.m_spriteSize.Y / 4 &&
                            m_pos.Y <= player.m_pos.Y + player.m_spriteSize.Y / 4)
                        {
                            currentState = EnemyState.FORWARD;
                            m_vel = Vector2.Zero;
                        }

                }
                break;
            case EnemyState.FORWARD:
                {
                    m_dir = -Vector2.UnitX;
                    this.m_maxVel = 200;

                    if (m_pos.X < 0)
                        m_world.Remove(this);

                        if (m_pos.Y < player.m_pos.Y - player.m_spriteSize.Y / 4 ||
                            m_pos.Y > player.m_pos.Y + player.m_spriteSize.Y / 4)
                        {
                            currentState = EnemyState.DIAGONAL;
                            m_vel = Vector2.Zero;
                        }
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
    }
}
