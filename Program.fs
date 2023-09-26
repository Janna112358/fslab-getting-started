open FSharp.Data
open Deedle
open Plotly.NET
open FSharp.Stats
open FSharpAux
open FSharp.Stats.Correlation
open Fitting.LinearRegression.OLS

// Retrieve data using the FSharp.Data package
let rawData =
    Http.RequestString @"https://raw.githubusercontent.com/dotnet/machinelearning/master/test/data/housing.txt"

// And create a data frame object using the ReadCsvString method provided by Deedle.
// Note: Of course you can directly provide the path to a local source.
let housesAllColumns =
    Frame.ReadCsvString(rawData, hasHeaders = true, separators = "\t")

let houses =
    let targetColumns =
        seq {
            "RoomsPerDwelling"
            "MedianHomeValue"
            "CharlesRiver"
        }

    housesAllColumns |> Frame.sliceCols targetColumns

let innerJoinUnzip (data: Frame<int, string>) (columnA: string) (columnB: string) =
    let seriesA: Series<_, float> = data |> Frame.getCol columnA
    let seriesB: Series<_, float> = data |> Frame.getCol columnB
    Series.zipInner seriesA seriesB |> Series.values |> Seq.unzip

let correlate (data: Frame<int, string>) (columnA: string) (columnB: string) =
    innerJoinUnzip data columnA columnB ||> Seq.pearson

let linearModels (data: Frame<int, string>) (label: string) (columnA: string) (columnB: string) =
    let valuesA, valuesB = innerJoinUnzip data columnA columnB

    let coefficients = Linear.Univariable.fit (vector valuesA) (vector valuesB)
    let c0 = coefficients.getCoefficient 0
    let c1 = coefficients.getCoefficient 1
    let predictedB = valuesA |> Seq.map (Linear.Univariable.predict coefficients)

    [ Chart.Point(valuesA, valuesB) |> Chart.withTraceInfo $"{label}: data"
      Chart.Line(valuesA, predictedB)
      |> Chart.withTraceInfo $"{label}: prediction y = {c1} x + {c0}" ]
    |> Chart.combine
    |> Chart.withXAxisStyle (columnA)
    |> Chart.withYAxisStyle (columnB)

[<EntryPoint>]
let main args =
    let housesNotAtRiver =
        houses |> Frame.filterRowValues (fun x -> x.GetAs<bool>("CharlesRiver") |> not)

    let housesAtRiver =
        houses |> Frame.filterRowValues (fun x -> x.GetAs<bool>("CharlesRiver"))

    // Histogram.histogram houses |> Chart.show

    correlate houses "MedianHomeValue" "RoomsPerDwelling"
    |> printfn "Correlation between prices and number of rooms: %A"

    [ linearModels housesNotAtRiver "not at river" "RoomsPerDwelling" "MedianHomeValue"
      linearModels housesAtRiver "at river" "RoomsPerDwelling" "MedianHomeValue" ]
    |> Chart.combine
    |> Chart.withSize (1200., 700.)
    |> Chart.show


    0
