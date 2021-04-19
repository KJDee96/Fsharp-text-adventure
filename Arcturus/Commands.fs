namespace Arcturus.Core

open Arcturus.Types.Player
open Arcturus.Types.GameState
open Arcturus.Types.Level
open Arcturus.Utils.Errors
open Arcturus.Utils.Printing
open FSharpPlus
open FSharpPlus.Lens
open System
open System.Text.RegularExpressions

module Commands =

    type Command =
        | Move of int * int
        | Check
        | Inventory
        | Grab
        | Help
        | Quit

    (*regex ^start
        \s- whitespace
        * - match 0 or more of the preceding token
        (||) - capture group with alternation
        {0,1} - match 0 or 1 of the preceding token
        $end*)
    let northPattern = "^\s*(n|north|N|North)\s*$"
    let eastPattern = "^\s*(e|east|E|East)\s*$"
    let southPattern = "^\s*(s|south|S|South)\s*$"
    let westPattern = "^\s*(w|west|W|West)\s*$"

    let movePattern = "^\s*m(ove){0,1}\s*$"
    let checkPattern = "^\s*(c|C)(heck){0,1}\s*$"
    let invPattern = "^\s*(i|I)(nv|nventory){0,1}\s*$"
    let GrabPattern = "^\s*(g|G)(rab){0,1}\s*$"
    let helpPattern = "^\s*(h|H)(elp){0,1}\s*$"
    let quitPattern = "^\s*(q|Q)(uit){0,1}\s*$"

    //matching the regex for commands
    let (|MoveMatch|CheckMatch|InvMatch|GrabMatch|HelpMatch|QuitMatch|NoMatch|) input =
        match Regex.Match(input, movePattern),
              Regex.Match(input, checkPattern),
              Regex.Match(input, invPattern),
              Regex.Match(input, GrabPattern),
              Regex.Match(input, helpPattern),
              Regex.Match(input, quitPattern) with
        | moveMatch, _, _, _, _, _ when moveMatch.Success -> MoveMatch
        | _, checkMatch, _, _, _, _ when checkMatch.Success -> CheckMatch
        | _, _, invMatch, _, _, _ when invMatch.Success -> InvMatch
        | _, _, _, grabMatch, _, _ when grabMatch.Success -> GrabMatch
        | _, _, _, _, helpMatch, _ when helpMatch.Success -> HelpMatch
        | _, _, _, _, _, quitMatch when quitMatch.Success -> QuitMatch
        | _ -> NoMatch

    //matching the regex for directions
    let (|NorthMatch|EastMatch|SouthMatch|WestMatch|NoMatch|) input =
        match Regex.Match(input, northPattern),
              Regex.Match(input, eastPattern),
              Regex.Match(input, southPattern),
              Regex.Match(input, westPattern) with
        | northMatch, _, _, _ when northMatch.Success -> NorthMatch
        | _, eastMatch, _, _ when eastMatch.Success -> EastMatch
        | _, _, southMatch, _ when southMatch.Success -> SouthMatch
        | _, _, _, westMatch when westMatch.Success -> WestMatch
        | _ -> NoMatch

    //returning state for player movement
    let moveDir (dir: Coordinates) (state: State) =
        let newLocation =
            { state.player.location with
                  x = state.player.location.x + dir.x
                  y = state.player.location.y + dir.y }

        if newLocation.x > state.gameWorld.size.x
           || newLocation.y > state.gameWorld.size.y then
            Choice2Of2 CannotMove
        else if newLocation.x = -1 || newLocation.y = -1 then
            Choice2Of2 CannotMove
        else
            let returnState : State =
                over (_player << _location) (fun _ -> newLocation) state // update player location and then the player gamestate

            Choice1Of2 returnState

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
            match moveDir { x = x; y = y } state with
            | Choice1Of2 newState ->
                printNewLocation newState
                Choice1Of2 newState
            | Choice2Of2 error -> Choice2Of2 error
        | Check ->
            printLocationData state
            printGameWorldItems state
            printExits state
            Choice1Of2 state
        | Inventory ->
            printInv state
            Choice1Of2 state
        | Grab ->
            printGameWorldItems state //prints items at location with index + 1
            if not state.gameWorld.levelItems.IsEmpty then
                match grabItem state with
                | Choice1Of2 newState ->
                    printInv newState
                    Choice1Of2 newState
                | Choice2Of2 error -> Choice2Of2 error
            else
                Choice1Of2 state
        | Help ->
            printfn "%s" help //prints help
            Choice1Of2 state
        | Quit ->
            Environment.Exit(0) //exits
            Choice1Of2 state
