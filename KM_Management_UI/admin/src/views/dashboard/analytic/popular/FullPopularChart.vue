<script setup>
import IconMinimize from '@/components/icons/IconMinimize.vue'
import IconUndo from '@/components/icons/IconUndo.vue'

import {
  fullScreen,
  dataPopular,
  GetDetailPopularData
} from '@/components/pages/dashboard/analytic/useAnalytic.js'

import { Pie } from 'vue-chartjs'
import { Chart as ChartJs, Tooltip, Legend, ArcElement } from 'chart.js'
import { ToastSwal } from '@/extension/SwalExt.js'
import { onBeforeMount, ref } from 'vue'

ChartJs.register(Tooltip, Legend, ArcElement)

const chartEl = ref(null)
const reference = ref(null)
const data = ref(null)
const options = ref({
  responsive: true,
  maintainAspectRatio: false,

  plugins: {
    legend: {
      position: 'bottom',
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
            const percent = ((count / total) * 100).toFixed(1)

            label += `${count} of ${total} (${percent}%)`
          }
          return label
        }
      }
    }
  },
  onClick: async (e) => {
    if (fullScreen.value.isDetail) return

    const activePoints = chartEl.value.chart.getElementsAtEventForMode(e, 'point', options)
    const firstPoint = activePoints[0]

    if (firstPoint) {
      const labels = chartEl.value.chart.data.labels[firstPoint.index]
      const selected = dataPopular.value.raw.find((key) => key.name === labels)

      reference.value = labels

      const result = await GetDetailPopularData(selected.reference)

      if (result === null) {
        return await ToastSwal.fire({ icon: 'info', text: 'No Detail Data Found' })
      }

      data.value = result
      fullScreen.value.isDetail = true

      console.log(dataPopular.value)
    }
  }
})

onBeforeMount(() => {
  data.value = dataPopular.value.chart

  // data.value = {
  //   labels: ['VPN', 'Email', 'SAP', 'Map', 'Kain', 'Orange'],
  //   datasets: [
  //     {
  //       backgroundColor: ['#9195F6', '#E99497', '#DBCC95', '#638889', '#A4907C', '#D2AEE9'],
  //       data: [5, 4, 1, 0, 0, 0],
  //       total: 10
  //     },
  //     {
  //       backgroundColor: ['#9195F6', '#E99497', '#DBCC95', '#638889', '#A4907C', '#D2AEE9'],
  //       data: [0, 0, 0, 5, 4, 1],
  //       total: 10
  //     }
  //   ]
  // }
})

function onBack() {
  reference.value = null
  fullScreen.value.isDetail = false

  data.value = dataPopular.value.chart
}

function onClose() {
  fullScreen.value.stats = 'none'
  fullScreen.value.isDetail = false
  reference.value = null
  data.value = dataPopular.value.chart
}
</script>

<template>
  <div class="fixed top-0 left-0 w-full h-full bg-gray-50 z-50 flex flex-col flex-nowrap gap-3">
    <button
      class="fixed right-20 top-7 bg-gray-200 p-2 rounded-lg hover:drop-shadow-lg hover:scale-105"
      title="Back"
      @click="onBack"
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

    <div class="mx-10 mt-16">
      <div class="w-full text-lg border-b-2 border-green-800 pb-3 space-x-2">
        <span class="text-orange-400" :class="{ '!text-gray-500': fullScreen.isDetail }">
          Popular Categories
        </span>
        <span v-show="fullScreen.isDetail"> > </span>
        <span v-show="fullScreen.isDetail" class="text-orange-400"> {{ reference }} </span>
      </div>
    </div>

    <div class="mx-10 mt-16 flex flex-col">
      <div class="basis-[40%]">
        <Pie ref="chartEl" :data="data" :options="options" />
      </div>

      <div class="basis-[55%]">
        <table class="w-full text-sm text-left my-3" v-show="reference !== null">
          <thead>
            <tr class="text-amber-600 bg-orange-100">
              <th colspan="w-[10%]"></th>
              <th class="w-[15%]">
                <div class="flex items-center py-2.5">
                  <p class="leading-4 text-base font-medium">No.</p>
                </div>
              </th>
              <th class="w-[45%]">
                <div class="flex items-center">
                  <p class="leading-4 text-base font-medium">Category</p>
                </div>
              </th>
              <th class="w-[15%]">
                <div class="flex items-center">
                  <p class="leading-4 text-base font-medium">Percentage</p>
                </div>
              </th>
              <th class="w-[15%]">
                <div class="flex items-center">
                  <p class="leading-4 text-base font-medium">Total</p>
                </div>
              </th>
            </tr>
          </thead>

          <tbody>
            <tr
              class="text-sm border-b-2 hover:bg-orange-50"
              v-for="(detail, index) in dataPopular?.details"
              :key="index"
            >
              <td>&nbsp;</td>

              <td class="py-2.5">
                {{ index + 1 }}
              </td>

              <td>
                <p>{{ detail.name }}</p>
              </td>

              <td>
                <p>{{ detail.percent }}%</p>
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

<style scoped></style>