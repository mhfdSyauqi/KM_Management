<script setup>
const props = defineProps({
  disable: {
    type: Boolean,
    default: false
  }
})
</script>

<template>
  <button
    class="min-w-24 font-semibold rounded-3xl border text-white active:scale-95 p-2"
    :class="[props.disable ? 'bg-gray-400' : 'bg-green-700  hover:bg-teal-700']"
    :disabled="props.disable"
  >
    <slot></slot>
  </button>
</template>