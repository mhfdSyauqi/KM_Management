<script setup>
import IconSidebar from '@/components/sidebars/IconSidebar.vue'
import IconExpand from '@/components/icons/IconExpand.vue'
import IconDropdown from '@/components/icons/IconDropdown.vue'

import { RouterLink } from 'vue-router'
import { ref } from 'vue'

const menus = ref([
  {
    name: 'Dashboard',
    sub: [
      {
        name: 'Top 10 Analytics',
        url: '/dashboard/analytic',
        active: true
      },
      {
        name: 'Rate & Feedback',
        url: '/dashboard/rate',
        active: false
      }
    ],
    active: true,
    expand: true
  }
])

function setMenu(menu) {
  menus.value.map((key) => {
    key.active = key.name === menu
    key.sub?.map((item) => (item.active = false))
    if ('expand' in key) {
      key.expand = false
    }
  })
}

function toggleExpand(menu) {
  menus.value.map((key) => {
    if (key.name === menu && 'expand' in key) {
      key.expand = !key.expand
    } else if (key.name !== menu && 'expand' in key) {
      key.expand = false
    }
  })
}

function setSub(sub, menu) {
  menus.value.map((key) => {
    key.sub?.map((item) => (item.active = item.name === sub))
    key.active = key.name === menu
  })
}
</script>

<template>
  <div class="mt-12 flex flex-col gap-2">
    <template v-for="menu in menus" :key="menu.name">
      <div
        class="w-full rounded-2xl py-2 px-3.5 cursor-pointer select-none"
        :class="[menu.active ? 'bg-green-800' : 'hover:bg-[#b1d3b9]']"
      >
        <template v-if="menu.sub">
          <div
            class="flex gap-2 items-center justify-items-center align-middle"
            @click="toggleExpand(menu.name)"
          >
            <IconSidebar
              class="basis-[20%]"
              :icon="menu.icon"
              :class="[menu.active ? 'fill-white' : 'fill-primary']"
            />
            <p class="basis-[60%] text-sm font-medium" :class="{ 'text-white': menu.active }">
              {{ menu.name }}
            </p>

            <div class="basis-[20%]">
              <IconExpand v-if="!menu.active && !menu.expand" />
              <IconDropdown
                :class="[menu.active && menu.expand ? 'fill-white' : 'fill-primary']"
                v-else
              />
            </div>
          </div>
        </template>

        <template v-else>
          <RouterLink
            class="flex gap-2 items-center justify-items-center align-middle"
            :to="menu.url"
            @click="setMenu(menu.name)"
          >
            <IconSidebar
              class="basis-[20%]"
              :icon="menu.icon"
              :class="[menu.active ? 'fill-white' : 'fill-primary']"
            />
            <p class="basis-[60%] text-sm font-medium" :class="{ 'text-white': menu.active }">
              {{ menu.name }}
            </p>
            <span class="basis-[20%]"></span>
          </RouterLink>
        </template>
      </div>

      <template v-if="menu.sub && menu.expand">
        <RouterLink
          v-for="item in menu.sub"
          :key="item.name"
          :to="item.url"
          @click="setSub(item.name, menu.name)"
          class="w-full rounded-2xl py-2 px-3.5 cursor-pointer select-none hover:bg-[#b1d3b9] flex"
        >
          <span class="basis-[20%]"></span>
          <p class="basis-[80%] text-xs" :class="{ 'font-bold': item.active }">
            {{ item.name }}
          </p>
        </RouterLink>
      </template>
    </template>
  </div>
</template>

<style scoped></style>