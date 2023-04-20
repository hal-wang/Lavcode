import { Action } from "@halsp/router";
import { V } from "@halsp/validator";

@V()
  .Tags("auth")
  .Summary("Verify token")
  .ResponseDescription(204, "success")
  .Security({
    Bearer: [],
  })
export default class extends Action {
  async invoke() {
    this.noContent();
  }
}
