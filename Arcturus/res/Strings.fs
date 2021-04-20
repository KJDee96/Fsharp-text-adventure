namespace Arcturus.Res

module Strings =
    let help = "      Tutorial - Type M or Move to choose which direction to move.
        \n      Tutorial - Then type a compass direction to finalise your choice.
        \n
        \n      Tutorial - Type C or Check to have a look at your surroundings.
        \n      Tutorial - Type I or Inv to have a look at what items are in your inventory.
        \n      Tutorial - Type G or Grab to grab an item from the location
        \n      Tutorial - Type H or Help to re-read this tutorial
        \n      Tutorial - Type Q or Quit to quit the game.
        \n      You are also encouraged to draw a grid in order to figure out the size of each floor
        \n          and remember where you have or haven't been
        \n"

    let opening =
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
        + help
    
    let movePrompt = "-Move where > "
    let setNamePrompt = "-Enter the name of your character > "
    let grabItemPrompt = "-Which item do you wish to grab? (Enter the number) > "
    let itemsLocationPrompt= "The items here are: "
    let noItemsLocationString = "There are no items at this location"
    let noItemsInvString = "You don't have any items"

    let errorInputString = "ERROR: Cannot parse input, invalid command"
    let errorNoRoomsString = "ERROR: No more rooms to move to that way"
    let errorDirectionString = "ERROR: Cannot match a compass direction - N/E/S/W"