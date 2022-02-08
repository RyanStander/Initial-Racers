using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using GXPEngine.Core;


namespace GXPEngine
{

    public class PlayerCar : Sprite
    {
        private float _speed = 0;//the speed of the player carr
        private float _turn; //used to calculate the rotation of the car
        private int _wp=1; //used to set where the player currently is
        private int _prevWp;//the value of the last waypoing the player hit
        private float _prevX;//the last x value the player was at when hitting a waypoint
        private float _prevY;//the last y value the player was at when hitting a waypoint
        private float _prevRot=90;//the last rotation value the player was at when hitting a waypoint
        private Sound _rev;//sound of the engine
        private SoundChannel _revChannel;//settings for engine sounds
        private Sound _bump;//sound played when colliding with a blockade
        int lastBumpSoundTime;//the last time a player hit a barrel
        int bumpSoundTimeIntervalMS = 500;//the time between collision allowed
        private State _curState=State.STARTING;//used to select the state of the car
        private int _playerPos;
        private int _lapCount=0;
        private int _maxWp;
        private float _respTime=0;//used to set the point at which the player starts respawning
        public enum State
        {
            STARTING,
            DRIVING,
            FALLING,
            RESPAWNING
        }
        public PlayerCar(float lastx,float lasty) : base("car5.png")
        {
            SetOrigin(width / 2, height / 2);
            StartSound();
            _bump = new Sound("Impact.wav", false, false);
        }

        public void Update()        
        {
            _turn = rotation;
            //used for collision with barrels to avoid getting stuck inside of them(for the most part)
            switch (_curState)
            {
                //the main state the player is in, driving with arrow keyes
                case State.DRIVING:
                    if (Input.GetKey(Key.LEFT))
                    {
                        _turn -= 2;
                    }
                    if (Input.GetKey(Key.RIGHT))
                    {
                        _turn += 2;
                    }
                    if (Input.GetKey(Key.UP))
                    {
                        _speed += 0.2f;
                        _revChannel.Frequency = _revChannel.Frequency + 200;
                        if (_revChannel.Frequency > 150000)
                        {
                            _revChannel.Frequency = 150000;
                        }
                        if (_speed > 16)
                        {
                            _speed = 16;
                        }
                    }
                    if (Input.GetKey(Key.DOWN))
                    {
                        _speed -= 0.3f;
                        _revChannel.Frequency = _revChannel.Frequency - 1000;
                        if (_revChannel.Frequency < 50000)
                        {
                            _revChannel.Frequency = 50000;
                        }
                        if (_speed < -10)
                        {
                            _speed = -10;
                        }
                    }
                    if (!(Input.GetKey(Key.UP)) && !(Input.GetKey(Key.DOWN)))
                    {
                        if (_speed > 0.1)
                        {
                            _speed -= 0.1f;
                        }
                        if (_speed < -0.1)
                        {
                            _speed += 0.1f;
                        }
                        _revChannel.Frequency = _revChannel.Frequency - 500;
                        if (_revChannel.Frequency < 60000)
                        {
                            _revChannel.Frequency = 60000;
                        }

                    }
                    rotation = _turn;
                    Move(0, -_speed);
                    break;
                case State.FALLING:

                    break;
                case State.STARTING:

                    break;
                case State.RESPAWNING:
                    //this state goes on when the player skips too many waypoints and is returned to the last one they touched.
                    _revChannel.Frequency = 60000;
                    if (_respTime == 0)
                    {
                        _respTime =Time.time;
                        Console.WriteLine("Set to active");
                    }   
                    if(_respTime>Time.time)
                    {
                        this.alpha = 0;
                    }
                    if (_respTime + 700 < Time.time)
                    {
                        this.alpha = 1;
                    }
                    if (_respTime + 1300 < Time.time)
                    {
                        this.alpha = 0;
                    }
                    if (_respTime + 1800 < Time.time)
                    {
                        this.alpha = 1;
                    }
                    if (_respTime + 2200 < Time.time)
                    {
                        this.alpha = 0;
                    }
                    if (_respTime + 2500 < Time.time)
                    {
                        this.alpha = 1;
                        _respTime = 0;
                        SetState(State.DRIVING);
                        
                    }
                    break;
            }
            
            
        }

        

        public void OnCollision (GameObject other)
        {
            if (other is Blockades)
            {
                //obstacles that if the player touchers will push them away and the obstacle is destroyed
                if (Time.time > lastBumpSoundTime + bumpSoundTimeIntervalMS) {
                    _bump.Play();
                    Console.WriteLine("Playing bump sound");
                    lastBumpSoundTime = Time.time;
                }
                if (_speed > 0)
                {
                    _speed = -5;
                    _revChannel.Frequency = 50000;
                }
                else if (_speed < 0)
                {
                    _speed = 5;
                    _revChannel.Frequency = 50000;
                }
                other.LateDestroy();
            }
            if (other is Waypoint)
            {
             //the waypoints used to know how far the player is through the track   
                Waypoint wayPoint = (Waypoint)other;
                if (!(wayPoint.GetWpNum() == _wp))
                {
                    
                    
                    if (_prevWp < _wp - 3)
                    {
                        x = _prevX;
                        y = _prevY;
                        rotation = _prevRot;
                        SetState(State.RESPAWNING);
                    }
                    else
                    {
                        if (_maxWp-3 > _prevWp&& wayPoint.GetWpNum()==1)
                        {
                            x = _prevX;
                            y = _prevY;
                            rotation = _prevRot;
                            SetState(State.RESPAWNING);
                        }
                         else{
                            _prevX = x;
                            _prevY = y;
                            _prevRot = other.rotation;
                            _prevWp = _wp;
                        
                        _wp = wayPoint.GetWpNum();
                            if (_wp == 1)
                            {
                                Console.WriteLine("up");
                                _lapCount++;
                            }
                        }
                    }
                }                
            }
        }
        public int GetWp()
        {
            return _wp;
        }
        public void StartSound()
        {
            _rev = new Sound("Idle.wav", true, false);
            _revChannel = _rev.Play();
            _revChannel.Volume = 0.6f;

        }

        public void SetState(State state)
        {
            _curState = state;
            
        }
        public void SetPosition(int tempPos)
        {

        }
        public void SetMaxWp(int maxWp)
        {
            _maxWp = maxWp;
        }
        public int GetLap()
        {
            return (_lapCount);
        }
    }
}
