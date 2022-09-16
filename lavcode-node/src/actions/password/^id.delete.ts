import { Inject } from "@ipare/inject";
import { Param } from "@ipare/pipe";
import { Action } from "@ipare/router";
import { V } from "@ipare/validator";
import { CollectionService } from "../../services/collection.service";

@V()
  .Tags("password")
  .Summary("Delete a password")
  .ResponseDescription(204, "success")
  .Security({
    Bearer: [],
  })
export default class extends Action {
  @Inject
  private readonly collectionService!: CollectionService;

  @Param("id")
  private readonly passwordId!: string;

  async invoke() {
    const transaction = await this.collectionService.startTransaction();
    try {
      await transaction.iconCollection
        .where({
          _id: this.passwordId,
        })
        .remove();
      await transaction.keyValuePairCollection
        .where({
          passwordId: this.passwordId,
        })
        .remove();
      await transaction.passwordCollection
        .where({
          _id: this.passwordId,
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
