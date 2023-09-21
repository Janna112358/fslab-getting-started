open FSharp.Data
open Deedle
open Plotly.NET

// Retrieve data using the FSharp.Data package
let rawData =
    Http.RequestString @"https://raw.githubusercontent.com/dotnet/machinelearning/master/test/data/housing.txt"

// And create a data frame object using the ReadCsvString method provided by Deedle.
// Note: Of course you can directly provide the path to a local source.
let housesFull = Frame.ReadCsvString(rawData, hasHeaders = true, separators = "\t")

// get dataframe with only some of the columns, and only houses not on the river
let housesNotAtRiver =
    let targetColumns =
        seq {
            "RoomsPerDwelling"
            "MedianHomeValue"
            "CharlesRiver"
        }

    housesFull
    |> Frame.sliceCols targetColumns
    |> Frame.filterRowValues (fun x -> x.GetAs<bool>("CharlesRiver") |> not)


let pricesNotAtRiver: seq<float> =
    housesNotAtRiver.GetColumn "MedianHomeValue" |> Series.values

Chart.Histogram(pricesNotAtRiver)
|> Chart.withYAxisStyle ("median value of owner occupied home in 1000s")
|> Chart.withXAxisStyle ("price distribution")
|> Chart.show
