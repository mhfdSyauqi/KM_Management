<script setup>
import IconFullscreen from '@/components/icons/IconFullscreen.vue'
import IconSave from '@/components/icons/IconSave.vue'

import { Pie } from 'vue-chartjs'
import { Chart as ChartJs, Tooltip, Legend, ArcElement } from 'chart.js'
import { onMounted, ref, watch } from 'vue'
import {
  filterData,
  fullScreen,
  GetExcelPopular,
  GetGeneralPopularData
} from '@/components/pages/dashboard/analytic/useAnalytic.js'

ChartJs.register(Tooltip, Legend, ArcElement)

const data = ref(null)
const options = ref({
  responsive: true,
  maintainAspectRatio: false,
  layout: {
    padding: {
      top: 15,
      bottom: 15,
      right: 55
    }
  },
  plugins: {
    legend: {
      position: 'right',
      labels: {
        generateLabels: (e) => {
          const labels = e.data.labels
          const bgColor = e.data.datasets[0].backgroundColor

          return labels.map((label, index) => ({
            text: label,
            fillStyle: bgColor[index],
            textAlign: 'center'
          }))
        }
      }
    },
    tooltip: {
      callbacks: {
        label: function (context) {
          const total = context.dataset?.total
          const count = context.dataset?.data[context.dataIndex]

          let label = ' '

          if (count && total) {
            const percent = ((count / total) * 100).toFixed(2)

            label += `${count} of ${total} (${percent}%)`
          }
          return label
        }
      }
    }
  }
})

onMounted(async () => {
  data.value = await GetGeneralPopularData()
})

watch(
  filterData,
  async () => {
    data.value = await GetGeneralPopularData()
  },
  { deep: true }
)
</script>

<template>
  <div class="basis-2/3 bg-chart-item rounded-xl shadow-lg flex flex-col">
    <div class="flex justify-between items-center px-5 py-3 border-b-2 border-green-800 w-full">
      <h1 class="text-lg font-medium text-green-800">Popular Categories</h1>

      <div class="space-x-2">
        <button
          class="p-2 rounded-lg"
          :class="
            data === null
              ? 'bg-gray-400 drop-shadow'
              : 'bg-gray-100 hover:drop-shadow-lg hover:scale-105'
          "
          title="Export Excel"
          @click="GetExcelPopular"
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
          title="Fullscreen"
          :disabled="data === null"
          @click="fullScreen.stats = 'Popular'"
        >
          <IconFullscreen class="w-5 h-5" />
        </button>
      </div>
    </div>

    <div class="flex justify-center items-center bg-gray-100 m-4 min-h-52">
      <p v-if="data === null" class="italic font-bold">No Data Found</p>

      <Pie v-else :data="data" :options="options" />
    </div>
  </div>
</template>

<style scoped></style>