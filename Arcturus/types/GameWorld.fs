namespace Arcturus.Types
open Arcturus.Types.Levels
open FSharpPlus.Lens

module GameWorld =
    type GameWorld = {
        size: Coordinates;
        levelName: string;
        floorNumber: int;
        startLocation: Coordinates;
        levelItems: ItemLocation list
    }

    //Prism for gameWorld type
    let inline _size  f gameWorld = f gameWorld.size <&> fun size -> { gameWorld with size = size }
    let inline _levelName  f gameWorld = f gameWorld.levelName <&> fun levelName -> { gameWorld with levelName = levelName }
    let inline _floorNumber  f gameWorld = f gameWorld.floorNumber <&> fun floorNumber -> { gameWorld with floorNumber = floorNumber }
    let inline _startLocation  f gameWorld = f gameWorld.startLocation <&> fun startLocation -> { gameWorld with startLocation = startLocation }
    let inline _levelItems  f gameWorld = f gameWorld.levelItems <&> fun levelItems -> { gameWorld with levelItems = levelItems }
