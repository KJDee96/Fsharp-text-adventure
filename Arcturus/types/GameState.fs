namespace Arcturus.Types

open Arcturus.Types.Level
open Arcturus.Types.Items
open Arcturus.Types.Player
open FSharpPlus.Lens

module GameState =
    type State =
        { player: Player
          gameWorld: Level }

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


    let getInitialState =
        { player = getInitialPlayer floor_4
          gameWorld = floor_4 }
       