<script setup>
import ReaderEditor from '@/components/editors/ReaderEditor.vue'
import { LoadArticleById, article } from '@/components/pages/article/useArticle.js'

import { ErrorSwal } from '@/extension/SwalExt.js'
import { onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useDateFormat } from '@vueuse/core'

const route = useRoute()

onMounted(async () => {
  const articleId = route.params.id
  const isFound = await LoadArticleById(articleId)
  if (!isFound) {
    await ErrorSwal.fire({ text: `Article was not found!!!` })
  }
})

function useFormat(dateStr) {
  return useDateFormat(dateStr, 'MMM, DD YYYY HH:mm').value
}
</script>

<template>
  <div class="w-full min-h-svh bg-side-bar py-10 font-roboto">
    <main
      class="w-[50%] bg-slate-100 mx-auto min-h-[50svh] p-10 shadow-2xl drop-shadow-2xl rounded-xl space-y-7"
    >
      <div class="flex flex-col gap-1">
        <h1 class="text-3xl font-semibold italic">{{ article.title }}</h1>
        <h5 class="text-xs">
          Posted by :
          <span class="font-bold">{{ article.create_by }} </span>
          <span> ({{ useFormat(article.create_at) }}) </span>
        </h5>
      </div>

      <ReaderEditor :article="article.article" />

      <div v-if="article.additional_link !== ''">
        <p>
          untuk informasi lebih lengkap anda dapat mengunjungi link :
          <a
            :href="article.additional_link"
            target="_blank"
            class="text-green-800 font-bold hover:underline visited:text-gray-600"
          >
            click here
          </a>
        </p>
      </div>

      <br />
      <br />
      <div class="border-t-2 w-full text-right fixed bottom-1 right-0 py-3.5 px-7">
        <span class="text-xs text-gray-600">
          Knowledge Management property of <b>DMS3</b> | since 2024
        </span>
      </div>
    </main>
  </div>
</template>

<style scoped></style>