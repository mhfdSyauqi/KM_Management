import { createFetch } from '@vueuse/core'

const useRequest = createFetch({
  baseUrl: 'https://localhost:44362/api',
  combination: 'overwrite',
  fetchOptions: {
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json'
    }
  }
})

export { useRequest }