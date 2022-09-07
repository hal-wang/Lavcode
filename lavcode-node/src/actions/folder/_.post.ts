import { Inject } from "@ipare/inject";
import { Body } from "@ipare/pipe";
import { Action } from "@ipare/router";
import { V } from "@ipare/validator";
import { CollectionService } from "../../services/collection.service";
import { CreateFolderDto } from "./dtos/create-folder.dto";
import { GetFolderDto } from "./dtos/get-folder.dto";
import { FolderService } from "./services/folder.service";

@V()
  .Tags("folder")
  .Description("Create folder")
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

    let folderId: string | undefined;
    const transaction = await this.collectionService.startTransaction();
    try {
      const folderRes = await transaction.folderCollection.add({
        name: this.folder.name,
        order: order + 1,
        updatedAt: new Date().valueOf(),
      });
      folderId = folderRes.id;
      await transaction.iconCollection.add({
        _id: folderRes.id,
        iconType: this.folder.icon.iconType,
        value: this.folder.icon.value,
      });
      await transaction.commit();
    } catch {
      await transaction.rollback({});
      this.internalServerErrorMsg("创建失败");
      return;
    }

    const folders = await this.folderService.getFolders({
      _id: folderId,
    });
    this.ok(folders[0]);
  }
}
