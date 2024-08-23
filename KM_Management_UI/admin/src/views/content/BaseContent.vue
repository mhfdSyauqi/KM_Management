<script setup>
import IconEdit from '@/components/icons/IconEdit.vue'
import IconSort from '@/components/icons/IconSort.vue'
import SearchButton from '@/components/buttons/SearchButton.vue'
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import OptionButton from '@/components/buttons/OptionButton.vue'
import NextButton from '@/components/buttons/NextButton.vue'
import PreviousButton from '@/components/buttons/PreviousButton.vue'

import {
  contents,
  navigation,
  GetContentsByFilter,
  HandleSearch,
  HandleCheck,
  HandlePagination,
  HandlingSort,
  HandlingPageLimit
} from '@/components/pages/content/useContents.js'

import { onMounted, ref } from 'vue'
import { ToastSwal } from '@/extension/SwalExt.js'
import { useNotificationStore } from '@/stores/useNotification.js'

const notificationStore = useNotificationStore()

onMounted(async () => {
  await GetContentsByFilter()

  if (notificationStore.status === 'add') {
    await ToastSwal.fire({ icon: 'success', text: 'Success add new article' })
    notificationStore.reset()
  }

  if (notificationStore.status === 'edit') {
    await ToastSwal.fire({ icon: 'success', text: 'Success edit article' })
    notificationStore.reset()
  }
})

const statusModel = ref({
  Draft: false,
  Published: false,
  Inactive: false
})

const pageLimit = ref(10)

async function onCheck(e) {
  const checkValue = e.target.value
  const isCheck = statusModel.value[checkValue]
  await HandleCheck(isCheck, checkValue)
}
</script>

<template>
  <p class="text-sm text-orange-400 mb-3">Content List</p>

  <div class="w-full rounded-lg bg-white min-h-[25%] py-5 px-8">
    <div class="flex gap-2.5 items-center">
      <h1 class="basis-[65%] text-2xl font-bold text-green-800">Content List</h1>
      <SearchButton @on-search="HandleSearch" />
      <OptionButton>
        <template #default> Status </template>

        <template #options>
          <label>
            <input
              type="checkbox"
              name="status"
              value="Draft"
              v-model="statusModel.Draft"
              @change="onCheck"
            />
            <span class="ml-3">Draft</span>
          </label>

          <label>
            <input
              type="checkbox"
              name="status"
              value="Published"
              v-model="statusModel.Published"
              @change="onCheck"
            />
            <span class="ml-3">Published</span>
          </label>

          <label>
            <input
              type="checkbox"
              name="status"
              value="Inactive"
              v-model="statusModel.Inactive"
              @change="onCheck"
            />
            <span class="ml-3">Inactive Category</span>
          </label>
        </template>
      </OptionButton>
      <RouterLink :to="{ name: 'create-content' }">
        <PrimaryButton>Create</PrimaryButton>
      </RouterLink>
    </div>

    <br />

    <table class="w-full text-sm text-left my-3">
      <thead>
        <tr class="text-amber-600 bg-orange-100">
          <th class="p-2.5 w-[5%]">&nbsp;</th>
          <th class="w-[35%]">
            <div class="flex gap-1 items-center">
              <p class="leading-4 text-base font-medium">Title</p>
              <button @click="HandlingSort('Title')">
                <IconSort />
              </button>
            </div>
          </th>
          <th class="w-[35%]">
            <div class="flex gap-1 items-center">
              <p class="leading-4 text-base font-medium">Category</p>
              <button @click="HandlingSort('Category')">
                <IconSort />
              </button>
            </div>
          </th>
          <th class="w-[25%]">
            <div class="flex gap-1 items-center">
              <p class="leading-4 text-base font-medium">Status</p>
              <button @click="HandlingSort('Status')">
                <IconSort />
              </button>
            </div>
          </th>
        </tr>
      </thead>

      <tbody>
        <tr
          class="text-sm border-b-2 hover:bg-orange-50"
          v-for="content in contents"
          :key="content.id"
        >
          <td class="py-2.5 text-center">
            <button title="Edit">
              <RouterLink :to="{ name: 'edit-content', params: { id: content.id } }">
                <IconEdit class="w-5 h-5 hover:fill-green-800" />
              </RouterLink>
            </button>
          </td>
          <td>
            <p>{{ content.title }}</p>
          </td>
          <td>
            <p>{{ content.category }}</p>
          </td>
          <td>
            <div class="flex gap-1 items-center">
              <span
                class="rounded-xl px-2.5 py-1 bg-green-200 text-green-600 text-xs"
                v-show="content.article_status === 'PUBLISHED'"
              >
                Published
              </span>
              <span
                class="rounded-xl px-2.5 py-1 bg-red-200 text-red-600 text-xs"
                v-show="!content.category_status"
              >
                Inactive Category
              </span>
              <span
                class="rounded-xl px-2.5 py-1 bg-slate-200 text-slate-600 text-xs"
                v-show="content.article_status !== 'PUBLISHED'"
              >
                Draft
              </span>
            </div>
          </td>
        </tr>
        <tr v-show="contents.length === 0" class="text-sm border-b-2 bg-slate-50">
          <td colspan="4" class="py-5 text-center">
            <p class="text-center">Data Not Found</p>
          </td>
        </tr>
      </tbody>
    </table>

    <nav
      v-show="contents.length > 0"
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
</template>

<style scoped></style>