import { Inject } from "@ipare/inject";
import { Param } from "@ipare/pipe";
import { Action } from "@ipare/router";
import { ApiDescription, ApiResponses, ApiTags } from "@ipare/swagger";
import { CollectionService } from "../../services/collection.service";

@ApiTags("folder")
@ApiDescription("Delete folder")
@ApiResponses({
  "204": {
    description: "success",
  },
})
export default class extends Action {
  @Inject
  private readonly collectionService!: CollectionService;

  @Param("id")
  private readonly folderId!: string;

  async invoke() {
    await this.collectionService.folder
      .where({
        _id: this.folderId,
      })
      .remove();
    this.noContent();
  }
}
