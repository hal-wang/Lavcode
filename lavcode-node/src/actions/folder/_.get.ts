import { Inject } from "@ipare/inject";
import { Action } from "@ipare/router";
import {
  ApiDescription,
  ApiResponses,
  ApiSecurity,
  ApiTags,
} from "@ipare/swagger";
import { FolderService } from "./services/folder.service";

@ApiTags("folder")
@ApiDescription("Get all folders")
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
  private readonly folderService!: FolderService;

  async invoke() {
    const folders = await this.folderService.getFolders();
    this.ok(folders);
  }
}
