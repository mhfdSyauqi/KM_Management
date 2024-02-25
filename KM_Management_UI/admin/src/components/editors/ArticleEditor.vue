<script setup>
import { QuillEditor, Quill } from '@vueup/vue-quill'
import BlotFormatter, {
  AlignAction,
  DeleteAction,
  ResizeAction,
  ImageSpec
} from 'quill-blot-formatter'
import '@vueup/vue-quill/dist/vue-quill.snow.css'

Quill.register('modules/blotFormatter', BlotFormatter)

class CustomImageSpec extends ImageSpec {
  getActions() {
    return [AlignAction, DeleteAction, ResizeAction]
  }
}

const articleOption = {
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
      'clean',
      'link',
      'image'
    ],
    blotFormatter: {
      specs: [CustomImageSpec],
      overlay: {
        style: {
          border: '2px solid red'
        }
      }
    }
  }
}

const model = defineModel('modelValue')
const prop = defineProps({
  error: Object
})
</script>

<template>
  <div class="flex flex-col w-full gap-1">
    <label class="text-gray-400">
      Article
      <sup class="text-red-500"> * {{ prop.error?.isError ? prop.error?.message : null }} </sup>
    </label>
    <div>
      <QuillEditor v-model:content="model" :options="articleOption" class="min-h-56" />
    </div>
  </div>
</template>

<style scoped></style>