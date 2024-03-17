<script setup>
import IconArticle from '../icons/IconArticle.vue'
import IconList from '@/components/icons/IconList.vue'
import IconSettings from '@/components/icons/IconSettings.vue'
import IconDashboard from '@/components/icons/IconDashboard.vue'

defineProps({
  icon: {
    type: String,
    default: 'dashboard'
  }
})
</script>

<template>
  <IconArticle v-if="icon === 'article'" />

  <IconList v-else-if="icon === 'list'" />

  <IconSettings v-else-if="icon === 'settings'" />

  <IconDashboard v-else />
</template>

<style scoped></style>