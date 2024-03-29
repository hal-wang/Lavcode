import { Inject } from "@halsp/inject";
import { Action } from "@halsp/router";
import { V } from "@halsp/validator";
import { GetFolderDto } from "./dtos/get-folder.dto";
import { FolderService } from "./services/folder.service";

@V()
  .Tags("folder")
  .Summary("Get all folders")
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
