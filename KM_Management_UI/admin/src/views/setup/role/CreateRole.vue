<script setup>
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import AbortButton from '@/components/buttons/AbortButton.vue'
import TextForm from '@/components/forms/TextForm.vue'
import {
  Combobox,
  ComboboxInput,
  ComboboxOptions,
  ComboboxOption,
  Dialog,
  DialogPanel,
  DialogTitle,
  DialogDescription
} from '@headlessui/vue'

import {
  newUser,
  userRef,
  query,
  selected,
  error,
  GetUserReferences,
  ResetInput,
  HandleSave
} from '@/components/pages/setup/roles/postRoles.js'

import { useNotificationStore } from '@/stores/useNotification.js'
import { watchEffect, onMounted } from 'vue'
import { ToastSwal } from '@/extension/SwalExt.js'

const notificationStore = useNotificationStore()
const isOpen = defineModel({ default: false })
function setIsOpen(value) {
  isOpen.value = value
  ResetInput()
}

onMounted(async () => {
  await GetUserReferences()
})

async function onInput(e) {
  selected.value = ''
  query.value = e.target.value
  await GetUserReferences()
}

async function onSave() {
  const result = await HandleSave()

  if (result === 500) {
    return await ToastSwal.fire({
      icon: 'error',
      text: "can't connect to server, please try in a few minutes"
    })
  }

  if (!result) {
    return await ToastSwal.fire({
      icon: 'warning',
      text: 'Please re-check your input!!'
    })
  }

  setIsOpen(false)
  ResetInput()
  notificationStore.set('reload')
  return await ToastSwal.fire({
    icon: 'success',
    text: 'success add account'
  })
}

watchEffect(() => {
  if (selected.value) {
    newUser.value.user_name = selected.value.login_name
    newUser.value.full_name = selected.value.full_name
    newUser.value.email = selected.value.email
    newUser.value.job_title = selected.value.job_title
  } else {
    newUser.value.user_name = ''
    newUser.value.full_name = ''
    newUser.value.email = ''
    newUser.value.job_title = ''
  }
})
</script>

<template>
  <Dialog :open="isOpen" @close="setIsOpen" class="relative z-50 select-none">
    <div class="fixed inset-0 bg-black bg-opacity-75 flex w-screen items-center justify-center p-4">
      <DialogPanel class="w-full max-w-md rounded-xl bg-white">
        <DialogTitle as="div" class="flex items-center gap-1 border-b-2 px-6 py-3.5">
          <h3 class="basis-[70%] text-green-700 font-bold text-xl">Add Account</h3>

          <AbortButton class="!min-w-20 basis-auto" @click="setIsOpen(false)">Cancel</AbortButton>
          <PrimaryButton
            class="!min-w-20 basis-auto"
            :disable="selected === ''"
            @click.prevent="onSave"
            >Save</PrimaryButton
          >
        </DialogTitle>

        <DialogDescription as="div" class="px-6 py-3.5 space-y-2 text-sm">
          <div class="flex flex-col w-full gap-1">
            <label for="username" class="text-gray-400">
              Username
              <sup class="text-red-500">*<span v-if="error.username">Required</span></sup>
            </label>
            <Combobox v-model="selected">
              <div class="relative border rounded-xl">
                <div
                  class="relative w-full cursor-default overflow-hidden rounded-xl text-left focus:outline-none focus-visible:ring-2 focus-visible:ring-white/75 focus-visible:ring-offset-2 focus-visible:ring-offset-teal-300 sm:text-sm"
                >
                  <ComboboxInput
                    class="w-full border-none rounded-xl py-2 pl-3 pr-10 text-sm leading-5 text-gray-900 focus:outline-1 focus:outline-green-500"
                    id="username"
                    :displayValue="(person) => person.login_name"
                    @input="onInput"
                  />
                </div>

                <ComboboxOptions
                  class="absolute mt-1 max-h-60 w-full overflow-auto rounded-md bg-teal-100 py-1 text-base shadow-lg ring-1 ring-black/5 focus:outline-none sm:text-sm"
                >
                  <div
                    v-if="userRef.length === 0 && query !== ''"
                    class="relative cursor-default select-none px-4 py-2 text-gray-700"
                  >
                    Nothing found.
                  </div>

                  <ComboboxOption
                    v-for="person in userRef"
                    as="template"
                    :key="person.login_name"
                    :value="person"
                    v-slot="{ active }"
                  >
                    <li
                      class="relative cursor-default select-none py-2 px-4"
                      :class="{
                        'bg-teal-600 text-white': active,
                        'text-gray-900': !active
                      }"
                    >
                      <span class="block truncate">
                        ( {{ person.login_name }} ) {{ person.full_name }}
                      </span>
                    </li>
                  </ComboboxOption>
                </ComboboxOptions>
              </div>
            </Combobox>
          </div>

          <TextForm name="Full Name" v-model="newUser.full_name" disabled />
          <TextForm name="Email" v-model="newUser.email" disabled />
          <TextForm name="Job Title" v-model="newUser.job_title" disabled />

          <div class="flex flex-col w-full gap-1">
            <label for="role" class="text-gray-400">
              Role
              <sup class="text-red-500">*<span v-if="error.role">Required</span></sup>
            </label>

            <select
              class="border rounded-xl p-2 focus:outline focus:outline-green-500"
              v-model="newUser.role"
            >
              <option value="" disabled>--- Select ---</option>
              <option value="Admin">Admin</option>
              <option value="Super">Super Admin</option>
            </select>
          </div>
        </DialogDescription>

        <div>&nbsp;</div>
      </DialogPanel>
    </div>
  </Dialog>
</template>

<style scoped></style>