<script setup>
import { onMounted, ref } from 'vue'
import iconPerson from '@/components/icons/IconPerson.vue'
import iconPaper from '@/components/icons/IconPaper.vue'
import iconChart from '@/components/icons/IconChart.vue'
import starFull from '@/components/icons/IconStarFull.vue'
import starEmpty from '@/components/icons/IconStarEmpty.vue'
import SecondaryButton from '@/components/buttons/SecondaryButton.vue'
import { Popover, PopoverButton, PopoverPanel } from '@headlessui/vue'
import IconDropdown from '@/components/icons/IconDropdown.vue'
import {
  filter,
  summary,
  rate_and_feedback,
  GetRateAndFeedbackByFilter
} from '@/components/pages/dashboard/useDashboard.js'

const dateFilter = ref('last_year')

const selectedRatings = ref([1, 2, 3, 4])

const checkBoxChange = () => {
  const stringResult = selectedRatings.value.join(',') // Output: "1,2,3,4"

  fetchRate(stringResult)
}
const fetchRate = async (ratings) => {
  try {
    filter.value.filter_date = dateFilter.value
    filter.value.start_date = null
    filter.value.end_date = null
    filter.value.rating = ratings
    filter.value.page_limit = '10'
    filter.value.current_page = '1'
    await GetRateAndFeedbackByFilter()
    rate_and_feedback.value.forEach((rate) => {
      rate.create_at = formatCreatedAt(rate.create_at)
    })
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

onMounted(async () => {
  checkBoxChange()
})

const formatCreatedAt = (createdAt) => {
  const date = new Date(createdAt)
  const day = String(date.getDate()).padStart(2, '0')
  const month = String(date.getMonth() + 1).padStart(2, '0')
  const year = date.getFullYear()
  const hours = String(date.getHours()).padStart(2, '0')
  const minutes = String(date.getMinutes()).padStart(2, '0')
  return `${day}-${month}-${year} ${hours}:${minutes}`
}
</script>
<template>
  <div class="flex items-center align-middle gap-3 mb-3 bg-tea">
    <p class="text-sm">Setup</p>
    <span> > </span>
    <p class="text-sm text-orange-400">Rate & Feedback</p>
  </div>

  <div class="w-full rounded-lg bg-white min-h-[25%] max-h-[96%] box-border flex flex-col">
    <div class="flex gap-2.5 justify-between border-b-2 py-5 px-8">
      <div class="flex items-start space-x-2">
        <h1 class="basis-[100%] text-2xl font-bold text-green-800">Rate & Feedback</h1>
      </div>
      <div class="flex items-end space-x-2">
        <SecondaryButton class="text-green-700">{{ dateFilter }}</SecondaryButton>
      </div>
    </div>
    <div class="pr-10 pl-10 pt-4 grid grid-cols-1 sm:grid-cols-2 gap-10">
      <!-- Kolom 1 -->
      <div class="grid grid-cols-1 sm:grid-cols-2 gap-10">
        <!-- Sub Kolom 1A -->
        <div
          class="bg-gray-50 transition-transform transform-gpu hover:scale-110 rounded-2xl border-2 border-t-0 border-r-2 border-gray-300 p-2 shadow-md flex"
        >
          <div class="w-full rounded-lg box-border flex flex-col">
            <div class="flex gap-2.5 justify-between">
              <div class="flex space-x-2 items-center">
                <div class="bg-gray-200 px-1 py-1 rounded-lg flex items-center justify-center">
                  <!-- Tambahkan ikon di sini -->
                  <iconPerson class="fill-gray-600" />
                </div>
                <h1 class="font-bold text-green-800">User Preview</h1>
              </div>
            </div>
            <div class="flex w-full p-2 items-center justify-center">
              <p class="text-gray-700 text-5xl font-semibold">
                {{ summary.user_preview }}
              </p>
            </div>
          </div>
        </div>
        <div
          class="bg-gray-50 transition-transform transform-gpu hover:scale-110 rounded-2xl border-2 border-t-0 border-gray-300 p-2 shadow-md flex"
        >
          <div class="w-full rounded-lg box-border flex flex-col">
            <div class="flex gap-2.5 justify-between">
              <div class="flex space-x-2 items-center">
                <div class="bg-gray-200 px-1 py-1 rounded-lg flex items-center justify-center">
                  <!-- Tambahkan ikon di sini -->
                  <iconPaper class="fill-gray-600" />
                </div>
                <h1 class="font-bold text-green-800">Total Feedback</h1>
              </div>
            </div>
            <div class="flex w-full p-2 items-center justify-center">
              <p class="text-gray-700 text-5xl font-semibold">
                {{ summary.total_feedback }}
              </p>
            </div>
          </div>
        </div>
      </div>
      <!-- Kolom 2 -->
      <div
        class="bg-gray-50 transition-transform transform-gpu hover:scale-110 rounded-2xl border-2 border-t-0 border-gray-300 p-2 shadow-md flex"
      >
        <div class="w-full rounded-lg flex flex-col">
          <div class="flex gap-2.5 justify-between">
            <div class="flex space-x-2 items-center">
              <div class="bg-gray-200 px-1 py-1 rounded-lg flex items-center justify-center">
                <!-- Tambahkan ikon di sini -->
                <iconChart class="fill-gray-600" />
              </div>
              <h1 class="font-bold text-green-800">Overall Rating</h1>
            </div>
          </div>
          <div class="flex w-full p-2 items-center justify-center">
            <p class="text-gray-700 text-5xl font-semibold">
              {{ summary.overall_rating }}
            </p>
          </div>
          <div class="flex w-full p-2 items-center justify-center">
            <span
              v-if="
                !isNaN(summary.overall_rating) &&
                summary.overall_rating != null &&
                summary.overall_rating != 0
              "
              class="flex text-amber-400"
              role="img"
              :aria-label="'Rating: ' + summary.overall_rating + ' out of 4 stars'"
            >
              <div v-for="star in Math.floor(summary.overall_rating)" :key="star">
                <span aria-hidden="true">
                  <starFull class="h-5 w-5 fill-yellow-400" />
                </span>
              </div>
            </span>
            <span
              v-else
              class="flex text-amber-400"
              role="img"
              :aria-label="'Rating: ' + summary.overall_rating + ' out of 4 stars'"
            >
              <span aria-hidden="true"> <starFull class="h-5 w-5 fill-yellow-400" /> </span>
            </span>
          </div>
        </div>

        <div class="flex">
          <div class="ml-5 mr-5 border-l border-gray-300"></div>
        </div>
        <div class="w-full">
          <div class="mb-2">
            <div class="flex items-center gap-4">
              <div class="flex">
                <starFull class="h-5 w-5 fill-yellow-400"> </starFull>
                <!-- Bintang pertama -->
                <starFull class="h-5 w-5 fill-yellow-400"> </starFull>
                <!-- Bintang kedua -->
                <starFull class="h-5 w-5 fill-yellow-400"> </starFull>
                <!-- Bintang ketiga -->
                <starFull class="h-5 w-5 fill-yellow-400"> </starFull>
                <!-- Bintang keempat -->
              </div>
              <div class="w-[40%] h-3">
                <div class="h-3 bg-yellow-400 rounded-full"></div>
              </div>
              <div class="relative text-yellow-400">{{ summary.rating_four }}</div>
            </div>
          </div>
          <div class="mb-2">
            <div class="flex items-center gap-4">
              <div class="flex">
                <starFull class="h-5 w-5 fill-yellow-400"> </starFull>
                <!-- Bintang pertama -->
                <starFull class="h-5 w-5 fill-yellow-400"> </starFull>
                <!-- Bintang kedua -->
                <starFull class="h-5 w-5 fill-yellow-400"> </starFull>
                <!-- Bintang ketiga -->
                <starEmpty class="h-5 w-5 fill-yellow-400"> </starEmpty>
                <!-- Bintang keempat -->
              </div>
              <div class="w-[20%] h-3">
                <div class="h-3 bg-yellow-400 rounded-full"></div>
              </div>
              <div class="relative text-yellow-400">{{ summary.rating_three }}</div>
            </div>
          </div>
          <div class="mb-2">
            <div class="flex items-center gap-4">
              <div class="flex">
                <starFull class="h-5 w-5 fill-yellow-400"> </starFull>
                <!-- Bintang pertama -->
                <starFull class="h-5 w-5 fill-yellow-400"> </starFull>
                <!-- Bintang kedua -->
                <starEmpty class="h-5 w-5 fill-yellow-400"> </starEmpty>
                <!-- Bintang ketiga -->
                <starEmpty class="h-5 w-5 fill-yellow-400"> </starEmpty>
                <!-- Bintang keempat -->
              </div>
              <div class="w-[10%] h-3">
                <div class="h-3 bg-yellow-400 rounded-full"></div>
              </div>
              <div class="relative text-yellow-400">{{ summary.rating_two }}</div>
            </div>
          </div>
          <div class="mb-2">
            <div class="flex items-center gap-4">
              <div class="flex">
                <starFull class="h-5 w-5 fill-yellow-400"> </starFull>
                <!-- Bintang pertama -->
                <starEmpty class="h-5 w-5 fill-yellow-400"> </starEmpty>
                <!-- Bintang kedua -->
                <starEmpty class="h-5 w-5 fill-yellow-400"> </starEmpty>
                <!-- Bintang ketiga -->
                <starEmpty class="h-5 w-5 fill-yellow-400"> </starEmpty>
                <!-- Bintang keempat -->
              </div>
              <div class="w-[5%] h-3">
                <div class="h-3 bg-yellow-400 rounded-full"></div>
              </div>
              <div class="relative text-yellow-400">{{ summary.rating_one }}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="pr-8 pt-4 pb-4 gap-5 flex justify-end items-end">
      <SecondaryButton class="text-green-700">Export To Excel</SecondaryButton>
      <Popover class="relative" v-slot="{ open }">
        <PopoverButton
          class="min-w-32 flex p-2 border border-green-800 text-green-800 items-center justify-center gap-2 focus:outline-none"
          :class="[open ? 'rounded-tr-3xl rounded-tl-3xl text-white bg-green-800 ' : 'rounded-3xl']"
        >
          <span>
            <slot name="default">Rating</slot>
          </span>
          <IconDropdown :class="[open ? 'fill-white' : 'fill-green-800']" />
        </PopoverButton>

        <PopoverPanel class="absolute z-10 right-0">
          <div
            class="grid grid-cols-1 p-3 gap-2.5 bg-green-100 border border-green-800 rounded-2xl rounded-tr-none min-w-44 text-green-800 select-none"
          >
            <slot name="options">
              <label class="flex items-center">
                <input
                  type="checkbox"
                  name="rating1"
                  v-model="selectedRatings"
                  :value="1"
                  @change="checkBoxChange()"
                />
                <span class="ml-3 flex items-center">
                  <span v-for="star in 1" :key="star">
                    <span aria-hidden="true">
                      <starFull class="h-5 w-5 fill-yellow-400" />
                    </span>
                  </span>
                </span>
              </label>
              <label class="flex items-center">
                <input
                  type="checkbox"
                  name="rating1"
                  v-model="selectedRatings"
                  :value="2"
                  @change="checkBoxChange()"
                />
                <span class="ml-3 flex items-center">
                  <span v-for="star in 2" :key="star">
                    <span aria-hidden="true">
                      <starFull class="h-5 w-5 fill-yellow-400" />
                    </span>
                  </span>
                </span>
              </label>
              <label class="flex items-center">
                <input
                  type="checkbox"
                  name="rating1"
                  v-model="selectedRatings"
                  :value="3"
                  @change="checkBoxChange()"
                />
                <span class="ml-3 flex items-center">
                  <span v-for="star in 3" :key="star">
                    <span aria-hidden="true">
                      <starFull class="h-5 w-5 fill-yellow-400" />
                    </span>
                  </span>
                </span>
              </label>
              <label class="flex items-center">
                <input
                  type="checkbox"
                  name="rating1"
                  v-model="selectedRatings"
                  :value="4"
                  @change="checkBoxChange()"
                />
                <span class="ml-3 flex items-center">
                  <span v-for="star in 4" :key="star">
                    <span aria-hidden="true">
                      <starFull class="h-5 w-5 fill-yellow-400" />
                    </span>
                  </span>
                </span>
              </label>
            </slot>
          </div>
        </PopoverPanel>
      </Popover>
    </div>
    <div class="px-8">
      <div class="flex bg-gray-50 rounded-t-2xl justify-between border-b-2 border-gray-300 py-2">
        <div class="flex items-start space-x-2">
          <h1 class="basis-[100%] text-xl font-bold text-green-800 px-8">Rate & Feedback</h1>
        </div>
        <div class="flex items-end space-x-2"></div>
      </div>
    </div>
    <div class="px-8 overflow-y-auto">
      <div class="bg-gray-50 flex flex-col gap-4 px-8">
        <div v-for="rate in rate_and_feedback" :key="rate" class="px-4 flex-wrap">
          <div>{{ rate.create_by }}</div>
          <div class="flex items-center">
            <div class="flex items-center">
              <span v-for="index in parseInt(rate.rating)" :key="index">
                <starFull class="h-5 w-5 fill-yellow-400"></starFull>
              </span>
              <span v-for="index in 4 - parseInt(rate.rating)" :key="index">
                <starEmpty class="h-5 w-5 fill-yellow-400"></starEmpty>
              </span>
            </div>
          </div>
          <div>{{ rate.create_at }}</div>
          <div><span class="font-medium text-gray-400">Feedback :</span> {{ rate.remark }}</div>
          <div class="pt-2 pb-2 justify-between border-b-2 border-gray-300"></div>
        </div>
      </div>
    </div>
    <div class="px-8">
      <div class="flex justify-between border-t-2 border-gray-300 py-2">
        <div class="flex text-sm items-start space-x-2 px-2">Pagination start</div>
        <div class="flex text-sm items-end space-x-2">Pagination End</div>
      </div>
    </div>
  </div>
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
</style>
