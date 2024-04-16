<script setup>
import { Popover, PopoverButton, PopoverPanel } from '@headlessui/vue'
import IconDropdown from '@/components/icons/IconDropdown.vue'
import { ref } from 'vue'
const hover = ref(false)
</script>

<template>
  <Popover class="relative" v-slot="{ open }">
    <PopoverButton
      class="min-w-32 flex p-2 border border-green-800 text-green-800 items-center justify-center gap-2 focus:outline-none"
      :class="[
        open
          ? 'rounded-tr-3xl rounded-tl-3xl text-white bg-green-800 '
          : 'rounded-3xl hover:bg-[#71b483] hover:text-white hover:border-[#71b483]'
      ]"
      @mouseenter="hover = true"
      @mouseleave="hover = false"
    >
      <span>
        <slot name="default"></slot>
      </span>
      <IconDropdown
        :class="[open ? 'fill-white' : 'fill-green-800', hover ? 'fill-white' : 'fill-green-800']"
      />
    </PopoverButton>

    <PopoverPanel class="absolute z-10">
      <div
        class="grid grid-cols-1 p-3 gap-2.5 bg-green-100 border border-green-800 rounded-2xl rounded-tl-none min-w-48 text-green-800 select-none"
      >
        <slot name="options"></slot>
      </div>
    </PopoverPanel>
  </Popover>
</template>

<style scoped></style>
