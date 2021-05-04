namespace Arcturus.Types

open Arcturus.Types.Level
open Arcturus.Types.Items
open Arcturus.Types.Player
open FSharpPlus.Lens

module GameState =
    type state =
        { player: player
          gameWorld: level }

    //Prism for State type
    let inline _player f state =
        f state.player
        <&> fun player -> { state with player = player }

    let inline _gameWorld f state =
        f state.gameWorld
        <&> fun gameWorld -> { state with gameWorld = gameWorld }

    let getInitialPlayer =
        { name = ""
          inventory = [ keycard ]
          stats = {
              Strength = StatValue 1uy
              Perception = StatValue 1uy
              Endurance = StatValue 1uy
              Charisma = StatValue 1uy
              Intelligence = StatValue 1uy
              Agility = StatValue 1uy
              Luck = StatValue 1uy
              }
          }
        
    let getInitialState =
        { player = getInitialPlayer
          gameWorld = floor_4 }
       