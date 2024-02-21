<script setup>
import IconSidebar from '@/components/sidebars/IconSidebar.vue'
import { RouterLink } from 'vue-router'

const menus = [
  {
    name: 'Content',
    url: '/content',
    icon: 'article'
  }
]
</script>

<template>
  <div class="mt-12 flex flex-col gap-2">
    <div class="w-full rounded-2xl bg-green-800 py-2 px-5" v-for="menu in menus" :key="menu.name">
      <RouterLink class="flex gap-2 items-center" :to="menu.url">
        <IconSidebar :icon="menu.icon" />
        <p class="text-white font-semibold">{{ menu.name }}</p>
      </RouterLink>
    </div>
  </div>
</template>

<style scoped></style>