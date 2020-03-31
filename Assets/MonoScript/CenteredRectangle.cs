using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.MonoScript
{
    public class CenteredRectangle
    {
        public float width;
        public float height;
        public Vector3 center;
        public Vector3 topLeft;
        public Vector3 botRight;

        public CenteredRectangle(float width, float height, Vector3 center)
        {
            this.width = width;
            this.height = height;
            this.center = center;
            topLeft = new Vector3(center.x - width / 2, center.y + height / 2, 0);
            botRight = new Vector3(center.x + width / 2, center.y - height / 2, 0);
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
