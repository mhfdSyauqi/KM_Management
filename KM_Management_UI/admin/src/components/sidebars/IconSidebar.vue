<script setup>
import IconArticle from '../icons/IconArticle.vue'
import IconDocumentation from '../icons/IconDocumentation.vue'

defineProps({
  icon: {
    type: String,
    default: 'dashboard'
  }
})
</script>

<template>
  <IconArticle v-if="icon === 'article'" class="fill-white" />

  <IconDocumentation v-else class="fill-white" />
</template>

<style scoped></style>
