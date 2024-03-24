<script setup>
import {
  GetTopIssueAvailableByFilter,
  filterAvailable,
  top_issue_available,
  GetTopIssueSelectedByFilter,
  top_issue_selected,
  filterSelected
} from '@/components/pages/category/useCategoryTopIssue.js'

import {
  HandlePublishSelected,
  newSelected
} from '@/components/pages/category/postCategoryTopIssue.js'

import { HandleDelete, deleteSelected } from '@/components/pages/category/deleteCategorySelected.js'
import { HandleAllDelete } from '@/components/pages/category/deleteAllCategorySelected.js'
import { HandlePublishAllSelected } from '@/components/pages/category/postAllCategorySelected.js'
import { HandleRePublish, editSequence } from '@/components/pages/category/patchSequenceSelected.js'
import { ref, onMounted, computed } from 'vue'
import { useStore } from '@/stores/topIssueStore.js'
import Swal from 'sweetalert2'
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import OptionButton from '@/components/buttons/OptionButton.vue'
import IconSearch from '@/components/icons/IconSearch.vue'
import AbortButton from '@/components/buttons/AbortButton.vue'

const useTopIssueStore = useStore()
const searchQueryAvailableItem = ref('')
const searchQuerySelectedItem = ref('')
const availableItems = ref([])
const searchAvailableItems = ref([])
const searchSelectedItems = ref([])
const selectedItems = ref([])
const createBy = ref('dummyData')
const modifiedBy = ref('dummyData')
const isActiveYesToggle = ref(true)
const isActiveNoToggle = ref(true)
const isActiveDropDownOpen = ref(false)

const addNewSelected = async (uid_Bot_Category) => {
  try {
    newSelected.value.Uid_Bot_Category = uid_Bot_Category

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
      const response = await HandlePublishSelected()
      if (response !== 400) {
        Swal.fire({
          icon: 'success',
          title: 'Add new selected item successful!',
          confirmButtonColor: '#2c7b4b'
        })
        resetSearchItem()
        checkBoxChange()
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      Swal.fire({
        icon: 'info',
        title: 'Add new selected item canceled',
        confirmButtonColor: '#2c7b4b'
      })
      resetSearchItem()
      checkBoxChange()
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const addAllSelected = async () => {
  try {
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
      const response = await HandlePublishAllSelected()
      if (response !== 400) {
        Swal.fire({
          icon: 'success',
          title: 'Add all items to new selected item successful!',
          confirmButtonColor: '#2c7b4b'
        })
        resetSearchItem()
        checkBoxChange()
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      Swal.fire({
        icon: 'info',
        title: 'Add all items to new selected item canceled',
        confirmButtonColor: '#2c7b4b'
      })
      resetSearchItem()
      checkBoxChange()
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const deleteSelectedItem = async (sequence) => {
  try {
    deleteSelected.value.Sequence = sequence

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
      const response = await HandleDelete()
      if (response !== 400) {
        Swal.fire({
          icon: 'success',
          title: 'Remove item from new selected item successful!',
          confirmButtonColor: '#2c7b4b'
        })
        resetSearchItem()
        checkBoxChange()
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      Swal.fire({
        icon: 'info',
        title: 'Remove item from new selected item canceled',
        confirmButtonColor: '#2c7b4b'
      })
      resetSearchItem()
      checkBoxChange()
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const removeAllSelectedItems = async () => {
  try {
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
      const response = await HandleAllDelete()
      if (response !== 400) {
        Swal.fire({
          icon: 'success',
          title: 'Remove all items from new selected item successful!',
          confirmButtonColor: '#2c7b4b'
        })
        resetSearchItem()
        checkBoxChange()
      } else {
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      Swal.fire({
        icon: 'info',
        title: 'Remove all items from new selected item canceled',
        confirmButtonColor: '#2c7b4b'
      })
      resetSearchItem()
      checkBoxChange()
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
      const response = await HandleRePublish()
      checkBoxChange()
    } else if (move == 'down' && currentSequence < length) {
      editSequence.value.Current_Sequence = currentSequenceInt
      editSequence.value.New_Sequence = downSequence
      const response = await HandleRePublish()
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
                v-model="isActiveYesToggle"
                @change="checkBoxChange()"
                type="checkbox"
                name="status"
              />
              <span class="ml-3">No</span>
            </label>
            <label>
              <input
                v-model="isActiveNoToggle"
                @change="checkBoxChange()"
                type="checkbox"
                name="status"
              />
              <span class="ml-3">Yes</span>
            </label>
          </template>
        </OptionButton>

        <button
          class="min-w-36 font-semibold rounded-3xl border text-green-700 bg-white border-green-700 p-2 hover:border-white hover:text-white hover:bg-green-700 active:scale-95"
          @click="addAllSelected"
        >
          Add All
        </button>
        <button
          class="min-w-36 font-semibold rounded-3xl border text-green-700 bg-white border-green-700 p-2 hover:border-white hover:text-white hover:bg-green-700 active:scale-95"
          @click="removeAllSelectedItems"
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
              class="flex-grow p-4 min-h-[150px] max-w-[49%] max-h-[250px] overflow-y-auto border border-gray-300 rounded-2xl"
            >
              <div v-if="searchQueryAvailableItem.length > 0" class="relative">
                <div v-for="(item, index) in searchAvailableItems">
                  <span
                    class="block text-sm mb-2 rounded-xl text-gray-500 bg-gray-100 hover:bg-green-200 hover:text-[#77a383] pt-1 pb-1 pl-4 pr-4"
                    style="display: flex; justify-content: space-between; align-items: center"
                  >
                    <span :class="{ 'line-through': !item.is_active }">{{ item.name }}</span>
                    <button v-if="item.is_active" @click="addNewSelected(item.uid)">
                      <svg
                        class="h-5 w-5 fill-gray-600 hover:fill-[#77a383]"
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 -960 960 960"
                      >
                        <path
                          d="M440-280h80v-160h160v-80H520v-160h-80v160H280v80h160v160Zm40 200q-83 0-156-31.5T197-197q-54-54-85.5-127T80-480q0-83 31.5-156T197-763q54-54 127-85.5T480-880q83 0 156 31.5T763-763q54 54 85.5 127T880-480q0 83-31.5 156T763-197q-54 54-127 85.5T480-80Zm0-80q134 0 227-93t93-227q0-134-93-227t-227-93q-134 0-227 93t-93 227q0 134 93 227t227 93Zm0-320Z"
                        />
                      </svg>
                    </button>
                  </span>
                </div>
              </div>
              <div v-else class="relative">
                <div v-for="(item, index) in availableItems">
                  <span
                    class="block text-sm mb-2 rounded-xl text-gray-500 bg-gray-100 hover:bg-green-200 hover:text-[#77a383] pt-1 pb-1 pl-4 pr-4"
                    style="display: flex; justify-content: space-between; align-items: center"
                  >
                    <span :class="{ 'line-through': !item.is_active }">{{ item.name }}</span>
                    <button v-if="item.is_active" @click="addNewSelected(item.uid)">
                      <svg
                        class="h-5 w-5 fill-gray-600 hover:fill-[#77a383]"
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 -960 960 960"
                      >
                        <path
                          d="M440-280h80v-160h160v-80H520v-160h-80v160H280v80h160v160Zm40 200q-83 0-156-31.5T197-197q-54-54-85.5-127T80-480q0-83 31.5-156T197-763q54-54 127-85.5T480-880q83 0 156 31.5T763-763q54 54 85.5 127T880-480q0 83-31.5 156T763-197q-54 54-127 85.5T480-80Zm0-80q134 0 227-93t93-227q0-134-93-227t-227-93q-134 0-227 93t-93 227q0 134 93 227t227 93Zm0-320Z"
                        />
                      </svg>
                    </button>
                  </span>
                </div>
              </div>
            </div>

            <!-- Kolom Kanan -->
            <div
              class="flex-grow p-4 min-h-[150px] max-w-[49%] max-h-[250px] overflow-y-auto border border-gray-300 rounded-2xl"
            >
              <div v-if="searchQuerySelectedItem.length > 0" class="relative">
                <div v-for="(item, index) in searchSelectedItems">
                  <span
                    class="block text-sm mb-2 rounded-xl text-gray-500 bg-gray-100 hover:bg-green-200 hover:text-[#77a383] pt-1 pb-1 pl-4 pr-4"
                    style="display: flex; justify-content: space-between; align-items: center"
                  >
                    <span :class="{ 'line-through': !item.is_active }">{{ item.name }}</span>
                    <div class="justify-end flex">
                      <button @click="deleteSelectedItem(item.sequence)">
                        <svg
                          class="h-5 w-5 fill-red-700 hover:fill-white hover:bg-red-700 hover:rounded-full"
                          xmlns="http://www.w3.org/2000/svg"
                          viewBox="0 -960 960 960"
                        >
                          <path
                            d="M280-440h400v-80H280v80ZM480-80q-83 0-156-31.5T197-197q-54-54-85.5-127T80-480q0-83 31.5-156T197-763q54-54 127-85.5T480-880q83 0 156 31.5T763-763q54 54 85.5 127T880-480q0 83-31.5 156T763-197q-54 54-127 85.5T480-80Zm0-80q134 0 227-93t93-227q0-134-93-227t-227-93q-134 0-227 93t-93 227q0 134 93 227t227 93Zm0-320Z"
                          />
                        </svg>
                      </button>
                    </div>
                  </span>
                </div>
              </div>
              <div v-else class="relative">
                <div v-for="(item, index) in selectedItems">
                  <span
                    class="block text-sm mb-2 rounded-xl text-gray-500 bg-gray-100 hover:bg-green-200 hover:text-[#77a383] pt-1 pb-1 pl-4 pr-4"
                    style="display: flex; justify-content: space-between; align-items: center"
                  >
                    <span :class="{ 'line-through': !item.is_active }">{{ item.name }}</span>
                    <div class="justify-end flex">
                      <button
                        v-if="item.is_active"
                        @click="updateSequence('down', item.sequence, selectedItems.length)"
                      >
                        <svg
                          class="h-5 w-5 fill-gray-600 hover:fill-[#77a383]"
                          xmlns="http://www.w3.org/2000/svg"
                          viewBox="0 -960 960 960"
                        >
                          <path
                            d="m480-340 180-180-57-56-123 123-123-123-57 56 180 180Zm0 260q-83 0-156-31.5T197-197q-54-54-85.5-127T80-480q0-83 31.5-156T197-763q54-54 127-85.5T480-880q83 0 156 31.5T763-763q54 54 85.5 127T880-480q0 83-31.5 156T763-197q-54 54-127 85.5T480-80Zm0-80q134 0 227-93t93-227q0-134-93-227t-227-93q-134 0-227 93t-93 227q0 134 93 227t227 93Zm0-320Z"
                          />
                        </svg>
                      </button>
                      <button
                        v-if="item.is_active"
                        @click="updateSequence('up', item.sequence, selectedItems.length)"
                      >
                        <svg
                          class="h-5 w-5 fill-gray-600 hover:fill-[#77a383]"
                          xmlns="http://www.w3.org/2000/svg"
                          viewBox="0 -960 960 960"
                        >
                          <path
                            d="m357-384 123-123 123 123 57-56-180-180-180 180 57 56ZM480-80q-83 0-156-31.5T197-197q-54-54-85.5-127T80-480q0-83 31.5-156T197-763q54-54 127-85.5T480-880q83 0 156 31.5T763-763q54 54 85.5 127T880-480q0 83-31.5 156T763-197q-54 54-127 85.5T480-80Zm0-80q134 0 227-93t93-227q0-134-93-227t-227-93q-134 0-227 93t-93 227q0 134 93 227t227 93Zm0-320Z"
                          />
                        </svg>
                      </button>
                      <button @click="deleteSelectedItem(item.sequence)">
                        <svg
                          class="h-5 w-5 fill-red-700 hover:fill-white hover:bg-red-700 hover:rounded-full"
                          xmlns="http://www.w3.org/2000/svg"
                          viewBox="0 -960 960 960"
                        >
                          <path
                            d="M280-440h400v-80H280v80ZM480-80q-83 0-156-31.5T197-197q-54-54-85.5-127T80-480q0-83 31.5-156T197-763q54-54 127-85.5T480-880q83 0 156 31.5T763-763q54 54 85.5 127T880-480q0 83-31.5 156T763-197q-54 54-127 85.5T480-80Zm0-80q134 0 227-93t93-227q0-134-93-227t-227-93q-134 0-227 93t-93 227q0 134 93 227t227 93Zm0-320Z"
                          />
                        </svg>
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
