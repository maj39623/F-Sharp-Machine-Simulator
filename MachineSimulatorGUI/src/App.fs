module App.View

open Elmish
open Elmish.Navigation
open Elmish.UrlParser
open Fable.Core
open Fable.Core.JsInterop
open Types
open App.State
open Global
open MachineSimulator

importSideEffects "../sass/main.sass"

open Fable.React
open Fable.React.Props

type Page =
  | Home 
  | Counter
  | About
  | MachineDashboard

type Model = {
  machines: Machine list
  selectedMachine: Machine option
  
}

