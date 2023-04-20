import { LambdaStartup } from "@halsp/lambda";
import startup from "./startup";

const app = startup(new LambdaStartup());
export const main = (e: any, c: any) => app.run(e, c);
