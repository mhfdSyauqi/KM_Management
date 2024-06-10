<script setup>
import PopularChart from '@/views/dashboard/analytic/popular/PopularChart.vue'
import FullPopularChart from '@/views/dashboard/analytic/popular/FullPopularChart.vue'
import HitChart from '@/views/dashboard/analytic/hit/HitChart.vue'
import LeaderboardChart from '@/views/dashboard/analytic/lead/LeaderboardChart.vue'
import DefaultDatePicker from '@/components/datepicker/DefaultDatePicker.vue'

import { filterData, fullScreen } from '@/components/pages/dashboard/analytic/useAnalytic.js'
import FullHitChart from '@/views/dashboard/analytic/hit/FullHitChart.vue'
import FullLeadChart from '@/views/dashboard/analytic/lead/FullLeadChart.vue'
</script>

<template>
  <div class="flex items-center align-middle gap-3 mb-3 bg-tea">
    <p class="text-sm">Dashboard</p>

    <span> > </span>
    <p class="text-sm text-orange-400">Analytics</p>
  </div>

  <div class="w-full rounded-lg bg-white min-h-[25%] py-5 px-8">
    <div class="flex justify-between items-center">
      <h1 class="text-2xl font-bold text-green-800">Top 10 Analytics</h1>

      <DefaultDatePicker v-model:date="filterData" />
    </div>

    <br />
    <div class="flex gap-3.5 flex-wrap xl:flex-nowrap">
      <div class="basis-full xl:basis-2/3 flex flex-col gap-3.5">
        <PopularChart />
        <HitChart />
      </div>
      <div class="basis-full xl:basis-1/3">
        <LeaderboardChart />
      </div>
    </div>
  </div>

  <FullPopularChart v-if="fullScreen.stats === 'Popular'" />
  <FullHitChart v-if="fullScreen.stats === 'Hit'" />
  <FullLeadChart v-if="fullScreen.stats === 'Lead'" />
</template>

<style scoped></style>