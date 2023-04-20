import { Inject } from "@halsp/inject";
import { Body } from "@halsp/pipe";
import { Action } from "@halsp/router";
import { V } from "@halsp/validator";
import { CollectionService } from "../../services/collection.service";
import { CreatePasswordDto } from "./dtos/craete-password.dto";
import { GetPasswordDto } from "./dtos/get-password.dto";
import { PasswordService } from "./services/password.service";

@V()
  .Tags("password")
  .Summary("Create password")
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

  @Body
  private readonly password!: CreatePasswordDto;

  async invoke() {
    const orderRes = await this.collectionService.password
      .where({
        folderId: this.password.folderId,
      })
      .orderBy("order", "desc")
      .field({
        order: true,
      })
      .limit(1)
      .get();
    const order = orderRes.data[0]?.order ?? 0;

    let passwordId: string | undefined;
    const transaction = await this.collectionService.startTransaction();
    try {
      const addRes = await transaction.passwordCollection.add({
        folderId: this.password.folderId,
        title: this.password.title,
        value: this.password.value,
        remark: this.password.remark,
        order: order + 1,
        updatedAt: new Date().valueOf(),
      });
      passwordId = addRes.id;
      await transaction.iconCollection.add({
        _id: passwordId,
        iconType: this.password.icon.iconType,
        value: this.password.icon.value,
      });
      if (this.password.keyValuePairs.length) {
        await transaction.keyValuePairCollection.add(
          this.password.keyValuePairs.map((item) => ({
            passwordId,
            key: item.key,
            value: item.value,
          }))
        );
      }
      await transaction.commit();
    } catch {
      await transaction.rollback({});
      this.internalServerErrorMsg("创建失败");
      return;
    }

    const passwords = await this.passwordService.getPasswords({
      _id: passwordId,
    });
    this.ok(passwords[0]);
  }
}
