import { defineAsyncComponent } from 'vue'

export default {
  enhance: ({ app }) => {    
      app.component("Donate", defineAsyncComponent(() => import("C:/dev/h/Lavcode/lavcode-docs/docs/components/Donate.vue")))
  },
}
