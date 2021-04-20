namespace Arcturus.Core

open Arcturus.Res.Strings
open Arcturus.Types.Player
open Arcturus.Types.GameState
open Arcturus.Types.Level
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
        | Inventory
        | Grab
        | Help
        | Quit

    let setName state =
        printf "%s" setNamePrompt
        let name = Console.ReadLine()
        over (_player << _name) (fun _ -> name) state // update player name 

    //returning state for player movement
//    let moveDir (dir: Coordinates) (state: State) =
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
            
    let grabItem (state: State) =
        printf "%s" grabItemPrompt

        let parsed, index =
            Int32.TryParse(Console.ReadLine().Trim().ToLower())

        if parsed then
            let item =
                List.tryItem (index - 1) state.gameWorld.levelItems

            if item <> None then
                let newPlayerInventory = addItemToInv state.player item.Value.item

                let newGameWorldItemList = removeItemFromWorld state.gameWorld item.Value

                let returnState : State =
                    state
                    |> over (_player << _playerItems) (fun _ -> newPlayerInventory) // update player items
                    |> over (_gameWorld << _levelItems) (fun _ -> newGameWorldItemList) // update world items

                Choice1Of2 returnState
            else
                Choice2Of2 CannotParseInvalidCommand
        else
            Choice2Of2 CannotParseInvalidCommand


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
    let parseInput input =
        match input with
        | MoveMatch ->
            printf "%s" movePrompt

            let input = Console.ReadLine().Trim().ToLower()

            parseDirectionInput input //pass result of input to parse direction
        | CheckMatch -> Choice1Of2 Check
        | InvMatch -> Choice1Of2 Inventory
        | GrabMatch -> Choice1Of2 Grab
        | HelpMatch -> Choice1Of2 Help
        | QuitMatch -> Choice1Of2 Quit
        | _ -> Choice2Of2 CannotParseInvalidCommand
        
    //execute command
    let executeCommand state command =
        match command with
        | Move (x, y) ->
//            match moveDir { x = x; y = y } state with
//            | Choice1Of2 state ->
//                printNewLocation state
//                Choice1Of2 state
//            | Choice2Of2 error -> Choice2Of2 error
            Choice1Of2 state
        | Check ->
//            printLocationData state
//            printGameWorldItems state
//            printExits state
            Choice1Of2 state
        | Inventory ->
            printInv state
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
        | Help ->
            printfn "%s" help //prints help
            Choice1Of2 state
        | Quit ->
            Environment.Exit(0) //exits
            Choice1Of2 state
