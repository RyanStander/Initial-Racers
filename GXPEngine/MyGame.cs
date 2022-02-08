using System;									// System contains a lot of default C# libraries 
using System.Drawing;                           // System.Drawing contains a library used for canvas drawing below
using GXPEngine;								// GXPEngine contains the engine
using System.Collections.Generic;

public class MyGame : Game
{

    //public MyGame() : base(960,720, false)     // Create a window that's 1920x1080 and NOT fullscreen
    public MyGame() : base(1920, 1080, false)
    {
        Menu menu = new Menu();
        AddChild(menu);
        

    }
    

    void Update()
    {

    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new MyGame().Start();                   // Create a "MyGame" and start it
    }
}