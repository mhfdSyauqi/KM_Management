<script setup>
const model = defineModel()
const props = defineProps({
  required: Boolean,
  error: Object,
  name: String
})
</script>

<template>
  <div class="flex flex-col w-full gap-1">
    <label :for="props.name" class="text-gray-400">
      {{ props.name }}
      <sup class="text-red-500" v-show="required">
        * {{ error?.isError ? error?.message : null }}
      </sup>
    </label>
    <input
      v-model="model"
      type="text"
      :id="props.name"
      class="p-2 border rounded-xl focus:outline focus:outline-green-500"
    />
  </div>
</template>

<style scoped></style>