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

    let movePattern = "^m(ove)?$"
    let checkPattern = "^(c|C)(heck)?$"
    let invPattern = "^(i|I)(nv|nventory)?$"
    let GrabPattern = "^(g|G)(rab)?$"
    let helpPattern = "^(h|H)(elp)?$"
    let quitPattern = "^(q|Q)(uit)?$"
    let digitPattern = "^\d$"

    //matching the regex for commands
    let (|MoveMatch|CheckMatch|InvMatch|GrabMatch|HelpMatch|QuitMatch|NoMatch|) input =
        match Regex.Match(input, movePattern),
              Regex.Match(input, checkPattern),
              Regex.Match(input, invPattern),
              Regex.Match(input, GrabPattern),
              Regex.Match(input, helpPattern),
              Regex.Match(input, quitPattern) with
        | moveMatch, _, _, _, _, _ when moveMatch.Success -> MoveMatch
        | _, checkMatch, _, _, _, _ when checkMatch.Success -> CheckMatch
        | _, _, invMatch, _, _, _ when invMatch.Success -> InvMatch
        | _, _, _, grabMatch, _, _ when grabMatch.Success -> GrabMatch
        | _, _, _, _, helpMatch, _ when helpMatch.Success -> HelpMatch
        | _, _, _, _, _, quitMatch when quitMatch.Success -> QuitMatch
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
