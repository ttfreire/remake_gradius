﻿using System;
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

  public class Player : Character {
    KeyboardState previousKey = Keyboard.GetState();
    KeyboardState currentKey = Keyboard.GetState();
    public Player(Game1 world, Vector2 pos, Vector2 size, float maxVel, float accel, float friction, Texture2D sprite, MovableType type) :
      base(world, pos, size, maxVel, accel, friction, sprite, type) {

      m_depth -= 0.1f;
    }

    public override void Update(GameTime gameTime) {

      //fill direction vector using the keyboard:

      m_dir = Vector2.Zero;
      currentKey = Keyboard.GetState();

      if (currentKey.IsKeyUp(Keys.Z))
          previousKey = currentKey;

      if (currentKey.IsKeyDown(Keys.Right))
        m_dir.X += 1.0f;

      if (currentKey.IsKeyDown(Keys.Left))
      {
          m_dir.X += -1.0f;
          previousKey = currentKey;
      }

      if (currentKey.IsKeyDown(Keys.Up))
      {
          m_dir.Y += -1.0f;
          previousKey = currentKey;
      }

      if (currentKey.IsKeyDown(Keys.Down))
      {
          m_dir.Y += 1.0f;
          previousKey = currentKey;
      }

      if (previousKey != currentKey)
          if (currentKey.IsKeyDown(Keys.Z))
          {
              base.Shoot();
              previousKey = currentKey;
          }

      base.Update(gameTime);
    }
  }
}
