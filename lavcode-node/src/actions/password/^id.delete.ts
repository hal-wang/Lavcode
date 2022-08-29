import { Inject } from "@ipare/inject";
import { Param } from "@ipare/pipe";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiSecurity,
  ApiTags,
} from "@ipare/swagger";
import { CollectionService } from "../../services/collection.service";

@ApiTags("password")
@ApiDescription("Delete a password")
@ApiResponses({
  "204": {
    description: "success",
  },
})
@ApiSecurity({
  Bearer: [],
})
export default class extends Action {
  @Inject
  private readonly collectionService!: CollectionService;

  @Param("id")
  private readonly passwordId!: string;

  async invoke() {
    await this.collectionService.password
      .where({
        _id: this.passwordId,
      })
      .remove();
    this.noContent();
  }
}
