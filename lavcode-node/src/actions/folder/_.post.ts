import { Inject } from "@ipare/inject";
import { Body } from "@ipare/pipe";
import { Action } from "@ipare/router";
import { ApiDescription, ApiResponses, ApiTags } from "@ipare/swagger";
import { FolderEntity } from "../../entities/folder.entity";
import { CollectionService } from "../../services/collection.service";
import { DbhelperService } from "../../services/dbhelper.service";
import { CreateFolderDto } from "./dtos/create-folder.dto";

@ApiTags("folder")
@ApiDescription("Create folder")
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

  @Body
  private readonly folder!: CreateFolderDto;

  async invoke() {
    const orderRes = await this.collectionService.folder
      .orderBy("order", "desc")
      .field({
        order: true,
      })
      .limit(1)
      .get();
    const order = orderRes.data[0]?.order ?? 0;

    const folder: FolderEntity = await this.dbHelperService.add(
      this.collectionService.folder,
      {
        name: this.folder.name,
        order: order,
        lastEditTime: new Date().valueOf(),
      }
    );
    const icon = await this.dbHelperService.add(this.collectionService.icon, {
      _id: folder._id,
      iconType: this.folder.icon.iconType,
      value: this.folder.icon.value,
    });
    this.ok({
      folder,
      icon,
    });
  }
}
