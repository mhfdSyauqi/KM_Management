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
    <input
      v-model="model"
      type="text"
      :id="props.name"
      :readonly="props.readonly"
      :disabled="props.disabled"
      class="p-2 border rounded-xl focus:outline focus:outline-green-500"
    />
  </div>
</template>

<style scoped></style>