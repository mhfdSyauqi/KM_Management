<script setup>
import { QuillEditor } from '@vueup/vue-quill'
import '@vueup/vue-quill/dist/vue-quill.snow.css'

const descOption = {
  theme: 'snow',
  placeholder: 'Please write here...',
  modules: {
    toolbar: false
  }
}
</script>

<template>
  <div class="flex flex-col w-full gap-1">
    <label class="text-gray-400">Description<sup class="text-red-500">*</sup></label>
    <div>
      <QuillEditor
        ref="editor"
        :options="descOption"
        :style="{ minHeight: '160px', 'border-radius': '15px' }"
      />
    </div>
  </div>
</template>

<style scoped></style>