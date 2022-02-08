using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace GXPEngine
{
    public class Menu : Sprite
    {
        private Sound _music;
        private SoundChannel _musicChannel;
        private Sound _musicDesert;
        private SoundChannel _musicDesertChannel;
        private Button _exitButton;
        private Button _startButton;
        private bool _hasStarted=false;
        public Menu() : base("initialStartup.jpg")
        {
            SetScaleXY(2.4f, 2.4f);
            _exitButton = new Button("ExitLogo.png");
            AddChild(_exitButton);
            _exitButton.x = (game.width - _exitButton.width) / 3;
            _exitButton.y = (game.height - _exitButton.height) / 8;
            _startButton = new Button("StartLogo.png");
            _startButton.x = (game.width - _startButton.width) / 3;
            _startButton.y = (game.height - _startButton.height) /36;
            AddChild(_startButton);
            _musicDesert = new Sound("8BitDejaVu.mp3", true, true);           
            _music = new Sound("8BitGasGasGas.mp3", true, true);
            _musicChannel = _music.Play();
            StartDesertMusic();
            StartMenuMusic();
        }
        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //check what button is pressed to determine if what to do
                if (_exitButton.HitTestPoint(Input.mouseX, Input.mouseY)){
                    Environment.Exit(1);
                }
                if (_startButton.HitTestPoint(Input.mouseX, Input.mouseY))
                {
                    _musicChannel.Stop();
                    this.alpha = 0;
                    LoadLevel("DesertTrack.tmx");
                    HideMenu();
                    
                } 
            }
            //backspace returns to main menu
            if (Input.GetKey(Key.BACKSPACE))
            {
                DestroyLevel("DesertTrack.tmx");
                _exitButton = new Button("ExitLogo.png");
                AddChild(_exitButton);
                _exitButton.x = (game.width - _exitButton.width) / 3;
                _exitButton.y = (game.height - _exitButton.height) / 8;
                _startButton = new Button("StartLogo.png");
                _startButton.x = (game.width - _startButton.width) / 3;
                _startButton.y = (game.height - _startButton.height) / 36;
                AddChild(_startButton);
                this.alpha = 1;
                StartMenuMusic();
                _hasStarted = false;
            }
            
        }
        public void HideMenu()
        {
            _startButton.visible = false;
            _exitButton.visible = false;
        }
        public void LoadLevel(string name)
        {
            //loads the actual game
            if (_hasStarted == false)
            {
                StartDesertMusic();
                //Destroys old level:
                List<GameObject> children = GetChildren();
                for (int i = children.Count - 1; i >= 0; i--)
                {
                    children[i].Destroy();
                }
                AddChild(new Level(name));
                scale = 0.5f;
                _hasStarted = true;
            }
        }
        public void DestroyLevel(string name)
        {
                //Destroys old level:
                List<GameObject> children = GetChildren();
                for (int i = children.Count - 1; i >= 0; i--)
                {
                    children[i].Destroy();
                }
            scale = 2.4f;
        }
        public void StartMenuMusic()
        {
            _musicDesertChannel.Stop();
            _musicChannel = _music.Play();
            _musicChannel.Volume = 0.2f;
        }
        public void StartDesertMusic()
        {
            _musicChannel.Stop();
            _musicDesertChannel = _musicDesert.Play();
            _musicDesertChannel.Volume = 0.2f;
        }
    }
}
