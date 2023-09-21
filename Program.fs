open FSharp.Data
open Deedle
open Plotly.NET

// Retrieve data using the FSharp.Data package
let rawData =
    Http.RequestString @"https://raw.githubusercontent.com/dotnet/machinelearning/master/test/data/housing.txt"

// And create a data frame object using the ReadCsvString method provided by Deedle.
// Note: Of course you can directly provide the path to a local source.
let housesFull = Frame.ReadCsvString(rawData, hasHeaders = true, separators = "\t")

let housesWorkingData =
    let targetColumns =
        seq {
            "RoomsPerDwelling"
            "MedianHomeValue"
            "CharlesRiver"
        }

    housesFull |> Frame.sliceCols targetColumns

let housesNotAtRiver =
    housesWorkingData
    |> Frame.filterRowValues (fun x -> x.GetAs<bool>("CharlesRiver") |> not)

let housesAtRiver =
    housesWorkingData
    |> Frame.filterRowValues (fun x -> x.GetAs<bool>("CharlesRiver"))

// let normalizeByTotal (values: seq<float>) =
//     let total = Seq.sum (values)
//     Seq.map (fun x -> x / total) values

let homeValuesNotAtRiver: seq<float> =
    housesNotAtRiver.GetColumn "MedianHomeValue" |> Series.values

let homeValuesAtRiver: seq<float> =
    housesAtRiver.GetColumn "MedianHomeValue" |> Series.values

[ Chart.Histogram(
      homeValuesAtRiver,
      Opacity = 0.66,
      OffsetGroup = "A",
      HistNorm = StyleParam.HistNorm.ProbabilityDensity
  )
  |> Chart.withTraceInfo "at river"
  Chart.Histogram(
      homeValuesNotAtRiver,
      Opacity = 0.66,
      OffsetGroup = "A",
      HistNorm = StyleParam.HistNorm.ProbabilityDensity
  )
  |> Chart.withTraceInfo "not at river" ]
|> Chart.combine
|> Chart.withYAxisStyle ("median value of owner occupied homes in 1000s")
|> Chart.withXAxisStyle ("Comparison of price distributions (noramlized)")
|> Chart.show
