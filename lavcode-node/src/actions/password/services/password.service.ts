import { Inject } from "@ipare/inject";
import { IconEntity } from "../../../entities/icon.entity";
import { KeyValuePairEntity } from "../../../entities/key-value-pair.entity";
import { PasswordEntity } from "../../../entities/password-entity";
import { CbappService } from "../../../services/cbapp.service";
import { CollectionService } from "../../../services/collection.service";
import { GetPasswordDto } from "../dtos/get-password.dto";

export class PasswordService {
  @Inject
  private readonly collectionService!: CollectionService;
  @Inject
  private readonly cbappService!: CbappService;

  async getPasswords(match?: any) {
    const $ = this.cbappService.db.command.aggregate;

    let query = this.collectionService.password.aggregate();
    if (match) {
      query = query.match(match);
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
        foreignField: "passwordId",
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
    return passwords;
  }
}
