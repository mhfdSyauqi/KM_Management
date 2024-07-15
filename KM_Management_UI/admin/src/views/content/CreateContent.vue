<script setup>
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import AbortButton from '@/components/buttons/AbortButton.vue'
import SecondaryButton from '@/components/buttons/SecondaryButton.vue'
import TextForm from '@/components/forms/TextForm.vue'
import DropdownForm from '@/components/forms/DropdownForm.vue'
import ArticleEditor from '@/components/editors/ArticleEditor.vue'
import DescriptionEditor from '@/components/editors/DescriptionEditor.vue'

import {
  categoryRef,
  newArticle,
  errorInput,
  GetCategoryReference,
  HandleSave,
  ResetInput
} from '@/components/pages/content/postContents.js'

import { onMounted, onUnmounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'

import { ConfirmSwal, ToastSwal } from '@/extension/SwalExt.js'
import { useNotificationStore } from '@/stores/useNotification.js'

const router = useRouter()
const route = useRoute()
const notificationStore = useNotificationStore()

const userRole = route?.meta.userInfo.role

onMounted(async () => {
  await GetCategoryReference()
})

onUnmounted(() => {
  ResetInput()
})

async function onSave(action) {
  const { isConfirmed } = await ConfirmSwal.fire({ text: `your article will be ${action}` })
  if (!isConfirmed) return

  const result = await HandleSave(action)
  if (result === 400 || result === false) {
    return await ToastSwal.fire({ icon: 'warning', text: 'Please re-check your input!!' })
  }

  if (result === true) {
    notificationStore.set('add')
    return await router.push('/content')
  }
}
</script>

<template>
  <div class="flex items-center align-middle gap-3 mb-3 bg-tea">
    <RouterLink :to="{ name: 'content' }">
      <p class="text-sm">Content List</p>
    </RouterLink>
    <span> > </span>
    <p class="text-sm text-orange-400">New Content</p>
  </div>

  <div class="w-full rounded-lg bg-white min-h-[25%] max-h-[96%] box-border flex flex-col">
    <div class="flex gap-2.5 items-center border-b-2 py-5 px-8">
      <h1 class="basis-[85%] text-2xl font-bold text-green-800">New Content</h1>

      <RouterLink :to="{ name: 'content' }">
        <AbortButton>Cancel</AbortButton>
      </RouterLink>
      <SecondaryButton @click.prevent="onSave('Draft')"> Save as Draft </SecondaryButton>
      <PrimaryButton @click.prevent="onSave('Publish')" v-if="userRole === 'super'">
        Publish
      </PrimaryButton>
    </div>

    <div class="overflow-y-auto max-h-[80%]">
      <form class="w-full p-8 flex flex-col align-baseline gap-5">
        <TextForm
          :name="'Title'"
          :required="true"
          :error="errorInput.title"
          v-model="newArticle.title"
        />

        <DropdownForm
          :required="true"
          :name="'Category'"
          :items="categoryRef"
          :error="errorInput.categoryId"
          v-model="newArticle.categoryId"
        />

        <DescriptionEditor
          v-model="newArticle.description"
          v-model:html="newArticle.descriptionHtml"
          :error="errorInput.description"
        />

        <ArticleEditor v-model="newArticle.article" :error="errorInput.article" />

        <TextForm :name="'Additional Link'" :required="false" v-model="newArticle.additionalLink" />
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