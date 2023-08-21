import { Record, Union } from "./fable_modules/fable-library.4.1.4/Types.js";
import { record_type, obj_type, union_type } from "./fable_modules/fable-library.4.1.4/Reflection.js";
import "../sass/main.sass";


export class Page extends Union {
    constructor(tag, fields) {
        super();
        this.tag = tag;
        this.fields = fields;
    }
    cases() {
        return ["Home", "Counter", "About", "MachineDashboard"];
    }
}

export function Page_$reflection() {
    return union_type("App.View.Page", [], Page, () => [[], [], [], []]);
}

export class Model extends Record {
    constructor(machines, selectedMachine) {
        super();
        this.machines = machines;
        this.selectedMachine = selectedMachine;
    }
}

export function Model_$reflection() {
    return record_type("App.View.Model", [], Model, () => [["machines", obj_type], ["selectedMachine", obj_type]]);
}

//# sourceMappingURL=App.fs.js.map
