using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Circuit.Utilities
{
    static class Data
    {
        // Player
        public const float P_PICKUP_RANGE = 5;                 // Range in which the player can pick up fragments
        public const float P_LIFEPOINT_BASE = 100;
        public const float P_BASESPEED = 16;
        public const float P_MODELSCALE = .7f;
        public const float P_BASEJUMP = 20f;
        public const float P_HEIGHT = 6.0f;
        public const float P_RADIUS = 1.2f;
        public const float P_MASS = 10;

        //Player Glow Constants
        public static Vector3 PG_STARTLIFE = new Vector3(0, 0f, 1f);
        public static Vector3 PG_MIDDLELIFE = new Vector3(0f, 1f, 0);
        public static Vector3 PG_ENDLIFE = new Vector3(1f, 0, 0);

        public static Vector3 PG_DEFAULT_BOOST = new Vector3(0.25f, 0.25f, 0.25f);
        public static Vector3 PG_DELETE = new Vector3(0.5f, 0, 0);
        public static Vector3 PG_FIREWALL = new Vector3(0, 0f, 0.5f);
        public static Vector3 PG_OPTIMIZE = new Vector3(0.25f, 0.25f, 0.25f);
        public static Vector3 PG_GOTO = new Vector3(0, 0.5f, 0);

        //PlayerScreens
        public const String PS_DEATHNAME = "DeathScreen";
        public const String PS_GAMEENDNAME = "ResultScreen";
        public const String PS_SCORENAME = "Score";

        // FragmentCloud
        public const float FC_BASE_RADIUS = .3f;                // Average fly-radius for a fragment in the cloud
        public const float FC_BASE_YSPAN = .1f;                 // Average Y-Wobble for a fragment in the cloud
        public const float FC_BASE_YSPEED = .2f;                // Average Y-Wobbling-Speed for a fragment in the cloud
        public const float FC_BASE_FRAGSCALE = .009f;           // Average scale for a fragment in the cloud
        public const float FC_BASE_ROTATIONSPEED = .005f;       // Average rotation-speed on its circle for a fragment in the cloud
        public const float FC_BASE_ORIENTATIONSPEED = .005f;    // Average In-Itself-Rotation-Speed for a fragment in the cloud
        public const float FC_BASE_MINMANIPULATOR = .9f;        // Dunno lol

        public const float FC_AWAYSPEED = .5f;                 // The speed with which fragments leave the cloud
        public const float FC_AWAYRANGE = .01f;                 // Length of the away vector (which determines the direction fragments fly leaving the cloud)
        public const int FC_AWAYPERCENTAGE = 150;               // The percentage of the away vector after which a fragment leaving the cloud despawns
        public const float FC_TOCLOUDSPEED = .17f;



        // Arsenal                
        public const float A_MAXSIZE = 5.0f;
        public const int A_NUMBEROFWEAPONS = 4;                // Number of programm-kinds currently implemented
        public const int A_CAPACITY = 20;                      // Max ammo the player can carry of one kind
        public const float A_START_DELETE = 20f;
        public const float A_START_FIREWALL = 20f;
        public const float A_START_OPTIMIZE = 20f;
        public const float A_START_GOTO = 20f;
        public enum RunType { TRAP, SHOT, BOOST, PLAYER }
        public enum ProgrammType { DEFAULT, DELETE, OPTIMIZE, FIREWALL, GOTO, OPTIMIZE_N }
        public const int A_SHOTDELETIONTIME_ONCOL = 2000;            // Milliseconds after which a shot is deleted after a collision
        public const int A_SHOTDELETIONTIME = 7000;

        // Delete
        public const string DEL_MODELPATH = "DeleteBaseModel";  // Filename for the basic Delete model
        public const string DEL_PATH = "Programms/Delete";
        public const float DEL_FRAGSCALE = .01f;                // Scale for Delete fragments
        public const float DEL_CLOUDSCALE = .009f;              // Scale for Delete fragments in the cloud
        public const float DEL_GROWTHFACTOR = 1.004f;
        public const bool DEL_GRAVITYAFFECTED = false;

            // DeleteShot
            public const float DEL_SHOTMODELSCALE = .02f;           // Scale for new Delete shots 
            public const float DEL_SHOTENTITYSCALE = .5f;           // Scale for new Delete shots 
            public const float DEL_SHOTBASEMASS = 1;
            public const float DEL_SHOTSPEED = 120;
            public const float DEL_SHOTCOST = .3f;
            public const float DEL_SHOTGROWTH = .05f;

            // DeleteTrap
            public const float DEL_TRAPMODELSCALE = .02f;           // Scale for new Delete traps 
            public const float DEL_TRAPENTITYSCALE = 1.1f;           // Scale for new Delete traps 
            public const float DEL_TRIGGEREDTRAPSPEED = 10;
            public const float DEL_MAXCHECKRANGE = 20;
            public const float DEL_TRAPCOST = 1.0f;
            public const float DEL_TRAPDAMAGE = 20;

            // DeleteBoost
            public const int DEL_BOOSTPOWER = 3000;              // Duration for the Boost in milliseconds
            public const float DEL_BOOSTCOST = 1f;
            public const float DEL_BOOSTDAMAGE = 5;


        // Optimize
        public const string OPT_MODELPATH = "OptimizeBaseModel";
        public const string OPT_PATH = "Programms/Optimize";
        public const float OPT_FRAGSCALE = .01f;
        public const float OPT_CLOUDSCALE = .009f;
        public const float OPT_GROWTHFACTOR = 1.008f;
        public const bool OPT_GRAVITYAFFECTED = false;

            // OptimizeShot
            public const float OPT_SHOTMODELSCALE = .02f;
            public const float OPT_SHOTENTITYSCALE = .5f;
            public const float OPT_SHOTBASEMASS = 1;
            public const float OPT_SHOTSPEED = 120;
            public const float OPT_SHOTCOST = .6f;
            public const float OPT_SHOTGROWTH = .1f;

            // OptimizeTrap
            public const float OPT_TRAPMODELSCALE = .02f;
            public const float OPT_TRAPENTITYSCALE = 1.7f;
            public const float OPT_TRAPCOST = 1.0f;
            public const float OPT_TRAPDAMAGE = 4f;         //Factor to divide speed by

            // OptimizeBoost
            public const int OPT_BOOSTPOWER = 10000;
            public const int OPT_NEGBOOSTPOWER = 3000;      //Negative Boosts are used as a shotattribute
            public const float OPT_BOOSTCOST = 5f;
            public const float OPT_BOOSTFACTOR = 2f;       //Factor to multiply player speed with
            public const float OPT_NEGBOOSTFACTOR = 0.5f;  //Factor to reduce speed, both boosts cancel each other out


        // Firewall
        public const string FIRE_MODELPATH = "FirewallBaseModel";
        public const string FIRE_PATH = "Programms/Firewall";
        public const float FIRE_FRAGSCALE = .01f;
        public const float FIRE_CLOUDSCALE = .009f;
        public const float FIRE_GROWTHFACTOR = 1.008f;
        public const bool FIRE_GRAVITYAFFECTED = false;
        public const float FIRE_RATIOKEY = .067f;                // EntityScale * RatioKey = ModelScale

            // FirewallShot
            public const float FIRE_SHOTMODELSCALE = .067f;
            public const float FIRE_SHOTENTITYSCALE = 1.0f;
            public const float FIRE_SHOTBASEMASS = 1;
            public const float FIRE_SHOTSPEED = 120;
            public const float FIRE_SHOTCOST = 4.0f;

            // FirewallTrap
            public const float FIRE_TRAPMODELSCALE = .20f;          // Reference ratio: .067 to 1
            public const float FIRE_TRAPENTITYSCALE = 3.0f;
            public const float FIRE_TRAPCOST = 3.0f;
            public const float FIRE_TRAPDAMAGE = 4f;         //Factor to divide speed by
            public const float FIRE_TRAPMASS = 100;

            // FirewallBoost
            public const int FIRE_BOOSTPOWER = 2000;
            public const float FIRE_BOOSTCOST = 5f;
            public const float FIRE_BOOSTFACTOR = 2f;       //Factor to multiply player speed with
            public const float FIRE_NEGBOOSTFACTOR = 0.5f;  //Factor to reduce speed, both boosts cancel each other out


        // GoTo
        public const string GOTO_MODELPATH = "GoToBaseModel";
        public const string GOTO_PATH = "Programms/GoTo";
        public const float GOTO_FRAGSCALE = .01f;
        public const float GOTO_CLOUDSCALE = .009f;
        public const float GOTO_GROWTHFACTOR = 1.008f;
        public const bool GOTO_GRAVITYAFFECTED = false;

            // GoToShot
            public const float GOTO_SHOTMODELSCALE = .012f;
            public const float GOTO_SHOTENTITYSCALE = .5f;
            public const float GOTO_SHOTBASEMASS = .01f;
            public const float GOTO_SHOTSPEED = 80;
            public const float GOTO_SHOTCOST = .18f;
            public const int GOTO_SHOTCOOLDOWN = 130;
            public const float GOTO_SHOTHOMINGMINANGLE = .62f;     // .97

            // GoToTrap
            public const float GOTO_TRAPMODELSCALE = .02f;
            public const float GOTO_TRAPENTITYSCALE = 1.1f;
            public const float GOTO_TRAPCOST = 1.0f;
            public const float GOTO_TRAPDAMAGE = .1f;         //Factor to divide speed by
            public const float GOTO_TRAPBASEMASS = 1.0f;
            public const float GOTO_SPLINTERENTITYSCALE = .008f;

            // GoToBoost
            public const int GOTO_BOOSTPOWER = 10000;
            public const float GOTO_BOOSTCOST = 5f;
            public const float GOTO_BOOSTFACTOR = 2f;       //Factor to multiply player speed with


        // Destructibles

        // Box
        public const int D_BOXWIDTH = 5;                    // Width for a destructible in fragments
        public const int D_BOXHEIGHT = 5;                   // Vice versa
        public const int D_BOXDEPTH = 5;                    // ..
        public const float D_BOXPARTSIZE = .3f;              // Space the destructible reserves for a single fragment (can be used to regulate the spacing of the fragments)


        // Ball
        public const int D_BALLRADIUS = 10;


        // Cylinder
        public const float D_CYLINDERRADIUS = 1;
        public const int D_CYLINDERHEIGHT = 5;


        // Option Settings
        //Resolution
        public static int R_VIEWPORTHEIGHT = 768;              // Screen height
        public static int R_VIEWPORTWIDTH = 1024;              // Screen width
        public static bool R_FULLSCREEN = false;               // Is the game running in fullscreen?

        //Camera Control
        public static float O_CAMERA_SENSITIVITY1 = 1f;         // The speed with which the camera moves
        public static bool O_INVERT_X1 = false;                 // Inverts the X-axis of the camera
        public static bool O_INVERT_Y1 = false;                 // Inverts the Y-axis of the camera

        public static float O_CAMERA_SENSITIVITY2 = 1f;         // The speed with which the camera moves
        public static bool O_INVERT_X2 = false;                 // Inverts the X-axis of the camera
        public static bool O_INVERT_Y2 = false;                 // Inverts the Y-axis of the camera

        //Sound
        public static float S_MASTERVOLUME = 1f;

        //Game Rules
        public static int POINTS_TO_WIN = 5;
        public static bool USE_TIMELIMIT = false;
        public static int TIMELIMIT = 5;     // Timelimit in Minutes   

        //Submenus
        public static string[] SM_FULLSCREEN = { "Yes", "No" };
        //public static string[] SM_RESOLUTION = {"1024x768", "1152x864", "1280x960", "1600x1200"};

        public static string[] SM_SOUND = { "0", "0.1", "0.2", "0.3", "0.4", "0.5", "0.6", "0.7", "0.8", "0.9", "1" };

        public static string[] SM_USETIMELIMIT = { "Yes", "No" };
        public static string[] SM_TIMELIMIT = { "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };
        public static string[] SM_POINTLIMIT = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };

        public static string[] SM_SENSITIVITY = { "0.5", "0.6", "0.7", "0.8", "0.9", "1", "1.1", "1.2", "1.3", "1.4", "1.5" };
        public static string[] SM_INVERTX = { "Yes", "No" };
        public static string[] SM_INVERTY = { "Yes", "No" };

        //Sounds
        public const float S_MENUHIGHLIGHTVOL = 1f; // Menu Highlight
        public const float S_MENUACCEPT = 1f;       // Menu Accept

        public const int S_NUMBER_OF_SOUNDS_IN_SOUND_ARRAY = 7;
        public const float S_LOADVOL = 0.5f;        // Aufladen .5
        public const float S_SHOOTVOL = 1f;         // Schießen 1
        public const float S_JUMPVOL = 1f;          // Springen 1
        public const float S_DIEVOL = 0.5f;         // Sterben .5
        public const float S_RESPAWNVOL = 1f;       // Respawn 1
        public const float S_HIT = 1f;              // Hit 1
        public const float S_BOOST = 1f;            // Boost 1 


        // Glow-Blur
        public const int GLOW_BLUR_AMOUNT = 2;

        // Other
        public const int TRAPCHECKTIME = 10; // Time in milliseconds after which a Trap raycasts for a Player again

    }
}
