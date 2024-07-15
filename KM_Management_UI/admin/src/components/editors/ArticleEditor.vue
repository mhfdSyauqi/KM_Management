<script setup>
import { QuillEditor, Quill } from '@vueup/vue-quill'
import BlotFormatter from 'quill-blot-formatter/dist/BlotFormatter.js'
import ImageSpec from 'quill-blot-formatter/dist/specs/ImageSpec.js'
import DeleteAction from 'quill-blot-formatter/dist/actions/DeleteAction.js'
import AlignAction from 'quill-blot-formatter/dist/actions/align/AlignAction.js'
import ResizeAction from 'quill-blot-formatter/dist/actions/ResizeAction.js'
import '@vueup/vue-quill/dist/vue-quill.snow.css'
import { onBeforeUnmount, ref, watchEffect } from 'vue'

Quill.register('modules/blotFormatter', BlotFormatter)

const BaseImageFormat = Quill.import('formats/image')
const ImageFormatAttributesList = ['alt', 'height', 'width', 'style']

class ImageFormat extends BaseImageFormat {
  static formats(domNode) {
    return ImageFormatAttributesList.reduce(function (formats, attribute) {
      if (domNode.hasAttribute(attribute)) {
        formats[attribute] = domNode.getAttribute(attribute)
      }
      return formats
    }, {})
  }
  format(name, value) {
    if (ImageFormatAttributesList.indexOf(name) > -1) {
      if (value) {
        this.domNode.setAttribute(name, value)
      } else {
        this.domNode.removeAttribute(name)
      }
    } else {
      super.format(name, value)
    }
  }
}
Quill.register(ImageFormat, true)

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

const model = defineModel()
const prop = defineProps({
  error: Object,
  oldInput: {
    required: false
  }
})

let isOutdated = ref(true)
onBeforeUnmount(() => {
  isOutdated.value = true
})

function onReady(quill) {
  watchEffect(() => {
    if (prop.oldInput && isOutdated.value) {
      quill.setContents(prop.oldInput, 'api')
      isOutdated.value = false
    }
  })
}

function onInput(delta) {
  if (delta.ops.length === 1 && delta.ops[0].insert.trim() === '') {
    model.value = ''
  }
}
</script>

<template>
  <div class="flex flex-col w-full gap-1">
    <label class="text-gray-400">
      Article
      <sup class="text-red-500"> * {{ prop.error?.isError ? prop.error?.message : null }} </sup>
    </label>
    <div>
      <QuillEditor
        @ready="onReady"
        v-model:content="model"
        @update:content="onInput"
        :options="articleOption"
        class="min-h-56"
      />
    </div>
  </div>
</template>

<style scoped></style>