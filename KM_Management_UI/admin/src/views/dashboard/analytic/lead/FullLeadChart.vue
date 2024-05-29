<script setup>
import IconMinimize from '@/components/icons/IconMinimize.vue'
import IconExpand from '@/components/icons/IconExpand.vue'
import { Disclosure, DisclosureButton, DisclosurePanel } from '@headlessui/vue'

import {
  dataLead,
  fullScreen,
  GetDetailLeadData
} from '@/components/pages/dashboard/analytic/useAnalytic.js'
import { onBeforeMount, ref } from 'vue'

const data = ref(null)

onBeforeMount(async () => {
  data.value = await GetDetailLeadData()
})
</script>

<template>
  <div class="fixed top-0 left-0 w-full h-full bg-gray-50 z-50 flex flex-col flex-nowrap gap-3">
    <button
      class="fixed right-10 top-7 bg-gray-200 p-2 rounded-lg hover:drop-shadow-lg hover:scale-105"
      title="Minimize"
      @click="fullScreen.stats = 'none'"
    >
      <IconMinimize class="w-5 h-5" />
    </button>

    <div class="mx-10 mt-16">
      <div class="w-full text-lg border-b-2 border-green-800 pb-3 space-x-2">
        <span class="text-orange-400"> Leaderboard </span>
      </div>
    </div>

    <div class="px-10 py-5 flex flex-col gap-3 overflow-y-scroll">
      <Disclosure v-slot="{ open }" v-for="(item, index) in dataLead" :key="item.user_name">
        <div class="border px-5 rounded-2xl select-none border-orange-400" :class="open && 'pb-7'">
          <DisclosureButton class="py-2 px-5 flex justify-between items-center w-full">
            <div class="flex flex-col justify-start items-start">
              <p class="font-medium text-gray-500 text-lg">{{ index + 1 }}. {{ item.user_name }}</p>
              <p class="ml-4 text-sm text-gray-400">{{ item.count }} Categories</p>
            </div>
            <IconExpand :class="open && 'rotate-90 transform'" />
          </DisclosureButton>
          <DisclosurePanel>
            <section class="flex gap-10">
              <table class="w-full text-sm text-left my-3">
                <thead>
                  <tr class="text-amber-600 bg-orange-100">
                    <th colspan="w-[10%]"></th>
                    <th class="w-[45%]">
                      <div class="flex items-center py-2.5">
                        <p class="leading-4 text-base font-medium">No.</p>
                      </div>
                    </th>
                    <th class="w-[45%]">
                      <div class="flex items-center">
                        <p class="leading-4 text-base font-medium">Category</p>
                      </div>
                    </th>
                  </tr>
                </thead>

                <tbody>
                  <tr
                    class="text-sm border-b-2 hover:bg-orange-50"
                    v-for="(detail, index) in data.filter(
                      (key) => key.user_name === item.user_name
                    )"
                    :key="index"
                  >
                    <td>&nbsp;</td>

                    <td class="py-2.5">
                      {{ index + 1 }}
                    </td>

                    <td>
                      <p>{{ detail.category }}</p>
                    </td>
                  </tr>
                </tbody>
              </table>
            </section>
          </DisclosurePanel>
        </div>
      </Disclosure>
    </div>
  </div>
</template>

<style scoped>
::-webkit-scrollbar {
  width: 0.15rem;
}

/* Track */
::-webkit-scrollbar-track {
  box-shadow: inset 0 0 5px transparent;
}

/* Handle */
::-webkit-scrollbar-thumb {
  background: transparent;
}

/* Handle on hover */
::-webkit-scrollbar-thumb:hover {
  background: transparent;
}
</style>