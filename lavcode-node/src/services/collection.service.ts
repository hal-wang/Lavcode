import { Database } from "@cloudbase/node-sdk";
import { Inject } from "@ipare/inject";
import { CbappService } from "./cbapp.service";

export class CollectionService {
  @Inject
  private readonly cbappService!: CbappService;

  private getCollection(collection: string): Database.CollectionReference {
    return this.cbappService.db.collection(collection);
  }

  get folder(): Database.CollectionReference {
    return this.getCollection("folder");
  }
  get password(): Database.CollectionReference {
    return this.getCollection("password");
  }
  get icon(): Database.CollectionReference {
    return this.getCollection("icon");
  }
  get keyValuePair(): Database.CollectionReference {
    return this.getCollection("keyValuePair");
  }
}
