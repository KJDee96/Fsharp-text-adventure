namespace Arcturus.Types

module PlayerStat =
    type PlayerStat = Stat of uint8
             
    let create i = //let x = Stat 8uy;;
        if (i >= 1uy && i <= 5uy )
        then Some (Stat i)
        else None
