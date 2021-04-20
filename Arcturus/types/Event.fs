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
        id: int
        text: string
        nextPath: path option
        pathRequirement: pathRequirement option
    }
   
    type event = {
        title: string
        startPath: path
        currentPath: path
        finished: bool
        eventResult: eventResult option
    }