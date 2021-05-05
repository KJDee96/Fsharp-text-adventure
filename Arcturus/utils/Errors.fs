namespace Arcturus.Utils

module Errors =
    type error =
        | CannotParseInvalidCommand
        | CannotMove
        | CannotMatchCompass
        | CannotMatchEventChoice
