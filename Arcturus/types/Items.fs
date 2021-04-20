namespace Arcturus.Types

module Items =

    type item = { name: string; description: string }

    let wrench =
        { name = "Wrench"
          description = "Grab, Twist, Let go, Repeat" }

    let screwdriver =
        { name = "Screwdriver"
          description = "Twisty Twisty" }

    let hammer =
        { name = "Hammer"
          description = "Thump" }

    let screw =
        { name = "Screw"
          description = "Twisty poke" }

    let nail = { name = "Nail"; description = "Poke" }

    let keycard =
        { name = "Keycard"
          description = "Swipe to enter" }
