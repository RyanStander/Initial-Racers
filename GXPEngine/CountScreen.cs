using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{
    public class CountScreen : Sprite
    
    {
        private float _timer;
        public CountScreen(string filename) : base(filename)
        {
            SetOrigin(width / 2, height * 2);
            SetScaleXY(0.5f);
            _timer = Time.time;
        }
        public void Update()
        {
            if (_timer+1000 < Time.time)
            {
                Destroy();
            }
        }
    }
}
