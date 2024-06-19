<script setup>
import { ref } from 'vue'
import IconEyes from '@/components/icons/IconEyes.vue'

const show = ref(false)
const model = defineModel()
const props = defineProps({
  required: {
    type: Boolean,
    default: false
  },
  readonly: {
    type: Boolean,
    default: false
  },
  disabled: {
    type: Boolean,
    default: false
  },
  error: {
    type: Object,
    default(rawProp) {
      return {
        isError: false,
        message: ''
      }
    }
  },
  name: String
})
</script>

<template>
  <div class="flex flex-col w-full gap-1">
    <label :for="props.name" class="text-gray-400">
      {{ props.name }}
      <sup class="text-red-500" v-show="props.required">
        * {{ props.error.isError ? props.error.message : '' }}
      </sup>
    </label>
    <div class="flex w-full">
      <input
        v-model="model"
        :type="show ? 'text' : 'password'"
        :id="props.name"
        :readonly="props.readonly"
        :disabled="props.disabled"
        class="basis-[90%] p-2 border rounded-l-xl focus:outline focus:outline-green-500"
      />
      <button
        class="basis-[10%] rounded-r-xl border border-l-0 flex items-center justify-center"
        @click="show = !show"
      >
        <IconEyes class="w-5 fill-green-500" :show="show" />
      </button>
    </div>
  </div>
</template>

<style scoped></style>