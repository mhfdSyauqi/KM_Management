<script setup>
const model = defineModel()
const props = defineProps({
  required: Boolean,
  name: String,
  items: Array
})
</script>

<template>
  <div class="flex flex-col w-full gap-1">
    <label for="dropdown" class="text-gray-400">
      {{ props.name }}
      <sup class="text-red-500" v-show="required">*</sup>
    </label>
    <select
      class="p-2 border border-gray-300 rounded-xl focus:outline focus:outline-green-500"
      v-model="model"
    >
      <option value="" disabled>None</option>
      <option v-for="item in items" :key="item.id" :value="item.id">
        {{ item.name }}
      </option>
    </select>
  </div>
</template>

<style scoped></style>