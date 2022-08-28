import { Inject } from "@ipare/inject";
import { Body, Param } from "@ipare/pipe";
import { Action } from "@ipare/router";
import { ApiDescription, ApiResponses, ApiTags } from "@ipare/swagger";
import { FolderEntity } from "../../entities/folder.entity";
import { CollectionService } from "../../services/collection.service";
import { DbhelperService } from "../../services/dbhelper.service";
import { UpdateFolderDto } from "./dtos/update-folder.dto";

@ApiTags("folder")
@ApiDescription("Edit folder")
@ApiResponses({
  "204": {
    description: "success",
  },
})
export default class extends Action {
  @Inject
  private readonly collectionService!: CollectionService;
  @Inject
  private readonly dbHelperService!: DbhelperService;

  @Param("id")
  private readonly folderId!: string;

  @Body
  private readonly folder!: UpdateFolderDto;

  async invoke() {
    const orderRes = await this.collectionService.folder
      .orderBy("order", "desc")
      .field({
        order: true,
      })
      .limit(1)
      .get();
    const order = orderRes.data[0]?.order ?? 0;

    const folder: FolderEntity = await this.dbHelperService.update(
      this.collectionService.folder,
      this.folderId,
      {
        name: this.folder.name,
        order: order,
        lastEditTime: new Date().valueOf(),
      }
    );
    let icon: FolderEntity | undefined = undefined;
    if (this.folder.icon) {
      icon = await this.dbHelperService.update(
        this.collectionService.icon,
        this.folderId,
        {
          iconType: this.folder.icon.iconType,
          value: this.folder.icon.value,
        }
      );
    } else {
      icon = await this.dbHelperService.getOne(
        this.collectionService.icon,
        this.folderId
      );
    }
    this.ok({
      folder,
      icon,
    });
  }
}
