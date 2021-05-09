namespace Arcturus.Core

open Arcturus.Res.Strings
open Arcturus.Utils.Errors
open Arcturus.Utils.Printing
open Arcturus.Types.GameState
open Arcturus.Core.Commands
open FSharpPlus

module Program =
    let init = getInitialState //initialiser for state

    let input =
        writeSlowly (List.ofSeq openingString) // writeslowly passing in msg
        seq { yield! userInput }

    let checkStateInEvent state =
        match state.inEvent with
        | true -> printEventTitlePrompt state.gameWorld.event.Value
        | false -> ()
        
    let checkError error state = 
        match error with
            //printing errors
            | CannotParseInvalidCommand ->
                printfn "%s" errorCannotParseInvalidCommandString
                checkStateInEvent state
                state
            | CannotMove ->
                printfn "%s" errorCannotMoveString
                checkStateInEvent state
                state
            | CannotMatchCompass ->
                printfn "%s" errorCannotMatchCompassString
                checkStateInEvent state
                state
            | CannotMatchEventChoice ->
                printfn "%s" errorCannotMatchEventChoiceString
                checkStateInEvent state
                state

    let checkChoiceResult state choice = 
        match choice with
            | Choice1Of2 newState ->
                checkStateInEvent newState
                newState
            | Choice2Of2 error -> checkError error state
                    
    //parse and execute with state as input
    let parseAndExecute state =
        parseInput
        >=> executeCommand state //Kleisli (choice) composition
        >> checkChoiceResult state
        
    [<EntryPoint>]
    let main argv =
        let gameState = init |> setName
        
        input //call input function
            //fold sequence for input
        |> Seq.fold parseAndExecute gameState
        |> ignore

        0 // return an integer exit code
