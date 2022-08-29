import { Inject } from "@ipare/inject";
import { Body } from "@ipare/pipe";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiSecurity,
  ApiTags,
} from "@ipare/swagger";
import { KeyValuePairEntity } from "../../entities/key-value-pair.entity";
import { PasswordEntity } from "../../entities/password-entity";
import { CollectionService } from "../../services/collection.service";
import { DbhelperService } from "../../services/dbhelper.service";
import { CreatePasswordDto } from "./dtos/craete-password.dto";
import { GetPasswordDto } from "./dtos/get-password.dto";

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
  private readonly dbHelperService!: DbhelperService;

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

    const password: PasswordEntity = await this.dbHelperService.add(
      this.collectionService.password,
      {
        folderId: this.password.folderId,
        title: this.password.title,
        value: this.password.value,
        remark: this.password.remark,
        order: order + 1,
        updatedAt: new Date().valueOf(),
      }
    );
    const icon = await this.dbHelperService.add(this.collectionService.icon, {
      _id: password._id,
      iconType: this.password.icon.iconType,
      value: this.password.icon.value,
    });
    await this.collectionService.password.add(
      this.password.keyValuePairs.map((item) => ({
        sourceId: password._id,
        key: item.key,
        value: item.value,
      }))
    );
    const kvpsRes = await this.collectionService.keyValuePair
      .where({
        sourceId: password._id,
      })
      .get();
    const kvps = kvpsRes.data as KeyValuePairEntity[];
    this.ok(GetPasswordDto.fromEntity(password, icon, kvps));
  }
}
