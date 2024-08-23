<template>
  <div class="mb-4">
    <div
      class="cursor-pointer"
      :class="{
        'h-10 bg-white w-full pl-4 pr-4 flex items-center relative': true,
        'rounded-t-2xl border-t border-l border-r border-[#E99300]': expanded,
        'rounded-2xl border border-gray-300': !expanded
      }"
      @click="toggleExpand"
    >
      <div class="flex justify-between items-center min-w-full">
        <div class="flex items-start space-x-2">
          <h1>
            <span
              :class="{
                'text-[#888888] ': !expanded,
                'text-[#2C7B4B] ': expanded
              }"
              class="font-medium"
              >{{ nameInput }}</span
            >
          </h1>
        </div>

        <div class="flex items-end space-x-2">
          <div title="Collapse" v-if="expanded" v-html="arrowRightIcon"></div>
          <div title="Expand" v-else v-html="arrowBottomIcon"></div>
        </div>
      </div>
    </div>

    <div v-if="expanded" class="flex items-center justify-center" @click="toggleExpand">
      <div class="w-screen">
        <div
          :class="'h-fit bg-white w-full pl-4 pr-4 flex items-center relative border-l border-r border-[#E99300]'"
        >
          <div class="w-full">
            <div
              class="flex justify-items-center w-full pt-4 pb-4"
              v-for="(msg, index) in messageData"
              :key="index"
            >
              <div
                v-if="isDropDownOpen[index]"
                @click.stop
                @click="closeDropdown(index)"
                class="overlay"
              ></div>
              <div
                v-if="expanded"
                class="bg-orange-100 h-14 w-8 rounded-xl flex items-center justify-center"
                style="margin-right: 10px"
              >
                <span class="text-[#4A5449]">{{ msg.sequence }}.</span>
              </div>
              <div class="w-[70%] relative">
                <div style="position: relative">
                  <textarea
                    :readonly="!editMode[index]"
                    v-model="localUpdateText[index]"
                    v-if="expanded"
                    maxlength="360"
                    @click.stop
                    class="w-full pt-1 pb-1 pl-2 pr-2 border rounded-xl focus:outline focus:outline-[#888888] border-gray-300"
                  ></textarea>
                  <span
                    v-if="editMode[index]"
                    style="position: absolute; bottom: 5px; right: 5px"
                    class="text-gray-500 text-sm bg-opacity-50 bg-slate-300 pl-1 pr-1"
                    >Limit {{ updateText[index].length }} / 360</span
                  >
                </div>
                <div v-if="errorUpdateMessage[index]" class="relative text-red-500 mt-2 ml-2">
                  {{ errorUpdateMessage[index] }}
                </div>
              </div>
              <div class="relative">
                <button
                  title="Edit"
                  v-if="!editMode[index]"
                  class="px-2 py-2 ml-2 mr-2 bg-[#888888] text-white rounded-xl hover:bg-[#2C7B4B]"
                  @click="editModeMessage(index)"
                  @click.stop
                >
                  <div v-html="editIcon"></div>
                </button>
                <button
                  title="Save"
                  v-if="editMode[index]"
                  class="px-2 py-2 ml-2 mr-2 bg-[#56956f] text-white rounded-xl hover:bg-[#2C7B4B]"
                  @click.stop
                  @click="
                    updateSetupMessage(typeMessage, msg.sequence, updateText[index], true, index)
                  "
                >
                  <div v-html="saveIcon"></div>
                </button>
                <button
                  title="Cancel"
                  v-if="editMode[index]"
                  class="px-2 py-2 ml-2 mr-2 bg-red-500 text-white rounded-xl hover:bg-red-600"
                  @click="cancelMessage(index)"
                  @click.stop
                >
                  <div v-html="removeIcon"></div>
                </button>
              </div>
              <Popover v-if="!editMode[index]" class="relative" v-slot="{ open }">
                <PopoverButton
                  title="More"
                  class="border px-2 py-2 ml-2 mr-2 bg-[#888888] hover:bg-[#2C7B4B] text-green-800 items-center justify-center focus:outline-none"
                  :class="[
                    open ? 'rounded-tr-xl rounded-tl-xl text-white bg-green-800 ' : 'rounded-xl'
                  ]"
                >
                  <span>
                    <div v-html="moreIcon"></div>
                  </span>
                </PopoverButton>

                <PopoverPanel class="absolute z-10">
                  <div
                    class="grid grid-cols-1 p-3 gap-2.5 bg-green-100 border border-green-800 rounded-2xl rounded-tl-none min-w-48 text-green-800 select-none"
                  >
                    <slot name="options">
                      <div
                        v-show="messageData.length > 1 && index != 0"
                        class="py-2 px-4 cursor-pointer rounded-3xl hover:bg-gray-200 flex items-center"
                        @click="
                          updateSequenceSetupMessage(
                            typeMessage,
                            'up',
                            msg.sequence,
                            messageData.length,
                            index
                          )
                        "
                        @click.stop
                      >
                        <div v-html="arrowUpwardIcon" class="mr-2"></div>
                        Move Up
                      </div>
                      <div
                        v-show="messageData.length > 1 && index != messageData.length - 1"
                        class="py-2 px-4 cursor-pointer rounded-3xl hover:bg-gray-200 flex items-center"
                        @click="
                          updateSequenceSetupMessage(
                            typeMessage,
                            'down',
                            msg.sequence,
                            messageData.length,
                            index
                          )
                        "
                        @click.stop
                      >
                        <div v-html="arrowDownwardIcon" class="mr-2"></div>
                        Move Down
                      </div>
                      <div
                        class="py-2 px-4 cursor-pointer rounded-3xl hover:bg-gray-200 flex items-center"
                        @click="deleteSetupMessage(typeMessage, msg.sequence, index)"
                        @click.stop
                      >
                        <div v-html="deleteIcon" class="mr-2"></div>
                        Delete
                      </div></slot
                    >
                  </div>
                </PopoverPanel>
              </Popover>

              <!-- dropdownclose -->
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- add start -->
    <div v-if="expanded && showAddMessage" class="flex items-center justify-center">
      <div class="w-screen">
        <div
          :class="'h-fit bg-white w-full pl-4 pr-4 flex items-center relative border-l border-r border-[#E99300]'"
        >
          <div class="w-full">
            <div
              class="flex justify-items-center w-full pt-4 pb-4"
              v-for="(message, index) in addedMessages"
              :key="index"
            >
              <div
                v-if="expanded"
                class="bg-orange-100 h-14 w-8 rounded-xl flex items-center justify-center"
                style="margin-right: 10px"
              >
                <span class="text-[#4A5449]">{{ messageData.length + index + 1 }}.</span>
              </div>
              <div class="w-[70%] relative">
                <div style="position: relative">
                  <textarea
                    v-model="addText[index]"
                    v-if="expanded"
                    @click.stop
                    class="w-full pt-1 pb-1 pl-2 pr-2 border rounded-xl focus:outline focus:outline-[#888888] border-gray-300"
                    maxlength="360"
                  ></textarea>
                  <span
                    style="position: absolute; bottom: 5px; right: 5px"
                    class="text-gray-500 text-sm bg-opacity-50 bg-slate-300 pl-1 pr-1"
                    >Limit {{ addText[index].length }} / 360</span
                  >
                </div>
                <div v-show="errorAddMessage[index]" class="relative text-red-500 mt-2 ml-2">
                  {{ errorAddMessage[index] }}
                </div>
              </div>
              <div class="relative">
                <button
                  class="ml-2 mr-2 px-2 py-2 bg-[#56956f] text-white rounded-xl hover:bg-[#2C7B4B]"
                  style="margin-top: -25px"
                  @click="addNewMessage(typeMessage, addText[index], index)"
                  @click.stop
                >
                  <div v-html="saveIcon"></div>
                </button>
                <button
                  class="ml-2 mr-2 px-2 py-2 bg-red-500 text-white rounded-xl hover:bg-red-600"
                  @click="removeMessage(index)"
                  @click.stop
                >
                  <div v-html="removeIcon"></div>
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- add ended-->
    <!-- addbutton -->
    <div v-if="expanded" class="flex items-center justify-center" @click="toggleExpand">
      <div class="w-screen">
        <div
          class="h-10 bg-white w-full pl-4 pr-4 flex items-center relative rounded-b-xl border-b border-l border-r border-[#E99300]"
        >
          <div class="w-full">
            <div
              @click="toggleAddMessage(messageData.length)"
              @click.stop
              class="flex items-center w-fit pb-2"
            >
              <button class="bg-[#888888] text-white px-2 py-2 rounded-xl hover:bg-[#56956f]">
                <div v-html="plusIcon"></div>
              </button>
              <label class="cursor-pointer p-2 text-sm text-[#888888] hover:text-[#2C7B4B]"
                >Add Message</label
              >
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- addbutton end -->
  </div>
</template>

<script setup>
import { ref, defineProps, defineEmits, watch } from 'vue'
import Swal from 'sweetalert2'
import {
  newMessage,
  errorPost,
  HandlePublish
} from '@/components/pages/setup/message/postMessage.js'

import {
  errorEdit,
  editMessage,
  HandleRePublish
} from '@/components/pages/setup/message/patchMessage.js'
import { Popover, PopoverButton, PopoverPanel } from '@headlessui/vue'
import { deleteMessage, HandleUnPublish } from '@/components/pages/setup/message/deleteMessage.js'

import {
  HandleSequence,
  editSequence
} from '@/components/pages/setup/message/patchSequenceMessage.js'
import { ToastSwal } from '@/extension/SwalExt.js'
const props = defineProps([
  'nameInput',
  'messageData',
  'updateText',
  'editMode',
  'modifiedBy',
  'createdBy',
  'typeMessage',
  'isDropDownOpen',
  'fetchData',
  'expanded'
])

const emits = defineEmits(['toggleExpand'])

const showAddMessage = ref(false)

const editMode = ref(props.editMode)

const isDropDownOpen = ref(props.isDropDownOpen)

const addedMessages = ref([])

const addText = ref([])

const localUpdateText = ref([props.updateText])

watch(
  () => props.updateText,
  (newValue) => {
    localUpdateText.value = newValue
  }
)

const errorAddMessage = ref([])
const errorUpdateMessage = ref([])

const editModeMessage = (index) => {
  editMode.value[index] = !editMode.value[index]
}

const arrowRightIcon = `<svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="gray"
                            height="20"
                            viewBox="0 -960 960 960"
                            width="20"
                          >
                            <path d="M504-480 320-664l56-56 240 240-240 240-56-56 184-184Z" />
                          </svg>`
const arrowBottomIcon = `<svg
                              xmlns="http://www.w3.org/2000/svg"
                              fill="gray"
                              height="20"
                              viewBox="0 -960 960 960"
                              width="20"
                            >
                              <path d="M480-345 240-585l56-56 184 184 184-184 56 56-240 240Z" />
                            </svg>`

const editIcon = `<svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="white"
                    height="15"
                    viewBox="0 -960 960 960"
                    width="15"
                  >
                    <path
                      d="M200-120q-33 0-56.5-23.5T120-200v-560q0-33 23.5-56.5T200-840h357l-80 80H200v560h560v-278l80-80v358q0 33-23.5 56.5T760-120H200Zm280-360ZM360-360v-170l367-367q12-12 27-18t30-6q16 0 30.5 6t26.5 18l56 57q11 12 17 26.5t6 29.5q0 15-5.5 29.5T897-728L530-360H360Zm481-424-56-56 56 56ZM440-440h56l232-232-28-28-29-28-231 231v57Zm260-260-29-28 29 28 28 28-28-28Z"
                    />
                  </svg>`
const saveIcon = `<svg
                    xmlns="http://www.w3.org/2000/svg"
                    height="15"
                    viewBox="0 -960 960 960"
                    width="15"
                    fill="white"
                  >
                    <path
                      d="M840-680v480q0 33-23.5 56.5T760-120H200q-33 0-56.5-23.5T120-200v-560q0-33 23.5-56.5T200-840h480l160 160Zm-80 34L646-760H200v560h560v-446ZM480-240q50 0 85-35t35-85q0-50-35-85t-85-35q-50 0-85 35t-35 85q0 50 35 85t85 35ZM240-560h360v-160H240v160Zm-40-86v446-560 114Z"
                    />
                  </svg>`

const removeIcon = `<svg
                    xmlns="http://www.w3.org/2000/svg"
                    height="15"
                    viewBox="0 -960 960 960"
                    width="15"
                    fill="white"
                  >
                    <path
                      d="m256-200-56-56 224-224-224-224 56-56 224 224 224-224 56 56-224 224 224 224-56 56-224-224-224 224Z"
                    />
                  </svg>`
const moreIcon = `<svg
                    xmlns="http://www.w3.org/2000/svg"
                    height="15"
                    viewBox="0 -960 960 960"
                    width="15"
                    fill="white"
                  >
                    <path
                      d="M240-400q-33 0-56.5-23.5T160-480q0-33 23.5-56.5T240-560q33 0 56.5 23.5T320-480q0 33-23.5 56.5T240-400Zm240 0q-33 0-56.5-23.5T400-480q0-33 23.5-56.5T480-560q33 0 56.5 23.5T560-480q0 33-23.5 56.5T480-400Zm240 0q-33 0-56.5-23.5T640-480q0-33 23.5-56.5T720-560q33 0 56.5 23.5T800-480q0 33-23.5 56.5T720-400Z"
                    />
                  </svg>`
const plusIcon = `<svg
                    xmlns="http://www.w3.org/2000/svg"
                    height="15"
                    viewBox="0 -960 960 960"
                    width="15"
                    fill="white"
                  >
                    <path
                      d="M440-440H200v-80h240v-240h80v240h240v80H520v240h-80v-240Z"
                    />
                  </svg>`

const arrowUpwardIcon = `<svg xmlns="http://www.w3.org/2000/svg"
                      height="24"
                      fill="green"
                      viewBox="0 -960 960 960"
                      width="24"
                      >
                      <path d="M440-160v-487L216-423l-56-57 320-320 320 320-56 57-224-224v487h-80Z"/>
                    </svg>`

const arrowDownwardIcon = `<svg xmlns="http://www.w3.org/2000/svg"
                        height="24"
                        fill="green"
                        viewBox="0 -960 960 960"
                        width="24"
                        >
                        <path d="M440-800v487L216-537l-56 57 320 320 320-320-56-57-224 224v-487h-80Z"/>
                      </svg>`
const deleteIcon = `<svg xmlns="http://www.w3.org/2000/svg"
                      fill="green"
                      height="24"
                      viewBox="0 -960 960 960"
                      width="24"
                      >
                      <path d="M280-120q-33 0-56.5-23.5T200-200v-520h-40v-80h200v-40h240v40h200v80h-40v520q0 33-23.5 56.5T680-120H280Zm400-600H280v520h400v-520ZM360-280h80v-360h-80v360Zm160 0h80v-360h-80v360ZM280-720v520-520Z"/>
                    </svg>`

const toggleExpand = () => {
  emits('toggleExpand')
  showAddMessage.value = false
  addedMessages.value = []
  errorAddMessage.value = []
  errorUpdateMessage.value = []
  addText.value = []
}

const toggleAddMessage = (lengthAllMessage) => {
  if (lengthAllMessage == 5 || lengthAllMessage + addedMessages.value.length == 5) {
    Swal.fire({
      icon: 'error',
      title: 'Error',
      text: 'Messages is already at the maximum limit',
      confirmButtonColor: '#d33'
    })
    return
  }

  showAddMessage.value = true
  if (showAddMessage.value) {
    addedMessages.value.push({ contents: '' })
  }
  addText.value.push('')
}

const updateSetupMessage = async (type, sequence, contents, is_Active, index) => {
  try {
    editMessage.value.type = type
    editMessage.value.sequence = sequence
    editMessage.value.contents = contents
    editMessage.value.is_active = is_Active

    const result = await Swal.fire({
      icon: 'info',
      title: 'Are you sure?',
      text: 'want to update this message?',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
      confirmButtonColor: '#2c7b4b',
      cancelButtonColor: 'white',
      reverseButtons: true,
      didOpen: () => {
        const confirmButton = Swal.getConfirmButton()
        confirmButton.style.borderRadius = '15px'
        confirmButton.style.width = '70px'
        const cancelButton = Swal.getCancelButton()
        cancelButton.style.borderRadius = '15px'
        cancelButton.style.width = '70px'
        cancelButton.style.backgroundColor = 'white'
        cancelButton.style.color = 'red'
        cancelButton.addEventListener('mouseover', () => {
          cancelButton.style.backgroundColor = 'red'
          cancelButton.style.color = 'white'
        })
        cancelButton.addEventListener('mouseout', () => {
          cancelButton.style.backgroundColor = 'white'
          cancelButton.style.color = 'red'
        })
      }
    })
    if (result.isConfirmed) {
      const response = await HandleRePublish()
      if (response !== 400) {
        resetUpdateMessage(index)
        await ToastSwal.fire({ icon: 'success', text: 'Update message successful!' })
      } else {
        errorUpdateMessage.value[index] = errorEdit.value.contents.message
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      await ToastSwal.fire({ icon: 'error', text: 'Update message canceled' })
    }

    // await storeContent.updateMessage(updateMessage);
    // errorUpdateMessage.value[index] =
    //   storeContent.getErrorUpdateMessageContents;
    // if (errorUpdateMessage.value[index].length == 0) {
    //   editMode.value[index] = !editMode.value[index];
    //   fetchData();
    // }
    // editMode.value[index] = !editMode.value[index];
    // fetchData();
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const updateSequenceSetupMessage = async (type, move, currentSequence, length, index) => {
  try {
    const currentSequenceInt = parseInt(currentSequence)
    const upSequence = currentSequenceInt - 1
    const downSequence = currentSequenceInt + 1
    if (move == 'up' && upSequence != 0) {
      editSequence.value.type = type
      editSequence.value.current_sequence = currentSequenceInt
      editSequence.value.new_sequence = upSequence
      await HandleSequence()
      fetchData()
      isDropDownOpen.value[index] = false
    } else if (move == 'down' && currentSequence < length) {
      editSequence.value.type = type
      editSequence.value.current_sequence = currentSequenceInt
      editSequence.value.new_sequence = downSequence
      await HandleSequence()
      fetchData()
      isDropDownOpen.value[index] = false
    } else {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'Sequence is already at the maximum/minimum limit',
        confirmButtonColor: '#d33'
      })
      isDropDownOpen.value[index] = false
    }
  } catch (error) {
    console.error('Terjadi kesalahan:', error)
  }
}

const deleteSetupMessage = async (type, sequence, index) => {
  try {
    deleteMessage.value.type = type
    deleteMessage.value.sequence = sequence

    const result = await Swal.fire({
      icon: 'info',
      title: 'Are you sure?',
      text: 'want to delete this message?',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
      confirmButtonColor: '#2c7b4b',
      cancelButtonColor: 'white',
      reverseButtons: true,
      didOpen: () => {
        const confirmButton = Swal.getConfirmButton()
        confirmButton.style.borderRadius = '15px'
        confirmButton.style.width = '70px'
        const cancelButton = Swal.getCancelButton()
        cancelButton.style.borderRadius = '15px'
        cancelButton.style.width = '70px'
        cancelButton.style.backgroundColor = 'white'
        cancelButton.style.color = 'red'
        cancelButton.addEventListener('mouseover', () => {
          cancelButton.style.backgroundColor = 'red'
          cancelButton.style.color = 'white'
        })
        cancelButton.addEventListener('mouseout', () => {
          cancelButton.style.backgroundColor = 'white'
          cancelButton.style.color = 'red'
        })
      }
    })
    if (result.isConfirmed) {
      const response = await HandleUnPublish()
      if (response !== 400) {
        isDropDownOpen.value[index] = false
        fetchData()
        await ToastSwal.fire({ icon: 'success', text: 'Delete message successful!' })
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      await ToastSwal.fire({ icon: 'error', text: 'Delete message canceled' })
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const cancelMessage = (index) => {
  editMode.value[index] = !editMode.value[index]
  errorUpdateMessage.value[index] = ''
  fetchData()
}

// const getNextSequenceNumber = (index) => {
//   return messageData.value.length + index + 1;
// };

const removeMessage = (index) => {
  addedMessages.value.splice(index, 1)
  // showInvalidAdd.value.splice(index, 1);
  errorAddMessage.value[index] = ''
  addText.value.splice(index, 1)
}

const closeDropdown = (index) => {
  isDropDownOpen.value[index] = false
}

const resetAddMessage = (index) => {
  addText.value[index] = []
  showAddMessage.value = false
  addedMessages.value = []
  addText.value = []
  errorAddMessage.value[index] = ''
}

const resetUpdateMessage = (index) => {
  editMode.value[index] = !editMode.value[index]
  errorUpdateMessage.value[index] = ''
}

const addNewMessage = async (type, contents, index) => {
  try {
    newMessage.value.type = type
    newMessage.value.contents = contents
    const result = await Swal.fire({
      icon: 'info',
      title: 'Are you sure?',
      text: 'want to add this message?',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
      confirmButtonColor: '#2c7b4b',
      cancelButtonColor: 'white',
      reverseButtons: true,
      didOpen: () => {
        const confirmButton = Swal.getConfirmButton()
        confirmButton.style.borderRadius = '15px'
        confirmButton.style.width = '70px'
        const cancelButton = Swal.getCancelButton()
        cancelButton.style.borderRadius = '15px'
        cancelButton.style.width = '70px'
        cancelButton.style.backgroundColor = 'white'
        cancelButton.style.color = 'red'
        cancelButton.addEventListener('mouseover', () => {
          cancelButton.style.backgroundColor = 'red'
          cancelButton.style.color = 'white'
        })
        cancelButton.addEventListener('mouseout', () => {
          cancelButton.style.backgroundColor = 'white'
          cancelButton.style.color = 'red'
        })
      }
    })
    if (result.isConfirmed) {
      const response = await HandlePublish()
      if (response !== 400) {
        fetchData()
        resetAddMessage(index)

        await ToastSwal.fire({ icon: 'success', text: 'Add message successful!' })
      } else {
        errorAddMessage.value[index] = errorPost.value.contents.message
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      await ToastSwal.fire({ icon: 'error', text: 'Add message canceled' })
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const fetchData = async () => {
  if (props.fetchData) {
    props.fetchData()
  }
}
</script>

<style scoped>
.overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(0, 0, 0, 0.5);
  z-index: 1000;
  pointer-events: auto;
}
.isDropdown {
  z-index: 1001;
}

.cursor-pointer {
  cursor: pointer;
}
</style>
