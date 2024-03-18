<script setup>
import { ref, watchEffect } from 'vue'
import { QuillEditor } from '@vueup/vue-quill'
import '@vueup/vue-quill/dist/vue-quill.snow.css'

const prop = defineProps({
  article: {
    required: true
  }
})
const editor = ref(null)

const descOption = {
  theme: 'snow',
  readOnly: true,
  modules: {
    toolbar: false
  }
}
function onReady(quill) {
  watchEffect(() => {
    if (prop.article) {
      quill.setContents(prop.article, 'api')
    }
  })
}
</script>

<template>
  <div class="mx-auto flex flex-col w-[95%]">
    <QuillEditor class="!font-roboto p-3" ref="editor" @ready="onReady" :options="descOption" />
  </div>
</template>

<style scoped></style>