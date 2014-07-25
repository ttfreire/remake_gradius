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
    public class HUDController
    {
        public List<HUDpowerup> m_powerupHUD;
        public int m_selectedPowerupIndex;
        Texture2D m_livesIcon;
        public int m_lives = 3;
        int m_numberofPlayers = 1;
        int m_score = 0;
        int m_higherScore = 50000;
        Vector2 m_pos;
        SpriteFont m_hudFont;

        public HUDController(List<HUDpowerup> powerupHUD, Texture2D livesIcon, Vector2 pos, SpriteFont hudFont)
        {
            m_powerupHUD = powerupHUD;
            m_livesIcon = livesIcon;
            m_pos = pos;
            m_hudFont = hudFont;
            m_selectedPowerupIndex = -1;
        }

        public void Update(GameTime gameTime)
        {
            if (m_score > m_higherScore)
                m_higherScore = m_score;

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            // Powerups
            
            for (int i = 0; i < m_powerupHUD.Count; i++)
            {
                Rectangle powerUpRect = m_powerupHUD[i].getSpriteRectangle();
                spriteBatch.Draw(m_powerupHUD[i].m_sprite, m_pos + new Vector2(40+ i * powerUpRect.Width, 0), powerUpRect, Color.White, 0.0f,
                new Vector2(m_powerupHUD[i].m_sprite.Width, m_powerupHUD[i].m_sprite.Height) / 2, 1, SpriteEffects.None, 0);
            }

            // Lives
            spriteBatch.Draw(m_livesIcon, m_pos + new Vector2(-200, 20), null, Color.White, 0.0f,
                new Vector2(m_livesIcon.Width, m_livesIcon.Height) / 2, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(m_hudFont, m_lives.ToString(), m_pos + new Vector2(-180, 20), Color.White, 0.0f,
                m_hudFont.MeasureString(m_lives.ToString()) / 2, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(m_hudFont, m_numberofPlayers.ToString()+"P", m_pos + new Vector2(-120, 20), Color.White, 0.0f,
                m_hudFont.MeasureString(m_lives.ToString()) / 2, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(m_hudFont, m_score.ToString(), m_pos + new Vector2(-60, 20), Color.White, 0.0f,
                m_hudFont.MeasureString(m_lives.ToString()) / 2, 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(m_hudFont, "HI   "+m_higherScore.ToString(), m_pos + new Vector2(50, 20), Color.White, 0.0f,
                m_hudFont.MeasureString(m_lives.ToString()) / 2, 1, SpriteEffects.None, 0);


            spriteBatch.End();
        }

        public void subtractLife()
        {
            if (m_lives > 0)
                m_lives--;
        }

        public void updateScore(int points)
        {
            m_score += points;
        }

        public void selectPowerupsHUD()
        {

            if (m_powerupHUD[m_selectedPowerupIndex].m_isAvailable)
            {
                m_powerupHUD[m_selectedPowerupIndex].setSelection(false);

                if (m_powerupHUD[m_selectedPowerupIndex].m_type == PowerUpType.DOUBLE)
                {
                    int laserPowerUpIndex = m_powerupHUD.FindIndex(s => s.m_type == PowerUpType.LASER);
                    m_powerupHUD[laserPowerUpIndex].setAvailability(true);
                    m_powerupHUD[laserPowerUpIndex].setSelection(false);
                    m_powerupHUD[laserPowerUpIndex].m_depletionCount = m_powerupHUD[laserPowerUpIndex].m_totalBeforeDepletion;
                }
                if (m_powerupHUD[m_selectedPowerupIndex].m_type == PowerUpType.LASER)
                {
                    int doublePowerUpIndex = m_powerupHUD.FindIndex(s => s.m_type == PowerUpType.DOUBLE);
                    m_powerupHUD[doublePowerUpIndex].setAvailability(true);
                    m_powerupHUD[doublePowerUpIndex].setSelection(false);
                    m_powerupHUD[doublePowerUpIndex].m_depletionCount = m_powerupHUD[doublePowerUpIndex].m_totalBeforeDepletion;
                }
                m_powerupHUD[m_selectedPowerupIndex].deplete();
                resetPowerupHUD();
            }
        }

        public void nextPowerupHUD()
        {
            if (m_selectedPowerupIndex == -1)
            {
                m_selectedPowerupIndex++;
                m_powerupHUD[m_selectedPowerupIndex].setSelection(true);
            }
            else if (m_selectedPowerupIndex < m_powerupHUD.Count - 1)
            {
                if (!m_powerupHUD[m_selectedPowerupIndex].isDepleted())
                    m_powerupHUD[m_selectedPowerupIndex].setAvailability(true);
                m_powerupHUD[m_selectedPowerupIndex].setSelection(false);
                m_selectedPowerupIndex++;
                m_powerupHUD[m_selectedPowerupIndex].setSelection(true);
            }

        }

        public void resetPowerupHUD()
        {
            m_selectedPowerupIndex = -1;
        }
    }
}
