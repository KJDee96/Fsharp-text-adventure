namespace Arcturus.Types

open Arcturus.Types.Items
open Arcturus.Types.Player
open FSharpPlus.Lens

module Event =
    type eventResult =
        | Item of item
        | StatIncrease of playerStat

    type responseRequirement =
        | Item of item
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

    let updateEventCurrentPath (event: event) response =
        match event.checkFinish with
        | false ->
            Some(over _currentPath (fun _ -> response.next.Value) event)
        | _ -> Some(event)


    let doNothingPath =
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
                               requirement = Some(StatCheck(Intelligence, StatValue 6uy)) }
                             { text = "Break it"
                               next = Some 2
                               requirement = Some(Item wrench) }
                             doNothingPath ]
                  result = None }

                { text = "You hack the computer successfully"
                  options = None
                  result = Some(StatIncrease(Intelligence, StatValue 1uy)) }

                { text = "You smash the computer"
                  options = None
                  result = Some(StatIncrease(Strength, StatValue 1uy)) } ]
          currentPath = 0
          finished = false }

    let testResponse : response =
        { text = "Hack it"
          next = Some 1
          requirement = Some(StatCheck(Intelligence, StatValue 6uy)) }
