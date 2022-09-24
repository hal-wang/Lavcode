import { Inject } from "@ipare/inject";
import { Param } from "@ipare/pipe";
import { Action } from "@ipare/router";
import { V } from "@ipare/validator";
import { CbappService } from "../../services/cbapp.service";
import { CollectionService } from "../../services/collection.service";

@V()
  .Tags("folder")
  .Summary("Delete a folder")
  .ResponseDescription(204, "success")
  .Security({
    Bearer: [],
  })
export default class extends Action {
  @Inject
  private readonly collectionService!: CollectionService;
  @Inject
  private readonly cbappService!: CbappService;

  @Param("id")
  private readonly folderId!: string;

  async invoke() {
    const _ = this.cbappService.db.command;
    const passwordIdsRes = await this.collectionService.password
      .where({
        folderId: this.folderId,
      })
      .field({
        _id: true,
      })
      .get();
    const passwordIds = passwordIdsRes.data.map((item) => item._id as string);
    const transaction = await this.collectionService.startTransaction();
    try {
      await transaction.iconCollection
        .where({
          _id: _.in(passwordIds),
        })
        .remove();
      await transaction.keyValuePairCollection
        .where({
          passwordId: _.in(passwordIds),
        })
        .remove();
      await transaction.iconCollection
        .where({
          _id: this.folderId,
        })
        .remove();
      await transaction.passwordCollection
        .where({
          folderId: this.folderId,
        })
        .remove();
      await transaction.folderCollection
        .where({
          _id: this.folderId,
        })
        .remove();
      await transaction.commit();
    } catch {
      await transaction.rollback({});
      this.internalServerErrorMsg("删除失败");
      return;
    }

    this.noContent();
  }
}
