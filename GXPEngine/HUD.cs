using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GXPEngine;

namespace GXPEngine
{
    public class HUD : Sprite
    {
        private float _xPos, _yPos;
        private PlayerCar _player;
        public HUD(PlayerCar playerCar, string filename,float xPos,float yPos) : base(filename)
        {
            //a normal hud does not work with a moving screen so i had to use sprites (the hud appears black and unreadable, teacher said the follow camera is the reason for it)
            _player = playerCar;
            SetOrigin(width / 2, height / 2);
            _xPos = xPos;
            _yPos = yPos;
        }
        public void Update()
        {


            x = _player.x+ _xPos;
            y = _player.y+ _yPos;
        }
    }
}
