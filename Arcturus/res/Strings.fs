namespace Arcturus.Res

module Strings =
    let helpString = "[m/move] then [n/e/s/w north/east/south/west] to choose which direction to move.
        \n[c/check] To have a look at your surroundings.
        \n[g/grab] To grab an item from the location
        \n[i/inv] To have a look at what items are in your inventory.
        \n[s/stats] To check your stats
        \n[h/help] To re-read this tutorial
        \n[q/quit] To quit the game.
        \nYou are also encouraged to draw a grid in order to figure out the size of each floor and remember where you have or haven't been
        \n"

    let openingString =
        "Welcome to the SpaceShip Arcturus!
        \nThe year is 2455 and you're part of the second round of colonists on their way to the star Proxima Centauri.
        \nIt has been 16 years since you were put into cryo-sleep with another 42 to go.
        \n
        \nBut you have awoken...
        \n
        \nStill hazy, you gently raise yourself expecting to be greeted by the welcoming crew like the ads said.
        \nBut there's no one.
        \nIt's dark.
        \nYou can barely see the rest of the cryo-pods.
        \n
        \n"
        + helpString

    let movePrompt = "-Move where > "
    let setNamePrompt = "-Enter the name of your character > "

    let grabItemPrompt =
        "-Which item do you wish to grab? (Enter the number) > "
    
    let itemAddedToInvString = " has been added to your inventory"
    
    let foundItemString = "You found a "
    
    let itemsLocationPrompt = "The items here are: "
    let noItemsLocationString = "There are no items at this location"
    let noItemsInvString = "You don't have any items"

    let errorCannotParseInvalidCommandString =
        "ERROR: Cannot parse input, invalid command"

    let errorCannotMoveString =
        "ERROR: No more rooms to move to that way"

    let errorCannotMatchCompassString =
        "ERROR: Cannot match a compass direction - N/E/S/W"
        
    let errorCannotMatchEventChoiceString =
        "That was not a valid event choice"
