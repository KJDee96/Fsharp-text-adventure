namespace Arcturus.Utils

open System.Text.RegularExpressions

module PatternMatching =
    (*regex ^start
        \s- whitespace
        * - match 0 or more of the preceding token
        (||) - capture group with alternation
        ? - match 0 or 1 of the preceding token
        $end*)
    let northPattern = "^(n|north|N|North)$"

    let eastPattern = "^(e|east|E|East)$"
    let southPattern = "^(s|south|S|South)$"
    let westPattern = "^(w|west|W|West)$"

    let movePattern = "^(m|M)(ove)?$"
    let checkPattern = "^(c|C)(heck)?$"
    let GrabPattern = "^(g|G)(rab)?$"
    let invPattern = "^(i|I)(nv|nventory)?$"
    let statsPattern = "^(s|S)(tats)?$"
    let helpPattern = "^(h|H)(elp)?$"
    let quitPattern = "^(q|Q)(uit)?$"
    let digitPattern = "^\d$"

    //matching the regex for commands
    let (|MoveMatch|CheckMatch|GrabMatch|InvMatch|HelpMatch|QuitMatch|NoMatch|) input =
        match Regex.Match(input, movePattern),
              Regex.Match(input, checkPattern),
              Regex.Match(input, invPattern),
              Regex.Match(input, GrabPattern),
              Regex.Match(input, helpPattern),
              Regex.Match(input, quitPattern) with
        | moveMatch, _, _, _, _, _ when moveMatch.Success -> MoveMatch
        | _, checkMatch, _, _, _, _ when checkMatch.Success -> CheckMatch
        | _, _, grabMatch, _, _, _ when grabMatch.Success -> GrabMatch
        | _, _, _, invMatch, _, _ when invMatch.Success -> InvMatch
        | _, _, _, _, helpMatch, _ when helpMatch.Success -> HelpMatch
        | _, _, _, _, _, quitMatch when quitMatch.Success -> QuitMatch
        | _ -> NoMatch
    
    let (|StatsMatch|NoMatch|) input =
        match Regex.Match(input, statsPattern) with
        | statsMatch when statsMatch.Success -> StatsMatch
        | _ -> NoMatch

    let (|DigitMatch|NoMatch|) input =
        match Regex.Match(input, digitPattern) with
        | digitMatch when digitMatch.Success -> DigitMatch
        | _ -> NoMatch

    //matching the regex for directions
    let (|NorthMatch|EastMatch|SouthMatch|WestMatch|NoMatch|) input =
        match Regex.Match(input, northPattern),
              Regex.Match(input, eastPattern),
              Regex.Match(input, southPattern),
              Regex.Match(input, westPattern) with
        | northMatch, _, _, _ when northMatch.Success -> NorthMatch
        | _, eastMatch, _, _ when eastMatch.Success -> EastMatch
        | _, _, southMatch, _ when southMatch.Success -> SouthMatch
        | _, _, _, westMatch when westMatch.Success -> WestMatch
        | _ -> NoMatch
