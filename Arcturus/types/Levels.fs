namespace Arcturus.Types

open Items

module Levels =

    type Coordinates = { x: int; y: int }

    type ItemLocation = { item: Item; location: Coordinates }

    type Level =
        { size: Coordinates
          levelName: string
          floorNum: int
          startLocation: Coordinates
          levelItems: ItemLocation list }

    let floor_4 =
        { size = { x = 2; y = 4 }
          levelName = "Cryo and Power"
          floorNum = 4
          startLocation = { x = 1; y = 3 }
          levelItems =
              [ { item = wrench
                  location = { x = 1; y = 3 } } ] }

    let floor_3 =
        { size = { x = 0; y = 3 }
          levelName = "Canteen and Storage"
          floorNum = 3
          startLocation = { x = 0; y = 1 }
          levelItems =
              [ { item = hammer
                  location = { x = 0; y = 2 } } ] }
