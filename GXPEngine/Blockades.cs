using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GXPEngine
{

    public class Blockades : Sprite
    {
        public Blockades(string filename, float bheight, float bwidth) : base(filename + ".png")
        {
            
            width = (int)bwidth;
            height = (int)bheight;
            SetOrigin(width/2, height/2);
        }
       

    }
}
