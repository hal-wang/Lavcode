import { Inject } from "@ipare/inject";
import { Param } from "@ipare/pipe";
import { Action } from "@ipare/router";
import { ApiDescription, ApiResponses, ApiTags } from "@ipare/swagger";
import { IconEntity } from "../../entities/icon.entity";
import { CollectionService } from "../../services/collection.service";
import { DbhelperService } from "../../services/dbhelper.service";
import { GetIconDto } from "./dtos/get-icon.dto";

@ApiTags("icon")
@ApiDescription("Get one icon")
@ApiResponses({
  "200": {
    description: "success",
  },
})
export default class extends Action {
  @Inject
  private readonly collectionService!: CollectionService;
  @Inject
  private readonly dbHelperService!: DbhelperService;

  @Param("id")
  private readonly iconId!: string;

  async invoke() {
    const icon = await this.dbHelperService.getOne<IconEntity>(
      this.collectionService.icon,
      this.iconId
    );
    if (!icon) {
      this.notFoundMsg("The icon is not exist");
    } else {
      this.ok(new GetIconDto().fromEntity(icon));
    }
  }
}
