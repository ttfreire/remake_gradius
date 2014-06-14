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

  public enum EnemyState {FORWARD, DIAGONAL, RETREAT }
  public class Enemy : Character {

      public EnemyState currentState = EnemyState.FORWARD;
      public WorldMap worldmap;
      public Enemy(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, Texture2D sprite, MovableType type, Texture2D projectileSprite, WorldMap map) :
          base(world, pos, size, maxVel, accel, friction, sprite, type, projectileSprite)
      {
          worldmap = map;
      }

    public override void Update(GameTime gameTime) {

      //@REVIE@ AI must fill "m_dir" to move this enemy!
        switch (currentState)
        {
            case EnemyState.FORWARD:
                {
                    m_dir.X -= 1.0f;

                    if (m_pos.X < worldmap.m_screenMiddle.X)
                        currentState = EnemyState.DIAGONAL;
                }
                break;
            case EnemyState.DIAGONAL:
                {
                    if (m_pos.Y <= worldmap.m_screenMiddle.Y)
                    {
                        m_dir.X += 1.0f;
                        m_dir.Y += 1.0f;
                    }
                    else
                    {
                        m_dir.X += 1.0f;
                        m_dir.Y -= 1.0f;
                    }
                    foreach (Entity e in m_world.m_entities)
                    {
                        if (e is Player)
                        {
                            Player player = (Player)e;
                            if (m_pos.Y == player.m_pos.Y)
                                currentState = EnemyState.RETREAT;
                            break;
                        }
                    }
                    
                }
                break;
            case EnemyState.RETREAT:
                {
                    m_dir.Y = 0.0f;
                    m_dir.X += 1.0f;
                }
                break;
        }
     

      base.Update(gameTime);
    }

    
  }
}
