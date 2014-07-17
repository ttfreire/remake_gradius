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
      public enum EnemyState { FORWARD, SHOOT, BACK }
      public EnemyState currentState = EnemyState.FORWARD;
      public Ducker(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp)
      {
          worldmap = map;
      }

    public override void Update(GameTime gameTime) {
        Player player = (Player) m_world.m_entities.Find(s => s is Player);
        
        switch (currentState)
        {
            case EnemyState.FORWARD:
                {
                    m_dir = Vector2.UnitX;

                    if (player != null)
                        if (m_pos.X > player.m_pos.X + 300)
                        {
                            currentState = EnemyState.SHOOT;
                            m_vel = Vector2.Zero;
                        }
                }
                break;
            case EnemyState.SHOOT:
                {
                    m_dir = Vector2.Zero;
                    if (player != null)
                    {
                        Vector2 dir = (player.m_pos - m_pos) + new Vector2(player.m_size.Length(), 0);
                        Shoot(dir, new Vector2(this.m_pos.X, this.m_pos.Y), dir);
                        currentState = EnemyState.BACK;                     
                    }
                  

                }
                break;
                case EnemyState.BACK:
                {
                    m_dir = -Vector2.UnitX;
                    if (m_pos.X < 100)
                        currentState = EnemyState.FORWARD;
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
