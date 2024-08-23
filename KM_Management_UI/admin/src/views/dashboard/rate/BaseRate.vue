<script setup>
import { onMounted, ref } from 'vue'
import iconPerson from '@/components/icons/IconPerson.vue'
import iconPaper from '@/components/icons/IconPaper.vue'
import { DatePicker } from 'v-calendar'
import 'v-calendar/style.css'

import iconChart from '@/components/icons/IconChart.vue'
import starFull from '@/components/icons/IconStarFull.vue'
import starHalf from '@/components/icons/IconStarHalf.vue'
import starEmpty from '@/components/icons/IconStarEmpty.vue'
import { Popover, PopoverButton, PopoverPanel } from '@headlessui/vue'
import IconDropdown from '@/components/icons/IconDropdown.vue'

import IconNext from '@/components/icons/IconNext.vue'
import IconExpand from '@/components/icons/IconExpand.vue'
import IconPrevious from '@/components/icons/IconPrevious.vue'
import {
  filter,
  summary,
  rate_and_feedback,
  GetRateAndFeedbackByFilter,
  HandlePagination,
  navigation,
  HandlingPageLimit
} from '@/components/pages/dashboard/rate/useDashboard.js'

import {
  HandleExcelExport,
  filterExportExcel
} from '@/components/pages/dashboard/rate/postExportRateAndFeedback.js'

const hoverFilterDate = ref(false)
const hoverFilterRating = ref(false)
const pageLimit = ref(10)

//calendar

const range = ref({})

//calendar
const selectedRatings = ref([])
const selectedCategoryDate = ref('last_30_days')

const filteringDate = ref({
  category: 'last_30_days',
  start_date: null,
  end_date: null
})

const checkBoxChange = () => {
  const stringResult = selectedRatings.value.join(',')
  if (selectedRatings.value.length == 0) {
    fetchRate(null)
    fetchExportExcel(null)
  } else {
    fetchRate(stringResult)
    fetchExportExcel(stringResult)
  }
}

const formatDate = (createdAt) => {
  const date = new Date(createdAt)
  const day = String(date.getDate()).padStart(2, '0')
  const monthNames = [
    'Jan',
    'Feb',
    'Mar',
    'Apr',
    'May',
    'Jun',
    'Jul',
    'Aug',
    'Sep',
    'Oct',
    'Nov',
    'Dec'
  ]
  const monthIndex = date.getMonth()
  const year = date.getFullYear() % 100
  return `${day}-${monthNames[monthIndex]}-${year < 10 ? '0' : ''}${year} `
}

function convertToTitleCase(inputString) {
  return inputString
    .split('_')
    .map((word) => word.charAt(0).toUpperCase() + word.slice(1))
    .join(' ')
}

const fetchRate = async (ratings) => {
  try {
    if (ratings == null) {
      ratings = '1,2,3,4'
    }
    filter.value.filter_date = filteringDate.value.category
    filter.value.start_date = filteringDate.value.start_date
    filter.value.end_date = filteringDate.value.end_date
    filter.value.rating = ratings
    filter.value.page_limit = pageLimit.value
    filter.value.current_page = 1
    await GetRateAndFeedbackByFilter()
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

const fetchExportExcel = async (ratings) => {
  try {
    if (ratings == null) {
      ratings = '1,2,3,4'
    }
    filterExportExcel.value.filter_date = filteringDate.value.category
      ? filteringDate.value.category
      : null
    filterExportExcel.value.start_date = filteringDate.value.start_date
    filterExportExcel.value.end_date = filteringDate.value.end_date
    filterExportExcel.value.rating = ratings
    filterExportExcel.value.page_limit = filter.value.page_limit
    filterExportExcel.value.current_page = filter.value.current_page
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

const HandleAllPagination = async (page) => {
  try {
    HandlePagination(page)
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

const HandleAllPageLimit = async (page) => {
  try {
    HandlingPageLimit(page)
  } catch (error) {
    console.error('Error fetching content:', error)
  }
}

const applyDateFilter = async () => {
  if (selectedCategoryDate.value != null) {
    filteringDate.value.category = selectedCategoryDate.value
    filteringDate.value.start_date = null
    filteringDate.value.end_date = null
    range.value = {}
    checkBoxChange()
  } else if (selectedCategoryDate.value == null) {
    filteringDate.value.start_date = range.value.start
    filteringDate.value.end_date = range.value.end
    filteringDate.value.category = null
    checkBoxChange()
  }
}

const closePopover = () => {
  // Mengatur nilai open menjadi false untuk menutup popover
  open.value = false
}

onMounted(async () => {
  try {
    await checkBoxChange()
  } catch (error) {
    console.error('Error in onMounted:', error)
  }
})
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
        <Popover class="relative" v-slot="{ open }">
          <PopoverButton
            class="min-w-fit flex pl-4 pr-4 pt-2 pb-2 border border-green-800 text-green-800 items-center justify-between gap-2 focus:outline-none"
            :class="[
              open
                ? 'rounded-tr-3xl rounded-tl-3xl text-white bg-green-800 '
                : 'rounded-3xl hover:bg-[#71b483] hover:text-white hover:border-[#71b483] bg-[#f8fbf9]'
            ]"
            :style="{ width: '250px' }"
            @mouseenter="hoverFilterDate = true"
            @mouseleave="hoverFilterDate = false"
          >
            <div class="flex items-start">
              <span>
                <slot name="default" v-if="filteringDate.category != null">{{
                  convertToTitleCase(filteringDate.category)
                }}</slot>
                <slot
                  name="default"
                  v-else-if="
                    filteringDate.category == null &&
                    filteringDate.end_date != null &&
                    filteringDate.start_date != null
                  "
                  >{{
                    formatDate(filteringDate.start_date) +
                    ' to ' +
                    formatDate(filteringDate.end_date)
                  }}</slot
                >
              </span>
            </div>
            <div class="items-end">
              <IconDropdown
                v-if="open"
                :class="[
                  open ? 'fill-white' : 'fill-green-800',
                  hoverFilterDate ? 'fill-white' : 'fill-green-800'
                ]"
              />

              <IconExpand :class="hoverFilterDate ? 'fill-white' : 'fill-green-800'" v-else />
            </div>
          </PopoverButton>

          <PopoverPanel class="absolute z-10 right-0">
            <div
              class="grid p-3 bg-white border border-green-800 rounded-2xl rounded-tr-none min-w-fit text-green-800 select-none"
            >
              <slot name="options">
                <div class="flex w-full gap-10">
                  <div class="flex flex-col">
                    <div class="flex flex-col">
                      <button
                        class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                        :class="{ 'bg-[#d3eedb]': selectedCategoryDate == 'today' }"
                        @click="selectedCategoryDate = 'today'"
                      >
                        Today
                      </button>
                      <button
                        class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                        :class="{ 'bg-[#d3eedb]': selectedCategoryDate == 'yesterday' }"
                        @click="selectedCategoryDate = 'yesterday'"
                      >
                        Yesterday
                      </button>
                      <button
                        class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                        :class="{ 'bg-[#d3eedb]': selectedCategoryDate == 'last_7_days' }"
                        @click="selectedCategoryDate = 'last_7_days'"
                      >
                        Last 7 Days
                      </button>
                      <button
                        class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                        :class="{ 'bg-[#d3eedb]': selectedCategoryDate == 'last_30_days' }"
                        @click="selectedCategoryDate = 'last_30_days'"
                      >
                        Last 30 Days
                      </button>
                      <button
                        class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                        :class="{ 'bg-[#d3eedb]': selectedCategoryDate == 'last_3_months' }"
                        @click="selectedCategoryDate = 'last_3_months'"
                      >
                        Last 3 Months
                      </button>
                      <button
                        class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                        :class="{ 'bg-[#d3eedb]': selectedCategoryDate == 'last_1_year' }"
                        @click="selectedCategoryDate = 'last_1_year'"
                      >
                        Last 1 Year
                      </button>
                    </div>
                    <div class="flex mt-20 items-start gap-2">
                      <PopoverButton>
                        <button
                          @click="closePopover"
                          class="hover:bg-[#c4ddcc] bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg"
                        >
                          Cancel
                        </button></PopoverButton
                      >
                      <PopoverButton>
                        <button
                          class="bg-green-700 hover:bg-green-800 text-white pl-2 pr-2 pt-1 pb-1 rounded-lg"
                          @click="applyDateFilter"
                        >
                          Apply
                        </button>
                      </PopoverButton>
                    </div>
                  </div>
                  <div class="flex">
                    <DatePicker
                      :color="'green'"
                      transparent
                      borderless
                      v-model.range="range"
                      :timezone="'UTC'"
                      mode="date"
                      @click="range.start ? (selectedCategoryDate = null) : selectedCategoryDate"
                    >
                    </DatePicker>
                  </div>
                </div>
              </slot>
            </div>
          </PopoverPanel>
        </Popover>
        <!-- <SecondaryButton class="text-green-700">{{ dateFilter }}</SecondaryButton> -->
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
                  <iconPerson class="fill-[#4a5449]" />
                </div>
                <h1 class="font-bold text-green-700">User Preview</h1>
              </div>
            </div>
            <div class="flex w-full p-2 items-center justify-center">
              <p class="text-[#4a5449] text-5xl font-semibold">
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
                  <iconPaper class="fill-[#4a5449]" />
                </div>
                <h1 class="font-bold text-green-700">Total Feedback</h1>
              </div>
            </div>
            <div class="flex w-full p-2 items-center justify-center">
              <p class="text-[#4a5449] text-5xl font-semibold">
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
                <iconChart class="fill-[#4a5449]" />
              </div>
              <h1 class="font-bold text-green-700">Overall Rating</h1>
            </div>
          </div>
          <div class="flex w-full p-2 items-center justify-center">
            <p class="text-[#4a5449] text-5xl font-semibold">
              {{ summary.overall_rating }}
            </p>
          </div>
          <div class="flex w-full p-2 items-center justify-center">
            <span
              v-if="
                !isNaN(summary.overall_rating) &&
                summary.overall_rating != null &&
                summary.overall_rating >= 0.5 // Batas minimal untuk menampilkan bintang
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
              <!-- Tambahkan bintang setengah jika nilai rating memiliki desimal -->
              <span v-if="summary.overall_rating % 1 >= 0.1" aria-hidden="true">
                <starHalf class="h-5 w-5 fill-yellow-400" />
              </span>
            </span>
            <!-- Jika rating kurang dari 0.5, maka tidak menampilkan bintang -->
            <span v-else class="text-[#4a5449]">No Rating</span>
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
              <div
                :style="{
                  width:
                    summary.rating_four > 0
                      ? `${(summary.rating_four / summary.total_feedback) * 100}%`
                      : '0%'
                }"
                class="h-3 bg-yellow-400 rounded-full transition-width duration-1000"
              ></div>
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
              <div
                :style="{
                  width:
                    summary.rating_three > 0
                      ? `${(summary.rating_three / summary.total_feedback) * 100}%`
                      : '0%'
                }"
                class="h-3 bg-yellow-400 rounded-full transition-width duration-1000"
              ></div>
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
              <div
                :style="{
                  width:
                    summary.rating_two > 0
                      ? `${(summary.rating_two / summary.total_feedback) * 100}%`
                      : '0%'
                }"
                class="h-3 bg-yellow-400 rounded-full transition-width duration-1000"
              ></div>
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
              <div
                :style="{
                  width:
                    summary.rating_one > 0
                      ? `${(summary.rating_one / summary.total_feedback) * 100}%`
                      : '0%'
                }"
                class="h-3 bg-yellow-400 rounded-full transition-width duration-1000"
              ></div>
              <div class="relative text-yellow-400">{{ summary.rating_one }}</div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="pr-8 pt-4 pb-4 gap-5 flex justify-end items-end">
      <button
        class="min-w-36 rounded-3xl border text-green-700 bg-white border-green-700 p-2 hover:border-white hover:bg-[#eeeeee] active:scale-95"
        @click="exportExcel"
        v-if="rate_and_feedback.length > 0"
      >
        Export To Excel
      </button>
      <button
        v-else
        class="min-w-32 rounded-3xl border p-2 active:scale-95 bg-[#f5f5f5] text-[#90958f] border-[#e3e3e3]"
      >
        Export To Excel
      </button>
      <Popover class="relative" v-slot="{ open }">
        <PopoverButton
          class="min-w-32 flex p-2 border border-green-800 text-green-800 items-center justify-center gap-2 focus:outline-none"
          :class="[
            open
              ? 'rounded-tr-3xl rounded-tl-3xl text-white bg-green-800 '
              : 'rounded-3xl hover:bg-[#71b483] hover:text-white hover:border-[#71b483] bg-[#f8fbf9]'
          ]"
          @mouseenter="hoverFilterRating = true"
          @mouseleave="hoverFilterRating = false"
        >
          <span>
            <slot name="default">Rating</slot>
          </span>
          <!-- <IconDropdown
            :class="[
              open ? 'fill-white' : 'fill-green-800',
              hoverFilterRating ? 'fill-white' : 'fill-green-800'
            ]"
          /> -->

          <IconDropdown
            v-if="open"
            :class="[
              open ? 'fill-white' : 'fill-green-800',
              hoverFilterRating ? 'fill-white' : 'fill-green-800'
            ]"
          />

          <IconExpand :class="hoverFilterRating ? 'fill-white' : 'fill-green-800'" v-else />
        </PopoverButton>

        <PopoverPanel class="absolute z-10 right-0">
          <div
            class="grid grid-cols-1 p-3 gap-2.5 bg-white border border-green-800 rounded-2xl rounded-tr-none min-w-44 text-green-800 select-none"
          >
            <slot name="options">
              <div class="flex flex-col">
                <label class="flex items-center">
                  <input type="checkbox" name="rating1" v-model="selectedRatings" :value="1" />
                  <span class="ml-3 flex items-center">
                    <span v-for="star in 1" :key="star">
                      <span aria-hidden="true">
                        <starFull class="h-5 w-5 fill-yellow-400" />
                      </span>
                    </span>
                  </span>
                </label>
                <label class="flex items-center">
                  <input type="checkbox" name="rating1" v-model="selectedRatings" :value="2" />
                  <span class="ml-3 flex items-center">
                    <span v-for="star in 2" :key="star">
                      <span aria-hidden="true">
                        <starFull class="h-5 w-5 fill-yellow-400" />
                      </span>
                    </span>
                  </span>
                </label>
                <label class="flex items-center">
                  <input type="checkbox" name="rating1" v-model="selectedRatings" :value="3" />
                  <span class="ml-3 flex items-center">
                    <span v-for="star in 3" :key="star">
                      <span aria-hidden="true">
                        <starFull class="h-5 w-5 fill-yellow-400" />
                      </span>
                    </span>
                  </span>
                </label>
                <label class="flex items-center">
                  <input type="checkbox" name="rating1" v-model="selectedRatings" :value="4" />
                  <span class="ml-3 flex items-center">
                    <span v-for="star in 4" :key="star">
                      <span aria-hidden="true">
                        <starFull class="h-5 w-5 fill-yellow-400" />
                      </span>
                    </span>
                  </span>
                </label>
                <PopoverButton
                  @click="checkBoxChange"
                  class="mt-4 bg-green-800 text-white rounded-lg"
                >
                  Ok
                </PopoverButton>
              </div>
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
    <div v-if="rate_and_feedback.length > 0" class="px-8 overflow-y-auto">
      <div class="bg-gray-50 flex flex-col gap-4 h-[100%]">
        <div
          v-for="rate in rate_and_feedback"
          :key="rate"
          class="px-16 flex-wrap border-b-2 border-gray-300 pt-2 pb-2"
        >
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
          <div style="word-wrap: break-word">
            <span class="font-medium text-gray-400">Feedback :</span>
            <!-- Teks rate.remark akan mengikuti lebar div dan tidak keluar dari batasnya saat zoom in atau zoom out -->
            <p>{{ rate.remark }}</p>
          </div>
        </div>
      </div>
      <div class="px-8">
        <div class="min-h-[100%] flex justify-between py-2">
          <nav
            v-show="rate_and_feedback.length > 0"
            class="flex flex-row w-full justify-between items-start md:items-center space-y-5 md:space-y-0"
          >
            <div class="space-x-2">
              <select
                class="border rounded border-gray-500"
                v-model="pageLimit"
                @change="HandleAllPageLimit(pageLimit)"
              >
                <option :value="10">10</option>
                <option :value="20">20</option>
                <option :value="50">50</option>
              </select>

              <span class="text-sm text-gray-500 pl-2 pr-2">
                {{ navigation.show }}
              </span>
            </div>

            <ul class="inline-flex items-stretch -space-x-px">
              <li v-show="navigation.current > 1">
                <button
                  @click="HandleAllPagination(1)"
                  class="rounded-l-md px-2 py-2 text-gray-400 ring-1 ring-inset ring-gray-300 hover:bg-gray-300 h-full"
                >
                  <IconPrevious class="fill-gray-500" />
                </button>
              </li>

              <li v-show="navigation.previous !== null">
                <button
                  @click="HandleAllPagination(navigation.previous)"
                  class="px-3.5 py-2 text-gray-400 ring-1 ring-inset ring-gray-300 hover:bg-gray-300"
                >
                  <span class="text-sm">{{ navigation.previous }}</span>
                </button>
              </li>
              <li v-show="navigation.current !== null">
                <button
                  class="px-3.5 py-2 bg-gray-500 text-white font-bold ring-1 ring-inset ring-gray-300"
                >
                  <span class="text-sm">{{ navigation.current }}</span>
                </button>
              </li>
              <li v-show="navigation.next !== null">
                <button
                  @click="HandleAllPagination(navigation.next)"
                  class="px-3.5 py-2 text-gray-400 ring-1 ring-inset ring-gray-300 hover:bg-gray-300"
                >
                  <span class="text-sm">{{ navigation.next }}</span>
                </button>
              </li>
              <li v-show="navigation.max !== null && navigation.next !== null">
                <button
                  @click="HandleAllPagination(navigation.max)"
                  class="rounded-r-md px-2 py-2 text-gray-400 ring-1 ring-inset ring-gray-300 hover:bg-gray-300 h-full"
                >
                  <IconNext class="fill-gray-500" />
                </button>
              </li>
            </ul>
          </nav>
        </div>
      </div>
    </div>
    <div v-else class="px-8 overflow-y-auto">
      <div class="min-h-[80px]">
        <p class="text-gray-500 items-center justify-center flex min-h-[80px]">Data Not Found</p>
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
