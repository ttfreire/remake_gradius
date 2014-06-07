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

      public Player nearestPlayer = null;
      public float nearestPlayerDistance = 9999f;
    public Enemy(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, Texture2D sprite, MovableType type) :
      base(world, pos, size, maxVel, accel, friction, sprite, type) {
      
    }

    public override void Update(GameTime gameTime) {

      //@REVIE@ AI must fill "m_dir" to move this enemy!


        Player nearestPlayer = null;
        float  nearestPlayerDist = 9999.0f;
       
       foreach (Movable e in m_world.m_entities)
        {
            if (e is Player)
            {
                if (this.TestVision(e, MovableType.Player))
                {
                    if (nearestPlayer == null)
                    {
                        nearestPlayer = (Player)e;
                        nearestPlayerDist = Vector2.Distance(this.m_pos, nearestPlayer.m_pos);
                    }
                    else
                    {
                        float distance = Vector2.Distance(this.m_pos, e.m_pos);
                        if (distance < nearestPlayerDist)
                        {
                            nearestPlayerDist = distance;
                            nearestPlayer = (Player)e;
                        }
                    }
            }
            }
            
        }

       if (nearestPlayer != null)
       {
           m_dir = nearestPlayer.m_pos - m_pos;
       }
       else m_dir = Vector2.Zero;
      

      base.Update(gameTime);
    }

    
  }
}
