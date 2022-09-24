import { Action } from "@ipare/router";
import { V } from "@ipare/validator";

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
