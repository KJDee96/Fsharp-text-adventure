namespace Arcturus.Types

open Arcturus.Types.Items
open Arcturus.Types.Player

module Event =
    type eventResult =
        | Item of item
        | StatIncrease of statType * playerStat
        
    type pathRequirement =
        | Item of item
        | StatCheck of statType * playerStat

    
    type path = {
        text: string
        nextPath: int option
        pathRequirement: pathRequirement option
    }
   
    type event = {
        title: string
        paths: path list
        currentPath: int 
        finished: bool
        eventResult: eventResult option
    } with
        member this.checkFinish =
            if (List.item this.currentPath this.paths).nextPath = None
            then true
            else false
            
    let path1 = {text="test";nextPath=None;pathRequirement=None}
    let event1 = {title="eventTest";paths=[path1];currentPath=0;finished=false;eventResult=None}