﻿namespace Arcturus.Core

open Arcturus.Res.Strings
open Arcturus.Types.Player
open Arcturus.Types.GameState
open Arcturus.Types.Level
open Arcturus.Types.Event
open Arcturus.Utils.Errors
open Arcturus.Utils.Printing
open Arcturus.Utils.PatternMatching
open FSharpPlus
open FSharpPlus.Lens
open System

module Commands =

    type Command =
        | Move of int * int
        | Check
        | Grab
        | Inventory
        | Stats
        | Help
        | EventPathChoice of int
        | Quit

    let setName state =
        printf "%s" setNamePrompt
        let name = Console.ReadLine()
        updatePlayerName name state // update player name

    let testEventCycle (state: state) =
        let event = state.gameWorld.event.Value
        let path = event.getPath

        printEventTitle event
        printPathText path
        printPathOptions path

        state

    let checkForUnfinishedEvent (state: state) =
        match state.gameWorld.event with
        | None -> Choice1Of2 state
        | _ ->
            match state.gameWorld.event.Value.checkFinish with
            | true ->
                Choice1Of2 state
            | false ->
                testEventCycle (setGameStateInEvent state)
                |> Choice1Of2


    //returning state for player movement
//    let moveDir (dir: coordinates) (state: state) =
//        let newLocation =
//            { state.player.location with
//                  x = state.player.location.x + dir.x
//                  y = state.player.location.y + dir.y }
//
//        if newLocation.x > state.gameWorld.size.x
//           || newLocation.y > state.gameWorld.size.y then
//            Choice2Of2 CannotMove
//        else if newLocation.x = -1 || newLocation.y = -1 then
//            Choice2Of2 CannotMove
//        else
//            let returnState : State =
//                over (_player << _location) (fun _ -> newLocation) state // update player location and then the player gamestate
//
//            Choice1Of2 returnState

    let grabItem (state: state) =
        printf "%s" grabItemPrompt

        let parsed, index =
            Int32.TryParse(Console.ReadLine().Trim().ToLower())

        if parsed then
            let itemInWorld =
                List.tryItem (index - 1) state.gameWorld.levelItems

            if itemInWorld <> None then
                let newPlayerInventory =
                    addItemToInv state.player itemInWorld.Value.item

                let newGameWorldItemList =
                    removeItemFromWorld state.gameWorld itemInWorld.Value
                                    
                Choice1Of2 (updateGameStateItems newPlayerInventory newGameWorldItemList state)
            else
                Choice2Of2 CannotParseInvalidCommand
        else
            Choice2Of2 CannotParseInvalidCommand
            
    let matchStat stat value state= 
        match stat with
        |Strength ->
            increaseStrengthStat value state
        |Perception ->
            increasePerceptionStat value state
        |Endurance ->
            increaseEnduranceStat value state
        |Charisma ->
            increaseCharismaStat value state
        |Intelligence ->
            increaseIntelligenceStat value state
        |Agility ->
            increaseAgilityStat value state
        |Luck ->
            increaseLuckStat value state
            
    let performEventResult (state: state) =
        match state.gameWorld.event.Value.getPath.result.Value with
        | Item item ->
            let newPlayerInventory = addItemToInv state.player item
            updatePlayerItems newPlayerInventory state
        | StatIncrease playerStat ->
            let stat,value = playerStat
            let newState = matchStat stat value state
            newState

    //parsing the matched direction command
    let parseDirectionInput input =
        match input with
        | NorthMatch ->
            let d = (0, -1)
            Choice1Of2(Move d) //MoveN
        | EastMatch ->
            let d = (1, 0)
            Choice1Of2(Move d)
        | WestMatch ->
            let d = (-1, 0)
            Choice1Of2(Move d)
        | SouthMatch ->
            let d = (0, 1)
            Choice1Of2(Move d)
        | _ -> Choice2Of2 CannotMatchCompass

    //parsing the matched command
    let parseInput (input: string) =
        match input.Trim() with
        | MoveMatch ->
            printf "%s" movePrompt

            let input = Console.ReadLine().Trim().ToLower()

            parseDirectionInput input //pass result of input to parse direction
        | CheckMatch -> Choice1Of2 Check
        | GrabMatch -> Choice1Of2 Grab
        | InvMatch -> Choice1Of2 Inventory
        | StatsMatch -> Choice1Of2 Stats
        | HelpMatch -> Choice1Of2 Help
        | DigitMatch ->
            let _parsed, input = Int32.TryParse input
            Choice1Of2(EventPathChoice input)
        | QuitMatch -> Choice1Of2 Quit
        | _ -> Choice2Of2 CannotParseInvalidCommand

    let checkRequirementMet (state: state) (requirement: responseRequirement) =
        match requirement with
        | ItemInInvCheck item -> state.player.hasItem item
        | StatCheck (statType, playerStat) -> state.player.hasStat statType playerStat
        

    //execute command
    let executeCommand state command =
        if not state.inEvent then
            match command with
            | Move (x, y) ->
                //            match moveDir { x = x; y = y } state with
                //            | Choice1Of2 state ->
                //                printNewLocation state
                //                Choice1Of2 state
                //            | Choice2Of2 error -> Choice2Of2 error
                match checkForUnfinishedEvent state with
                | Choice1Of2 state -> Choice1Of2 state
                | Choice2Of2 error -> Choice2Of2 error
            | Check ->
                //            printLocationData state
                //            printGameWorldItems state
                //            printExits state
                Choice1Of2 state
            | Grab ->
                printGameWorldItems state //prints items at location with index + 1

                if not state.gameWorld.levelItems.IsEmpty then
                    match grabItem state with
                    | Choice1Of2 state ->
                        printInv state
                        Choice1Of2 state
                    | Choice2Of2 error -> Choice2Of2 error
                else
                    Choice1Of2 state
            | Inventory ->
                printInv state
                Choice1Of2 state
            | Stats ->
                printStats state
                Choice1Of2 state
            | Help ->
                printfn "%s" helpString //prints help
                Choice1Of2 state
            | Quit ->
                Environment.Exit(0) //exits
                Choice1Of2 state
            | _ -> Choice2Of2 CannotParseInvalidCommand
        else
            match command with
            | EventPathChoice input ->
                let response =
                    state.gameWorld.event.Value.getPath.options.Value.Item(input - 1)

                match response with
                | response when response = doNothingResponse -> setGameStateOutEvent state |> Choice1Of2
                | _ ->
                    match checkRequirementMet state response.requirement.Value with
                    | false ->
                        printfn "Requirement not met"
                        setGameStateOutEvent state
                        |> Choice1Of2
                    | true ->
                        let updatedEvent = updateEventCurrentPath state.gameWorld.event.Value response
                        let newState = updateEventInGameState updatedEvent state
                        // let result = checkRequirementMet state path.requirement.Value
                        printPathText newState.gameWorld.event.Value.getPath
                        
                        match newState.gameWorld.event.Value.checkFinish with
                        | true ->
                            let updatedEvent = updateEventSetFinished newState.gameWorld.event.Value
                            
                            updateEventInGameState (Some(updatedEvent)) state
                            |> performEventResult
                            |> setGameStateOutEvent
                            |> Choice1Of2
                        | false ->
                            Choice1Of2 newState
            | Quit ->
                Environment.Exit(0) //exits
                Choice1Of2 state
            | _ -> Choice2Of2 CannotMatchEventChoice
