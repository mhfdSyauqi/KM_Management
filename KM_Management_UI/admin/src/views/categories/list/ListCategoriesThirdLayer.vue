<script setup>
import Swal from 'sweetalert2'
import { useCategoriesStore } from '@/stores/listCategoriesStore.js'
import {
  GetCategoryListByFilter,
  category_list,
  filter
} from '@/components/pages/category/useCategoryList.js'
import { ToastSwal } from '@/extension/SwalExt.js'
import { ref, onMounted, computed, watchEffect } from 'vue'
import SearchModalCategories from '@/views/categories/list/SearchModalCategories.vue'
import { useRouter } from 'vue-router'
import ContainerModal from '@/components/modal/ContainerModal.vue'
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import OptionButton from '@/components/buttons/OptionButton.vue'
import IconSearch from '@/components/icons/IconSearch.vue'
import IconEdit from '@/components/icons/IconEdit.vue'
import {
  HandlePublish,
  ResetPostInput,
  errorInput,
  newCategory
} from '@/components/pages/category/postCategoryList.js'
import {
  HandleRePublish,
  ResetEditInput,
  errorEdit,
  editCategory
} from '@/components/pages/category/patchCategoryList.js'
import {
  HandleExcelExport,
  filterExportCategories
} from '@/components/pages/category/postExportExcelCategoryList.js'

const router = useRouter()
const storeCategories = useCategoriesStore()
const thirdLayer = ref([])
const firstCategory = ref(router.currentRoute.value.params.secondLayer)
const secondCategory = ref(router.currentRoute.value.params.thirdLayer)
const firstActive = ref(JSON.parse(storeCategories.getFirstActive(firstCategory.value)))
const secondActive = ref(JSON.parse(storeCategories.getSecondActive(secondCategory.value)))
const secondUid = ref(storeCategories.getFirstUid(secondCategory.value))

const isCreateModalOpen = ref(false)
const isEditModalOpen = ref(false)
const selectedEditCategory = ref(null)
const selectedEditUid = ref(null)
const selectedCreateCategory = ref(null)
const isToggledEdit = ref(false)
const isToggledCreate = ref(false)
const isSearchModalOpen = ref(false)
const hightLightUid = ref()
const isActiveYesToggle = ref(true)
const isActiveNoToggle = ref(true)
const errorAddCategory = ref('')
const errorUpdateCategory = ref('')

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
  isCreateModalOpen.value = false
  selectedCreateCategory.value = ''
  ResetPostInput()
}

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

const fetchThirdLayer = async (is_Active) => {
  try {
    filter.value.Uid_Reference = secondUid.value
    filter.value.Layer = 3
    filter.value.Is_Active = is_Active
    await GetCategoryListByFilter()
    thirdLayer.value = category_list.value
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

const addNewCategory = async (name, is_Active) => {
  try {
    errorAddCategory.value = ''

    newCategory.value.Name = name
    newCategory.value.Layer = 3
    newCategory.value.Is_Active = is_Active
    newCategory.value.Uid_Reference = secondUid.value

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
      await ToastSwal.fire({ icon: 'error', text: 'Update category canceled!' })
    }
  } catch (error) {
    console.error('Terjadi Kesalahan :', error)
  }
}

const getCategoryThirdtLetter = (category) => {
  // Extract the third letter of the category
  return category.charAt(0).toUpperCase()
}

const checkBoxChange = () => {
  if (isActiveYesToggle.value == true && isActiveNoToggle.value == true) {
    fetchThirdLayer(null) // Both checkboxes are selected, get all categories
  } else if (isActiveYesToggle.value == true) {
    fetchThirdLayer(true) // Only the "Yes" checkbox is selected, get inactive categories
  } else if (isActiveNoToggle.value == true) {
    fetchThirdLayer(false) // Only the "No" checkbox is selected, get active categories
  } else {
    thirdLayer.value = []
  }
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

const groupedThirdLayer = computed(() => {
  // Group items by the first letter of their names
  const groups = {}
  for (const third of thirdLayer.value) {
    const firstLetter = getCategoryThirdtLetter(third.name)
    if (!groups[firstLetter]) {
      groups[firstLetter] = []
    }
    groups[firstLetter].push(third)
  }

  // Convert the groups object into an array of objects
  const groupedArray = Object.keys(groups).map((letter) => ({
    letter,
    items: groups[letter]
  }))

  return groupedArray
})

const getHightlight = async () => {
  hightLightUid.value = await storeCategories.getHightlightUid
  closeSearchModal()
}

const getRowSpan = (itemCount) => {
  if (itemCount > 3) {
    if (groupedThirdLayer.value.length <= 3) {
      if (itemCount % 3 === 0) {
        return itemCount
      } else {
        return Math.floor(itemCount) + 1
      }
    } else if (groupedThirdLayer.value.length > 3) {
      if (itemCount % 3 === 0) {
        return itemCount / 3
      } else {
        return Math.floor(itemCount / 3) + 1
      }
    }
  }
}

const exportExcel = async () => {
  try {
    await HandleExcelExport()
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

onMounted(() => {
  checkBoxChange()
})
watchEffect(() => {
  filterExportExcel()
  getHightlight()
})
</script>

<template>
  <div class="flex items-center align-middle gap-3 mb-3 bg-tea">
    <p class="text-sm text-[#999999]">Categories</p>
    <span> > </span>
    <RouterLink :to="{ name: 'categories' }">
      <p class="text-sm text-[#999999]">List</p>
    </RouterLink>
    <span> > </span>
    <RouterLink :to="{ name: 'categories-second-layer', params: { secondLayer: firstCategory } }">
      <p class="text-sm text-[#999999]">{{ firstCategory }}</p>
    </RouterLink>
    <span> > </span>
    <p class="text-sm text-orange-400">{{ secondCategory }}</p>
  </div>

  <div class="w-full rounded-lg bg-white min-h-[25%] max-h-[96%] box-border flex flex-col">
    <div class="flex gap-2.5 justify-between border-b-2 py-5 px-8">
      <div class="flex items-start space-x-2">
        <h1 class="basis-[85%] text-2xl font-bold text-green-800">Categories List</h1>
        <span class="rounded-xl min-w-14 px-2 py-1 bg-blue-50 text-[10px]"> Layer 3 </span>

        <span
          v-if="secondActive === false"
          class="rounded-xl min-w-fit px-2 py-1 bg-red-200 text-red-700 text-[10px]"
        >
          Inactive Category
        </span>
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
        <PrimaryButton
          @click="openCreateModal"
          v-if="firstActive === true && secondActive === true"
        >
          Create
        </PrimaryButton>
        <div v-else class="mr-2 relative flex flex-col items-center group">
          <button
            class="min-w-24 group border border-[#77a383] text-white bg-[#77a383] rounded-3xl p-2"
          >
            Create
          </button>
          <div class="absolute w-40 top-0 flex-col items-center hidden mt-5 group-hover:flex">
            <span
              class="absolute z-10 p-2 text-xs leading-none text-black whitespace-no-wrap bg-blue-200 rounded-l-lg rounded-br-lg shadow-lg tooltip-button"
            >
              First, you must set this parent <br />category to active
              <span class="arrow-button"></span>
            </span>
          </div>
        </div>
      </div>
    </div>

    <div class="overflow-y-auto max-h-[80%] grid grid-cols-2 md:grid-cols-3 gap-8 p-10">
      <div
        v-show="groupedThirdLayer.length > 0"
        class="bg-[#fef9f2] border border-orange-400 min-h-[180px] w-[100%] p-4 rounded-tr-3xl rounded-bl-3xl grid grid-cols-1"
        v-for="(group, index) in groupedThirdLayer"
        :key="index"
        :style="{ gridRow: `span ${getRowSpan(group.items.length)}` }"
      >
        <div v-for="(item, itemIndex) in group.items" :key="item.uid">
          <h1 v-if="itemIndex === 0" class="flex text-xl font-semibold italic text-[#2c7b4b] mb-5">
            {{ group.letter + '_' }}
          </h1>
          <div class="flex items-center pb-1">
            <button
              class="mr-2"
              v-if="secondActive === true"
              @click="openEditModal(item.uid, item.name, item.is_active)"
            >
              <IconEdit class="w-4 h-4 hover:fill-[#2c7b4b] fill-gray-500" />
            </button>
            <div v-else class="mr-2 relative flex flex-col items-center group">
              <IconEdit class="w-4 h-4 hover:fill-[#2c7b4b] fill-gray-500" />
              <div class="absolute w-40 top-0 flex-col items-center hidden mt-5 group-hover:flex">
                <span
                  class="absolute z-10 p-2 text-xs leading-none text-black whitespace-no-wrap bg-blue-200 rounded-r-lg rounded-bl-lg shadow-lg tooltip"
                >
                  First, you must set this parent <br />category to active
                  <span class="arrow"></span>
                </span>
              </div>
            </div>
            <span
              :class="{
                ' mr-2 text-gray-500  hover:text-[#2c7b4b]': item.is_active,
                ' mr-2 line-through text-gray-500 hover:text-[#2c7b4b]': !item.is_active,
                ' mr-2  text-gray-500 bg-yellow-300 pl-2 pr-2 rounded-2xl drop-shadow-2xl hover:text-slate-500':
                  hightLightUid == item.uid
              }"
            >
              {{ item.name }}
            </span>
          </div>
        </div>
      </div>
      <div v-show="groupedThirdLayer.length === 0">
        <div class="relative bg-[#eeeeee] w-[100%] pl-6 pb-4 pt-4 rounded-tr-3xl rounded-bl-3xl">
          <span class="italic mr-2 text-gray-500">Data Not Available</span>
        </div>
      </div>
    </div>
  </div>

  <ContainerModal v-if="isCreateModalOpen" @close-modal="closeCreateModal" style="z-index: 1001">
    <div class="flex justify-between items-center w-[450px] sticky top-0 pt-5 pb-5">
      <div class="flex items-start space-x-2">
        <h1>
          <span class="font-semibold text-2xl">New Categories</span>
        </h1>
        <span class="rounded-3xl pl-2 pr-2 pb-1 pt-1 bg-blue-50 text-[10px] text-gray-500"
          >Layer 3</span
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
          <label class="block text-gray-400 mb-2">Category Layer 1</label>
          <input
            v-model="firstCategory"
            type="text"
            class="w-full px-3 py-2 border rounded-xl focus:outline focus:outline-[#2c7b4b]"
            :disabled="true"
          />
        </div>
        <div class="pt-2 pb-2">
          <label class="block text-gray-400 mb-2">Category Layer 2</label>
          <input
            v-model="secondCategory"
            type="text"
            class="w-full px-3 py-2 border rounded-xl focus:outline focus:outline-[#2c7b4b]"
            :disabled="true"
          />
        </div>
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
          >Layer 3</span
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
          <label class="block text-gray-400 mb-2">Category Layer 1</label>
          <input
            v-model="firstCategory"
            type="text"
            class="w-full px-3 py-2 border rounded-xl focus:outline focus:outline-[#2c7b4b]"
            :disabled="true"
          />
        </div>
        <div class="pt-2 pb-2">
          <label class="block text-gray-400 mb-2">Category Layer 2</label>
          <input
            v-model="secondCategory"
            type="text"
            class="w-full px-3 py-2 border rounded-xl focus:outline focus:outline-[#2c7b4b]"
            :disabled="true"
          />
        </div>
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
.tooltip {
  position: absolute;
  margin-left: 160px;
  width: 200px;
}

.arrow {
  position: absolute;
  bottom: 100%;
  left: 10%;
  margin-left: -6px; /* Sesuaikan nilai ini sesuai kebutuhan desain */
  border-width: 6px;
  border-style: solid;
  border-color: transparent transparent #bfdbfe transparent;
}

.tooltip-button {
  position: absolute;
  margin-left: -100px;
  top: 10px;
}

.arrow-button {
  position: absolute;
  bottom: 100%;
  right: 10%;
  margin-right: -6px; /* Sesuaikan nilai ini sesuai kebutuhan desain */
  border-width: 6px;
  border-style: solid;
  border-color: transparent transparent #bfdbfe transparent;
}
</style>
