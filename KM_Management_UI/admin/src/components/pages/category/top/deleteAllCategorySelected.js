import { DeleteCategoryTopIssueAllSelectedAsync} from '@/api/reqCategory.js'
import { ref } from 'vue'

const errorAllDeleted = ref({
  name: { isError: false, message: '' },
})


async function HandleAllDelete() {

  const patchCategory = await DeleteCategoryTopIssueAllSelectedAsync()

  if (!patchCategory.is_success) {
    if (patchCategory.error.statuscode === 400) {
      errorAllDeleted.value.name.isError = true
      errorAllDeleted.value.name.message = patchCategory.error.message
    }
    return patchCategory.error.statuscode
  }
  return true
}

export { errorAllDeleted, HandleAllDelete }