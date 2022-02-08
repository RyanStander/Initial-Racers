using GXPEngine;
using System.Collections;
using TiledMapParser;
using System;

namespace GXPEngine
{
    
    public class Level : GameObject
    {
        private PlayerCar _player;
        private EnemyCar _enemy;
        private ArrayList _enemyCars = new ArrayList();
        private FollowCam _playerCam;
        private Waypoint _wp;
        private bool _didCompleteLaps=false;
        private Sound _countReady;
        private Sound _countGo;
        private float _startTime;
        private HUD _placement;
        private HUD _lapcounter;
        private CountScreen _countScreen;
        private int _maxWp=0;
        private int _lastLap;
        private int _playerPos;
        private int _lastPos;

        public Level(string filename) 
        {
            //intialises all the objects
            Map leveldata = MapParser.ReadMap(filename); 
            SpawnTiles(leveldata);
            SpawnObjects(leveldata);
            _player.SetMaxWp(_maxWp);
            _playerCam = new FollowCam(0, 0, 1920, 1080, _player);
            AddChild(_playerCam);
            _placement = new HUD(_player,"1st.png",-850, -450);            
            AddChild(_placement);
            HUD _lapTitle = new HUD(_player, "Laps.png", 850, -500);
            AddChild(_lapTitle);
            _lapcounter = new HUD(_player, "0out3.png",850,-400);
            AddChild(_lapcounter);
            CountdownStart();
        }
       
        

        public void Update()
        {
            //used to display the countdown of the traffic light
            if (_startTime + 1000 < Time.time &&_startTime + 1100 > Time.time)
            {
                _countReady.Play();
                _countScreen = new CountScreen("Lights1.png");
                _playerCam.AddChild(_countScreen);
            }
            if (_startTime + 2000 < Time.time  && _startTime + 2100 > Time.time )
            {
                _countReady.Play();
                _countScreen = new CountScreen("Lights2.png");
                _playerCam.AddChild(_countScreen);
            }
            if (_startTime + 3000 < Time.time  && _startTime + 3100 > Time.time )
            {
                _countReady.Play();
                _countScreen = new CountScreen("Lights3.png");
                _playerCam.AddChild(_countScreen);
            }
            if (_startTime + 4000 < Time.time && _startTime + 4100 > Time.time)
            {
                _countGo.Play();
                _countScreen = new CountScreen("LightsGo.png");
                _playerCam.AddChild(_countScreen);
                _player.SetState(PlayerCar.State.DRIVING);
                foreach (EnemyCar _enemy in _enemyCars) {
                    _enemy.SetState(EnemyCar.State.DRIVING);
                }
            }
            _playerPos = 1;
             foreach(EnemyCar _enemy in _enemyCars)
            {

                if (_player.GetLap() > _enemy.GetLap())
                {
                    _playerPos++;
                }
                else
                {
                    if (_player.GetWp() < _enemy.GetWp()|| _player.GetLap() > _enemy.GetLap())
                    {
                        _playerPos++;
                    }
                }                   
                   
                
            }
            if (_lastPos < _playerPos || _playerPos < _lastPos)
            {
                _placement.Destroy();
                switch (_playerPos)
                {
                    case 1:
                        _placement = new HUD(_player, "1st.png", -850, -450);
                        AddChild(_placement);
                        break;
                    case 2:
                        _placement = new HUD(_player, "2nd.png", -850, -450);
                        AddChild(_placement);
                        break;
                    case 3:
                        _placement = new HUD(_player, "3rd.png", -850, -450);
                        AddChild(_placement);
                        break;
                    case 4:
                        _placement = new HUD(_player, "4th.png", -850, -450);
                        AddChild(_placement);
                        break;
                }
                _lastPos = _playerPos;
            }
            //gets the players current lap and displays it.
            if (_player.GetLap() > _lastLap)
            {
                _lapcounter.Destroy();
                if (_player.GetLap() == 1)
                {
                    _lapcounter = new HUD(_player, "1out3.png", 850, -400);
                    AddChild(_lapcounter);                    
                }
                if (_player.GetLap() == 2)
                {
                    _lapcounter = new HUD(_player, "2out3.png", 850, -400);
                    AddChild(_lapcounter);
                }
                if (_player.GetLap() == 3)
                {
                    _lapcounter = new HUD(_player, "3out3.png", _player.x + 850, -400);
                    AddChild(_lapcounter);
                }
                _lastLap = _player.GetLap();
            }
            //cant figure out how to link everything to have a proper ending, can make it so that game just ends after reaching the final lap but thats about it.
            }

        public void SpawnTiles(Map leveldata)
        {
            //spawns the background of the tiled file
            if (leveldata.Layers == null || leveldata.Layers.Length == 0)    
                return;
            Layer mainLayer = leveldata.Layers[0];
            short[,] tileNumbers = mainLayer.GetTileArray();
            for (int row = 0; row < mainLayer.Height; row++)
            {
                for (int col = 0; col < mainLayer.Width; col++)
                {
                    short tileNumber = tileNumbers[col, row];
                    TileSet tiles = leveldata.GetTileSet(tileNumber);
                    if (tileNumber > 0)
                    {
                        CollisionTile tile = new CollisionTile(tiles.Image.FileName, tiles.Columns, tiles.Rows);
                        tile.SetFrame(tileNumber - tiles.FirstGId);
                        tile.x = col * tile.width;
                        tile.y = row * tile.height;
                        AddChild(tile);
                    }
                }
            }
            Layer backgroundLayer = leveldata.Layers[1];
            short[,] tileNumberss = backgroundLayer.GetTileArray();
            for (int row = 0; row < backgroundLayer.Height; row++)
            {
                for (int col = 0; col < backgroundLayer.Width; col++)
                {
                    short tileNumber = tileNumberss[col, row];
                    TileSet tiles = leveldata.GetTileSet(tileNumber);
                    if (tileNumber > 0)
                    {
                        CollisionTile tile = new CollisionTile(tiles.Image.FileName, tiles.Columns, tiles.Rows);
                        tile.SetFrame(tileNumber - tiles.FirstGId);
                        tile.x = col * tile.width;
                        tile.y = row * tile.height;
                        AddChild(tile);
                    }
                }
            }
            

            
        }
        public void SpawnObjects(Map leveldata)
        {
            //Spawns the object from the tiled file
            if (leveldata.ObjectGroups == null || leveldata.ObjectGroups.Length == 0)
                return;
            ObjectGroup objectGroup = leveldata.ObjectGroups[0];
            if (objectGroup.Objects == null || objectGroup.Objects.Length == 0)
                return;          
            foreach (TiledObject obj in objectGroup.Objects) { 
                Sprite newObj = null;
            
                switch (obj.Name)
                {
                    case "PlayerCar":
                        _player = new PlayerCar(obj.X,obj.Y);
                        newObj = _player;
                        break;
                    case "EnemyCar":
                         _enemy= new EnemyCar(obj.Type, obj.Rotation, obj.GetFloatProperty("MaxSpeed",16));
                        newObj = _enemy;
                        _enemyCars.Add(_enemy);
                        break;
                     case "blockade":
                        newObj = new Blockades(obj.Type,obj.Height,obj.Width);
                        break;
                    case "waypoint":
                        int s = int.Parse(obj.Type);
                        _wp = new Waypoint(s);

                        if (_wp != null)
                        {
                            _wp.x = obj.X;
                            _wp.y = obj.Y;
                            _wp.rotation = obj.Rotation;
                            
                            AddChild(_wp);
                            _maxWp++;                            
                        }
                        break;
                }
                if (newObj != null)
                {
                    newObj.x = obj.X;
                    newObj.y = obj.Y ;
                    newObj.rotation = obj.Rotation;  
                    AddChild(newObj);
                }                
            }
        }
        public void CountdownStart()
        {
            //initialises the game timer
            _countGo = new Sound("CountdownGo.wav", false, false);
            _countReady = new Sound("CountdownReady.wav", false, false);
            _startTime = Time.time;
            _countScreen = new CountScreen("LightsStart.png");
            _playerCam.AddChild(_countScreen);

        }
        public bool LapsCompleted()
        {
            return _didCompleteLaps;
        }
    }
}