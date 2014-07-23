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
    public enum PlayType { Once, Loop };
    public class Animation
    {
        public int[] m_frames;
        public PlayType m_type;
        float m_fps;
        public float m_currFrame;
        bool isplaying;
        public Animation(PlayType type, int[] frames, float fps)
        {
            m_frames = frames;
            m_type = type;
            m_fps = fps;
            m_currFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            m_currFrame += (m_fps * (float)gameTime.ElapsedGameTime.TotalSeconds);
            m_currFrame = m_currFrame % m_frames.Length;
            
        }

    }
}
