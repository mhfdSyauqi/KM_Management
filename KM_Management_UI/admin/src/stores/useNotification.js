import { acceptHMRUpdate, defineStore } from 'pinia'
import { ref } from 'vue'

export const useNotificationStore = defineStore('useNotification', () => {
  const status = ref('none')
  const set = (type) => (status.value = type)
  const reset = () => (status.value = 'none')
  return { status, set, reset }
})

if (import.meta.hot) {
  import.meta.hot.accept(acceptHMRUpdate(useNotificationStore, import.meta.hot))
}
