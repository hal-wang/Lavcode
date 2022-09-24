import { Inject } from "@ipare/inject";
import { FolderEntity } from "../../../entities/folder.entity";
import { IconEntity } from "../../../entities/icon.entity";
import { CbappService } from "../../../services/cbapp.service";
import { CollectionService } from "../../../services/collection.service";
import { GetFolderDto } from "../dtos/get-folder.dto";

export class FolderService {
  @Inject
  private readonly collectionService!: CollectionService;
  @Inject
  private readonly cbappService!: CbappService;

  async getFolders(match?: any) {
    const $ = this.cbappService.db.command.aggregate;

    let query = this.collectionService.folder.aggregate();
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
      .sort({
        order: 1,
      })
      .end();
    const folderEntities = res.data as (FolderEntity & {
      icon: IconEntity;
    })[];

    return folderEntities.map((f) => GetFolderDto.fromEntity(f, f.icon));
  }
}
