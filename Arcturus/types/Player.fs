namespace Arcturus.Types

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

    type playerStat = StatValue of uint8

    type special =
        { Strength: playerStat
          Perception: playerStat
          Endurance: playerStat
          Charisma: playerStat
          Intelligence: playerStat
          Agility: playerStat
          Luck: playerStat }

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
