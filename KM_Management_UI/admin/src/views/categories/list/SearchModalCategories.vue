<script setup>
import {
  GetCategoryListByFilter,
  category_list,
  filter
} from '@/components/pages/category/list/useCategoryList.js'
import { useCategoriesStore } from '@/stores/listCategoriesStore.js'
import { ref, onMounted } from 'vue'
import { defineEmits } from 'vue'

const emit = defineEmits(['close-modal'])

const closeModalOutside = (event) => {
  if (event.target.classList.contains('flex') || event.target.classList.contains('bg-black')) {
    emit('close-modal')
  }
}

import { useRouter } from 'vue-router'
const router = useRouter()

const storeCategories = useCategoriesStore()
const firstLayer = ref([])
const secondLayer = ref([])
const thirdLayer = ref([])
// const allLayer = ref([])
const resultSearch = ref({
  firstLayer: [],
  secondLayer: [],
  thirdLayer: []
})

const searchQueryCategory = ref('')

const navigateToSearchFirst = (uid) => {
  storeCategories.setHightLight(uid)
}

const navigateToSearchSecond = (firstName, firstUid, secondUid, isActiveFirst) => {
  storeCategories.setSelectedFirstUid(firstName, firstUid, isActiveFirst)
  storeCategories.setHightLight(secondUid)
}

const navigateToSearchThird = (
  firstName,
  secondName,
  firstUid,
  secondUid,
  thirdUid,
  isActiveFirst,
  isActiveSecond
) => {
  storeCategories.setSelectedFirstUid(firstName, firstUid, isActiveFirst)
  storeCategories.setSelectedSecondUid(secondName, secondUid, isActiveSecond)
  storeCategories.setHightLight(thirdUid)
}

const fetchAllLayer = async (isActive) => {
  try {
    // hightLightUid.value = storeContent.getHightlightUid;
    filter.value.Uid_Reference = null
    filter.value.Layer = 1
    filter.value.Is_Active = isActive
    await GetCategoryListByFilter()
    firstLayer.value = category_list.value

    for (const firstLayerItem of firstLayer.value) {
      const second = await fetchSecondLayer(firstLayerItem.uid, 2, isActive)

      // Add first layer details to each second layer item
      second.forEach((secondLayerItem) => {
        secondLayerItem.first_parent_name = firstLayerItem.name
        secondLayerItem.first_parent_uid = firstLayerItem.uid
        secondLayerItem.first_parent_active = firstLayerItem.is_active
      })

      secondLayer.value.push(...second)

      for (const secondLayerItem of second) {
        const third = await fetchThirdLayer(secondLayerItem.uid, 3, isActive)

        // Add first and second layer details to each third layer item
        third.forEach((thirdLayerItem) => {
          thirdLayerItem.first_parent_name = firstLayerItem.name
          thirdLayerItem.first_parent_uid = firstLayerItem.uid
          thirdLayerItem.first_parent_active = firstLayerItem.is_active
          thirdLayerItem.second_parent_name = secondLayerItem.name
          thirdLayerItem.second_parent_uid = secondLayerItem.uid
          thirdLayerItem.second_parent_active = secondLayerItem.is_active
        })

        thirdLayer.value.push(...third)
      }
    }
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

const fetchSecondLayer = async (uid, layer, isActive) => {
  try {
    filter.value.Uid_Reference = uid
    filter.value.Layer = layer
    filter.value.Is_Active = isActive
    await GetCategoryListByFilter()
    return category_list.value
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

const fetchThirdLayer = async (uid, layer, isActive) => {
  try {
    filter.value.Uid_Reference = uid
    filter.value.Layer = layer
    filter.value.Is_Active = isActive
    await GetCategoryListByFilter()
    return category_list.value
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}
const fetchSearchCategories = async (categoryName) => {
  try {
    // secondResult.value = []
    // thirdResult.value = []
    // await storeCategories.allLayer(firstLayer.value)
    await storeCategories.setSearchFirst(firstLayer.value)
    resultSearch.value.firstLayer = storeCategories.getSearchFirst(categoryName)
    await storeCategories.setSearchSecond(secondLayer.value)
    resultSearch.value.secondLayer = storeCategories.getSearchSecond(categoryName)
    await storeCategories.setSearchThird(thirdLayer.value)
    resultSearch.value.thirdLayer = storeCategories.getSearchThird(categoryName)
    // allLayer.value = storeCategories.getAllLayer
    // searchCategories.value = storeCategories.getSearch(categoryName)

    // Mengelompokkan hasil secondLayer dan thirdLayer
    // searchCategories.value.forEach((firstLayer) => {
    //   if (firstLayer.secondLayer && firstLayer.secondLayer.length > 0) {
    //     const secondLayerWithParent = firstLayer.secondLayer.map((second) => ({
    //       ...second,
    //       first_parent_name: firstLayer.name,
    //       first_parent_uid: firstLayer.uid,
    //       first_parent_active: firstLayer.is_active
    //     }))
    //     secondResult.value.push(...secondLayerWithParent)
    //   }
    //   if (firstLayer.secondLayer && firstLayer.secondLayer.length > 0) {
    //     firstLayer.secondLayer.forEach((second) => {
    //       if (second.thirdLayer && second.thirdLayer.length > 0) {
    //         const thirdLayerWithParent = second.thirdLayer.map((third) => ({
    //           ...third,
    //           first_parent_name: firstLayer.name,
    //           first_parent_uid: firstLayer.uid,
    //           first_parent_active: firstLayer.is_active,
    //           second_parent_name: second.name,
    //           second_parent_uid: second.uid,
    //           second_parent_active: second.is_active
    //         }))
    //         thirdResult.value.push(...thirdLayerWithParent)
    //       }
    //     })
    //   }
    // })
  } catch (error) {
    console.error('Error fetching content:', error)
    return []
  }
}
onMounted(async () => {
  await fetchAllLayer(null)
  // console.log(firstLayer.value)
  // console.log(secondLayer.value)
  // console.log(thirdLayer.value)
})
</script>

<template>
  <div class="fixed inset-0 z-50 flex items-center justify-center" @click="closeModalOutside">
    <!-- Modal background -->
    <div class="fixed inset-0 bg-black opacity-50"></div>

    <!-- Modal content -->
    <div class="bg-white pl-8 pr-8 rounded-2xl z-50 w-auto min-w-auto min-h-auto" @click.stop>
      <div class="relative h-[450px] w-[450px]">
        <div class="sticky top-0 min-w-[300px] bg-white pt-5 pb-5">
          <div class="relative w-fit min-w-full">
            <input
              v-model="searchQueryCategory"
              @input.prevent="fetchSearchCategories(searchQueryCategory)"
              type="search"
              class="block pt-2 pb-2 pl-10 pr-10 w-full z-20 text-gray-500 bg-gray-50 rounded-3xl border border-gray-300 outline-none"
              placeholder="Search..."
              required
            />
            <button
              type="submit"
              class="absolute top-0 left-0 h-full p-2.5 text-white bg-transparent rounded-l-lg"
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

        <div
          v-if="
            searchQueryCategory.length > 0 &&
            (resultSearch.firstLayer.length > 0 ||
              resultSearch.secondLayer.length > 0 ||
              resultSearch.thirdLayer.length > 0)
          "
          class="relative w-full pb-2 overflow-y-auto max-h-80"
        >
          <div class="mb-4">
            <div v-show="resultSearch.firstLayer.length > 0" class="pb-4">
              <span class="text-orange-500"> Layer 1 </span>
            </div>
            <div
              :class="index == resultSearch.firstLayer.length - 1 ? 'pb-4' : ''"
              v-show="resultSearch.firstLayer.length > 0"
              v-for="(search, index) in resultSearch.firstLayer"
              :key="index"
            >
              <RouterLink
                v-if="router.currentRoute.value.name !== 'categories'"
                :to="{ name: 'categories' }"
                @click="navigateToSearchFirst(search.uid)"
                class="flex w-[100%]"
              >
                <span
                  :class="[
                    search.is_active == true ? ' mr-2 ' : ' mr-2 line-through ',
                    'bg-orange-100 w-full p-1 pl-2 pr-2 text-gray-500 hover:bg-orange-200 hover:text-orange-500',
                    index === 0 ? 'rounded-t-xl' : '',
                    index === resultSearch.firstLayer.length - 1 ? 'rounded-b-xl' : ''
                  ]"
                >
                  {{ search.name }}
                </span>
              </RouterLink>
              <div
                v-else
                class="flex w-[100%]"
                @click="navigateToSearchFirst(search.uid)"
                style="cursor: pointer"
              >
                <span
                  :class="[
                    search.is_active ? ' mr-2 ' : ' mr-2 line-through ',
                    'bg-orange-100 w-full p-1 pl-2 pr-2 text-gray-500 hover:bg-orange-200 hover:text-orange-500',
                    index === 0 ? 'rounded-t-xl' : '',
                    index === resultSearch.firstLayer.length - 1 ? 'rounded-b-xl' : ''
                  ]"
                >
                  {{ search.name }}
                </span>
              </div>
            </div>
            <div v-show="resultSearch.secondLayer.length > 0" class="pb-4">
              <span class="text-orange-500">Layer 2</span>
            </div>
            <div
              :class="secondIndex == resultSearch.secondLayer.length - 1 ? 'pb-4' : ''"
              v-show="resultSearch.secondLayer.length > 0"
              v-for="(secondLayer, secondIndex) in resultSearch.secondLayer"
              :key="secondIndex"
            >
              <RouterLink
                v-if="router.currentRoute.value.params !== secondLayer.first_parent_name"
                :to="{
                  name: 'categories-second-layer',
                  params: { secondLayer: secondLayer.first_parent_name }
                }"
                :exact="true"
                @click="
                  navigateToSearchSecond(
                    secondLayer.first_parent_name,
                    secondLayer.first_parent_uid,
                    secondLayer.uid,
                    secondLayer.first_parent_active
                  )
                "
                replace
                class="flex w-[100%]"
              >
                <span
                  :class="[
                    secondLayer.is_active == true ? ' mr-2 ' : ' mr-2 line-through ',
                    'bg-orange-100 w-full p-1 pl-2 pr-2 text-gray-500 hover:bg-orange-200 hover:text-orange-500',
                    secondIndex === 0 ? 'rounded-t-xl' : '',
                    secondIndex === resultSearch.secondLayer.length - 1 ? 'rounded-b-xl' : ''
                  ]"
                >
                  {{ secondLayer.name }}
                </span>
              </RouterLink>
              <div
                class="flex w-[100%]"
                v-else
                @click="
                  navigateToSearchSecond(
                    secondLayer.first_parent_name,
                    secondLayer.first_parent_uid,
                    secondLayer.uid,
                    secondLayer.first_parent_active
                  )
                "
              >
                <span
                  :class="[
                    secondLayer.is_active == true ? ' mr-2 ' : ' mr-2 line-through ',
                    'bg-orange-100 w-full p-1 pl-2 pr-2 text-gray-500 hover:bg-orange-200 hover:text-orange-500',
                    secondIndex === 0 ? 'rounded-t-xl' : '',
                    secondIndex === resultSearch.secondLayer.length - 1 ? 'rounded-b-xl' : ''
                  ]"
                >
                  {{ secondLayer.name }}
                </span>
              </div>
            </div>
            <div v-show="resultSearch.thirdLayer.length > 0" class="pb-4">
              <span class="text-orange-500">Layer 3</span>
            </div>
            <div
              :class="thirdIndex == resultSearch.thirdLayer.length - 1 ? 'pb-4' : ''"
              v-show="resultSearch.thirdLayer.length > 0"
              v-for="(thirdLayer, thirdIndex) in resultSearch.thirdLayer"
              :key="thirdIndex"
            >
              <RouterLink
                v-if="router.currentRoute.value.params.thirdLayer !== thirdLayer.second_parent_name"
                :to="{
                  name: 'categories-third-layer',
                  params: {
                    secondLayer: thirdLayer.first_parent_name,
                    thirdLayer: thirdLayer.second_parent_name
                  }
                }"
                @click="
                  navigateToSearchThird(
                    thirdLayer.first_parent_name,
                    thirdLayer.second_parent_name,
                    thirdLayer.first_parent_uid,
                    thirdLayer.second_parent_uid,
                    thirdLayer.uid,
                    thirdLayer.first_parent_active,
                    thirdLayer.second_parent_active
                  )
                "
                class="flex w-[100%]"
              >
                <span
                  :class="[
                    thirdLayer.is_active == true ? ' mr-2 ' : ' mr-2 line-through ',
                    'bg-orange-100 w-full p-1 pl-2 pr-2 text-gray-500 hover:bg-orange-200 hover:text-orange-500',
                    thirdIndex === 0 ? 'rounded-t-xl' : '',
                    thirdIndex === resultSearch.thirdLayer.length - 1 ? 'rounded-b-xl' : ''
                  ]"
                >
                  {{ thirdLayer.name }}
                </span>
              </RouterLink>
              <div
                @click="
                  navigateToSearchThird(
                    thirdLayer.first_parent_name,
                    thirdLayer.second_parent_name,
                    thirdLayer.first_parent_uid,
                    thirdLayer.second_parent_uid,
                    thirdLayer.uid,
                    thirdLayer.first_parent_active,
                    thirdLayer.second_parent_active
                  )
                "
                v-else
                class="flex w-[100%]"
              >
                <span
                  :class="[
                    thirdLayer.is_active == true ? ' mr-2 ' : ' mr-2 line-through ',
                    'bg-orange-100 w-full p-1 pl-2 pr-2 text-gray-500 hover:bg-orange-200 hover:text-orange-500',
                    thirdIndex === 0 ? 'rounded-t-xl' : '',
                    thirdIndex === resultSearch.thirdLayer.length - 1 ? 'rounded-b-xl' : ''
                  ]"
                >
                  {{ thirdLayer.name }}
                </span>
              </div>
            </div>
          </div>
        </div>
        <div
          v-else-if="
            searchQueryCategory.length > 0 &&
            (resultSearch.firstLayer.length === 0 ||
              resultSearch.secondLayer.length === 0 ||
              resultSearch.thirdLayer.length === 0)
          "
          class="relative w-full overflow-y-auto h-80 flex justify-center items-center"
        >
          <div
            class="relative w-[100%] h-[100%] inset-0 flex justify-center items-center bg-slate-100 rounded-2xl text-slate-500"
          >
            <p>
              No Result For
              <span class="font-bold"> "{{ searchQueryCategory }}"</span>
            </p>
          </div>
        </div>
        <div v-else class="relative w-full overflow-y-auto h-80 flex justify-center items-center">
          <div
            class="relative w-[100%] h-[100%] inset-0 flex justify-center items-center bg-slate-100 rounded-2xl text-slate-500"
          >
            No Recent Searches
          </div>
        </div>
      </div>
    </div>
  </div>
</template>