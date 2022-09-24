import tcb = require("@cloudbase/node-sdk");

export class CbappService {
  public readonly app = tcb.init({
    env: process.env.ENV_ID,
    secretId: process.env.TENCENT_SECRET_ID,
    secretKey: process.env.TENCENT_SECRET_KEY,
  });
  public readonly db = this.app.database();
}
