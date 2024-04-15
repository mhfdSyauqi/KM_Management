<script setup>
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
  error: {
    type: Object,
    default(rawProp) {
      return {
        isError: false,
        message: ''
      }
    }
  },
  name: String,
  min: {
    type: Number,
    required: false
  },
  max: {
    type: Number,
    required: false
  },
  step: {
    type: Number,
    required: false
  }
})
</script>

<template>
  <div class="flex flex-col w-full gap-1">
    <label :for="props.name" class="text-gray-400">
      {{ props.name }}
      <sup class="text-red-500" v-show="props.required"> * </sup>
    </label>
    <input
      class="p-2 border rounded-xl focus:outline focus:outline-green-500"
      type="number"
      v-model="model"
      :id="props.name"
      :readonly="props.readonly"
      :min="props.min"
      :max="props.max"
      :step="props.step"
    />
    <span class="text-red-500 text-xs" v-if="props.error.isError">{{ props.error.message }}</span>
  </div>
</template>

<style scoped></style>