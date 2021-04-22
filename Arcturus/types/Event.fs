namespace Arcturus.Types

open Arcturus.Types.Items
open Arcturus.Types.Player

module Event =
    type eventResult =
        | Item of item
        | StatIncrease of statType * playerStat
        
    type responseRequirement =
        | Item of item
        | StatCheck of statType * playerStat
    
    type response = {
        text: string
        next: int option
        requirement: responseRequirement option
    }
    type path = {
        text: string
        options: response list option
        result: eventResult option
    }
      
    type event = {
        title: string
        paths: path list
        currentPath: int
        finished: bool
    } with member this.checkFinish =
            match (List.item this.currentPath this.paths).options with
            | None -> true
            | _ -> false

    let event1 = {
        title = "Test Event"
        paths = [{
            text = "There is a computer, what do you do?"
            options = Some [{text = "Hack it"; next = Some 1; requirement = Some (StatCheck (Intelligence, StatValue 6uy))}
                            {text = "Break it"; next = Some 2; requirement = Some (Item wrench)}]
            result = None};
                       
                       {text = "You hack it successfully"
                        options = None
                        result = Some (StatIncrease (Strength, StatValue 1uy))}]
        currentPath = 0
        finished = false
    }
