import { Database } from "@cloudbase/node-sdk";
import { PageListDto } from "../dtos/page-list.dto";
import { PageParamsDto } from "../dtos/page-params.dto";

export class DbhelperService {
  async getOne<T = any>(
    collection: Database.CollectionReference,
    doc: string
  ): Promise<T | undefined> {
    if (!doc) return;

    const res = await collection.doc(doc).get();
    if (!res.data || !res.data.length) return;
    return res.data[0] as T;
  }

  async getScalar<T = any>(
    collection: Database.CollectionReference,
    doc: string,
    field: string
  ): Promise<T | undefined> {
    if (!doc || !field) return;

    const fieldObj: Record<string, unknown> = {};
    fieldObj[field] = true;

    const res = await collection.doc(doc).field(fieldObj).get();
    if (!res.data || !res.data.length) return;
    return res.data[0][field] as T;
  }

  async updateScalar(
    collection: Database.CollectionReference,
    doc: string,
    field: string,
    value: unknown
  ): Promise<number | undefined> {
    const fieldObj: Record<string, unknown> = {};
    fieldObj[field] = value;

    const res = await collection.doc(doc).update(fieldObj);
    return res.updated;
  }

  /**
   * @param params data: { page, limit }
   * @param partQuery part of query, like where(...)
   */
  async getPageList<T = any>(
    params: PageParamsDto,
    partQuery: () =>
      | Database.Query
      | Database.CollectionReference
      | Database.Aggregation
  ): Promise<PageListDto<T>> {
    const limit = Number(params.limit ?? 20) ?? 20;
    const page = Number(params.page ?? 1) ?? 1;

    const countQ = partQuery();
    let total: number | undefined;
    if (!!(countQ as Database.Aggregation).end) {
      const apq = countQ as Database.Aggregation;
      total = (await apq.count("count").end()).data[0]?.count ?? 0;
    } else {
      const qpq = countQ as Database.Query;
      total = (await qpq.count()).total;
    }
    total = total ?? 0;

    const listQ = partQuery();
    let list: T[];
    if (!!(listQ as Database.Aggregation).end) {
      const apq = listQ as Database.Aggregation;
      list = (
        await apq
          .skip((page - 1) * limit)
          .limit(limit)
          .end()
      ).data;
    } else {
      const qpq = listQ as Database.Query;
      list = (
        await qpq
          .skip((page - 1) * limit)
          .limit(limit)
          .get()
      ).data;
    }

    return {
      list,
      total,
      limit,
      page,
    };
  }

  async add<T extends object = any, K = T>(
    collection: Database.CollectionReference,
    obj: T
  ): Promise<K> {
    const addRes = await collection.add(obj);
    const res = await collection.doc(addRes.id as string).get();
    return res.data[0] as K;
  }

  async set<T extends object = any, K = any>(
    collection: Database.CollectionReference,
    id: string,
    obj: T
  ): Promise<K> {
    await collection.doc(id).set(obj);
    const res = await collection.doc(id).get();
    return res.data[0] as K;
  }

  async update<T extends object = any, K = any>(
    collection: Database.CollectionReference,
    id: string,
    obj: T
  ): Promise<K> {
    await collection.doc(id).update(obj);
    const res = await collection.doc(id).get();
    return res.data[0] as K;
  }
}
