namespace Arcturus.Types

open System
open Arcturus.Types.Level
open Arcturus.Types.Items
open Arcturus.Types.Player
open Arcturus.Types.Event
open FSharpPlus.Lens

module GameState =
    type state =
        { player: player
          gameWorld: level
          inEvent: Boolean }

    //Prism for State type
    let inline _player f state =
        f state.player
        <&> fun player -> { state with player = player }

    let inline _gameWorld f state =
        f state.gameWorld
        <&> fun gameWorld -> { state with gameWorld = gameWorld }

    let inline _inEvent f state =
        f state.inEvent
        <&> fun inEvent -> { state with inEvent = inEvent }

    let getInitialPlayer =
        { name = ""
          inventory = [ keycard ]
          stats =
              { Strength = StatValue 1uy
                Perception = StatValue 1uy
                Endurance = StatValue 1uy
                Charisma = StatValue 1uy
                Intelligence = StatValue 1uy
                Agility = StatValue 1uy
                Luck = StatValue 1uy } }

    let getInitialState =
        { player = getInitialPlayer
          gameWorld = floor_4
          inEvent = false }

    let setInEvent (state: state) =
        over _inEvent (fun _ -> true) state

    let setOutEvent (state: state) =
        over _inEvent (fun _ -> false) state
        
    let updatePlayerName data (state: state)  =
        over (_player << _name) (fun _ -> data) state
        
    let updatePlayerItems data (state: state) =
        over (_player << _playerItems) (fun _ -> data) state

    let updateItemsInGameWorld data (state: state) =
        over (_gameWorld << _levelItems) (fun _ -> data) state
    
    let updateGameStateItems playerItems worldItems =
        updatePlayerItems playerItems >> updateItemsInGameWorld worldItems
        
    let updateEventInGameState data (state: state) =
        over (_gameWorld << _event) (fun _ -> data) state
        