import { Inject } from "@ipare/inject";
import { Body, Param } from "@ipare/pipe";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiSecurity,
  ApiTags,
} from "@ipare/swagger";
import { FolderEntity } from "../../entities/folder.entity";
import { IconEntity } from "../../entities/icon.entity";
import { CollectionService } from "../../services/collection.service";
import { DbhelperService } from "../../services/dbhelper.service";
import { GetFolderDto } from "./dtos/get-folder.dto";
import { UpdateFolderDto } from "./dtos/update-folder.dto";

@ApiTags("folder")
@ApiDescription("Update folder")
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
  @Inject
  private readonly dbHelperService!: DbhelperService;

  @Param("id")
  private readonly folderId!: string;

  @Body
  private readonly folder!: UpdateFolderDto;

  async invoke() {
    const folder: FolderEntity = await this.dbHelperService.update(
      this.collectionService.folder,
      this.folderId,
      {
        name: this.folder.name,
        order: this.folder.order,
        updatedAt: new Date().valueOf(),
      }
    );
    let icon: IconEntity | undefined = undefined;
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
    this.ok(GetFolderDto.fromEntity(folder, icon));
  }
}
