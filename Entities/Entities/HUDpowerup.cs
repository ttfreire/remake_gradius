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
using FuncWorks.XNA.XTiled;

namespace Gradius
{
    public enum PowerUpType {NONE, SPEEDUP, MISSILE, DOUBLE, LASER, OPTION, SHIELD }
    public class HUDpowerup
    {
        public bool m_isAvailable, m_isSelected;
        public int m_depletionCount, m_totalBeforeDepletion, m_spriteLine, m_spriteColumn, m_defaultSpriteColumn;
        public Texture2D m_sprite;
        public PowerUpType m_type;

        public HUDpowerup(int totalBeforeDepletion, Texture2D sprite, PowerUpType type, int spriteLine, int spriteColumn)
        {
            m_totalBeforeDepletion = totalBeforeDepletion;
            m_depletionCount = totalBeforeDepletion;
            m_sprite = sprite;
            m_type = type;
            m_spriteLine = spriteLine;
            m_spriteColumn = spriteColumn;
            m_defaultSpriteColumn = spriteColumn;
        }

        public void setAvailability(bool b_value)
        {
            m_isAvailable = b_value;
            if (b_value)
                m_spriteColumn = m_defaultSpriteColumn;
            else
                m_spriteColumn = 6;
        }

        public void setSelection(bool b_value)
        {
            m_isSelected = b_value;
            if (b_value)
                m_spriteLine = 1;
            else
                m_spriteLine = 0;
        }

        public void deplete()
        {
            m_depletionCount--;
            if (m_depletionCount == 0)
                setAvailability(false);
        }

        public Rectangle getSpriteRectangle()
        {
            int spriteWidth = m_sprite.Width / 7;
            int spriteHeight = m_sprite.Height / 2;

            int sx = spriteWidth * m_spriteColumn;
            int sy = spriteHeight * m_spriteLine;
            Rectangle rect = new Rectangle(sx, sy, spriteWidth, spriteHeight);
            return rect;
        }

        public bool isDepleted()
        {
            if (m_depletionCount == 0)
                return true;
            else return false;
        }
    }
}
