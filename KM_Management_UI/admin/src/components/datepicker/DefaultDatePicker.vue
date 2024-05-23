<script setup>
import IconDropdown from '@/components/icons/IconDropdown.vue'

import { Popover, PopoverButton, PopoverPanel } from '@headlessui/vue'
import { DatePicker } from 'v-calendar'
import 'v-calendar/style.css'

import { onBeforeUnmount, ref } from 'vue'
import { useDateFormat } from '@vueuse/core'

const dateModel = defineModel('date', {
  default: { filterStart: null, filterEnd: null, filterType: null }
})

const hoverFilterDate = ref(false)
const range = ref({})
const selectedCategoryDate = ref('Last 30 Days')
const displayItem = ref('Last 30 Days')

function SelectQuickDate(type) {
  range.value = {}
  selectedCategoryDate.value = type
}

function ApplyFilterDate(closeFilter) {
  closeFilter()

  if (range.value?.start) {
    const startDate = useDateFormat(range.value.start, 'DD-MMM-YY')
    const endDate = useDateFormat(range.value.end, 'DD-MMM-YY')

    displayItem.value = `${startDate.value} to ${endDate.value}`

    const filterStart = useDateFormat(range.value.start, 'YYYY-MM-DD')
    const filterEnd = useDateFormat(range.value.end, 'YYYY-MM-DD')
    dateModel.value.filterStart = filterStart.value
    dateModel.value.filterEnd = filterEnd.value
    dateModel.value.filterType = 'Custom'
  } else {
    displayItem.value = selectedCategoryDate.value
    dateModel.value.filterType = selectedCategoryDate.value
    dateModel.value.filterStart = null
    dateModel.value.filterEnd = null
  }
}

onBeforeUnmount(() => {
  dateModel.value.filterType = 'Today'
  dateModel.value.filterStart = null
  dateModel.value.filterEnd = null
})
</script>

<template>
  <Popover class="relative" v-slot="{ open, close }">
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
        <span> {{ displayItem }} </span>
      </div>
      <div class="items-end">
        <IconDropdown
          :class="[
            open ? 'fill-white' : 'fill-green-800',
            hoverFilterDate ? 'fill-white' : 'fill-green-800'
          ]"
        />
      </div>
    </PopoverButton>

    <PopoverPanel class="absolute z-10 right-0">
      <div
        class="grid p-3 bg-white border border-green-800 rounded-2xl rounded-tr-none min-w-fit text-green-800 select-none"
      >
        <slot name="options">
          <div class="flex w-full gap-10">
            <div class="flex flex-col">
              <div class="flex flex-col gap-0.5">
                <button
                  class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                  :class="{ 'bg-[#d3eedb]': selectedCategoryDate === 'Today' }"
                  @click="SelectQuickDate('Today')"
                >
                  Today
                </button>
                <button
                  class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                  :class="{ 'bg-[#d3eedb]': selectedCategoryDate === 'Yesterday' }"
                  @click="SelectQuickDate('Yesterday')"
                >
                  Yesterday
                </button>
                <button
                  class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                  :class="{ 'bg-[#d3eedb]': selectedCategoryDate === 'Last 7 Days' }"
                  @click="SelectQuickDate('Last 7 Days')"
                >
                  Last 7 Days
                </button>
                <button
                  class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                  :class="{ 'bg-[#d3eedb]': selectedCategoryDate === 'Last 30 Days' }"
                  @click="SelectQuickDate('Last 30 Days')"
                >
                  Last 30 Days
                </button>
                <button
                  class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                  :class="{ 'bg-[#d3eedb]': selectedCategoryDate === 'Last 3 Months' }"
                  @click="SelectQuickDate('Last 3 Months')"
                >
                  Last 3 Months
                </button>
                <button
                  class="hover:bg-[#d3eedb] text-[#293050] pl-2 pr-2 pt-1 pb-1 rounded-lg text-left"
                  :class="{ 'bg-[#d3eedb]': selectedCategoryDate === 'Last 1 Year' }"
                  @click="SelectQuickDate('Last 1 Year')"
                >
                  Last 1 Year
                </button>
              </div>
              <div class="flex mt-20 items-start gap-2">
                <button
                  class="hover:bg-[#c4ddcc] bg-[#d3eedb] text-[#293050] px-2 py-1 rounded-lg"
                  @click="close"
                >
                  Cancel
                </button>

                <button
                  class="bg-green-700 hover:bg-green-800 text-white px-2 py-1 rounded-lg"
                  @click="ApplyFilterDate(close)"
                >
                  Apply
                </button>
              </div>
            </div>
            <div class="flex">
              <DatePicker
                :color="'green'"
                transparent
                borderless
                v-model.range="range"
                mode="date"
                @click="range?.start ? (selectedCategoryDate = null) : selectedCategoryDate"
              />
            </div>
          </div>
        </slot>
      </div>
    </PopoverPanel>
  </Popover>
</template>

<style scoped></style>