<script setup>
import IconFullscreen from '@/components/icons/IconFullscreen.vue'
import IconSave from '@/components/icons/IconSave.vue'
import { onMounted, ref, watch } from 'vue'
import {
  filterData,
  fullScreen,
  GetExcelLead,
  GetGeneralLeadData
} from '@/components/pages/dashboard/analytic/useAnalytic.js'

const data = ref(null)

onMounted(async () => {
  data.value = await GetGeneralLeadData()
})

watch(
  filterData,
  async () => {
    data.value = await GetGeneralLeadData()
  },
  { deep: true }
)
</script>

<template>
  <div class="bg-chart-item h-full rounded-xl shadow-lg box-border flex flex-col">
    <div class="flex justify-between items-center px-5 py-3 border-b-2 border-green-800 w-full">
      <h1 class="text-lg font-medium text-green-800">Leaderboard</h1>

      <div class="space-x-2">
        <button
          class="p-2 rounded-lg"
          :class="
            data === null
              ? 'bg-gray-400 drop-shadow'
              : 'bg-gray-100 hover:drop-shadow-lg hover:scale-105'
          "
          title="Export to Excel"
          @click="GetExcelLead"
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
          @click="fullScreen.stats = 'Lead'"
        >
          <IconFullscreen class="w-5 h-5" />
        </button>
      </div>
    </div>

    <div
      v-if="data === null || data.length === 0"
      class="flex justify-center items-center bg-gray-100 m-4 h-full min-h-40"
    >
      <p class="italic font-bold">No Data Found</p>
    </div>

    <div v-else class="p-9 flex">
      <ul class="space-y-8 basis-full">
        <li v-for="item in data" :key="item.user_name" class="flex justify-between">
          <p class="font-normal">{{ item.user_name }}</p>
          <p class="font-bold">{{ item.count }}</p>
        </li>
      </ul>
    </div>
  </div>
</template>

<style scoped></style>