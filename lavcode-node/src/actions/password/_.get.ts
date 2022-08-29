import { Inject } from "@ipare/inject";
import { Query } from "@ipare/pipe";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiSecurity,
  ApiTags,
  DtoDescription,
} from "@ipare/swagger";
import { IconEntity } from "../../entities/icon.entity";
import { KeyValuePairEntity } from "../../entities/key-value-pair.entity";
import { PasswordEntity } from "../../entities/password-entity";
import { CbappService } from "../../services/cbapp.service";
import { CollectionService } from "../../services/collection.service";
import { GetPasswordDto } from "./dtos/get-password.dto";

@ApiTags("password")
@ApiDescription("Get passwords")
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
  private readonly cbappService!: CbappService;

  @Query("folderId")
  @DtoDescription("Empty value for all passwords")
  private readonly folderId!: string;

  async invoke() {
    const $ = this.cbappService.db.command.aggregate;

    let query = this.collectionService.password.aggregate();
    if (this.folderId) {
      query = query.match({
        folderId: this.folderId,
      });
    }

    const res = await query
      .lookup({
        from: this.collectionService.icon.name,
        localField: "_id",
        foreignField: "_id",
        as: "icons",
      })
      .addFields({
        icon: $.arrayElemAt(["$icons", 0]),
      })
      .project({
        icons: 0,
      })
      .lookup({
        from: this.collectionService.keyValuePair.name,
        localField: "_id",
        foreignField: "sourceId",
        as: "keyValuePairs",
      })
      .end();
    const passwordEntities = res.data as (PasswordEntity & {
      icon: IconEntity;
      keyValuePairs: KeyValuePairEntity[];
    })[];
    const passwords = passwordEntities.map((f) =>
      GetPasswordDto.fromEntity(f, f.icon, f.keyValuePairs)
    );
    this.ok(passwords);
  }
}
