<script setup>
import IconMinimize from '@/components/icons/IconMinimize.vue'
import IconUndo from '@/components/icons/IconUndo.vue'

import {
  dataHit,
  fullScreen,
  GetDetailHitData
} from '@/components/pages/dashboard/analytic/useAnalytic.js'
import { ref } from 'vue'

const reference = ref(null)
const data = ref(null)

async function onChoseRef(newRefrence) {
  if (newRefrence === reference.value || newRefrence === null) {
    reference.value = null
    fullScreen.value.isDetail = false
    data.value = null
    return
  }

  fullScreen.value.isDetail = true
  reference.value = newRefrence

  const selected = newRefrence === 'Match Categories' ? 'match' : 'unmatch'

  data.value = await GetDetailHitData(selected)
}

function onClose() {
  fullScreen.value.stats = 'none'
  fullScreen.value.isDetail = false
  reference.value = null
}
</script>

<template>
  <div
    class="fixed top-0 left-0 w-full h-full bg-gray-50 z-50 flex flex-col flex-nowrap gap-3 select-none"
  >
    <button
      class="fixed right-20 top-7 bg-gray-200 p-2 rounded-lg hover:drop-shadow-lg hover:scale-105"
      title="Back"
      @click="onChoseRef(null)"
      v-show="fullScreen.isDetail"
    >
      <IconUndo class="w-5 h-5" />
    </button>
    <button
      class="fixed right-10 top-7 bg-gray-200 p-2 rounded-lg hover:drop-shadow-lg hover:scale-105"
      title="Minimize"
      @click="onClose"
    >
      <IconMinimize class="w-5 h-5" />
    </button>

    <div class="basis-[10%] px-10 pt-14">
      <div class="w-full text-lg border-b-2 border-green-800 pb-3 space-x-2">
        <span class="text-orange-400" :class="{ '!text-gray-500': fullScreen.isDetail }">
          Hit Categories
        </span>
        <span v-show="fullScreen.isDetail"> > </span>
        <span v-show="fullScreen.isDetail" class="text-orange-400"> {{ reference }} </span>
      </div>
    </div>

    <div class="basis-[90%] px-20 pt-5 flex flex-col flex-nowrap gap-1.5">
      <div class="basis-[12%] flex w-full justify-center items-center">
        <div
          class="h-full bg-orange-500 rounded-l-xl cursor-pointer"
          :class="{ '!bg-gray-200': reference === 'No Match Categories' }"
          :style="{ 'flex-basis': `${dataHit.match_percentage}%` }"
          @click="onChoseRef('Match Categories')"
        >
          &nbsp;
        </div>
        <div
          class="h-full bg-orange-300 rounded-r-xl cursor-pointer"
          :class="{ '!bg-gray-200': reference === 'Match Categories' }"
          :style="{ 'flex-basis': `${dataHit.unmatch_percentage}%` }"
          @click="onChoseRef('No Match Categories')"
        >
          &nbsp;
        </div>
      </div>

      <div class="basis-[13%] flex w-full justify-center items-center gap-3">
        <div
          class="basis-1/2 bg-gray-200 rounded-xl px-5 py-3.5 gap-0.5"
          :class="{ '!basis-full': reference === 'Match Categories' }"
          v-show="reference === 'Match Categories' || reference === null"
        >
          <p class="text-2xl">{{ dataHit.match_percentage }} %</p>
          <div class="flex gap-2 justify-start items-center">
            <div class="bg-orange-500 w-5 h-2 rounded-xl">&nbsp;</div>
            <p class="text-xs text-gray-600">Match Categories</p>
          </div>
        </div>
        <div
          class="basis-1/2 bg-gray-200 rounded-xl px-5 py-3.5 gap-0.5"
          :class="{ '!basis-full': reference === 'No Match Categories' }"
          v-show="reference === 'No Match Categories' || reference === null"
        >
          <p class="text-2xl">{{ dataHit.unmatch_percentage }}%</p>
          <div class="flex gap-2 justify-start items-center">
            <div class="bg-orange-300 w-5 h-2 rounded-xl">&nbsp;</div>
            <p class="text-xs text-gray-600">No Match Categories</p>
          </div>
        </div>
      </div>

      <div class="basis-[75%] max-h-[55%] overflow-y-auto">
        <table class="w-full table-fixed text-sm text-left my-3" v-show="reference !== null">
          <thead class="sticky top-0">
            <tr class="text-amber-600 bg-orange-100">
              <th colspan="w-[10%]"></th>
              <th class="w-[22%]">
                <div class="flex items-center py-2.5">
                  <p class="leading-4 text-base font-medium">No.</p>
                </div>
              </th>
              <th class="w-[46%]">
                <div class="flex items-center">
                  <p class="leading-4 text-base font-medium">Category</p>
                </div>
              </th>
              <th class="w-[22%]">
                <div class="flex items-center">
                  <p class="leading-4 text-base font-medium">Hit Count</p>
                </div>
              </th>
            </tr>
          </thead>

          <tbody>
            <tr
              class="text-sm border-b-2 hover:bg-orange-50"
              v-for="(detail, index) in data?.categories"
              :key="index"
            >
              <td>&nbsp;</td>

              <td class="py-2.5">
                {{ index + 1 }}
              </td>

              <td>
                <p>{{ detail.category }}</p>
              </td>

              <td>
                <p>{{ detail.count }}</p>
              </td>
            </tr>
            <tr
              class="text-sm border-b-2 hover:bg-orange-50"
              v-for="(detail, index) in data?.categories"
              :key="index"
            >
              <td>&nbsp;</td>

              <td class="py-2.5">
                {{ index + 1 }}
              </td>

              <td>
                <p>{{ detail.category }}</p>
              </td>

              <td>
                <p>{{ detail.count }}</p>
              </td>
            </tr>
            <tr
              class="text-sm border-b-2 hover:bg-orange-50"
              v-for="(detail, index) in data?.categories"
              :key="index"
            >
              <td>&nbsp;</td>

              <td class="py-2.5">
                {{ index + 1 }}
              </td>

              <td>
                <p>{{ detail.category }}</p>
              </td>

              <td>
                <p>{{ detail.count }}</p>
              </td>
            </tr>
          </tbody>
        </table>
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