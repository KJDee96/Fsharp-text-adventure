namespace Arcturus.Core
open Arcturus.Utils.Errors
open Arcturus.Utils.Printing
open Arcturus.Types.GameState
open Arcturus.Core.Commands
open FSharpPlus

module Program =

    let init () = getInitialState //initialiser for state


    let input =
        seq {
            let msg = opening
            writeSlowly (List.ofSeq msg) // writeslowly passing in msg
            yield! userInput
        //sequence for input fold
        }

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
                    printfn "%s" "ERROR: Cannot parse input, invalid command"
                    state
                | CannotMove ->
                    printfn "%s" "ERROR: No more rooms to move to that way"
                    state
                | CannotMatchCompass ->
                    printfn "%s" "ERROR: Cannot match a compass direction - N/E/S/W"
                    state

    [<EntryPoint>]
    let main argv =
        input //call input function
        //fold sequence for input
        |> Seq.fold parseAndExecute (init ())
        |> ignore

        0 // return an integer exit code
