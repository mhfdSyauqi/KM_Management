<script setup>
import {
  GetTopIssueAvailableByFilter,
  filterAvailable,
  top_issue_available,
  GetTopIssueSelectedByFilter,
  top_issue_selected,
  filterSelected
} from '@/components/pages/category/useCategoryTopIssue.js'
import { ToastSwal } from '@/extension/SwalExt.js'
import {
  HandlePublishSelected,
  newSelected
} from '@/components/pages/category/postCategoryTopIssue.js'

import { HandleDelete, deleteSelected } from '@/components/pages/category/deleteCategorySelected.js'
import { HandleAllDelete } from '@/components/pages/category/deleteAllCategorySelected.js'
import { HandlePublishAllSelected } from '@/components/pages/category/postAllCategorySelected.js'
import { HandleRePublish, editSequence } from '@/components/pages/category/patchSequenceSelected.js'
import { ref, onMounted } from 'vue'
import IconMoveUp from '@/components/icons/IconMoveUp.vue'
import IconMoveDown from '@/components/icons/IconMoveDown.vue'
import IconAddNew from '@/components/icons/IconAddNew.vue'
import IconDelete from '@/components/icons/IconDeleteItem.vue'
import { useStore } from '@/stores/topIssueStore.js'
import Swal from 'sweetalert2'
import OptionButton from '@/components/buttons/OptionButton.vue'

const useTopIssueStore = useStore()
const searchQueryAvailableItem = ref('')
const searchQuerySelectedItem = ref('')
const availableItems = ref([])
const searchAvailableItems = ref([])
const searchSelectedItems = ref([])
const selectedItems = ref([])
const isActiveYesToggle = ref(true)
const isActiveNoToggle = ref(true)

const addNewSelected = async (uid_Bot_Category) => {
  try {
    newSelected.value.Uid_Bot_Category = uid_Bot_Category

    const result = await Swal.fire({
      icon: 'info',
      title: 'Are you sure?',
      text: 'want to add this item?',
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
      const response = await HandlePublishSelected()
      if (response !== 400) {
        resetSearchItem()
        checkBoxChange()
        await ToastSwal.fire({ icon: 'success', text: 'Add new selected item successful!' })
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      resetSearchItem()
      checkBoxChange()
      await ToastSwal.fire({ icon: 'error', text: 'Add new selected item canceled' })
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const addAllSelected = async () => {
  try {
    const result = await Swal.fire({
      icon: 'info',
      title: 'Are you sure?',
      text: 'want to add all items?',
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
      const response = await HandlePublishAllSelected()
      if (response !== 400) {
        resetSearchItem()
        checkBoxChange()
        await ToastSwal.fire({ icon: 'success', text: 'Add all items successful!' })
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      resetSearchItem()
      checkBoxChange()
      await ToastSwal.fire({ icon: 'error', text: 'Add all items canceled' })
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const deleteSelectedItem = async (sequence) => {
  try {
    deleteSelected.value.Sequence = sequence

    const result = await Swal.fire({
      icon: 'info',
      title: 'Are you sure?',
      text: 'want to delete this item?',
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
      const response = await HandleDelete()
      if (response !== 400) {
        resetSearchItem()
        checkBoxChange()
        await ToastSwal.fire({ icon: 'success', text: 'Delete item successful!' })
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      resetSearchItem()
      checkBoxChange()
      await ToastSwal.fire({ icon: 'error', text: 'Delete item canceled' })
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const removeAllSelectedItems = async () => {
  try {
    const result = await Swal.fire({
      icon: 'info',
      title: 'Are you sure?',
      text: 'want to remove all items?',
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
      const response = await HandleAllDelete()
      if (response !== 400) {
        resetSearchItem()
        checkBoxChange()
        await ToastSwal.fire({ icon: 'success', text: 'Remove all items successful!' })
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      resetSearchItem()
      checkBoxChange()
      await ToastSwal.fire({ icon: 'error', text: 'Remove all items canceled' })
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const searchAvailable = async (searchQueryAvailableItem) => {
  try {
    searchAvailableItems.value = useTopIssueStore.searchAvailableItems(searchQueryAvailableItem)
    checkBoxChange()
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const searchSelected = async (searchQuerySelectedItems) => {
  try {
    checkBoxChange()
    searchSelectedItems.value = useTopIssueStore.searchSelectedItems(searchQuerySelectedItems)
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const updateSequence = async (move, currentSequence, length) => {
  try {
    const currentSequenceInt = parseInt(currentSequence)
    const upSequence = currentSequenceInt - 1
    const downSequence = currentSequenceInt + 1

    if (move == 'up' && upSequence != 0) {
      editSequence.value.Current_Sequence = currentSequenceInt
      editSequence.value.New_Sequence = upSequence
      await HandleRePublish()
      checkBoxChange()
    } else if (move == 'down' && currentSequence < length) {
      editSequence.value.Current_Sequence = currentSequenceInt
      editSequence.value.New_Sequence = downSequence
      await HandleRePublish()
      checkBoxChange()
    } else {
      Swal.fire({
        icon: 'error',
        title: 'Error',
        text: 'Sequence Sudah Berada Pada Batas Maksimal/Minimal',
        confirmButtonColor: '#d33'
      })
    }
  } catch (error) {
    console.error('Terjadi kesalahan:', error)
  }
}

const checkBoxChange = () => {
  if (isActiveYesToggle.value == true && isActiveNoToggle.value == true) {
    fetchAvailableItems(null)
    fetchSelectedItems(null)
  } else if (isActiveYesToggle.value == true) {
    fetchAvailableItems(true)
    fetchSelectedItems(true)
  } else if (isActiveNoToggle.value == true) {
    fetchAvailableItems(false)
    fetchSelectedItems(false)
  } else {
    availableItems.value = []
    selectedItems.value = []
  }
}

const fetchSelectedItems = async (is_Active) => {
  try {
    filterSelected.value.Is_Active = is_Active
    await GetTopIssueSelectedByFilter()
    selectedItems.value = top_issue_selected.value
    await useTopIssueStore.setSelectedItems(selectedItems.value)
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

const fetchAvailableItems = async (is_Active) => {
  try {
    filterAvailable.value.Is_Active = is_Active
    await GetTopIssueAvailableByFilter()
    availableItems.value = top_issue_available.value
    await useTopIssueStore.setAvailableItems(availableItems.value)
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

function resetSearchItem() {
  searchQueryAvailableItem.value = ''
  searchQuerySelectedItem.value = ''
}
onMounted(() => {
  checkBoxChange()
})
</script>

<template>
  <div class="flex items-center align-middle gap-3 mb-3 bg-tea">
    <p class="text-sm">Categories</p>

    <span> > </span>
    <p class="text-sm text-orange-400">Top Issue</p>
  </div>

  <div class="w-full rounded-lg bg-white min-h-[25%] max-h-[96%] box-border flex flex-col">
    <div class="flex gap-2.5 justify-between border-b-2 py-5 px-8">
      <div class="flex items-start space-x-2">
        <h1 class="basis-[`100%] text-2xl font-bold text-green-800">Top Issue</h1>
      </div>
      <div class="flex items-end space-x-2">
        <OptionButton>
          <template #default> Active </template>
          <template #options>
            <label>
              <input
                v-model="isActiveNoToggle"
                @change="checkBoxChange()"
                type="checkbox"
                name="status"
              />
              <span class="ml-3">No</span>
            </label>
            <label>
              <input
                v-model="isActiveYesToggle"
                @change="checkBoxChange()"
                type="checkbox"
                name="status"
              />
              <span class="ml-3">Yes</span>
            </label>
          </template>
        </OptionButton>

        <button
          class="min-w-36 rounded-3xl border p-2 active:scale-95"
          :class="
            availableItems.length == 0
              ? 'bg-[#f5f5f5] text-[#90958f] border-[#e3e3e3]'
              : 'bg-white  hover:border-white hover:text-white hover:bg-green-700 text-green-700 border-green-700 '
          "
          @click="addAllSelected"
          :disabled="availableItems.length == 0"
        >
          Add All
        </button>
        <button
          class="min-w-36 rounded-3xl border text-green-700 border-green-700 p-2 active:scale-95"
          :class="
            selectedItems.length == 0
              ? 'bg-[#f5f5f5] text-[#90958f] border-[#e3e3e3]'
              : 'bg-white  hover:border-white hover:text-white hover:bg-green-700 text-green-700 border-green-700 '
          "
          @click="removeAllSelectedItems"
          :disabled="selectedItems.length == 0"
        >
          Remove All
        </button>
      </div>
    </div>

    <div class="overflow-y-auto max-h-[80%]">
      <div class="w-full rounded-lg bg-white min-h-[25%] max-h-[96%] box-border flex flex-col">
        <div class="p-10">
          <!-- start content -->
          <div class="flex justify-between font-bold space-x-2">
            <div class="flex-grow pr-2 pb-2">
              <div class="relative">
                <span class="text-green-700">Available Items:</span>
              </div>
            </div>
            <div class="flex-grow pl-2 pb-2">
              <div class="relative">
                <span class="text-green-700">Selected Items:</span>
              </div>
            </div>
          </div>
          <div class="flex justify-between space-x-2">
            <!-- Kolom Pencarian Kiri -->
            <!-- Kolom Pencarian Kiri -->

            <div class="flex-grow pr-2 pb-4">
              <div class="relative">
                <input
                  type="search"
                  v-model="searchQueryAvailableItem"
                  @input.prevent="searchAvailable(searchQueryAvailableItem)"
                  class="block pt-2 pb-2 pl-10 pr-10 w-full z-20 text-sm text-gray-500 bg-gray-50 rounded-3xl border border-gray-300 outline-none"
                  placeholder="Search..."
                  required
                />
                <button
                  type="submit"
                  class="absolute top-0 left-0 h-full p-2.5 text-sm font-medium text-white bg-transparent rounded-l-lg"
                >
                  <svg
                    class="w-5 h-5 text-gray-500 absolute left-3 top-1/2 transform -translate-y-1/2"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M21 21l-5-5m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
                    ></path>
                  </svg>
                  <span class="sr-only">Search</span>
                </button>
              </div>
            </div>

            <!-- Kolom Pencarian Kanan -->
            <div class="flex-grow pl-2 pb-4">
              <div class="relative">
                <input
                  type="search"
                  v-model="searchQuerySelectedItem"
                  @input.prevent="searchSelected(searchQuerySelectedItem)"
                  class="block pt-2 pb-2 pl-10 pr-10 w-full z-20 text-sm text-gray-500 bg-gray-50 rounded-3xl border border-gray-300 outline-none"
                  placeholder="Search..."
                  required
                />
                <button
                  type="submit"
                  class="absolute top-0 left-0 h-full p-2.5 text-sm font-medium text-white bg-transparent rounded-l-lg"
                >
                  <svg
                    class="w-5 h-5 text-gray-500 absolute left-3 top-1/2 transform -translate-y-1/2"
                    fill="none"
                    stroke="currentColor"
                    viewBox="0 0 24 24"
                    xmlns="http://www.w3.org/2000/svg"
                  >
                    <path
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M21 21l-5-5m2-5a7 7 0 11-14 0 7 7 0 0114 0z"
                    ></path>
                  </svg>
                  <span class="sr-only">Search</span>
                </button>
              </div>
            </div>
          </div>

          <div class="flex justify-between space-x-4">
            <!-- Kolom Kiri -->

            <div
              class="flex-grow p-4 min-h-[150px] max-w-[49%] max-h-[280px] overflow-y-auto border border-gray-300 rounded-2xl"
            >
              <div v-if="searchQueryAvailableItem.length > 0" class="relative">
                <div v-for="item in searchAvailableItems" :key="item">
                  <span
                    class="block text-sm mb-2 rounded-xl text-gray-500 bg-gray-100 hover:bg-[#ffdab3] hover:text-[#2c7b4b] hover:font-semibold pt-1 pb-1 pl-4 pr-4"
                    style="display: flex; justify-content: space-between; align-items: center"
                  >
                    <span :class="{ 'line-through': !item.is_active }">{{ item.name }}</span>
                    <button v-if="item.is_active" @click="addNewSelected(item.uid)">
                      <IconAddNew class="h-5 w-5 fill-gray-600 hover:fill-[#2c7b4b]" />
                    </button>
                  </span>
                </div>
              </div>
              <div v-else class="relative">
                <div v-for="item in availableItems" :key="item">
                  <span
                    class="block text-sm mb-2 rounded-xl text-gray-500 bg-gray-100 hover:bg-[#ffdab3] hover:text-[#2c7b4b] hover:font-semibold pt-1 pb-1 pl-4 pr-4"
                    style="display: flex; justify-content: space-between; align-items: center"
                  >
                    <span :class="{ 'line-through': !item.is_active }">{{ item.name }}</span>
                    <button v-if="item.is_active" @click="addNewSelected(item.uid)">
                      <IconAddNew class="h-5 w-5 fill-gray-600 hover:fill-[#2c7b4b]" />
                    </button>
                  </span>
                </div>
              </div>
            </div>

            <!-- Kolom Kanan -->
            <div
              class="flex-grow p-4 min-h-[150px] max-w-[49%] max-h-[280px] overflow-y-auto border border-gray-300 rounded-2xl"
            >
              <div v-if="searchQuerySelectedItem.length > 0" class="relative">
                <div v-for="item in searchSelectedItems" :key="item">
                  <span
                    class="block text-sm mb-2 rounded-xl text-gray-500 bg-gray-100 hover:bg-[#ffdab3] hover:text-[#2c7b4b] hover:font-semibold pt-1 pb-1 pl-4 pr-4"
                    style="display: flex; justify-content: space-between; align-items: center"
                  >
                    <span :class="{ 'line-through': !item.is_active }">{{ item.name }}</span>
                    <div class="justify-end flex">
                      <button @click="deleteSelectedItem(item.sequence)">
                        <IconDelete
                          class="h-5 w-5 fill-red-700 hover:fill-white hover:bg-red-700 hover:rounded-full"
                        />
                      </button>
                    </div>
                  </span>
                </div>
              </div>
              <div v-else class="relative">
                <div v-for="(item, index) in selectedItems" :key="item">
                  <span
                    class="block text-sm mb-2 rounded-xl text-gray-500 bg-gray-100 hover:bg-[#ffdab3] hover:text-[#2c7b4b] hover:font-semibold pt-1 pb-1 pl-4 pr-4"
                    style="display: flex; justify-content: space-between; align-items: center"
                  >
                    <span :class="{ 'line-through': !item.is_active }">{{ item.name }}</span>
                    <div class="justify-end flex">
                      <button
                        v-if="
                          item.is_active &&
                          selectedItems.length > 1 &&
                          index != selectedItems.length - 1
                        "
                        @click="updateSequence('down', item.sequence, selectedItems.length)"
                      >
                        <IconMoveDown class="w-5 h-5 fill-gray-600 hover:fill-[#2c7b4b]" />
                      </button>
                      <button
                        v-if="item.is_active && selectedItems.length > 1 && index != 0"
                        @click="updateSequence('up', item.sequence, selectedItems.length)"
                      >
                        <IconMoveUp class="w-5 h-5 fill-gray-600 hover:fill-[#2c7b4b]" />
                      </button>
                      <button @click="deleteSelectedItem(item.sequence)">
                        <IconDelete
                          class="h-5 w-5 fill-red-700 hover:fill-white hover:bg-red-700 hover:rounded-full"
                        />
                      </button>
                    </div>
                  </span>
                </div>
              </div>
            </div>
          </div>
          <!-- end content -->
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped></style>
