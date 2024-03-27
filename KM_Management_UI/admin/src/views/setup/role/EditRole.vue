<script setup>
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import AbortButton from '@/components/buttons/AbortButton.vue'
import TextForm from '@/components/forms/TextForm.vue'
import { Dialog, DialogPanel, DialogTitle, DialogDescription } from '@headlessui/vue'

import { useNotificationStore } from '@/stores/useNotification.js'
import { ToastSwal } from '@/extension/SwalExt.js'
import { editedUser, HandleEdit } from '@/components/pages/setup/roles/patchRoles.js'

const notificationStore = useNotificationStore()
const isOpen = defineModel({ default: false })
function setIsOpen(value) {
  isOpen.value = value
}

async function onEdit() {
  const result = await HandleEdit()

  if (!result) {
    return await ToastSwal.fire({
      icon: 'error',
      text: "can't connect to server, please try in a few minutes"
    })
  }

  setIsOpen(false)
  notificationStore.set('reload')
  return await ToastSwal.fire({
    icon: 'success',
    text: 'success edit account'
  })
}
</script>

<template>
  <Dialog :open="isOpen" @close="setIsOpen" class="relative z-50 select-none">
    <div class="fixed inset-0 bg-black bg-opacity-75 flex w-screen items-center justify-center p-4">
      <DialogPanel class="w-full max-w-md rounded bg-white">
        <DialogTitle as="div" class="flex items-center gap-1 border-b-2 px-6 py-3.5">
          <h3 class="basis-[70%] text-green-700 font-bold text-xl">Edit Account</h3>

          <AbortButton class="!min-w-20 basis-auto" @click="setIsOpen(false)">Cancel</AbortButton>
          <PrimaryButton class="!min-w-20 basis-auto" @click.prevent="onEdit">Save</PrimaryButton>
        </DialogTitle>

        <DialogDescription as="div" class="px-6 py-3.5 space-y-2 text-sm">
          <TextForm name="Full Name" v-model="editedUser.full_name" readonly />

          <div class="flex flex-col w-full gap-1">
            <label for="role" class="text-gray-400">
              Role
              <sup class="text-red-500">*</sup>
            </label>

            <select
              class="border rounded-xl p-2 focus:outline focus:outline-green-500"
              v-model="editedUser.role"
            >
              <option value="Admin">Admin</option>
              <option value="Super">Super Admin</option>
            </select>
          </div>
        </DialogDescription>
      </DialogPanel>
    </div>
  </Dialog>
</template>

<style scoped></style>