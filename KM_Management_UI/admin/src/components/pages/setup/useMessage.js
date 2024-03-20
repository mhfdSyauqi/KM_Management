import { ref } from 'vue'
import { GetActiveMessageAsync} from '@/api/reqSetup.js'

const active_message = ref("");




async function GetActiveMessage(type) {
  const result = await GetActiveMessageAsync(type)

  if (result.is_success) {
    active_message.value = result.active_message
  }
}
export { active_message, GetActiveMessage }