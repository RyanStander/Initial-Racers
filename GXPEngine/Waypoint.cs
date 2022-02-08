using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;

    public class Waypoint : Sprite
    {
        private int _wpNum;
        public Waypoint(int wpNum) : base ("square.png")
        {
        alpha=0;
        SetOrigin(width / 8, height / 2);
        SetScaleXY(8, 1);
        _wpNum = wpNum;
        }
        public int GetWpNum()
        {
            return _wpNum;
        }
    public void SetFinish()
    {

    }
    }

