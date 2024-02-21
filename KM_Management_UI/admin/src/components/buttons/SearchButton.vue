<script setup>
import IconSearch from '@/components/icons/IconSearch.vue'

const emit = defineEmits(['onSearch'])
function search(e) {
  emit('onSearch', e.target.value)
}
</script>

<template>
  <div class="min-w-60 flex items-center rounded-3xl border-2 p-2 px-4 gap-2">
    <IconSearch />
    <input
      type="search"
      class="h-full w-full border-0 focus:outline-none"
      placeholder="Search"
      @input="search"
    />
  </div>
</template>

<style scoped></style>