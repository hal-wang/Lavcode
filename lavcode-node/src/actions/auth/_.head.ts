import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiSecurity,
  ApiTags,
} from "@ipare/swagger";

@ApiTags("auth")
@ApiDescription("Verify token")
@ApiResponses({
  "204": {
    description: "success",
  },
})
@ApiSecurity({
  Bearer: [],
})
export default class extends Action {
  async invoke() {
    this.noContent();
  }
}
