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

namespace Gradius {

  
  public class Enemy : Character {
      public enum EnemyState { FORWARD, DIAGONAL, RETREAT }
      public EnemyState currentState = EnemyState.FORWARD;
      public WorldMap worldmap;
      public List<Enemy> mySquad;
      public Enemy(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite, 
          MovableType type, Texture2D projectileSprite, WorldMap map, List<Enemy> squad) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite)
      {
          worldmap = map;
          mySquad = squad;
      }

    public override void Update(GameTime gameTime) {

      //@REVIE@ AI must fill "m_dir" to move this enemy!
        switch (currentState)
        {
            case EnemyState.FORWARD:
                {
                    m_dir.X -= 1.0f;

                    if (m_pos.X < worldmap.m_screenMiddle.X)
                    {
                        currentState = EnemyState.DIAGONAL;
                        m_vel = Vector2.Zero;
                    }
                }
                break;
            case EnemyState.DIAGONAL:
                {
                    if (m_pos.Y <= worldmap.m_screenMiddle.Y)
                    {
                        m_dir.X += 1.0f;
                        m_dir.Y += 2.0f;
                    }
                    else
                    {
                        m_dir.X += 1.0f;
                        m_dir.Y -= 2.0f;
                    }
                    foreach (Entity e in m_world.m_entities)
                    {
                        if (e is Player)
                        {
                            Player player = (Player)e;
                            if (m_pos.Y >= player.m_pos.Y - player.m_spriteSize.Y / 4 &&
                                m_pos.Y <= player.m_pos.Y + player.m_spriteSize.Y / 4)
                            {
                                currentState = EnemyState.RETREAT;
                                m_vel = Vector2.Zero;
                            }
                            break;
                        }
                    }
                    
                }
                break;
            case EnemyState.RETREAT:
                {
                    m_dir = Vector2.UnitX;
                    m_maxVel = 300;

                    if (m_pos.X > worldmap.screenWidth)
                        m_world.Remove(this);
                }
                break;
        }
     

      base.Update(gameTime);
    }

    public void addToSquad()
    {
        mySquad.Add(this);
    }

    public override void Die()
    {
        Console.WriteLine("Enemy ID = " + m_id.ToString());
        if (mySquad != null && mySquad.Count == 1)
            dropPowerUp();
        mySquad.Remove(this);
        m_world.Remove(this);
    }

      public void dropPowerUp()
      {
          PowerUp powerup = new PowerUp(m_world, this.m_pos);
          m_world.Add(powerup);
      }
  }


}
