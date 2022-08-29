import { Inject } from "@ipare/inject";
import { Body, Param } from "@ipare/pipe";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiSecurity,
  ApiTags,
} from "@ipare/swagger";
import { IconEntity } from "../../entities/icon.entity";
import { KeyValuePairEntity } from "../../entities/key-value-pair.entity";
import { PasswordEntity } from "../../entities/password-entity";
import { CollectionService } from "../../services/collection.service";
import { DbhelperService } from "../../services/dbhelper.service";
import { GetPasswordDto } from "./dtos/get-password.dto";
import { UpdatePasswordDto } from "./dtos/update-password.dto";

@ApiTags("password")
@ApiDescription("Update password")
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
  private readonly dbHelperService!: DbhelperService;

  @Param("id")
  private readonly passwordId!: string;
  @Body
  private readonly password!: UpdatePasswordDto;

  async invoke() {
    const password: PasswordEntity = await this.dbHelperService.update(
      this.collectionService.password,
      this.passwordId,
      {
        folderId: this.password.folderId,
        title: this.password.title,
        value: this.password.value,
        remark: this.password.remark,
        order: this.password.order,
        updatedAt: new Date().valueOf(),
      }
    );
    let icon: IconEntity | undefined = undefined;
    if (this.password.icon) {
      icon = await this.dbHelperService.update(
        this.collectionService.icon,
        this.passwordId,
        {
          iconType: this.password.icon.iconType,
          value: this.password.icon.value,
        }
      );
    } else {
      icon = await this.dbHelperService.getOne(
        this.collectionService.icon,
        this.passwordId
      );
    }

    if (this.password.keyValuePairs) {
      await this.collectionService.keyValuePair
        .where({
          sourceId: this.passwordId,
        })
        .remove();
      await this.collectionService.password.add(
        this.password.keyValuePairs.map((item) => ({
          sourceId: password._id,
          key: item.key,
          value: item.value,
        }))
      );
    }
    const kvpsRes = await this.collectionService.keyValuePair
      .where({
        sourceId: password._id,
      })
      .get();
    const kvps = kvpsRes.data as KeyValuePairEntity[];
    this.ok(GetPasswordDto.fromEntity(password, icon, kvps));
  }
}
