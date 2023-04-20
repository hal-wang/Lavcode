import { NativeStartup } from "@halsp/native";
import startup from "./startup";

startup(new NativeStartup().useHttpJsonBody()).dynamicListen();
