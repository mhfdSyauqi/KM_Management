<script setup>
import AdminSidebar from '@/components/sidebars/AdminSidebar.vue'
import { RouterView } from 'vue-router'
</script>

<template>
  <main class="flex flex-wrap flex-col h-svh max-w-svw text-primary font-roboto max-lg:hidden">
    <section class="bg-side-bar basis-full w-[17svw] p-6">
      <h3 class="text-center font-bold text-xl font-comic-sans">Knowledge Management</h3>
      <AdminSidebar />
    </section>
    <section
      class="bg-head-bar basis-[8svh] w-[83svw] px-10 flex items-center justify-end justify-items-end drop-shadow-lg shadow-sm"
    >
      <h4 class="font-bold">Hi, Admin</h4>
    </section>
    <section class="bg-gray-100 basis-[92svh] w-[83svw] p-5 max-h-[92svh]">
      <RouterView />
    </section>
  </main>

  <section class="lg:hidden">
    <p>Please open Management App In Laptop or wide screen</p>
  </section>
</template>

<style scoped></style>