<script setup>
import { QuillEditor } from '@vueup/vue-quill'
import '@vueup/vue-quill/dist/vue-quill.snow.css'
import { ref } from 'vue'
const model = defineModel('modelValue')
const prop = defineProps({
  error: Object
})
const editor = ref(null)

const descOption = {
  theme: 'snow',
  placeholder: 'Please write here...',
  modules: {
    toolbar: false
  }
}
function onInput() {
  model.value = editor.value.getHTML()
}
</script>

<template>
  <div class="flex flex-col w-full gap-1">
    <label class="text-gray-400">
      Description
      <sup class="text-red-500"> * {{ prop.error?.isError ? prop.error?.message : null }} </sup>
    </label>
    <div>
      <QuillEditor
        ref="editor"
        @update:content="onInput"
        :options="descOption"
        :style="{ minHeight: '160px', 'border-radius': '15px' }"
      />
    </div>
  </div>
</template>

<style scoped></style>