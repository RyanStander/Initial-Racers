using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    class FollowCam : Camera 
    {
        private Sprite _target;

        public FollowCam(int winX, int winY, int winWidth, int winHeight, Sprite target) : base(winX,winY,winWidth,winHeight)
        {
            _target = target;
        }

        // follows the player around

        public void Update()
        {
            x = _target.x;
            y = _target.y;
        }
    }
}
