using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.MonoScript
{
    public class CenteredRectangle
    {
        public float width;
        public float height;
        public Vector2 center;
        public Vector2 topLeft;
        public Vector2 botRight;

        public CenteredRectangle(float w, float h, Vector2 c)
        {
            width = w;
            height = h;
            center = c;
            topLeft = new Vector2(center.X - width / 2, center.Y + height / 2);
            botRight = new Vector2(center.X + width / 2, center.Y - height / 2);
        }

        public bool Intersects(CenteredRectangle other)
        {
            if (topLeft.X > other.botRight.X || other.topLeft.X > botRight.X)
                return false;
            if (topLeft.Y < other.botRight.Y || other.topLeft.Y < botRight.Y)
                return false;

            return true;
        }

        public bool Contains(CenteredRectangle other)
        {
            if (topLeft.X <= other.topLeft.X && topLeft.Y >= other.topLeft.Y &&
                botRight.X >= other.botRight.X && botRight.Y <= other.botRight.Y)
            {
                return true;
            }
            return false;
        }
    }
}
