# Keyboard for a Cosmic Cure
Welcome to the README for *Keyboard for a Cosmic Cure*, a motion-control game developed for the Microsoft Kinect for burn patients at the Jaycee Burn Center.  The master branch houses the code for the Kinect v1.0 code, but we are working on developing it for Kinect v2.0 as well for better tracking abilities.

## Concept
*Keyboard for a Cosmic Cure* is a Simon Says piano game intended to get patients to focus on something fun while undergoing painful therapy.  Once a player selects a song (from easy, medium, or hard), one phrase of the song is played as the keys are lit up in the correct order.  The player must then play the notes back in the correct order to move on to the next level.  Once all phrases have been completed, the song is played back in real time, and the player’s score is determined based on how many correct notes they played.

## Builds
Builds can be found in the Builds folder - one for 32-bit and one for 64-bit.  Download and run the 2D.exe file to play the game.

## Assets

### Scenes

#### MainMenu
The main menu you open the game on.

#### OptionMenu
Options menu that allows you to choose between note speed and color of correct/incorrect notes for colorblind people.

#### PickSong
Pick your song from a selection of 3.  "London Bridge" is the easiest, "Happy Birthday" is moderate, "Take Me Out to the Ballgame" is difficult.

#### PickTutorial
Pick your tutorial from the 3 songs listed above.

#### Game
The main keyboard game where everything happens.

#### FreePlay
Users can play whatever they want on the piano with no scores or game mechanics.

### Important Scripts

#### KinectManager
Attached to every main camera.  This is the main script that tracks Kinect movement and maps out joints.  Mostly public domain or the previous team's code.  Found under the KinectScripts folder.

#### Key and Song
These are classes for determining how to play a song.  The Key class wraps around a GameObject and can be used to make up a song.  The Song class has a predetermined list of songs stored in an enum and, upon initialization, sets the correct arrays to the correct values.  Found under the Scripts folder.

#### ChangeScene
The script attached to the main camera for MainMenu.  Switches between the scenes for all of the different menus there or exits out of the game.  Found under the Scripts folder.

#### ChooseSong
The script attached to the main camera of the PickSong scene.  Selects the correct Song object to play the game and switches to the Game scene.  Found under the Scripts folder.

#### ChooseTutorial
The same as ChooseSong, but for tutorials instead.  Found under the Scripts folder.

#### FreePlay
The script attached to the main camera of the FreePlay scene.  Logic behind pressing keys and making sounds work.  Found under the Scripts folder.

#### GamePlay
The script attached to the main camera of the Game scene.  The entirety of the game logic lies here, although mostly in the PlayBack, PlayForTime, LightUpKey, and Update methods.  Found under the Scripts folder.

#### Options
The script attached to the main camera of the OptionMenu scene.  The logic behind storing the player preferences for the game.  Found under the Scripts folder.

### Sounds
The folder that houses all of the sound files for the keyboard.

### Images
The folder that houses all of the images for the game, from the background to the key sprites to the button sprites.  Button sprites were made at [DaButtonFactory.com](https://dabuttonfactory.com/), so any future additions should also be made through that website to keep the style intact.

## Kinect v2.0 Support
Our current progress on Kinect v2.0 is available on the kinectv2 branch.  It may need some more development in the future, but it generally tracks better than Kinect v1.0 and is worth moving to.
