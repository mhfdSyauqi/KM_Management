<script setup>
import PreviousButton from '@/components/buttons/PreviousButton.vue'
import SearchButton from '@/components/buttons/SearchButton.vue'
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import IconEdit from '@/components/icons/IconEdit.vue'
import IconSort from '@/components/icons/IconSort.vue'
import NextButton from '@/components/buttons/NextButton.vue'
import IconDelete from '@/components/icons/IconDelete.vue'
import CreateRole from '@/views/setup/role/CreateRole.vue'
import EditRole from '@/views/setup/role/EditRole.vue'

import {
  usersRole,
  navigation,
  modals,
  GetUsersRoleByFilter,
  HandleSearch,
  HandlePagination,
  HandlingSort,
  HandlingPageLimit
} from '@/components/pages/setup/roles/useRoles.js'

import { editedUser } from '@/components/pages/setup/roles/patchRoles.js'

import { useNotificationStore } from '@/stores/useNotification.js'
import { onMounted, ref, watchEffect } from 'vue'
import { HandleDelete } from '@/components/pages/setup/roles/deleteRoles.js'

const notificationStore = useNotificationStore()
const pageLimit = ref(10)

onMounted(async () => {
  await GetUsersRoleByFilter()
})

function onEdit(user) {
  editedUser.value.user_name = user.login_name
  editedUser.value.full_name = user.full_name
  editedUser.value.role = user.role

  modals.value.edit = true
}

watchEffect(async () => {
  if (notificationStore.status === 'reload') {
    await GetUsersRoleByFilter()
    notificationStore.reset()
  }
})
</script>

<template>
  <div class="flex items-center align-middle gap-3 mb-3 bg-tea">
    <p class="text-sm">Setup</p>

    <span> > </span>
    <p class="text-sm text-orange-400">User Role</p>
  </div>

  <div class="w-full rounded-lg bg-white min-h-[25%] py-5 px-8">
    <div class="flex gap-2.5 items-center">
      <h1 class="basis-[65%] text-2xl font-bold text-green-800">User Role</h1>
      <SearchButton @on-search="HandleSearch" />
      <PrimaryButton class="w-36" @click.passive="modals.add = true">Add Account</PrimaryButton>
    </div>

    <br />

    <table class="w-full text-sm text-left my-3">
      <thead>
        <tr class="text-amber-600 bg-orange-100">
          <th class="p-2.5 w-[10%]">
            <p class="leading-4 text-base font-medium">Action</p>
          </th>
          <th class="w-[22%]">
            <div class="flex gap-1 items-center">
              <p class="leading-4 text-base font-medium">Full Name</p>
              <button @click="HandlingSort('FullName')">
                <IconSort />
              </button>
            </div>
          </th>
          <th class="w-[13%]">
            <div class="flex gap-1 items-center">
              <p class="leading-4 text-base font-medium">Role</p>
              <button @click="HandlingSort('Role')">
                <IconSort />
              </button>
            </div>
          </th>
          <th class="w-[30%]">
            <div class="flex gap-1 items-center">
              <p class="leading-4 text-base font-medium">Email</p>
              <button @click="HandlingSort('Email')">
                <IconSort />
              </button>
            </div>
          </th>
          <th class="w-[25%]">
            <div class="flex gap-1 items-center">
              <p class="leading-4 text-base font-medium">Job Title</p>
              <button @click="HandlingSort('JobTitle')">
                <IconSort />
              </button>
            </div>
          </th>
        </tr>
      </thead>

      <tbody>
        <tr class="text-sm border-b-2 hover:bg-orange-50" v-for="(user, i) in usersRole" :key="i">
          <td class="p-2.5 space-x-1">
            <button @click.prevent="onEdit(user)">
              <IconEdit class="w-5 h-5 hover:fill-green-800" />
            </button>
            <button @click.prevent="HandleDelete(user)" :disabled="usersRole.length <= 1">
              <IconDelete class="w-5 h-5 hover:fill-green-800" />
            </button>
          </td>
          <td>
            <p>{{ user.full_name }}</p>
          </td>
          <td>
            <div class="flex gap-1 items-center">
              <span
                class="rounded-xl px-2.5 py-1 bg-orange-200 text-orange-600 text-xs"
                v-if="user.role === 'Super'"
              >
                Super Admin
              </span>
              <span class="rounded-xl px-2.5 py-1 bg-purple-200 text-purple-600 text-xs" v-else>
                Admin
              </span>
            </div>
          </td>
          <td>
            <p>{{ user.email }}</p>
          </td>
          <td>
            <p>{{ user.job_title }}</p>
          </td>
        </tr>

        <tr v-show="usersRole.length === 0" class="text-sm border-b-2 bg-slate-50">
          <td colspan="5" class="py-5 text-center">
            <p class="text-center">Data Not Found</p>
          </td>
        </tr>
      </tbody>
    </table>

    <nav
      v-show="usersRole.length > 0"
      class="flex flex-row justify-between items-start md:items-center space-y-5 md:space-y-0 p-4"
    >
      <div class="space-x-2">
        <select
          class="border rounded border-gray-500"
          v-model="pageLimit"
          @change="HandlingPageLimit(pageLimit)"
        >
          <option :value="10">10</option>
          <option :value="20">20</option>
          <option :value="50">50</option>
        </select>

        <span class="text-sm text-gray-500">
          {{ navigation.show }}
        </span>
      </div>

      <ul class="inline-flex items-stretch -space-x-px">
        <li v-show="navigation.current > 1">
          <PreviousButton @click="HandlePagination(1)" />
        </li>
        <li v-show="navigation.previous !== null">
          <button
            @click="HandlePagination(navigation.previous)"
            class="px-3.5 py-2 text-gray-400 ring-1 ring-inset ring-gray-300 hover:bg-gray-300"
          >
            <span class="text-sm">{{ navigation.previous }}</span>
          </button>
        </li>
        <li v-show="navigation.current !== null">
          <button
            class="px-3.5 py-2 bg-gray-500 text-white font-bold ring-1 ring-inset ring-gray-300"
          >
            <span class="text-sm">{{ navigation.current }}</span>
          </button>
        </li>
        <li v-show="navigation.next !== null">
          <button
            @click="HandlePagination(navigation.next)"
            class="px-3.5 py-2 text-gray-400 ring-1 ring-inset ring-gray-300 hover:bg-gray-300"
          >
            <span class="text-sm">{{ navigation.next }}</span>
          </button>
        </li>
        <li v-show="navigation.max !== null && navigation.next !== null">
          <NextButton @click="HandlePagination(navigation.max)" />
        </li>
      </ul>
    </nav>
  </div>

  <CreateRole v-model="modals.add" />
  <EditRole v-model="modals.edit" />
</template>

<style scoped></style>