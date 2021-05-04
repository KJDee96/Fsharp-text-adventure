namespace Arcturus.Core

open Arcturus.Res.Strings
open Arcturus.Utils.Errors
open Arcturus.Utils.Printing
open Arcturus.Types.GameState
open Arcturus.Core.Commands
open FSharpPlus

module Program =
    let init () = getInitialState //initialiser for state

    let input =
        writeSlowly (List.ofSeq opening) // writeslowly passing in msg
        seq { yield! userInput }

    //parse and execute with state as input
    let parseAndExecute state =
        parseInput
        //Kleisli composition composing parseinput result into execute command
        >=> executeCommand state
        >> fun x ->
            match x with
            | Choice1Of2 state -> state
            | Choice2Of2 error ->
                match error with
                //printing errors
                | CannotParseInvalidCommand ->
                    printfn "%s" errorInputString
                    state
                | CannotMove ->
                    printfn "%s" errorNoRoomsString
                    state
                | CannotMatchCompass ->
                    printfn "%s" errorDirectionString
                    state

    [<EntryPoint>]
    let main argv =
        let gameState = init () |> setName

        input //call input function
        //fold sequence for input
        |> Seq.fold parseAndExecute (gameState)
        |> ignore

        0 // return an integer exit code
