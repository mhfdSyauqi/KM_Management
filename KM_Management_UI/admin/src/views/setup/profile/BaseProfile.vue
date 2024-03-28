<script setup>
import Swal from 'sweetalert2'

import { profile, GetAssistantProfile } from '@/components/pages/setup/useAssistantProfile.js'

import { HandlePublish, errorInput } from '@/components/pages/setup/postAssistantProfile.js'

import { onMounted, ref } from 'vue'

const appName = ref('')
const appImage = ref('')
const selectedFile = ref(null)
const disabledButton = ref(true)

const handleFileChange = (event) => {
  disabledButton.value = false
  const file = event.target.files[0]

  if (file) {
    const reader = new FileReader()
    reader.onload = () => {
      appImage.value = reader.result
    }
    reader.readAsDataURL(file)
    selectedFile.value = file
  }
}

async function onPublish() {
  const formData = new FormData()
  formData.append('Files', selectedFile.value)
  formData.append('AppName', appName.value)
  const result = await Swal.fire({
    icon: 'question',
    title: 'Are you sure?',
    showCancelButton: true,
    confirmButtonText: 'Yes',
    cancelButtonText: 'No',
    confirmButtonColor: '#2c7b4b',
    cancelButtonColor: '#d33'
  })
  if (result.isConfirmed) {
    const response = await HandlePublish(formData)
    if (response !== 400) {
      Swal.fire({
        icon: 'success',
        title: 'File upload successful!',
        confirmButtonColor: '#2c7b4b'
      })
      disabledButton.value = true
      GetAssistantProfile()
    } else {
      Swal.fire({
        icon: 'error',
        title: 'Check your input',
        text: 'There is an error in your input. Please check again.',
        confirmButtonColor: '#d33'
      })
      disabledButton.value = false
    }
  } else {
    Swal.fire({
      icon: 'info',
      title: 'File upload canceled',
      confirmButtonColor: '#2c7b4b'
    })
    GetAssistantProfile()
  }
}

onMounted(async () => {
  await GetAssistantProfile()
  appName.value = profile.value.app_name
  appImage.value = profile.value.app_image
})

const inputNameOntyping = () => {
  disabledButton.value = false
}
</script>

<template>
  <div class="flex items-center align-middle gap-3 mb-3 bg-tea">
    <p class="text-sm">Setup</p>

    <span> > </span>
    <p class="text-sm text-orange-400">Assistant Profile</p>
  </div>

  <div class="w-full rounded-lg bg-white min-h-[25%] max-h-[96%] box-border flex flex-col">
    <div class="flex gap-2.5 justify-between border-b-2 py-5 px-8">
      <div class="flex items-start space-x-2">
        <h1 class="basis-[100%] text-2xl font-bold text-green-800">Assistant Profile</h1>
      </div>
      <div class="flex items-end space-x-2">
        <button
          class="group bg-[#2c7b4b] flex items-center border text-white hover:bg-[#2c4233] rounded-3xl pt-2 pb-2 pl-5 pr-5"
          :class="[disabledButton ? 'bg-[#62ac7f]' : 'bg-[#2c7b4b]']"
          @click="onPublish()"
          :disabled="disabledButton"
        >
          Save Changes
        </button>
      </div>
    </div>

    <br />

    <div class="flex flex-col p-10">
      <div class="flex">
        <div class="mb-4 ml-4">
          <div class="bg-slate-500 h-[150px] w-[150px] rounded-full overflow-hidden">
            <img class="object-cover rounded-full h-[150px] w-[150px]" :src="appImage" />
          </div>
        </div>

        <div class="mb-4 ml-10 flex - flex-col">
          <label :for="errorInput" class="pb-2 block font-bold text-[#2c7b4b]">Virtual Name </label>
          <input
            type="text"
            v-model="appName"
            @input="inputNameOntyping"
            class="w-[350px] pt-2 pb-2 pl-2 pr-2 border rounded-xl focus:outline focus:outline-orange-500"
          />
          <div
            v-show="errorInput.files.isError || errorInput.appName.isError"
            class="flex flex-col font-medium"
          >
            <span class="text-red-500">
              {{ errorInput.appName.message }}
            </span>
            <span class="text-red-500">
              {{ errorInput.files.message }}
            </span>
          </div>
        </div>
      </div>

      <div class="flex">
        <div class="mb-4 items-center">
          <label
            class="group bg-[#2c7b4b] w-[175px] flex items-center justify-center border text-white hover:bg-[#2c4233] rounded-3xl pt-2 pb-2 pl-5 pr-5"
          >
            Change Picture
            <input
              type="file"
              accept=".jpg, .jpeg, .png"
              style="display: none"
              @change="handleFileChange"
            />
          </label>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped></style>
