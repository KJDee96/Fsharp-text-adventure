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

    let getInitialPlayer  =
        { name = ""
          inventory = [ keycard ]
          stats = {
              Strength = (PlayerStat.create 1uy).Value
              Perception = (PlayerStat.create 1uy).Value
              Endurance = (PlayerStat.create 1uy).Value
              Charisma = (PlayerStat.create 1uy).Value
              Intelligence = (PlayerStat.create 1uy).Value
              Agility = (PlayerStat.create 1uy).Value
              Luck = (PlayerStat.create 1uy).Value}}
        
    let getInitialState=
        { player = getInitialPlayer
          gameWorld = floor_4 }
       