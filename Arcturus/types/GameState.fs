namespace Arcturus.Types

open GameWorld
open Items
open Levels
open Player
open FSharpPlus.Lens

module GameState =
    type State =
        { player: Player
          gameWorld: GameWorld }

    //Prism for State type
    let inline _player f state =
        f state.player
        <&> fun player -> { state with player = player }

    let inline _gameWorld f state =
        f state.gameWorld
        <&> fun gameWorld -> { state with gameWorld = gameWorld }

    let getInitialPlayer (level: Level) =
        { location =
              { x = level.startLocation.x
                y = level.startLocation.y }
          playerItems = [ keycard ] }

    let getInitialGameWorld (level: Level) =
        { size = level.size
          levelName = level.levelName
          floorNumber = level.floorNum
          startLocation = level.startLocation
          levelItems = level.levelItems }

    let getInitialState =
        { player = getInitialPlayer (floor_4)
          gameWorld = getInitialGameWorld (floor_4) }

    let getItemsAtLocation (location: Coordinates) (level: Level) =
        level.levelItems
        |> List.filter (fun i -> i.location = location)
        |> List.map (fun il -> il.item)
