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
        public Dictionary<string, Animation> m_animations;
        public string entityCurrentAnimation;
        int m_spriteSheetColumns, m_spriteSheetLines;
        float m_currentFrame;

        public AnimationController(Texture2D spritesheet, Dictionary<string, Animation> frames, int columns, int lines)
        {
            m_spriteSheet = spritesheet;
            m_animations = frames;
            m_currentFrame = 0.0f;
            m_spriteSheetColumns = columns;
            m_spriteSheetLines = lines;
        }

        public void Update(GameTime gameTime, string state)
        {
            entityCurrentAnimation = state;
            m_currentFrame = m_animations[entityCurrentAnimation].m_currFrame;
            m_currentSpriteRect = getSprite(entityCurrentAnimation, (int)m_currentFrame);
            m_animations[state].Update(gameTime);

        }

        public Rectangle getSprite(string currentAnimation, int currentFrame)
        {
            int frameInt = currentFrame;

            int totalFrames = m_spriteSheetColumns * m_spriteSheetLines;

            frameInt = frameInt % m_animations[currentAnimation].m_frames.Length;

            int spriteWidth = m_spriteSheet.Width / m_spriteSheetColumns;
            int spriteHeight = m_spriteSheet.Height / m_spriteSheetLines;
            int line, column;

            line = m_animations[currentAnimation].m_frames[currentFrame] / m_spriteSheetColumns;
            column = m_animations[currentAnimation].m_frames[currentFrame] % m_spriteSheetColumns;

            int sx = spriteWidth * column;
            int sy = spriteHeight * line;
            Rectangle rect = new Rectangle(sx, sy, spriteWidth, spriteHeight);
            return rect;
        }
    }
}
