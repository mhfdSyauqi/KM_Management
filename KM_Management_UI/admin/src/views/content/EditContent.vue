<script setup>
import TextForm from '@/components/forms/TextForm.vue'
import ArticleEditor from '@/components/editors/ArticleEditor.vue'
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import SecondaryButton from '@/components/buttons/SecondaryButton.vue'
import DropdownForm from '@/components/forms/DropdownForm.vue'
import AbortButton from '@/components/buttons/AbortButton.vue'
import DescriptionEditor from '@/components/editors/DescriptionEditor.vue'

import {
  editArticle,
  categoryRef,
  GetArticleById,
  GetCategoryReference,
  HandleEdit,
  errorEdit,
  ResetInput
} from '@/components/pages/content/patchContents.js'

import { ConfirmSwal, ErrorSwal, ToastSwal } from '@/extension/SwalExt.js'
import { useNotificationStore } from '@/stores/useNotification.js'

import { onMounted, onUnmounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useDateFormat } from '@vueuse/core'

const route = useRoute()
const router = useRouter()
const onLoad = ref(true)
const notificationStore = useNotificationStore()
const userRole = route?.meta.userInfo.role

onMounted(async () => {
  await GetCategoryReference()

  const articleId = route.params.id
  const isFound = await GetArticleById(articleId)
  if (!isFound) {
    await ErrorSwal.fire({ text: `Article was not found!!!` })
    await router.push('/content')
  }
  onLoad.value = !isFound
})

onUnmounted(() => {
  ResetInput()
})

async function onEdit(action) {
  const { isConfirmed } = await ConfirmSwal.fire({ text: `your article will be ${action}` })
  if (!isConfirmed) return

  const result = await HandleEdit(action)
  if (result === 400 || result === false) {
    return await ToastSwal.fire({ icon: 'warning', text: 'Please re-check your input!!' })
  }

  if (result === true) {
    notificationStore.set('edit')
    return await router.push('/content')
  }

  return await ErrorSwal.fire({ text: 'fail connect to server, try again!!' })
}

function FormatDate(dateStr) {
  if (dateStr) {
    const formatedDate = useDateFormat(dateStr, 'DD-MM-YYYY HH:mm')
    return formatedDate.value
  }
  return ''
}
</script>

<template>
  <div class="flex items-center align-middle gap-3 mb-3">
    <RouterLink :to="{ name: 'content' }">
      <p class="text-sm">Content List</p>
    </RouterLink>
    <span> > </span>
    <p class="text-sm text-orange-400">Edit Content</p>
  </div>

  <div class="w-full rounded-lg bg-white min-h-[25%] max-h-[96%] box-border flex flex-col">
    <div class="flex gap-2.5 items-center border-b-2 py-5 px-8">
      <div class="basis-[84%] flex gap-3 items-center">
        <h1 class="text-2xl font-bold text-green-800">Edit Content</h1>
        <span
          v-show="!editArticle.is_active && !onLoad"
          class="rounded-xl px-2 py-1 bg-red-200 text-red-600 text-xs"
        >
          Inactive Category
        </span>
        <span
          v-show="editArticle.status === 'DRAFT' && !onLoad"
          class="rounded-xl px-2.5 py-1 bg-gray-200 text-gray-600 text-xs"
        >
          Draft
        </span>
        <span
          v-show="editArticle.status !== 'DRAFT' && !onLoad"
          class="rounded-xl px-2.5 py-1 bg-green-200 text-green-600 text-xs"
        >
          Published
        </span>
      </div>

      <RouterLink :to="{ name: 'content' }">
        <AbortButton>Cancel</AbortButton>
      </RouterLink>
      <SecondaryButton @click="onEdit('Draft')"> Save as Draft </SecondaryButton>
      <PrimaryButton @click="onEdit('Publish')" v-if="userRole === 'super'">
        Publish
      </PrimaryButton>
    </div>

    <div class="overflow-y-auto max-h-[80%]">
      <form class="w-full p-8 flex flex-col align-baseline gap-5">
        <TextForm
          :name="'Title'"
          :required="true"
          :error="errorEdit.title"
          v-model="editArticle.title"
        />
        <DropdownForm
          :required="true"
          :error="errorEdit.category_id"
          :name="'Category'"
          :items="categoryRef"
          v-model="editArticle.category_id"
        >
          <option :value="editArticle.category_id">{{ editArticle.category_name }}</option>
        </DropdownForm>

        <DescriptionEditor
          v-model="editArticle.newDescription"
          v-model:html="editArticle.newDescriptionHtml"
          :error="errorEdit.newDescription"
          :old-input="editArticle.description"
        />
        <ArticleEditor
          v-model="editArticle.newArticle"
          :error="errorEdit.newArticle"
          :old-input="editArticle.article"
        />

        <TextForm
          :name="'Additional Link'"
          :required="false"
          v-model="editArticle.additional_link"
        />

        <div class="flex flex-nowrap">
          <p class="font-medium basis-[15%]">Create By</p>
          <p class="basis-auto">
            {{ editArticle.create_by }} {{ FormatDate(editArticle.create_at) }}
          </p>
        </div>

        <div class="flex flex-nowrap" v-if="editArticle.modified_by">
          <p class="font-medium basis-[15%]">Last Modified By</p>
          <p class="basis-auto">
            {{ editArticle.modified_by }} {{ FormatDate(editArticle.modified_at) }}
          </p>
        </div>

        <div class="flex flex-nowrap" v-if="editArticle.published_by">
          <p class="font-medium basis-[15%]">Publish By</p>
          <p class="basis-auto">
            {{ editArticle.published_by }} {{ FormatDate(editArticle.published_at) }}
          </p>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
::-webkit-scrollbar {
  width: 0.15rem;
}

/* Track */
::-webkit-scrollbar-track {
  box-shadow: inset 0 0 5px grey;
}

/* Handle */
::-webkit-scrollbar-thumb {
  background: rgba(94, 109, 92, 0.6);
}

/* Handle on hover */
::-webkit-scrollbar-thumb:hover {
  background: rgba(94, 109, 92, 1);
}
</style>