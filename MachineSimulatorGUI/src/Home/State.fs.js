import { empty } from "../fable_modules/fable-library.4.1.4/List.js";

export function init() {
    return ["", empty()];
}

export function update(msg, model) {
    const str = msg.fields[0];
    return [str, empty()];
}

//# sourceMappingURL=State.fs.js.map
