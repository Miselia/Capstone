using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;

namespace Assets.MonoScript
{
    public class CenteredRectangle
    {
        public float width;
        public float height;
        public float3 center;
        public float3 topLeft;
        public float3 botRight;

        public CenteredRectangle(float width, float height, float3 center)
        {
            this.width = width;
            this.height = height;
            this.center = center;
            topLeft = new float3(center.x - width / 2, center.y + height / 2, 0);
            botRight = new float3(center.x + width / 2, center.y - height / 2, 0);
        }

        public bool Intersects(CenteredRectangle other)
        {
            if (topLeft.x > other.botRight.x || other.topLeft.x > botRight.x)
                return false;
            if (topLeft.y < other.botRight.y || other.topLeft.y < botRight.y)
                return false;

            return true;
        }

        public bool Contains(CenteredRectangle other)
        {
            if (topLeft.x <= other.topLeft.x && topLeft.y >= other.topLeft.y &&
                botRight.x >= other.botRight.x && botRight.y <= other.botRight.y)
            {
                return true;
            }
            return false;
        }
    }
}
