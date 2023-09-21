open FSharp.Data
open Deedle

// Retrieve data using the FSharp.Data package
let rawData =
    Http.RequestString @"https://raw.githubusercontent.com/dotnet/machinelearning/master/test/data/housing.txt"

// And create a data frame object using the ReadCsvString method provided by Deedle.
// Note: Of course you can directly provide the path to a local source.
let df = Frame.ReadCsvString(rawData, hasHeaders = true, separators = "\t")
