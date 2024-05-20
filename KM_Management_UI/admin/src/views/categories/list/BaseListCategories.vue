<script setup>
import Swal from 'sweetalert2'

import { ref, onMounted, computed, watchEffect } from 'vue'
import { ToastSwal } from '@/extension/SwalExt.js'
import {
  GetCategoryListByFilter,
  category_list,
  filter
} from '@/components/pages/category/useCategoryList.js'
import { useCategoriesStore } from '@/stores/listCategoriesStore.js'
import SearchModalCategories from '@/views/categories/list/SearchModalCategories.vue'
import ContainerModal from '@/components/modal/ContainerModal.vue'
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import OptionButton from '@/components/buttons/OptionButton.vue'
import IconEdit from '@/components/icons/IconEdit.vue'
import IconSearch from '@/components/icons/IconSearch.vue'
import {
  HandlePublish,
  ResetPostInput,
  errorInput,
  newCategory
} from '@/components/pages/category/postCategoryList.js'
import {
  HandleExcelExport,
  filterExportCategories
} from '@/components/pages/category/postExportExcelCategoryList.js'

import {
  HandleRePublish,
  ResetEditInput,
  errorEdit,
  editCategory
} from '@/components/pages/category/patchCategoryList.js'

const storeCategories = useCategoriesStore()
const firstLayer = ref([])

const isEditModalOpen = ref(false)
const isCreateModalOpen = ref(false)
const isSearchModalOpen = ref(false)
const isToggledEdit = ref(false)
const isToggledCreate = ref(false)
const selectedEditCategory = ref(null)
const selectedEditUid = ref(null)
const selectedCreateCategory = ref(null)
const isActiveYesToggle = ref(false)
const isActiveNoToggle = ref(false)
const hightLightUid = ref()
const errorAddCategory = ref('')
const errorUpdateCategory = ref('')

const openEditModal = (uid, category, is_Active) => {
  isEditModalOpen.value = true
  selectedEditCategory.value = category
  isToggledEdit.value = is_Active
  selectedEditUid.value = uid
}
const closeEditModal = () => {
  errorUpdateCategory.value = ''
  isEditModalOpen.value = false
  ResetEditInput()
}

const openSearchModal = () => {
  isSearchModalOpen.value = true
}
const closeSearchModal = () => {
  isSearchModalOpen.value = false
}
const openCreateModal = () => {
  isCreateModalOpen.value = true
  isToggledCreate.value = true
}
const closeCreateModal = () => {
  errorAddCategory.value = ''
  selectedCreateCategory.value = ''
  isCreateModalOpen.value = false
  ResetPostInput()
}

const fetchFirstLayer = async (isActive) => {
  try {
    filter.value.Uid_Reference = null
    filter.value.Layer = 1
    filter.value.Is_Active = isActive

    await GetCategoryListByFilter()
    firstLayer.value = category_list.value
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

const exportExcel = async () => {
  try {
    await HandleExcelExport()
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

const updateCategory = async (uid, name, is_Active) => {
  try {
    errorUpdateCategory.value = ''
    editCategory.value.Uid = uid
    editCategory.value.Name = name
    editCategory.value.Is_Active = is_Active
    const result = await Swal.fire({
      icon: 'info',
      title: 'Are you sure?',
      text: 'want to update this category?',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
      confirmButtonColor: '#2c7b4b',
      cancelButtonColor: '#d33',
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
        checkBoxChange()
        closeEditModal()
        await ToastSwal.fire({ icon: 'success', text: 'Update category successful!' })
      } else {
        errorUpdateCategory.value = errorEdit.value.name.message
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      await ToastSwal.fire({ icon: 'error', text: 'Update category canceled' })
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const addNewCategory = async (name, is_Active) => {
  try {
    errorAddCategory.value = ''

    newCategory.value.Name = name
    newCategory.value.Layer = 1
    newCategory.value.Is_Active = is_Active
    newCategory.value.Uid_Reference = null

    const result = await Swal.fire({
      icon: 'info',
      title: 'Are you sure?',
      text: 'want to add this category?',
      showCancelButton: true,
      confirmButtonText: 'Yes',
      cancelButtonText: 'No',
      confirmButtonColor: '#2c7b4b',
      cancelButtonColor: '#d33',
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
        checkBoxChange()
        closeCreateModal()
        await ToastSwal.fire({ icon: 'success', text: 'Add new category successful!' })
      } else {
        errorAddCategory.value = errorInput.value.name.message
        Swal.fire({
          icon: 'error',
          title: 'Check your input',
          text: 'There is an error in your input. Please check again.',
          confirmButtonColor: '#d33'
        })
      }
    } else {
      await ToastSwal.fire({ icon: 'error', text: 'Add new category canceled!' })
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const getCategoryFirstLetter = (category) => {
  // Extract the first letter of the category
  return category.charAt(0).toUpperCase()
}

const navigateToFirstUid = (name, uid, is_Active) => {
  storeCategories.setSelectedFirstUid(name, uid, is_Active)
}

const checkBoxChange = () => {
  if (isActiveYesToggle.value == true && isActiveNoToggle.value == true) {
    fetchFirstLayer(null) // Both checkboxes are selected, get all categories
  } else if (isActiveYesToggle.value == true) {
    fetchFirstLayer(true) // Only the "Yes" checkbox is selected, get inactive categories
  } else if (isActiveNoToggle.value == true) {
    fetchFirstLayer(false) // Only the "No" checkbox is selected, get active categories
  } else {
    // firstLayer.value = []
    fetchFirstLayer(null)
  }
}

const groupedFirstLayer = computed(() => {
  // Group items by the first letter of their names
  const groups = {}
  for (const first of firstLayer.value) {
    const firstLetter = getCategoryFirstLetter(first.name)
    if (!groups[firstLetter]) {
      groups[firstLetter] = []
    }
    groups[firstLetter].push(first)
  }

  // Convert the groups object into an array of objects
  const groupedArray = Object.keys(groups).map((letter) => ({
    letter,
    items: groups[letter]
  }))

  // Create three groups based on the specific index pattern
  const group1 = []
  const group2 = []
  const group3 = []

  groupedArray.forEach((group, index) => {
    if (index % 3 === 0) {
      group1.push(group)
    } else if (index % 3 === 1) {
      group2.push(group)
    } else if (index % 3 === 2) {
      group3.push(group)
    }
  })

  return [group1, group2, group3]
})

const getHightlight = async () => {
  hightLightUid.value = await storeCategories.getHightlightUid
  closeSearchModal()
}

const filterExportExcel = () => {
  if (isActiveYesToggle.value == true && isActiveNoToggle.value == true) {
    filterExportCategories.value = null
  } else if (isActiveYesToggle.value == true) {
    filterExportCategories.value = true
  } else if (isActiveNoToggle.value == true) {
    filterExportCategories.value = false
  }
}

watchEffect(() => {
  getHightlight()
  filterExportExcel()
})

onMounted(() => {
  checkBoxChange()
})
</script>

<template>
  <div class="flex items-center align-middle gap-3 mb-3 bg-tea">
    <p class="text-sm text-[#999999]">Categories</p>

    <span> > </span>
    <p class="text-sm text-orange-400">List</p>
  </div>

  <div class="w-full rounded-lg bg-white min-h-[25%] max-h-[96%] box-border flex flex-col">
    <div class="flex gap-2.5 justify-between border-b-2 py-5 px-8">
      <div class="flex items-start space-x-2">
        <h1 class="basis-[85%] text-2xl font-bold text-green-800">Categories List</h1>
        <span class="rounded-xl min-w-14 px-2 py-1 bg-blue-50 text-[10px]"> Layer 1 </span>
      </div>
      <div class="flex items-end space-x-2">
        <button
          @click="openSearchModal"
          class="min-w-40 flex items-center rounded-3xl border p-2 px-4 gap-2 text-green-700 bg-white border-green-700 hover:border-white hover:bg-[#eeeeee] active:scale-95"
        >
          <IconSearch />
          Search
        </button>

        <button
          @click="exportExcel"
          :disabled="isActiveYesToggle == false && isActiveNoToggle == false"
          class="min-w-36 rounded-3xl border text-green-700 bg-white border-green-700 p-2 hover:border-white hover:bg-[#eeeeee] active:scale-95"
          :class="{ 'bg-gray-300': isActiveYesToggle == false && isActiveNoToggle == false }"
        >
          Export to Excel
        </button>
        <OptionButton>
          <template #default> Active </template>
          <template #options>
            <label>
              <input
                type="checkbox"
                name="status"
                v-model="isActiveNoToggle"
                @change="checkBoxChange()"
              />
              <span class="ml-3">No</span>
            </label>
            <label>
              <input
                type="checkbox"
                name="status"
                v-model="isActiveYesToggle"
                @change="checkBoxChange()"
              />
              <span class="ml-3">Yes</span>
            </label>
          </template>
        </OptionButton>
        <PrimaryButton @click="openCreateModal"> Create </PrimaryButton>
      </div>
    </div>

    <!-- masonry grid div -->

    <div class="overflow-y-auto max-h-[80%] grid grid-cols-2 md:grid-cols-3 gap-5 p-10">
      <div v-for="(subgroup, groupIndex) in groupedFirstLayer" :key="groupIndex">
        <div
          v-for="(group, index) in subgroup"
          :key="index"
          class="bg-[#fef9f2] border border-orange-400 min-h-[180px] w-[100%] p-4 rounded-tr-3xl rounded-bl-3xl grid grid-gap-4 mb-5"
        >
          <div v-for="(item, itemIndex) in group.items" :key="item.uid">
            <h1
              v-if="itemIndex === 0"
              class="flex text-xl font-semibold italic text-[#2c7b4b] mb-8"
            >
              {{ group.letter + '_' }}
            </h1>
            <div class="mb-4 flex items-center">
              <button class="mr-2" @click="openEditModal(item.uid, item.name, item.is_active)">
                <IconEdit class="w-4 h-4 hover:fill-[#2c7b4b] fill-[#888888]" />
              </button>

              <RouterLink
                class="max-w-[90%] break-words"
                :to="{ name: 'categories-second-layer', params: { secondLayer: item.name } }"
                @click="navigateToFirstUid(item.name, item.uid, item.is_active)"
              >
                <span
                  :class="{
                    'mr-2 text-gray-500 underline hover:font-semibold hover:text-[#2c7b4b]':
                      item.is_active,
                    'mr-2 line-through text-gray-500 hover:font-semibold hover:text-[#2c7b4b]':
                      !item.is_active,
                    'mr-2 text-gray-500 hover:font-semibold bg-yellow-300 pl-2 pr-2 rounded-2xl drop-shadow-2xl hover:text-slate-500':
                      hightLightUid == item.uid
                  }"
                >
                  {{ item.name }}
                </span>
              </RouterLink>
            </div>
          </div>
        </div>
      </div>
      <div v-show="firstLayer.length === 0">
        <div class="relative bg-[#eeeeee] w-[100%] pl-6 pb-4 pt-4 rounded-tr-3xl rounded-bl-3xl">
          <span class="italic mr-2 text-gray-500">Data Not Available</span>
        </div>
      </div>

      <!-- masonry grid image -->
      <!-- <div v-for="(imageGroup, index) in images" :key="index">
        <div v-for="(image, imgIndex) in imageGroup" :key="imgIndex">
          <img
            class="border border-orange-400 h-fit w-[100%] p-4 rounded-tr-3xl rounded-bl-3xl grid grid-gap-4"
            :src="image"
            alt=""
          />
        </div>
      </div> -->
    </div>
  </div>

  <ContainerModal v-if="isCreateModalOpen" @close-modal="closeCreateModal" style="z-index: 1001">
    <div class="flex justify-between items-center w-[450px] sticky top-0 pt-5 pb-5">
      <div class="flex items-start space-x-2">
        <h1>
          <span class="font-semibold text-2xl">New Categories</span>
        </h1>
        <span class="rounded-3xl pl-2 pr-2 pb-1 pt-1 bg-blue-50 text-[10px] text-gray-500"
          >Layer 1</span
        >
      </div>
      <div class="flex items-center space-x-2">
        <button
          @click="closeCreateModal"
          class="min-w-24 hover:bg-red-500 hover:text-white items-center text-red-500 rounded-3xl p-2 active:scale-95"
        >
          Cancel
        </button>
        <PrimaryButton @click="addNewCategory(selectedCreateCategory, isToggledCreate)"
          >Save</PrimaryButton
        >
      </div>
    </div>
    <form class="w-full pt-5">
      <div class="mb-4">
        <div class="pt-2 pb-2">
          <label class="block text-gray-400 mb-2"
            >Description<span class="text-red-500">*</span></label
          >
          <input
            v-model="selectedCreateCategory"
            type="text"
            class="w-full px-3 py-2 border rounded-xl focus:outline focus:outline-[#2c7b4b]"
          />
          <div v-if="errorAddCategory.length != ''" class="relative text-red-500 mt-2 ml-2">
            <div>
              {{ errorAddCategory }}
            </div>
          </div>
        </div>
        <div class="pt-2 pb-2">
          <label class="block text-gray-400 mb-2">Active</label>
          <label for="toggleSlideCreate" class="flex items-center cursor-pointer">
            <!-- toggle -->
            <div class="relative">
              <!-- input -->
              <input
                v-model="isToggledCreate"
                type="checkbox"
                id="toggleSlideCreate"
                class="sr-only"
              />
              <!-- line -->
              <div
                :class="{
                  'bg-[#579D3F]': isToggledCreate,
                  'bg-gray-600': !isToggledCreate
                }"
                class="block w-14 h-8 rounded-full transition"
              ></div>
              <!-- dot -->
              <div
                :class="{
                  'translate-x-6': isToggledCreate,
                  'translate-x-0': !isToggledCreate
                }"
                class="dot absolute left-1 top-1 bg-white w-6 h-6 rounded-full transition transform"
              ></div>
            </div>
            <!-- label -->
            <div class="ml-3 text-gray-700 font-medium"></div>
          </label>
        </div>
      </div>
    </form>
  </ContainerModal>

  <ContainerModal v-if="isEditModalOpen" @close-modal="closeEditModal" style="z-index: 1001">
    <div class="flex justify-between items-center w-[450px] sticky top-0 pt-5 pb-5">
      <div class="flex items-start space-x-2">
        <h1>
          <span class="font-semibold text-2xl">Edit Categories</span>
        </h1>
        <span class="rounded-3xl pl-2 pr-2 pb-1 pt-1 bg-blue-50 text-[10px] text-gray-500"
          >Layer 1</span
        >
      </div>
      <div class="flex items-center space-x-2">
        <button
          @click="closeEditModal"
          class="min-w-24 hover:bg-red-500 hover:text-white items-center text-red-500 rounded-3xl p-2 active:scale-95"
        >
          Cancel
        </button>
        <PrimaryButton @click="updateCategory(selectedEditUid, selectedEditCategory, isToggledEdit)"
          >Save</PrimaryButton
        >
      </div>
    </div>
    <form class="w-full pt-5">
      <div class="mb-4">
        <div class="pt-2 pb-2">
          <label class="block text-gray-400 mb-2"
            >Description<span class="text-red-500">*</span></label
          >
          <input
            v-model="selectedEditCategory"
            type="text"
            class="w-full px-3 py-2 border rounded-xl focus:outline focus:outline-[#2c7b4b]"
          />
          <div v-if="errorUpdateCategory != ''" class="relative text-red-500 mt-2 ml-2">
            <div>
              {{ errorUpdateCategory }}
            </div>
          </div>
        </div>
        <div class="pt-2 pb-2">
          <label class="block text-gray-400 mb-2">Active</label>
          <label for="toggleSlideEdit" class="flex items-center cursor-pointer">
            <!-- toggle -->
            <div class="relative">
              <!-- input -->
              <input v-model="isToggledEdit" type="checkbox" id="toggleSlideEdit" class="sr-only" />
              <!-- line -->
              <div
                :class="{
                  'bg-[#579D3F]': isToggledEdit,
                  'bg-gray-600': !isToggledEdit
                }"
                class="block w-14 h-8 rounded-full transition"
              ></div>
              <!-- dot -->
              <div
                :class="{
                  'translate-x-6': isToggledEdit,
                  'translate-x-0': !isToggledEdit
                }"
                class="dot absolute left-1 top-1 bg-white w-6 h-6 rounded-full transition transform"
              ></div>
            </div>
            <!-- label -->
            <div class="ml-3 text-gray-700 font-medium"></div>
          </label>
        </div>
      </div>
    </form>
  </ContainerModal>

  <SearchModalCategories
    v-if="isSearchModalOpen"
    @close-modal="closeSearchModal"
    style="z-index: 1001"
  />
</template>

<style scoped>
::-webkit-scrollbar {
  width: 0.15rem;
}

/* Track */
::-webkit-scrollbar-track {
  box-shadow: inset 0 0 5px grey;
}

/* Handle */
::-webkit-scrollbar-thumb {
  background: rgba(94, 109, 92, 0.6);
}

/* Handle on hover */
::-webkit-scrollbar-thumb:hover {
  background: rgba(94, 109, 92, 1);
}

.swal2-confirm {
  border-radius: 35px; /* Sesuaikan nilai border-radius sesuai kebutuhan Anda */
}
</style>
