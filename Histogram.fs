module Histogram

open FSharp.Data
open Deedle
open Plotly.NET

let histogram (houses: Frame<int, string>) =

    let housesNotAtRiver =
        houses |> Frame.filterRowValues (fun x -> x.GetAs<bool>("CharlesRiver") |> not)

    let housesAtRiver =
        houses |> Frame.filterRowValues (fun x -> x.GetAs<bool>("CharlesRiver"))

    let homeValuesNotAtRiver: seq<float> =
        housesNotAtRiver.GetColumn "MedianHomeValue" |> Series.values

    let homeValuesAtRiver: seq<float> =
        housesAtRiver.GetColumn "MedianHomeValue" |> Series.values

    [ Chart.Histogram(
          homeValuesAtRiver,
          Opacity = 0.66,
          OffsetGroup = "A"
      // HistNorm = StyleParam.HistNorm.ProbabilityDensity
      )
      |> Chart.withTraceInfo "at river"
      Chart.Histogram(
          homeValuesNotAtRiver,
          Opacity = 0.66,
          OffsetGroup = "A"
      // HistNorm = StyleParam.HistNorm.ProbabilityDensity
      )
      |> Chart.withTraceInfo "not at river" ]
    |> Chart.combine
    |> Chart.withYAxisStyle ("median value of owner occupied homes in 1000s")
    |> Chart.withXAxisStyle ("Comparison of price distributions (noramlized)")
