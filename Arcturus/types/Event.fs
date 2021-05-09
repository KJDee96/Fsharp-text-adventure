namespace Arcturus.Types

open Arcturus.Types.Items
open Arcturus.Types.Player
open FSharpPlus.Lens

module Event =
    type eventResult =
        | Item of item
        | StatIncrease of playerStat

    type responseRequirement =
        | ItemInInvCheck of item
        | StatCheck of playerStat

    type response =
        { text: string
          next: int option
          requirement: responseRequirement option }

    type path =
        { text: string
          options: response list option
          result: eventResult option }

    type event =
        { title: string
          paths: path list
          currentPath: int
          finished: bool }
        member this.getPath = this.paths.[this.currentPath]

        member this.checkFinish =
            match (List.item this.currentPath this.paths).options with
            | None -> true
            | _ -> false
                        
    let inline _currentPath f event =
        f event.currentPath
        <&> fun currentPath -> { event with currentPath = currentPath }
        
    let inline _finished f event =
        f event.finished
        <&> fun finished -> { event with finished = finished }

    let updateEventCurrentPath (event: event) response =
        match event.checkFinish with
        | false ->
            Some(over _currentPath (fun _ -> response.next.Value) event)
        | _ -> Some(event)

    let updateEventSetFinished (event: event) =
        over _finished (fun _ -> true) event
    
    let doNothingResponse =
        { text = "Come back later"
          next = None
          requirement = None }

    let event1 =
        { title = "Test Event"
          paths =
              [ { text = "There is a computer, what do you do?"
                  options =
                      Some [ { text = "Hack it"
                               next = Some 1
                               requirement = Some(StatCheck(Intelligence, StatValue 2uy)) }
                             { text = "Break it"
                               next = Some 2
                               requirement = Some(ItemInInvCheck wrench) }
                             doNothingResponse ]
                  result = None }

                { text = "You hack the computer successfully\nYou feel smarter"
                  options = None
                  result = Some(StatIncrease(Intelligence, StatValue 1uy)) }

                { text = "You smash the computer\nYou found a screw in the smashed parts"
                  options = None
                  result = Some(Item(screw)) } ]
          currentPath = 0
          finished = false }

    let testResponse : response =
        { text = "Hack it"
          next = Some 1
          requirement = Some(StatCheck(Intelligence, StatValue 6uy)) }
