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
    public class AnimationController
    {
        public Texture2D m_spriteSheet;
        public Rectangle m_currentSpriteRect;
        int[] m_frames;
        public int entityCurrentState;
        int m_spriteSheetColumns, m_spriteSheetLines;
        float m_framesPerSecond = 12.0f;
        float m_currentFrame;

        public AnimationController(Texture2D spritesheet, int[] frames, int columns, int lines)
        {
            m_spriteSheet = spritesheet;
            m_frames = frames;
            m_currentFrame = 0.0f;
            m_spriteSheetColumns = columns;
            m_spriteSheetLines = lines;
            m_currentSpriteRect = getSprite(entityCurrentState);
        }

        public void Update(GameTime gameTime, int state)
        {
            entityCurrentState = state;
            if (entityCurrentState != 0)
                m_currentSpriteRect = getSprite(entityCurrentState-1);
            else
            {
                m_currentFrame += m_framesPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_currentSpriteRect = getSprite((int)m_currentFrame);
            }
        }

        public Rectangle getSprite(int currentState)
        {
            int frameInt = currentState;

            int totalFrames = m_spriteSheetColumns * m_spriteSheetLines;

            frameInt = frameInt % m_frames.Length;

            int spriteWidth = m_spriteSheet.Width / m_spriteSheetColumns;
            int spriteHeight = m_spriteSheet.Height / m_spriteSheetLines;

            int line = m_frames[frameInt] / m_spriteSheetColumns;
            int column = m_frames[frameInt] % m_spriteSheetColumns;

            int sx = spriteWidth * column;
            int sy = spriteHeight * line;
            Rectangle rect = new Rectangle(sx, sy, spriteWidth, spriteHeight);
            return rect;
        }
    }
}
