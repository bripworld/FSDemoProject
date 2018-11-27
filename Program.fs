namespace FSDemoProject
module FSDemo =
    open System
    open System.Linq
    open FSharp.Data
    open FSharp.Data.CsvExtensions
    open System.Collections.Generic
    
    let SUCCESS = "Success"
    let [<Literal>] tradeFilePath = "C:/share/trade_data.txt"
    let [<Literal>] matchFilePath = "C:/share/match_data.txt"
    
    let matchCollection = new Dictionary<string,string>()
    
    // Check whats asked
    let validateFunction (outputPath: string) = 
        printfn "%s" outputPath
        if not(IO.Directory.Exists(outputPath)) then printfn "Directory %s doesn't exist" outputPath; failwithf "Directory %s doesn't exist" outputPath
    
    let doMatch(trade:CsvRow,mtch:CsvRow) =
        if trade?OptionSymbol.Contains(mtch?Option) then
            let succesValue = matchCollection.TryAdd(mtch?Name,trade?OptionSymbol)
            if succesValue then
                printfn "Matched %s" trade?OptionSymbol
            
    let doTradeMatch(trade:CsvRow,matches:CsvFile)=    
            for mtch in matches.Rows do
                doMatch(trade,mtch)
                
    // Extract zip, read and let them fight one by one 
    let processFiles() = 
        let tradeData = CsvFile.Load(tradeFilePath)
        let matchData = CsvFile.Load(matchFilePath)
        tradeData.Rows.AsParallel().Select(trade =>doTradeMatch(trade,matchData)
        0
    
    [<EntryPoint>]
    let main args =
        match args with
        | [| outputPath; |] ->
        validateFunction(outputPath)
        printfn "Bingo!! atleast args are not that bad, call it"
        printfn "Starting Processing"
        let result = processFiles() 
        printfn "Processing Complete with %s, press any key to exit" result 
        printfn "Match Found %i" matchCollection.Count
        // Save Collection to OutputPath if required
        let key = Console.ReadKey()
        0
        | _ -> failwithf "Expected 1 commandline arguments, but got %i arguments" args.Length