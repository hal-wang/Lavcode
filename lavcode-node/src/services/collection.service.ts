import { Database } from "@cloudbase/node-sdk";
import { Inject } from "@ipare/inject";
import { CbappService } from "./cbapp.service";

declare module "@cloudbase/node-sdk" {
  // eslint-disable-next-line @typescript-eslint/no-namespace
  namespace Database {
    interface Transaction {
      get folderCollection(): Database.CollectionReference;
      get passwordCollection(): Database.CollectionReference;
      get iconCollection(): Database.CollectionReference;
      get keyValuePairCollection(): Database.CollectionReference;
    }
  }
}

export const collections = {
  folder: "folder",
  password: "password",
  icon: "icon",
  keyValuePair: "keyValuePair",
};

export class CollectionService {
  @Inject
  private readonly cbappService!: CbappService;

  private getCollection(collection: string): Database.CollectionReference {
    return this.cbappService.db.collection(collection);
  }

  get folder(): Database.CollectionReference {
    return this.getCollection(collections.folder);
  }

  get password(): Database.CollectionReference {
    return this.getCollection(collections.password);
  }

  get icon(): Database.CollectionReference {
    return this.getCollection(collections.icon);
  }

  get keyValuePair(): Database.CollectionReference {
    return this.getCollection(collections.keyValuePair);
  }

  public async startTransaction() {
    const transaction = await this.cbappService.db.startTransaction();
    Object.keys(collections).forEach((key) => {
      Object.defineProperty(transaction, `${key}Collection`, {
        get: () => transaction.collection(collections[key]),
      });
    });
    return transaction;
  }
}
