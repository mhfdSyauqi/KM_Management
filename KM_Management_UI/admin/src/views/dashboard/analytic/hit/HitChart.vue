<script setup>
import IconFullscreen from '@/components/icons/IconFullscreen.vue'
import IconSave from '@/components/icons/IconSave.vue'
import { onMounted, ref, watch } from 'vue'
import {
  filterData,
  fullScreen,
  GetExcelHit,
  GetGeneralHitData
} from '@/components/pages/dashboard/analytic/useAnalytic.js'

const data = ref(null)

onMounted(async () => {
  data.value = await GetGeneralHitData()
})

watch(
  filterData,
  async () => {
    data.value = await GetGeneralHitData()
  },
  { deep: true }
)
</script>

<template>
  <div class="basis-1/3 bg-chart-item rounded-xl shadow-lg flex flex-col">
    <div class="flex justify-between items-center px-5 py-3 border-b-2 border-green-800 w-full">
      <h1 class="text-lg font-medium text-green-800">Hit Categories</h1>

      <div class="space-x-2">
        <button
          class="p-2 rounded-lg"
          :class="
            data === null
              ? 'bg-gray-400 drop-shadow'
              : 'bg-gray-100 hover:drop-shadow-lg hover:scale-105'
          "
          @click="GetExcelHit"
          title="Export to Excel"
          :disabled="data === null"
        >
          <IconSave class="w-5 h-5" />
        </button>
        <button
          class="p-2 rounded-lg"
          :class="
            data === null
              ? 'bg-gray-400 drop-shadow'
              : 'bg-gray-100 hover:drop-shadow-lg hover:scale-105'
          "
          title="Full Screen"
          :disabled="data === null"
          @click="fullScreen.stats = 'Hit'"
        >
          <IconFullscreen class="w-5 h-5" />
        </button>
      </div>
    </div>

    <div v-if="data === null" class="flex justify-center items-center bg-gray-100 m-4 min-h-32">
      <p class="italic font-bold">No Data Found</p>
    </div>

    <div v-else class="flex flex-col justify-center items-center m-4 gap-3.5 p-4">
      <div class="flex w-full justify-center items-center">
        <div
          class="h-12 bg-orange-500 rounded-l-xl"
          :style="{ 'flex-basis': `${data.match_percentage}%` }"
        >
          &nbsp;
        </div>
        <div
          class="h-12 bg-orange-300 rounded-r-xl"
          :style="{ 'flex-basis': `${data.unmatch_percentage}%` }"
        >
          &nbsp;
        </div>
      </div>

      <div class="flex w-full justify-center items-center gap-3">
        <div class="basis-1/2 bg-gray-200 rounded-xl px-5 py-3.5 gap-0.5">
          <p class="text-2xl">{{ data.match_percentage }} %</p>
          <div class="flex gap-2 justify-start items-center">
            <div class="bg-orange-500 w-5 h-2 rounded-xl">&nbsp;</div>
            <p class="text-xs text-gray-600">Match Categories</p>
          </div>
        </div>
        <div class="basis-1/2 bg-gray-200 rounded-xl px-5 py-3.5 gap-0.5">
          <p class="text-2xl">{{ data.unmatch_percentage }}%</p>
          <div class="flex gap-2 justify-start items-center">
            <div class="bg-orange-300 w-5 h-2 rounded-xl">&nbsp;</div>
            <p class="text-xs text-gray-600">No Match Categories</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped></style>