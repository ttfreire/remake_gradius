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
    class Garun : Enemy
    {
      float center;
      float offset; //The offset to add to your Y
      float radius; //Whatever you want your radius to be
      public enum EnemyState { FORWARD, DIAGONAL, RETREAT }
      public EnemyState currentState = EnemyState.FORWARD;
      public Garun(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, float rateoffire, float continuousrateoffire, Texture2D sprite,
          MovableType type, Texture2D projectileSprite, List<Enemy> squad, WorldMap map, bool dropsPowerUp, AnimationController animator, bool isAnimatedByState) :
          base(world, pos, size, maxVel, accel, friction, rateoffire, continuousrateoffire, sprite, type, projectileSprite, squad, dropsPowerUp, animator, isAnimatedByState)
      {
          worldmap = map;
          center = m_pos.Y;
          offset = 0; //The offset to add to your Y
          radius = 50; //Whatever you want your radius to be
      }

    public override void Update(GameTime gameTime) 
    {
      m_dir = new Vector2(-1, 0); 
      double msElapsed = gameTime.TotalGameTime.TotalMilliseconds / 150;
      offset = (float)Math.Sin(msElapsed) * radius;
      m_pos.Y = center + offset;
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
