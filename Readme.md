# cell
Cell is a game for AIs to play.

There is a certain genre of game called cell games. For Example:
* http://armorgames.com/play/2675/phage-wars
* http://store.steampowered.com/app/221180/

I wasn't very happy with the AI's presented in these games. This project seeks to emulate the basic nature of cell games, and provide a framework for running tournaments between different AI's.

## Game Objects
Board - Contains the game "pieces" (the Forts and the Guy Groups).
Player - An AI that's playing the game is assigned a player. That AI can then control that Player's Guys
Fort - Produces Guys. Owned by a Player.
GuyGroup - Used to move Guys from one Fort to another. Owned by a Player
Guy - The "soldiers" of the game. Owned by the Player that owns the Fort that created it. An AI can move Guys between Forts.

## Game Basics
AIs move Guys from Forts they control to other Forts via GuyGroups. The farther the destination Fort is from the originating Fort, the longer it takes for the GuyGroup to arrive at the destination Fort. If the destination Fort is owned by the same player that owns the GuyGroup then the Guys in the GuyGroup are added to the Guys currently in the Fort. If the destination Fort is owned by an opposing Player then the two groups of Guys will eliminate each other 1-to-1 until one group is depleted. If Guys remain in the GuyGroup, they move into the Fort and the Fort changes ownership to the GuyGroup's Player. If Guys remain in the Fort the Fort's Player retains ownership.