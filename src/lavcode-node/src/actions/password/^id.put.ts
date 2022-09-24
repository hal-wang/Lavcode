import { Inject } from "@ipare/inject";
import { Body, Param } from "@ipare/pipe";
import { Action } from "@ipare/router";
import { V } from "@ipare/validator";
import { CollectionService } from "../../services/collection.service";
import { GetPasswordDto } from "./dtos/get-password.dto";
import { UpdatePasswordDto } from "./dtos/update-password.dto";
import { PasswordService } from "./services/password.service";

@V()
  .Tags("password")
  .Summary("Update password")
  .Response(200, GetPasswordDto)
  .ResponseDescription(200, "success")
  .Security({
    Bearer: [],
  })
export default class extends Action {
  @Inject
  private readonly collectionService!: CollectionService;
  @Inject
  private readonly passwordService!: PasswordService;

  @Param("id")
  private readonly passwordId!: string;
  @Body
  private readonly password!: UpdatePasswordDto;

  async invoke() {
    const transaction = await this.collectionService.startTransaction();
    try {
      await transaction.passwordCollection.doc(this.passwordId).update({
        folderId: this.password.folderId,
        title: this.password.title,
        value: this.password.value,
        remark: this.password.remark,
        order: this.password.order,
        updatedAt: new Date().valueOf(),
      });
      if (this.password.icon) {
        await transaction.iconCollection.doc(this.passwordId).update({
          iconType: this.password.icon.iconType,
          value: this.password.icon.value,
        });
      }

      if (this.password.keyValuePairs) {
        await transaction.keyValuePairCollection
          .where({
            passwordId: this.passwordId,
          })
          .remove();
        await transaction.keyValuePairCollection.add(
          this.password.keyValuePairs.map((item) => ({
            passwordId: this.passwordId,
            key: item.key,
            value: item.value,
          }))
        );
      }
      await transaction.commit();
    } catch {
      await transaction.rollback({});
      this.internalServerErrorMsg("更新失败");
      return;
    }

    const passwords = await this.passwordService.getPasswords({
      _id: this.passwordId,
    });
    this.ok(passwords[0]);
  }
}
