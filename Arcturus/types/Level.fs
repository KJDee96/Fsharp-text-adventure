namespace Arcturus.Types
open Arcturus.Types.Items
open FSharpPlus.Lens

module Level =
    type Coordinates = { x: int; y: int }

    type ItemLocation = { item: Item; location: Coordinates }

    type Level = {
        size: Coordinates;
        levelName: string;
        floorNumber: int;
        startLocation: Coordinates;
        levelItems: ItemLocation list
    }
    
    let floor_4 =
        { size = { x = 2; y = 4 }
          levelName = "Cryo and Power"
          floorNumber = 4
          startLocation = { x = 1; y = 3 }
          levelItems =
              [ { item = wrench
                  location = { x = 1; y = 3 } } ] }

    let floor_3 =
        { size = { x = 0; y = 3 }
          levelName = "Canteen and Storage"
          floorNumber = 3
          startLocation = { x = 0; y = 1 }
          levelItems =
              [ { item = hammer
                  location = { x = 0; y = 2 } } ] }

    let removeItemFromWorld world item = List.except (List.toSeq [ item ]) world.levelItems
    
    //Prism for gameWorld type
    let inline _size  f gameWorld = f gameWorld.size <&> fun size -> { gameWorld with size = size }
    let inline _levelName  f gameWorld = f gameWorld.levelName <&> fun levelName -> { gameWorld with levelName = levelName }
    let inline _floorNumber  f gameWorld = f gameWorld.floorNumber <&> fun floorNumber -> { gameWorld with floorNumber = floorNumber }
    let inline _startLocation  f gameWorld = f gameWorld.startLocation <&> fun startLocation -> { gameWorld with startLocation = startLocation }
    let inline _levelItems  f gameWorld = f gameWorld.levelItems <&> fun levelItems -> { gameWorld with levelItems = levelItems }
