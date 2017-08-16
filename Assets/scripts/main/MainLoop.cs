using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoop   {
    public IEnumerator GameMainLoop()
    {
        while (GameManager.GameRunning)
        {
            // Can't find a better place.
            // SaveGameLoading = 0;
            //
            // Game logic part
            //
            if (!GameManager.GamePaused && NetManager.NetworkInSync && GameManager.SkipGameCycle == 0)
            {
              //  SinglePlayerReplayEachCycle();
                ++GameManager.GameCycle;
               // MultiPlayerReplayEachCycle();
               // NetworkCommands(); // Get network commands
                Main.actionManager.UnitActions();      // handle units
              //  MissileActions();   // handle missiles
              //  PlayersEachCycle(); // handle players
              //  UpdateTimer();      // update game timer

                //
                // Work todo each second.
                // Split into different frames, to reduce cpu time.
                // Increment mana of magic units.
                // Update mini-map.
                // Update map fog of war.
                // Call AI.
                // Check game goals.
                // Check rescue of units.
                //
                switch (GameManager.GameCycle % Consts.CYCLES_PER_SECOND)
                {
                    case 0: // At cycle 0, start all ai players...
                     /*   if (GameCycle == 0)
                        {
                            for (player = 0; player < NumPlayers; ++player)
                            {
                                PlayersEachSecond(player);
                            }
                        }*/
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3: // minimap update
                       // UI.Minimap.Update();
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6: // overtaking units
                       // RescueUnits();
                        break;
                   // default:
                       /* // FIXME: assume that NumPlayers < (CYCLES_PER_SECOND - 7)
                        player = (GameCycle % CYCLES_PER_SECOND) - 7;
                        Assert(player >= 0);
                        if (player < NumPlayers)
                        {
                            PlayersEachSecond(player);
                        }*/
                }
            }

            TriggersEachCycle();  // handle triggers
            UpdateMessages();     // update messages

            CheckMusicFinished(); // Check for next song

            //
            // Map scrolling
            //
            DoScrollArea(MouseScrollState | KeyScrollState, (KeyModifiers & ModifierControl) != 0);

            if (FastForwardCycle > GameCycle &&
                    RealVideoSyncSpeed != VideoSyncSpeed)
            {
                RealVideoSyncSpeed = VideoSyncSpeed;
                VideoSyncSpeed = 3000;
            }
            if (FastForwardCycle <= GameCycle || GameCycle <= 10 || !(GameCycle & 0x3f))
            {
                //FIXME: this might be better placed somewhere at front of the
                // program, as we now still have a game on the background and
                // need to go through the game-menu or supply a map file
                UpdateDisplay();

                //
                // If double-buffered mode, we will display the contains of
                // VideoMemory. If direct mode this does nothing. In X11 it does
                // XFlush
                //
                RealizeVideoMemory();
            }

            if (FastForwardCycle == GameCycle)
            {
                VideoSyncSpeed = RealVideoSyncSpeed;
            }
            if (FastForwardCycle <= GameCycle || !(GameCycle & 0x3f))
            {
                WaitEventsOneFrame();
            }
            if (!NetworkInSync)
            {
                NetworkRecover(); // recover network
            }
        }
    }
}
