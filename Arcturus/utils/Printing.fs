namespace Arcturus.Utils

open System
open System.Threading
open Arcturus.Types.GameState
open Arcturus.Types.Event
open Arcturus.Res.Strings
open Arcturus.Types.Items

module Printing =

    let rec writeSlowly input =
        match input with
        | [] -> () //empty list with unit
        | head :: tail ->
            printf "%c" head //print head
            Thread.Sleep(0) //wait n miliseconds
            writeSlowly tail //recursive call tail


    //user input prompt >
    let userInput =
        seq {
            while true do
                printf "> "
                yield Console.ReadLine().Trim().ToLower()
        }

    let printInv state =
        //prints inventory
        if not state.player.inventory.IsEmpty then
            state.player.inventory
            |> Seq.iter (fun item -> printfn "You have a %s, Description = %s" item.name item.description)
        else
            printfn "%s" noItemsInvString

    let printGameWorldItems state =
        if not state.gameWorld.levelItems.IsEmpty then
            //prints items at location
            printfn "%s" itemsLocationPrompt

            List.indexed state.gameWorld.levelItems
            |> List.iter
                (fun (index, items) ->
                    printfn "[%i] Name = %s, Description = %s \n" (index + 1) items.item.name items.item.description)
        else
            printfn "%s" noItemsLocationString

    let printEventTitle (event: event) = printfn "%s" event.title

    let printEventTitlePrompt (event: event) = printf "%s" event.title

    let printPathText (path: path) = printfn "%s" path.text

    let printResponseText (response: response) = printfn "%s" response.text

    let printPathOptions path =
        List.indexed path.options.Value
        |> List.iter (fun (index, option) -> printfn "[%i] %s" (index + 1) option.text)

    let printEventPath (event: event) =
        match event.checkFinish with
        | false -> printPathOptions event.getPath
        | _ -> printPathText event.getPath

    let printFoundItem (item: item) =
        printfn "%s" (foundItemString + item.name)

    let printAddedItemToInv (item: item) =
        printfn "%s " (item.name + itemAddedToInvString)

    let printStats state =
        let player = state.player

        printfn
            "Strength = %A\nPerception = %A\nEndurance = %A\nCharisma = %A\nIntelligence = %A\nAgility = %A\nLuck = %A"
            player.stats.Strength
            player.stats.Perception
            player.stats.Endurance
            player.stats.Charisma
            player.stats.Intelligence
            player.stats.Agility
            player.stats.Luck

//    let printLocationData state =
//        //prints floor name, location data
//        printfn
//            "Floor name is %s, location is x = %i, y = %i"
//            state.gameWorld.levelName
//            state.player.location.x
//            state.player.location.y
//
//    let printNewLocation state =
//        printfn "New location = x:%i y:%i" state.player.location.x state.player.location.y
//
//    let printExits state =
//        //prints exit options
//        match state.player.location.x, state.player.location.y with
//        | x, y when x = 0 && y = 0 ->
//            printfn "Available exits are S/E"
//        | x, _ when x = 0 ->
//            printfn "Available exits are N/E/S/"
//        | _, y when y = 0 ->
//            printfn "Available exits are E/S/W"
//        | x, y when
//            x = state.gameWorld.size.x
//            && y = state.gameWorld.size.y ->
//            printfn "Available exits are N/W"
//        | x, _ when x = state.gameWorld.size.x ->
//            printfn "Available exits are N/E/W"
//        | _, y when y = state.gameWorld.size.y ->
//            printfn "Available exits are N/S/W"
//        | _ ->
//            printfn "Available exits are N/E/S/W"
