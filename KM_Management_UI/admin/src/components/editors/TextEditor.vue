<script setup>
import { onBeforeUnmount, ref, watchEffect } from 'vue'
import { QuillEditor } from '@vueup/vue-quill'
import '@vueup/vue-quill/dist/vue-quill.snow.css'

const model = defineModel()
const htmlModel = defineModel('html')
const prop = defineProps({
  error: Object,
  oldInput: {
    required: false
  }
})
const editor = ref(null)

const descOption = {
  theme: 'snow',
  placeholder: 'Please write here...',
  modules: {
    toolbar: [
      { header: [1, 2, 3, 4, 5, 6, false] },
      'bold',
      'italic',
      'underline',
      { list: 'ordered' },
      { list: 'bullet' },
      { align: [] },
      'blockquote',
      { color: [] },
      'clean'
    ]
  }
}

let isOutdated = ref(true)
onBeforeUnmount(() => {
  isOutdated.value = true
})

function onReady(quill) {
  watchEffect(() => {
    if (prop.oldInput && isOutdated.value) {
      quill.setContents(prop.oldInput, 'api')
      htmlModel.value = editor.value.getHTML()
      isOutdated.value = false
    }
  })
}

function onInput(delta) {
  if (delta.ops.length === 1 && delta.ops[0].insert.trim() === '') {
    htmlModel.value = ''
    model.value = ''
  }
  htmlModel.value = editor.value.getHTML()
}
</script>

<template>
  <div class="flex flex-col w-full gap-1">
    <label class="text-gray-400">
      Email Content
      <sup class="text-red-500"> * {{ prop.error?.isError ? prop.error?.message : null }} </sup>
    </label>
    <div>
      <QuillEditor
        ref="editor"
        @ready="onReady"
        v-model:content="model"
        @update:content="onInput"
        :options="descOption"
        :style="{ minHeight: '160px' }"
      />
    </div>
  </div>
</template>

<style scoped></style>