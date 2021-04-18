namespace Arcturus.Types

open Levels
open Items
open FSharpPlus.Lens

module Player =
    type Player =
        { location: Coordinates
          playerItems: Item list }

    //Prism for player type        
    let inline _location f p =
        f p.location <&> fun l -> { p with location = l }

    let inline _playerItems f p =
        f p.playerItems <&> fun i -> { p with playerItems = i }
