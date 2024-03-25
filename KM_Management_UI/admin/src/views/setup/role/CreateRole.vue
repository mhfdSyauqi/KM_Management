<script setup>
import { ref, computed } from 'vue'
import { Dialog, DialogPanel, DialogTitle, DialogDescription } from '@headlessui/vue'
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import AbortButton from '@/components/buttons/AbortButton.vue'
import TextForm from '@/components/forms/TextForm.vue'

const isOpen = ref(true)

function setIsOpen(value) {
  isOpen.value = value
}
</script>

<template>
  <Dialog :open="isOpen" @close="setIsOpen" class="relative z-50 select-none">
    <div class="fixed inset-0 bg-black bg-opacity-75 flex w-screen items-center justify-center p-4">
      <DialogPanel class="w-full max-w-md rounded bg-white">
        <DialogTitle as="div" class="flex items-center gap-1 border-b-2 px-6 py-3.5">
          <h3 class="basis-[70%] text-green-700 font-bold text-xl">Add Account</h3>

          <AbortButton class="!min-w-20 basis-auto" @click="setIsOpen(false)">Cancel</AbortButton>
          <PrimaryButton class="!min-w-20 basis-auto">Save</PrimaryButton>
        </DialogTitle>

        <DialogDescription as="div" class="px-6 py-3.5">
          <TextForm name="Username" required @input="console.log()" />
        </DialogDescription>
      </DialogPanel>
    </div>
  </Dialog>
</template>

<style scoped></style>