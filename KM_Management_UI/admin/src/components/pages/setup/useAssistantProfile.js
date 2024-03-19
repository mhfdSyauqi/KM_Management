import { ref } from 'vue'
import { GetAssistantProfileAsync } from '@/api/reqSetup.js'

const profile = ref("");


async function GetAssistantProfile() {
  const result = await GetAssistantProfileAsync()

  if (result.is_success) {
    profile.value = result.profile
  }
}
export { profile, GetAssistantProfile }