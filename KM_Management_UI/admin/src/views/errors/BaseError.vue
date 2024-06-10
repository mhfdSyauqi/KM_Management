<script setup>
import IconUndo from '@/components/icons/IconUndo.vue'
import IconDashboard from '@/components/icons/IconDashboard.vue'

import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()

const errorCode = +route.params.code
const errorMsg =
  errorCode === 500
    ? 'Ops something went wrong...'
    : errorCode === 401
      ? 'You dont have authorization to access this page!!!'
      : 'Sorry, page not found...'

function onReload() {
  router.push('/')
}
</script>

<template>
  <div
    class="w-svw h-svh flex flex-col items-center align-middle justify-center bg-rose-500 text-white gap-3.5"
  >
    <p class="text-8xl italic">{{ errorCode }}</p>
    <div class="text-center flex flex-col gap-3 justify-center items-center align-middle">
      <p class="text-xl">{{ errorMsg }}</p>
      <button
        class="w-32 rounded-lg ring-2 ring-white p-2 flex gap-2 items-center justify-center hover:bg-white hover:text-rose-500 hover:ring-rose-500 group"
        v-show="errorCode !== 500"
        @click="onReload"
      >
        <IconDashboard class="w-5 h-5 fill-white group-hover:fill-rose-500" />
        <span>Main Menu</span>
      </button>

      <button
        class="w-32 rounded-lg ring-2 ring-white p-2 flex gap-2 items-center justify-center hover:bg-white hover:text-rose-500 hover:ring-rose-500 group"
        v-show="errorCode === 500"
        @click="onReload"
      >
        <IconUndo class="w-5 h-5 fill-white group-hover:fill-rose-500" />
        <span>Refresh</span>
      </button>
    </div>
  </div>
</template>

<style scoped></style>