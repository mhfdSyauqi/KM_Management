<script setup>
import TextForm from '@/components/forms/TextForm.vue'
import ArticleEditor from '@/components/editors/ArticleEditor.vue'
import PrimaryButton from '@/components/buttons/PrimaryButton.vue'
import DropdownForm from '@/components/forms/DropdownForm.vue'
import AbortButton from '@/components/buttons/AbortButton.vue'
import DescriptionEditor from '@/components/editors/DescriptionEditor.vue'
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
      <h1 class="basis-[84%] text-2xl font-bold text-green-800">Edit Content</h1>

      <RouterLink :to="{ name: 'content' }">
        <AbortButton>Cancel</AbortButton>
      </RouterLink>
      <PrimaryButton class="w-28">Re-Publish</PrimaryButton>
    </div>

    <div class="overflow-y-auto max-h-[80%]">
      <form class="w-full p-8 flex flex-col align-baseline gap-5">
        <TextForm :name="'Title'" :required="true" />

        <DropdownForm :required="true" :name="'Category'" />

        <DescriptionEditor />
        <ArticleEditor />

        <TextForm :name="'Additional Link'" :required="false" />

        <div class="grid grid-cols-9 grid-rows-3 w-full gap-y-2.5 mt-2">
          <p class="font-medium">Create By</p>
          <p class="col-span-8">DMS Syauqi {{ new Date() }}</p>
          <p class="font-medium">Last Modified By</p>
          <p class="col-span-8">DMS Syauqi {{ new Date() }}</p>
          <p class="font-medium">Publish By</p>
          <p class="col-span-8">DMS Cahya {{ new Date() }}</p>
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