using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;


namespace GXPEngine
{

    public class EnemyCar : Sprite
    {
        //private float wheelDist = 89;//The distance from the front wheel to back wheel
        //private float carTurn=15;//the rate of car turning (degrees)
        //private float steer_angle;
        //private float velocity = 1;//this should be done with vectors but i am peanut brain
        private float _speed = 0;//the speed of the player carr
        private float _curDirection;
        private float _doDirection;
        private float _maxSpeed;
        private State _curState = State.STARTING;//used to select the state of the car
        private float _lastX, _lastY;
        private float _lastBumpSoundTime;
        private float _bumpSoundTimeIntervalMS=500;
        private int _wp=0;
        private int _laps=-1;
        public enum State
        {
            STARTING,
            DRIVING,
            FALLING,
            RESPAWNING
        }
        

        public EnemyCar(string Car, float curDir,float maxSpeed) : base(Car+".png")
        {
            SetOrigin(width / 2, height / 2);
            _curDirection = curDir;
            _doDirection = curDir;
            _maxSpeed = maxSpeed;
        }

        public void Update()
        {
            switch (_curState)
            {  //main state for ai, used to traverse the race track
                case State.DRIVING:
                    _speed += 0.2f;
                    if (_speed > _maxSpeed)
                    {
                        _speed = _maxSpeed;
                    }
                    Move(0, -_speed);
                    if (_curDirection < _doDirection)
                    {
                        _curDirection = _curDirection + 2;

                    }
                    if (_curDirection > _doDirection)
                    {
                        _curDirection = _curDirection - 2;

                    }
                    if (_curDirection < _doDirection - 5)
                    {

                        if (_speed > 5)
                        {
                            _speed -= 0.3f;
                        }
                    }
                    if (_curDirection > _doDirection + 5)
                    {

                        if (_speed > 5)
                        {
                            _speed -= 0.3f;
                        }
                    }
                    rotation = _curDirection;
                    break;
                case State.FALLING:

                    break;
                case State.STARTING:

                    break;
                case State.RESPAWNING:
                    //no need to use, the ai are good bois which dont cheat c:
                    break;
            }
            _lastX = x;
            _lastY = y;
            

        }
        public void OnCollision(GameObject other)
        {
            if (other is Blockades)
            {
                //blockades bounces ai back if they collide with it
                if (Time.time > _lastBumpSoundTime + _bumpSoundTimeIntervalMS)
                {
                    _lastBumpSoundTime = Time.time;
                }
                if (_speed > 0)
                {
                    _speed = -5;
                }
                else if (_speed < 0)
                {
                    _speed = 5;
                }
                other.LateDestroy();
            }

            if (other is Waypoint)
            {
                //used to determine the pathing of the ai (a much needed essential) 
               
                Waypoint wayPoint = (Waypoint)other;

                if (wayPoint.GetWpNum() ==1)
                {
                    rotation = other.rotation;
                    _curDirection = other.rotation;
                    _laps++;
                }
                _doDirection = other.rotation;
                _wp=wayPoint.GetWpNum();
            }
            
        }
        public void SetState(State state)
        {
            _curState = state;
        }
        public int GetWp()
        {
            return _wp;
        }
        public int GetLap()
        {
            return _laps;
        }
    }
}
