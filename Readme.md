# cell
Cell is a game for AIs to play.

There is a certain sub-genre of RTS games called 'cell games'. For Example:
* http://armorgames.com/play/2675/phage-wars
* http://store.steampowered.com/app/221180/

I wasn't very happy with the AI's presented in these games. This project seeks to emulate the basic nature of cell games, and provide a framework for running tournaments between different AI's.

## Game Objects
Board - Contains the game state (the Forts and the GuyGroups).
Player - An AI that's playing the game is assigned a player. That AI can then control that Player's Forts
Guy - The "soldiers" of the game. An AI can move Guys between Forts if it controls the Player that owns the Fort
Fort - Produces Guys every turn. Owned by a Player.
GuyGroup - Used to move Guys from one Fort to another. Owned by a Player

## Game Basics
AIs move Guys from Forts they control to other Forts via GuyGroups. The farther the destination Fort is from the originating Fort, the longer it takes for the GuyGroup to arrive at the destination Fort. If the destination Fort is owned by the same player that owns the GuyGroup then the Guys in the GuyGroup are added to the Guys currently in the Fort. If the destination Fort is owned by an opposing Player then the two groups of Guys will eliminate each other 1-to-1 until one group is depleted. If there are still Guys remaining in the attacking GuyGroup after the Fort is depleted, they move into the Fort and the Fort changes ownership to the GuyGroup's Player. Otherwise, the Fort's Player retains ownership.

## Adding your AI
Your AI must implement the IBot interface.
It should be added to the cellTournament/bots folder.
It should be added to the solution so it gets compiled.
Bots are found by looking for all classes that implement the IBot interface.
In order for an AI to play in a tournament it must be added to the tournament config. See Running a Tournament

## Running a Tournament
Running a tournament needs 2 things. A tournament config and a map catalog.
There is a sample tournament config in the tournaments folder. 
Its file location must be passed in with the -t flag.
It specifies which bots are playing in the tournament and which maps they'll play on in json.
There is a sample map catalog in the maps folder.
Its file location must be passed in with the -c flag.
It contains descriptions of maps in json.