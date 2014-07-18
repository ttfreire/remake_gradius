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
        Dictionary<int, int[]> m_frames;
        public int entityCurrentState;
        int m_spriteSheetColumns, m_spriteSheetLines;
        float m_framesPerSecond = 3.0f;
        float m_currentFrame;
        public Movable m_animatedSprite;

        public AnimationController(Texture2D spritesheet, Dictionary<int, int[]> frames, int columns, int lines, Movable asset)
        {
            m_spriteSheet = spritesheet;
            m_frames = frames;
            m_currentFrame = 0.0f;
            m_spriteSheetColumns = columns;
            m_spriteSheetLines = lines;
            m_animatedSprite = asset;
        }

        public void Update(GameTime gameTime, int state)
        {
            entityCurrentState = state;
            if (m_animatedSprite.m_isAnimatedByState)
            {
                m_currentFrame += m_framesPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_currentSpriteRect = getSprite(entityCurrentState - 1, (int)m_currentFrame);
            }
            else
            {
                m_currentFrame += m_framesPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;
                m_currentSpriteRect = getSprite(0, (int)m_currentFrame);
            }
        }

        public Rectangle getSprite(int currentState, int currentFrame)
        {
            int frameInt = currentFrame;

            int totalFrames = m_spriteSheetColumns * m_spriteSheetLines;

            if (m_animatedSprite.m_isAnimatedByState)
                frameInt = frameInt % m_frames[currentState].Length;
            else
                frameInt = frameInt % m_frames[0].Length;

            int spriteWidth = m_spriteSheet.Width / m_spriteSheetColumns;
            int spriteHeight = m_spriteSheet.Height / m_spriteSheetLines;
            int line, column;
            if (m_animatedSprite.m_isAnimatedByState)
            {
                line = m_frames[currentState][frameInt] / m_spriteSheetColumns;
                column = m_frames[currentState][frameInt] % m_spriteSheetColumns;
            }
            else
            {
                line = m_frames[0][frameInt] / m_spriteSheetColumns;
                column = m_frames[0][frameInt] % m_spriteSheetColumns;
            }

            int sx = spriteWidth * column;
            int sy = spriteHeight * line;
            Rectangle rect = new Rectangle(sx, sy, spriteWidth, spriteHeight);
            return rect;
        }

        public void playAnimation(int state)
        {

        }
    }
}
