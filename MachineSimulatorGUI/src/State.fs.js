import { oneOf, s, map } from "./fable_modules/Fable.Elmish.UrlParser.1.0.1/parser.fs.js";
import { toHash, Page } from "./Global.fs.js";
import { empty, ofArray } from "./fable_modules/fable-library.4.1.4/List.js";
import { Msg, Model } from "./Types.fs.js";
import { some } from "./fable_modules/fable-library.4.1.4/Option.js";
import { Navigation_modifyUrl } from "./fable_modules/Fable.Elmish.Browser.4.0.1/navigation.fs.js";
import { update as update_2, init as init_1 } from "./Counter/State.fs.js";
import { update as update_1, init as init_2 } from "./Home/State.fs.js";
import { Cmd_map, Cmd_batch } from "./fable_modules/Fable.Elmish.4.0.1/cmd.fs.js";

export const pageParser = (() => {
    const parsers = ofArray([map(new Page(2, []), s("about")), map(new Page(1, []), s("counter")), map(new Page(0, []), s("home"))]);
    return (state) => oneOf(parsers, state);
})();

export function urlUpdate(result, model) {
    if (result != null) {
        const page = result;
        return [new Model(page, model.Counter, model.Home), empty()];
    }
    else {
        console.error(some("Error parsing url"));
        return [model, Navigation_modifyUrl(toHash(model.CurrentPage))];
    }
}

export function init(result) {
    const patternInput = init_1();
    const counterCmd = patternInput[1];
    const counter = patternInput[0] | 0;
    const patternInput_1 = init_2();
    const homeCmd = patternInput_1[1];
    const home = patternInput_1[0];
    const patternInput_2 = urlUpdate(result, new Model(new Page(0, []), counter, home));
    const model = patternInput_2[0];
    const cmd = patternInput_2[1];
    return [model, Cmd_batch(ofArray([cmd, Cmd_map((arg) => (new Msg(0, [arg])), counterCmd), Cmd_map((arg_1) => (new Msg(1, [arg_1])), homeCmd)]))];
}

export function update(msg, model) {
    if (msg.tag === 1) {
        const msg_2 = msg.fields[0];
        const patternInput_1 = update_1(msg_2, model.Home);
        const homeCmd = patternInput_1[1];
        const home = patternInput_1[0];
        return [new Model(model.CurrentPage, model.Counter, home), Cmd_map((arg_1) => (new Msg(1, [arg_1])), homeCmd)];
    }
    else {
        const msg_1 = msg.fields[0];
        const patternInput = update_2(msg_1, model.Counter);
        const counterCmd = patternInput[1];
        const counter = patternInput[0] | 0;
        return [new Model(model.CurrentPage, counter, model.Home), Cmd_map((arg) => (new Msg(0, [arg])), counterCmd)];
    }
}

//# sourceMappingURL=State.fs.js.map
