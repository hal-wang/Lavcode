import { Inject } from "@ipare/inject";
import { Action } from "@ipare/router";
import { V } from "@ipare/validator";
import { GetFolderDto } from "./dtos/get-folder.dto";
import { FolderService } from "./services/folder.service";

@V()
  .Tags("folder")
  .Description("Get all folders")
  .Response(200, [GetFolderDto])
  .ResponseDescription(200, "success")
  .Security({
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
