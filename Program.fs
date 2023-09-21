open Deedle
open FSharp.Data

// For more information see https://aka.ms/fsharp-console-apps
printfn "Hello from F#"

let animals = Frame.ReadCsv("./test.csv", hasHeaders=true, separators=",")

let weight = animals.GetColumn<float>("weight_in_kg")

printfn "Weight: %A" weight