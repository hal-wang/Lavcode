import { Inject } from "@ipare/inject";
import { Body, Param } from "@ipare/pipe";
import { Action } from "@ipare/router";
import { V } from "@ipare/validator";
import { CollectionService } from "../../services/collection.service";
import { GetFolderDto } from "./dtos/get-folder.dto";
import { UpdateFolderDto } from "./dtos/update-folder.dto";
import { FolderService } from "./services/folder.service";

@V()
  .Tags("folder")
  .Description("Update folder")
  .Response(200, GetFolderDto)
  .ResponseDescription(200, "success")
  .Security({
    Bearer: [],
  })
export default class extends Action {
  @Inject
  private readonly collectionService!: CollectionService;
  @Inject
  private readonly folderService!: FolderService;

  @Param("id")
  private readonly folderId!: string;

  @Body
  private readonly folder!: UpdateFolderDto;

  async invoke() {
    const transaction = await this.collectionService.startTransaction();
    try {
      await transaction.folderCollection.doc(this.folderId).update({
        name: this.folder.name,
        order: this.folder.order,
        updatedAt: new Date().valueOf(),
      });
      if (this.folder.icon) {
        transaction.iconCollection.doc(this.folderId).update({
          iconType: this.folder.icon.iconType,
          value: this.folder.icon.value,
        });
      }
      await transaction.commit();
    } catch {
      await transaction.rollback({});
      this.internalServerErrorMsg("更新失败");
      return;
    }

    const folders = await this.folderService.getFolders({
      _id: this.folderId,
    });
    this.ok(folders[0]);
  }
}
