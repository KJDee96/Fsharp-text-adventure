namespace Arcturus.Types

open System
open Arcturus.Types.Items
open FSharpPlus.Lens

module Player =
    type statType =
        | Strength
        | Perception
        | Endurance
        | Charisma
        | Intelligence
        | Agility
        | Luck

    type playerStatValue =
        | StatValue of uint8
        static member (+) (x: playerStatValue, y: playerStatValue) : playerStatValue =
            match x, y with
            | StatValue x, StatValue y -> StatValue (x+y)

    type playerStat = statType * playerStatValue

    type special =
        { Strength: playerStatValue
          Perception: playerStatValue
          Endurance: playerStatValue
          Charisma: playerStatValue
          Intelligence: playerStatValue
          Agility: playerStatValue
          Luck: playerStatValue }

    let inline _strength f special =
        f special.Strength
        <&> fun s -> { special with Strength = s }

    let inline _perception f special =
        f special.Perception
        <&> fun s -> { special with Perception = s }

    let inline _endurance f special =
        f special.Endurance
        <&> fun s -> { special with Endurance = s }

    let inline _charisma f special =
        f special.Charisma
        <&> fun s -> { special with Charisma = s }

    let inline _intelligence f special =
        f special.Intelligence
        <&> fun s -> { special with Intelligence = s }

    let inline _agility f special =
        f special.Agility
        <&> fun s -> { special with Agility = s }

    let inline _luck f special =
        f special.Luck
        <&> fun s -> { special with Luck = s }

    type player =
        { name: string
          inventory: item list
          stats: special }
        member this.hasItem(item: item) =
            List.tryFind (fun _ -> item = item) this.inventory
    //        member this.hasStat (requiredStat: playerStat) =
//            let stat,requiredValue = requiredStat
//
//            let specialProperties = typeof<special>.GetProperties ()
//            let x = specialProperties |> Array.tryFind (fun t -> t.Name = stat.ToString ())
//            let c = x |> Option.map (fun pi -> pi.GetValue this.stats)
//
//            Int32.TryParse(c.Value.ToString)


    //            playerstat = requiredStat

    let addItemToInv player item = item :: player.inventory

    //Prism for player type
    let inline _name f p =
        f p.name <&> fun name -> { p with name = name }

    let inline _playerItems f p =
        f p.inventory
        <&> fun inv -> { p with inventory = inv }

    let inline _stats f p =
        f p.stats
        <&> fun stats -> { p with stats = stats }
