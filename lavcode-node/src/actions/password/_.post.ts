import { Inject } from "@ipare/inject";
import { Body } from "@ipare/pipe";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiSecurity,
  ApiTags,
} from "@ipare/swagger";
import { CollectionService } from "../../services/collection.service";
import { CreatePasswordDto } from "./dtos/craete-password.dto";
import { PasswordService } from "./services/password.service";

@ApiTags("password")
@ApiDescription("Create password")
@ApiResponses({
  "200": {
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
            sourceId: passwordId,
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
