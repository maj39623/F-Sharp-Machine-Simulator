module MachineSimulator

// Define States
type MachineState =
    | Idle
    | Working
    | Error of string
    | ErrorAcknowledge
    | Maintenance
    | Paused

// Logs
type LogLevel =
    | Info
    | Warning
    | ErrorLog

type MachineLog = {
    Timestamp: System.DateTime
    Level: LogLevel
    Message: string
}

// Define Machine
type Machine = {
    Id: int
    CurrentState: MachineState
    Logs: MachineLog list
}

let logMessage level message =
    { Timestamp = System.DateTime.Now; Level = level; Message = message}

// Simulator Functions

let initiateMachineProcess machine =
    match machine.CurrentState with
    | Idle -> 
        { machine with 
            CurrentState = Working; 
            Logs = (logMessage Info (sprintf "Machine %d: Started" machine.Id)) :: machine.Logs }
    | _ -> 
        { machine with 
            Logs = (logMessage ErrorLog (sprintf "Machine %d: Failed to start. Not in Idle state." machine.Id)) :: machine.Logs }

let completeMachineProcess machine = 
    match machine.CurrentState with
    | Working -> 
        { machine with 
            CurrentState = Idle; 
            Logs = (logMessage Info (sprintf "Machine %d: Completed work" machine.Id)) :: machine.Logs }
    | _ -> 
        { machine with 
            Logs = (logMessage Warning (sprintf "Machine %d: Failed to complete. Not in Working state." machine.Id)) :: machine.Logs }

let triggerMachineError machine errorDetail = 
    { machine with 
        CurrentState = Error errorDetail; 
        Logs = (logMessage ErrorLog (sprintf "Machine %d: Error - %s" machine.Id errorDetail)) :: machine.Logs }

let scheduleMaintenance machine = 
    match machine.CurrentState with
    | Idle -> 
        { machine with 
            CurrentState = Maintenance; 
            Logs = (logMessage Info "Scheduled Maintenance") :: machine.Logs }
    | _ -> 
        { machine with 
            Logs = (logMessage Warning "Failed to schedule maintenance. Not in Idle state.") :: machine.Logs }

let pauseMachine machine =
    match machine.CurrentState with
    | Working -> 
        { machine with 
            CurrentState = Paused; 
            Logs = (logMessage Info "Machine Paused") :: machine.Logs }
    | _ -> 
        { machine with 
            Logs = (logMessage Warning "Failed to pause. Not in Working state.") :: machine.Logs }

let resumeMachine machine =
    match machine.CurrentState with
    | Paused -> 
        { machine with 
            CurrentState = Working; 
            Logs = (logMessage Info "Machine Resumed") :: machine.Logs }
    | _ -> 
        { machine with 
            Logs = (logMessage Warning "Failed to resume. Not in Paused state.") :: machine.Logs }

let acknowledgeError machine =
    match machine.CurrentState with
    | Error _ -> 
        { machine with 
            CurrentState = ErrorAcknowledge; 
            Logs = (logMessage Info "Error Acknowledged") :: machine.Logs }
    | _ -> machine

// Simulation randomizer

let random = System.Random()

let randomEvent machine =
    let chance = random.Next(0, 100)
    match machine.CurrentState, chance with
    | _, _ when chance < 10 ->
        let errors = ["Power Surge"; "Overheating"; "Component Failure"; "Memory Error"]
        triggerMachineError machine (errors.[random.Next(0, errors.Length)])
    | Error _, _ when chance < 20 -> acknowledgeError machine
    | ErrorAcknowledge, _ when chance < 30 -> { machine with CurrentState = Idle; Logs = (logMessage Info "Error Resolved") :: machine.Logs }
    | _, _ when chance < 40 -> pauseMachine machine
    | Paused, _ when chance < 50 -> resumeMachine machine
    | _, _ when chance < 60 -> scheduleMaintenance machine
    | _ -> machine

// Simulation over multiple ticks
let simulateTick machines = 
    machines
    |> List.map initiateMachineProcess
    |> List.map randomEvent
    |> List.map completeMachineProcess

let simulate factoryMachine ticks =
    let rec loop machines remainingTicks =
        if remainingTicks <= 0 then machines
        else loop (simulateTick machines) (remainingTicks - 1)
    loop factoryMachine ticks

// create a list of machines
let machines = [
    { Id = 1; CurrentState = Idle; Logs = []};
    { Id = 2; CurrentState = Idle; Logs = []};
]

// Run simulation
let simulationResult = simulate machines 5

// Print results
simulationResult |> List.iter (fun m -> 
    printfn "Machine %d State: %A" m.Id m.CurrentState
    m.Logs |> List.iter (fun log -> printfn "Timestamp: %A, Level: %A, Message: %s" log.Timestamp log.Level log.Message)
    printfn "----------------------"
)

