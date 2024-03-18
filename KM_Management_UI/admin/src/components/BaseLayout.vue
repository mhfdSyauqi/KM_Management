<script setup>
import AdminSidebar from '@/components/sidebars/AdminSidebar.vue'
import { RouterView, useRoute } from 'vue-router'
import SuperAdminSidebar from '@/components/sidebars/SuperAdminSidebar.vue'
import UserSidebar from '@/components/sidebars/UserSidebar.vue'

const route = useRoute()
const userInfo = route.meta.userInfo
</script>

<template>
  <main class="flex flex-wrap flex-col h-svh max-w-svw text-primary font-roboto max-lg:hidden">
    <section class="bg-side-bar basis-full w-[17%] p-6">
      <h3 class="text-center font-bold text-xl font-comic-sans">Knowledge Management</h3>
      <SuperAdminSidebar v-if="userInfo.role === 'super'" />
      <AdminSidebar v-else-if="userInfo.role === 'admin'" />
      <UserSidebar v-else />
    </section>
    <section
      class="bg-head-bar basis-[8svh] w-[83%] px-10 flex items-center justify-end justify-items-end drop-shadow-lg shadow-sm"
    >
      <h4 class="font-bold">Hi, {{ userInfo.userName }}</h4>
    </section>
    <section class="bg-gray-100 basis-[92svh] w-[83%] p-5 max-h-[92svh] overflow-y-auto">
      <RouterView />
    </section>
  </main>

  <section class="lg:hidden">
    <p>Please open Management App In Laptop or wide screen</p>
  </section>
</template>

<style scoped></style>